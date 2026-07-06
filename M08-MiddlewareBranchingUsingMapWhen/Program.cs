var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapWhen(context =>
    context.Request.Path.Equals("/checkout", StringComparison.OrdinalIgnoreCase)
    && context.Request.Query["mode"] == "new",
    b =>
    {
        b.Run(async context =>
        {
            await context.Response.WriteAsync("Modern checout processing pipeline");
        });
    }
);

app.Map("/checkout", b =>
{
    b.Run(async context =>
    {
        await context.Response.WriteAsync("Legacy checout processing pipeline");
    });
});
app.Run();
