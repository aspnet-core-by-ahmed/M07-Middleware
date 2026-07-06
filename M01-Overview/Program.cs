var builder = WebApplication.CreateBuilder(args);
// DI container 


var app = builder.Build();

// middleware setup goes here 
// framework sets up the pipeline infra when project created 
// pipeline conceptually empty , through templates may include some default middleware 
// you can add middleware from framework , 3rd paty library , custom components 
// the pipeline is a sequence of middleware that process HTTP requests , responses (chain of responsibility by using RequestDelegate)
// imagine middleware as function take RequestDelegate and return RequestDelegate 
// RequestDelegate takes HttpContext and return Task 
// Task -> promise that i will execute and return void  

app.Run();


// called Middleware of Middleware component
// middleware must apply SPR (Single Responsibilty Principle) 
// we will learn three ways to write middleware (3 overloads)

