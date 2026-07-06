
# ASP.NET Core Middleware

A comprehensive guide to understanding **ASP.NET Core Middleware** from the ground up.

This repository covers the Middleware pipeline in depth, including how requests and responses flow through the application, how middleware components are built, and how branching works inside the pipeline.

## Topics Covered

* Middleware Fundamentals
* Request & Response Pipeline
* `Use()`, `Run()`, and `Map()`
* `MapWhen()` vs `UseWhen()`
* Terminal Middleware
* Short-Circuiting the Pipeline
* RequestDelegate & HttpContext
* Built-in Middleware
* Middleware Ordering
* Branching the Pipeline
* Response Headers, Status Code, and Body
* Response Has Started
* Custom Middleware
* Best Practices

## What You'll Learn

* How ASP.NET Core processes every HTTP request.
* How middleware components are chained together.
* The differences between `Use`, `Run`, `Map`, `MapWhen`, and `UseWhen`.
* How to create custom middleware from scratch.
* How branching creates independent request pipelines.
* Why middleware order is critical.
* Common pitfalls and real-world examples.

---

**Tech Stack**

* C#
* ASP.NET Core
* .NET 8/9

This repository is intended for developers who want a solid understanding of the ASP.NET Core request pipeline before building production-grade Web APIs or MVC applications.
