using Domain.Entities;

namespace Application.Repositories;

public interface IAccountRepository
{
    Task SaveAsync(Account entity, CancellationToken cancellationToken = default);
}