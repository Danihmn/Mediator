using Application;
using Application.UseCases.Account.Create;
using Infrastructure;
using Mediator.Abstractions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication();
builder.Services.AddInfrastructure();

var app = builder.Build();

app.UseHttpsRedirection();

app.MapPost("accounts", async (Request request, IMediator mediator, CancellationToken cancellationToken) =>
{
    var result = await mediator.SendAsync(request, cancellationToken);
    return Results.Ok(result);
});

app.Run();