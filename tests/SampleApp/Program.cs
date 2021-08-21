// Copyright (c) Martin Costello, 2018. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.MapGet("/api/values", () => Results.Json(new string[] { "a", "b", "c" }));

app.MapGet("/api/values/{id}", (string id) => "value");

app.MapPost("/api/values", (string value) => Results.NoContent());

app.MapPut("/api/values/{id}", (string id) => Results.NoContent());

app.MapDelete("/api/values/{id}", (string id) => Results.NoContent());

app.Run();
