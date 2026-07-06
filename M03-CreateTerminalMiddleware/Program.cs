var builder = WebApplication.CreateBuilder(args);

// =======================================================
// Register Services (Dependency Injection Container)
// =======================================================

// builder.Services.AddControllers();
// builder.Services.AddScoped<IService, Service>();

var app = builder.Build();

// =======================================================
// Configure Middleware Pipeline
// =======================================================



/*
====================================================================
Way #1
Use(Func<RequestDelegate, RequestDelegate>)
--------------------------------------------------------------------

Signature:

RequestDelegate -> RequestDelegate

Take:
    RequestDelegate next

Return:
    RequestDelegate

This is called the Factory Style.

You receive the next RequestDelegate,
build a new RequestDelegate around it,
and return the new delegate.

Visual:

        next
         │
         ▼
 Build New Middleware
         │
         ▼
Return RequestDelegate
====================================================================
*/


// ----------------------------------------------------------
// Example #1
// Middleware does absolutely nothing
// ----------------------------------------------------------

app.Use((RequestDelegate next) =>
{
    // Simply return the next middleware.
    return next;
});

// Same as:
//
// app.Use(next => next);



// ----------------------------------------------------------
// Example #2
// Create a middleware manually
// ----------------------------------------------------------

app.Use((RequestDelegate next) =>
{
    // Return a new RequestDelegate.
    return async (HttpContext context) =>
    {
        // Code executed BEFORE the next middleware.
        await context.Response.WriteAsync("MW #2 Before\n");

        // Continue the pipeline.
        await next(context);

        // Code executed AFTER the next middleware.
        await context.Response.WriteAsync("MW #2 After\n");
    };
});

// Same as:
//
// app.Use(next =>
// {
//     return async context =>
//     {
//         await context.Response.WriteAsync("MW #2 Before\n");
//
//         await next(context);
//
//         await context.Response.WriteAsync("MW #2 After\n");
//     };
// });



/*
====================================================================
Way #2
Use(Func<HttpContext, RequestDelegate, Task>)
--------------------------------------------------------------------

Signature:

(HttpContext, RequestDelegate) -> Task

Take:
    HttpContext context
    RequestDelegate next

Return:
    Task

This is the most commonly used overload.

Instead of creating a RequestDelegate yourself,
ASP.NET Core creates it for you.

You only write the middleware logic.
====================================================================
*/

app.Use(async (HttpContext context, RequestDelegate next) =>
{
    // Code before the next middleware.
    await context.Response.WriteAsync("MW #1 Before\n");

    // Continue the pipeline.
    await next(context);

    // Code after the next middleware.
    await context.Response.WriteAsync("MW #1 After\n");

    // If next(context) is NOT called,
    // a Short Circuit occurs and the pipeline stops here.
});



/*
====================================================================
Way #3
Use(Func<HttpContext, Func<Task>, Task>)
--------------------------------------------------------------------

Signature:

(HttpContext, Func<Task>) -> Task

Take:
    HttpContext context
    Func<Task> next

Return:
    Task

Instead of RequestDelegate,
the next middleware is wrapped inside Func<Task>.

So instead of:

    await next(context);

you simply write:

    await next();

This overload is less common.
====================================================================
*/

app.Use(async (HttpContext context, Func<Task> next) =>
{
    // Code before the next middleware.
    await context.Response.WriteAsync("MW #3 Before\n");

    // Continue the pipeline.
    await next();

    // Code after the next middleware.
    await context.Response.WriteAsync("MW #3 After\n");
});



/*
====================================================================
Run()
--------------------------------------------------------------------

Signature:

(HttpContext) -> Task

Take:
    HttpContext context

Return:
    Task

Run() is a Terminal Middleware.

Unlike Use(), Run() does NOT receive
RequestDelegate next.

Therefore:

✔ It can generate a response.
✔ It always ends the pipeline.
✔ It cannot forward the request.
✔ Middleware registered after Run() will NEVER execute.
====================================================================
*/

app.Run(async (HttpContext context) =>
{
    await context.Response.WriteAsync(
        "This is the end of the pipeline (Terminal Middleware)"
    );
});



/*
====================================================================
Pipeline Execution

Request
   │
   ▼
Use #1 (Factory Style)
   │
   ▼
Use #2 (Most Common)
   │
   ▼
Use #3 (Func<Task>)
   │
   ▼
Run()  ← Terminal Middleware
   │
   ▼
Response


Execution Order

Before:
Use #1
    ↓
Use #2
    ↓
Use #3
    ↓
Run()

After:
Use #3
    ↑
Use #2
    ↑
Use #1
====================================================================
*/

app.Run();