var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.Use(async (context, next) =>
{
    await context.Response.WriteAsync("MW #1 Before\n");
    await next(context);
    await context.Response.WriteAsync("MW #1 After\n");
});



app.Use(async (context, next) =>
{
    await context.Response.WriteAsync(" MW #2 Before\n");
    await next(context);
    await context.Response.WriteAsync(" MW #2 After\n");
});



app.Use(async (context, next) =>
{
    await context.Response.WriteAsync("     MW #3 Before\n");
    await next(context);
    await context.Response.WriteAsync("     MW #3 After\n");
});



app.Run();
