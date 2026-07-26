using Application.Repositories;
using Domain.Entities;

namespace Infrastructure.Repositories;

public class AccountRepository : IAccountRepository
{
    public Task SaveAsync(Account entity, CancellationToken cancellationToken = default)
    {
        Console.WriteLine($"Account {entity.Id} saved");
        return Task.CompletedTask;
    }
}