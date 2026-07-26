using Application.Repositories;
using Mediator.Abstractions;

namespace Application.UseCases.Account.Create;

public class Handler(IAccountRepository repository) : IHandler<Request, string>
{
    public async Task<string> HandleAsync(Request request, CancellationToken cancellationToken = default)
    {
        var account = new Domain.Entities.Account
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
        };

        await repository.SaveAsync(account, cancellationToken);

        return $"Account  {account.Id} was created";
    }
}