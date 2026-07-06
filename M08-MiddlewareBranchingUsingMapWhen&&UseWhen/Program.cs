var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

/*
===========================================================================
MapWhen()
---------------------------------------------------------------------------

Predicate:
    Func<HttpContext, bool>

Purpose:
    Creates a completely NEW branch when the condition is TRUE.

If the condition is FALSE:
    Execution continues in the main pipeline.

If the condition is TRUE:
    The request enters the new branch.

Unlike UseWhen(), MapWhen() DOES NOT automatically return
to the main pipeline unless you explicitly call next()
inside that branch.

Use it when you want an entirely different pipeline.

Example:
    /checkout?mode=new
===========================================================================*/

app.MapWhen(
    context =>
        context.Request.Path.Equals("/checkout", StringComparison.OrdinalIgnoreCase)
        && context.Request.Query["mode"] == "new",

    branch =>
    {
        branch.Run(async context =>
        {
            await context.Response.WriteAsync(
                "Modern checkout processing pipeline"
            );
        });
    });




/*
===========================================================================
Map()
---------------------------------------------------------------------------

Purpose:
    Creates a new branch based ONLY on the request path.

Matching:

    /checkout
    /checkout/anything

When a request matches the path,
execution moves to the new branch.

Since Run() is used,
the pipeline ends inside this branch.

Use Map() when routing depends only on the URL path.
===========================================================================*/

app.Map("/checkout", branch =>
{
    branch.Run(async context =>
    {
        await context.Response.WriteAsync(
            "Legacy checkout processing pipeline"
        );
    });
});




/*
===========================================================================
UseWhen()
---------------------------------------------------------------------------

Predicate:
    Func<HttpContext, bool>

Purpose:
    Execute SOME middleware only when the condition is TRUE.

Difference from MapWhen():

MapWhen()
    ✔ Creates an independent pipeline.
    ✔ Usually does NOT return to the main pipeline.

UseWhen()
    ✔ Temporarily enters a branch.
    ✔ After the branch finishes,
      execution automatically returns
      to the main pipeline.

Think of it as:

Main Pipeline
      │
      ▼
Condition?
      │
   True
      │
      ▼
Branch Middleware
      │
      ▼
Back to Main Pipeline
===========================================================================*/

app.UseWhen(
    context =>
        context.Request.Path.Equals("/branch1", StringComparison.OrdinalIgnoreCase),

    branch =>
    {
        branch.Use(async (context, next) =>
        {
            await context.Response.WriteAsync("MW Branch 1\n");

            await next(context);
        });
    });




/*
===========================================================================
Terminal Middleware

Executed if no previous middleware
has already generated the final response.
===========================================================================*/

app.Run(async context =>
{
    await context.Response.WriteAsync("Terminal Middleware");
});