var builder = WebApplication.CreateBuilder(args);

// ===============================
// Register Services (DI Container)
// ===============================

// builder.Services.Add...();

var app = builder.Build();

// ===============================
// Configure Middleware Pipeline
// ===============================


/*
============================================================
Way #1
Use(Func<RequestDelegate, RequestDelegate>)
Factory Style
------------------------------------------------------------
Signature:

RequestDelegate -> RequestDelegate

Take:
    RequestDelegate next

Return:
    RequestDelegate

Think of it as:

    next  -------->  build new middleware  -------->  return new next
============================================================
*/


// ----------------------------------------------------------
// Example 1 : Middleware does absolutely nothing
// ----------------------------------------------------------

app.Use((RequestDelegate next) =>
{
    // Just return the next middleware without modification
    return next;
});

// Same as:
//
// app.Use(next => next);


// ----------------------------------------------------------
// Example 2 : Create a middleware manually
// ----------------------------------------------------------

app.Use((RequestDelegate next) =>
{
    // Return a new RequestDelegate
    return async (HttpContext context) =>
    {
        await context.Response.WriteAsync("MW #2\n");

        // Pass execution to the next middleware
        await next(context);

        // Code here executes after the next middleware
    };
});

// Same as:
//
// app.Use(next =>
// {
//     return async context =>
//     {
//         await context.Response.WriteAsync("MW #2\n");
//         await next(context);
//     };
// });



/*
============================================================
Way #2
Use(Func<HttpContext, RequestDelegate, Task>)

Signature:

(HttpContext, RequestDelegate) -> Task

Take:
    HttpContext context
    RequestDelegate next

Return:
    Task

This is the most common overload.

ASP.NET Core builds the RequestDelegate for you.
============================================================
*/

app.Use(async (HttpContext context, RequestDelegate next) =>
{
    await context.Response.WriteAsync("MW #1\n");

    // Continue to the next middleware
    await next(context);

    // This code runs after the next middleware finishes
});


/*
============================================================
Way #3
Use(Func<HttpContext, Func<Task>, Task>)

Signature:

(HttpContext, Func<Task>) -> Task

Take:
    HttpContext context
    Func<Task> next

Return:
    Task

This overload wraps RequestDelegate into Func<Task>.

Instead of:

    await next(context);

you simply write:

    await next();

This overload is less common than Way #2.
============================================================
*/

app.Use(async (HttpContext context, Func<Task> next) =>
{
    await context.Response.WriteAsync("MW #3\n");

    await next();

    // Executes after the remaining pipeline finishes
});


app.Run();


// Note RequestDelegate take HttpContext and return Task