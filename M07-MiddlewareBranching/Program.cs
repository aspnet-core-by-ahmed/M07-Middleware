var builder = WebApplication.CreateBuilder(args);

static void GetCommonBraches(IApplicationBuilder applicationBuilder)
{
    applicationBuilder.Use(async (context, next) =>
{
    await context.Response.WriteAsync("MW #1 \n");
    await next(context);
});

    applicationBuilder.Use(async (context, next) =>
    {
        await context.Response.WriteAsync("MW #2 \n");
        await next(context);
    });
}


static void GetBraches3_4(IApplicationBuilder applicationBuilder)
{
    applicationBuilder.Use(async (context, next) =>
   {
       await context.Response.WriteAsync("MW #3 \n");
       await next(context);
   });

    applicationBuilder.Use(async (context, next) =>
    {
        await context.Response.WriteAsync("MW #4 \n");
        await next(context);
    });


}


static void GetBranches5_6(IApplicationBuilder applicationBuilder)
{
    applicationBuilder.Use(async (context, next) =>
{
    await context.Response.WriteAsync("MW #5 \n");
    await next(context);
});
    applicationBuilder.Use(async (context, next) =>
    {
        await context.Response.WriteAsync("MW #6 \n");
        await next(context);
    });
}


var app = builder.Build();



app.Map("/branch1", b1 =>
{
    GetCommonBraches(b1);
    GetBraches3_4(b1);

    b1.Run(async context =>
{
    await context.Response.WriteAsync("Terminal Middleware branch 1");
});
});


app.Map("/branch2", b2 =>
{
    GetCommonBraches(b2);
    GetBranches5_6(b2);

    b2.Run(async context =>
    {
        await context.Response.WriteAsync("Terminal Middleware branch 2");
    });
});


app.Run(async context =>
{
    await context.Response.WriteAsync("Terminal Middleware");
});



app.Run();