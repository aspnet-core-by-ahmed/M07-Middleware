var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

/*
============================================================
Important Rule

The HTTP Response consists of:

1. Status Code
2. Headers
3. Body

Response Layout:

+------------------+
| Status Code      |
+------------------+
| Headers          |
+------------------+
| Body (Stream)    |
+------------------+

Once the Body starts being written:

    await context.Response.WriteAsync(...);

ASP.NET Core sends the Status Code and Headers
to the client first.

After that:

❌ Status Code cannot be changed.
❌ Headers cannot be changed.
✔ Body can continue to be written because it is a stream.
============================================================
*/


// ============================================================
// Example #1
// Valid
//
// Modify StatusCode and Headers BEFORE writing the response body.
// ============================================================

app.Use(async (context, next) =>
{
    // Safe: response has NOT started yet.
    context.Response.StatusCode = StatusCodes.Status200OK;
    context.Response.Headers.Append("X-Version", "1.0");

    // The response starts here.
    await context.Response.WriteAsync("MW #1\n");

    await next(context);
});


/*
// ============================================================
// Example #2
// Invalid
//
// Write the body first, then try to modify
// StatusCode or Headers.
//
// This throws InvalidOperationException because
// the response has already started.
// ============================================================

app.Use(async (context, next) =>
{
    // Response starts here.
    await context.Response.WriteAsync("MW #2\n");

    // ❌ Too late.
    context.Response.StatusCode = StatusCodes.Status202Accepted;

    // ❌ Too late.
    context.Response.Headers.Append("X-Test", "Value");

    await next(context);
});
*/


/*
// ============================================================
// Example #3
// Also Valid
//
// Even after the response starts,
// you may continue writing to the Body.
//
// The Body is a Stream.
// ============================================================

app.Use(async (context, next) =>
{
    context.Response.StatusCode = StatusCodes.Status200OK;

    await context.Response.WriteAsync("First Part\n");

    // ✔ Allowed because the body is a stream.
    await context.Response.WriteAsync("Second Part\n");

    await next(context);
});
*/


/*
============================================================
Why?

Status Code and Headers are sent only once.

Example:

HTTP/1.1 200 OK
Content-Type: text/plain
X-Version: 1.0

Body starts here...

After the Body begins,
the client has already received
the Status Code and Headers.

Therefore ASP.NET Core prevents changing them.
============================================================
*/


app.Run();