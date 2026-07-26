using Mediator.Abstractions;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();

services.AddTransient<IMediator, Mediator.Mediator>();
services.AddTransient<AccountRepository>();
services.AddTransient<IHandler<CreateAccountCommand, string>, CreateAccountHandler>();


var servicesProvider = services.BuildServiceProvider();
var mediator = servicesProvider.GetRequiredService<IMediator>();

var request = new CreateAccountCommand()
{
    Username = "Batman",
    Password = "123456"
};

var result = await mediator.SendAsync(request);

Console.WriteLine(result);

public class AccountRepository
{
    public void Save() => Console.WriteLine("Saved");
}

public class CreateAccountCommand : IRequest<string>
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class CreateAccountHandler(AccountRepository repository) : IHandler<CreateAccountCommand, string>
{
    public Task<string> HandleAsync(CreateAccountCommand request, CancellationToken cancellationToken = default)
    {
        Console.WriteLine($"Creating {request.Username} Account...");
        repository.Save();
        return Task.FromResult($"{request.Username} created");
    }
}