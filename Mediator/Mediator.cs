using Mediator.Abstractions;

namespace Mediator;

public class Mediator(IServiceProvider serviceProvider) : IMediator
{
    public async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request,
        CancellationToken cancellationToken = default)
    {
        var requestType = request.GetType();

        // Encontra o tipo do Handler dinamicamente, combinando o tipo da request e da response, em runtime
        var handlerType = typeof(IHandler<,>).MakeGenericType(requestType, typeof(TResponse));

        // Resolve a instância do Handler necessário no DI, buscando o pelo seu tipo
        var handler = serviceProvider.GetService(handlerType) ??
                      throw new InvalidOperationException($"Handler nos found for {requestType}");

        // Busca o método do Handler, em runtime
        var method = handlerType.GetMethod("HandleAsync") ??
                     throw new InvalidOperationException($"Method 'HandleAsync' not found in {handlerType}");

        // Chama o método, passando nos parâmetros a request e o cancellationToken
        var result = method.Invoke(handler, [request, cancellationToken]);

        if (result is not Task<TResponse> task)
            throw new InvalidOperationException("Method returns unexpected type");

        return await task;
    }
}