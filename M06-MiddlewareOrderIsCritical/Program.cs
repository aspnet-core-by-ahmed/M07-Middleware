var builder = WebApplication.CreateBuilder(args);

// ========================================================
// Register Services (Dependency Injection Container)
// ========================================================

// builder.Services.AddControllers();
// builder.Services.AddAuthentication();
// builder.Services.AddAuthorization();
// builder.Services.AddCors();

var app = builder.Build();

// ========================================================
// Configure HTTP Request Pipeline (Middleware Pipeline)
// ========================================================

/*
Request
   │
   ▼
Exception Handler
   │
   ▼
HSTS
   │
   ▼
HTTPS Redirection
   │
   ▼
Static Files
   │
   ▼
Routing
   │
   ▼
CORS
   │
   ▼
Authentication
   │
   ▼
Authorization
   │
   ▼
Custom Middleware
   │
   ▼
Endpoint
   │
   ▼
Response
*/


// ========================================================
// Exception Handler
// ========================================================
// Catches unhandled exceptions thrown by later middleware.
// Should be one of the first middleware in the pipeline.
app.UseExceptionHandler();


// ========================================================
// HTTP Strict Transport Security (HSTS)
// ========================================================
// Tells browsers to always use HTTPS for future requests.
// Usually enabled only in Production.
app.UseHsts();


// ========================================================
// HTTPS Redirection
// ========================================================
// Redirect HTTP requests to HTTPS.
//
// Example:
//
// http://localhost:5000
//          │
//          ▼
// https://localhost:5001
//
app.UseHttpsRedirection();


// ========================================================
// Static Files
// ========================================================
// Serves files from wwwroot.
//
// Examples:
//
// /css/site.css
// /images/logo.png
// /js/site.js
//
// If a static file is found,
// the pipeline ends immediately.
app.UseStaticFiles();


// ========================================================
// Routing
// ========================================================
// Matches the incoming request
// to the appropriate endpoint.
//
// Example:
//
// GET /products/10
//
// Routing decides which endpoint
// should handle this request.
app.UseRouting();


// ========================================================
// Cross-Origin Resource Sharing (CORS)
// ========================================================
// Allows browsers from another origin
// to access your application.
//
// Example:
//
// Frontend:
// https://myclient.com
//
// Backend:
// https://api.myserver.com
//
app.UseCors();


// ========================================================
// Authentication
// ========================================================
// Answers:
//
// "Who are you?"
//
// Reads JWT Token, Cookie,
// or another authentication method.
//
// If successful:
//
// context.User becomes authenticated.
app.UseAuthentication();


// ========================================================
// Authorization
// ========================================================
// Answers:
//
// "Are you allowed?"
//
// Checks:
//
// • Roles
// • Policies
// • Claims
//
// Authorization depends on Authentication.
//
// No Authentication
//        ↓
// No Authorization
app.UseAuthorization();


// ========================================================
// Custom Middleware
// ========================================================
// Your own middleware.
//
// Can inspect:
//
// • Request
// • Response
// • Headers
// • Cookies
// • Logging
// • Timing
//
app.Use(async (context, next) =>
{
    Console.WriteLine("Before Endpoint");

    await next();

    Console.WriteLine("After Endpoint");
});


// ========================================================
// Endpoint
// ========================================================
// The final destination that generates
// the response.
app.MapGet("/", () =>
{
    return "Hello World!";
});


// ========================================================
// Start Application
// ========================================================
app.Run();