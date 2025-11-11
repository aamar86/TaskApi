using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tasky.Application.Common;
using Tasky.Application.DTOs;
using Tasky.Application.Interfaces;
using Tasky.Application.Services;
using Tasky.Infrastructure.Ids;
using Tasky.Infrastructure.Repositories;
using Tasky.Infrastructure.Time;
using Tasky.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<TaskyDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<ITaskRepository, EfTaskRepository>();
builder.Services.AddScoped<ITaskService, TaskService>();
builder.Services.AddTransient<IDateTimeProvider, SystemClock>();
builder.Services.AddSingleton<IGuidProvider, GuidProvider>();

var app = builder.Build();

app.UseExceptionHandler(errApp =>
{
    errApp.Run(async context =>
    {
        var feature = context.Features.Get<IExceptionHandlerPathFeature>();
        var ex = feature?.Error ?? new Exception("Unknown error");
        var status = ex is ProblemException pe ? pe.StatusCode : 500;
        var problem = Results.Problem(
            title: ex is ProblemException ? "Request validation failed" : "Server error",
            detail: ex.Message,
            statusCode: status,
            instance: context.Request.Path);
        await problem.ExecuteAsync(context);
    });
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var group = app.MapGroup("/api/tasks");

group.MapGet("/", async ([FromServices] ITaskService svc, CancellationToken ct) => Results.Ok(await svc.ListAsync(ct)));
group.MapGet("/{id:guid}", async ([FromServices] ITaskService svc, Guid id, CancellationToken ct) =>
{
    var t = await svc.GetAsync(id, ct);
    return t is null ? Results.NotFound() : Results.Ok(t);
});
group.MapPost("/", async ([FromServices] ITaskService svc, [FromBody] CreateTaskRequest req, CancellationToken ct) =>
{
    var created = await svc.CreateAsync(req, ct);
    return Results.Created($"/api/tasks/{created.Id}", created);
});
group.MapPut("/{id:guid}", async ([FromServices] ITaskService svc, Guid id, [FromBody] UpdateTaskRequest req, CancellationToken ct) =>
{
    var updated = await svc.UpdateAsync(id, req, ct);
    return updated is null ? Results.NotFound() : Results.Ok(updated);
});
group.MapDelete("/{id:guid}", async ([FromServices] ITaskService svc, Guid id, CancellationToken ct) =>
{
    var ok = await svc.DeleteAsync(id, ct);
    return ok ? Results.NoContent() : Results.NotFound();
});

app.MapGet("/api/health", () => Results.Ok(new { status = "ok", time = DateTimeOffset.UtcNow }));

app.Run();
