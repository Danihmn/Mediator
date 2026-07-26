using Mediator.Abstractions;

namespace Application.UseCases.Account.Create;

public record Request(string Name) : IRequest<string>;