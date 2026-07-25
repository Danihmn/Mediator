using Mediator.Abstractions;

namespace Mediator;

public class Mediator(IServiceProvider serviceProvider) : IMediator
{
    public async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request,
        CancellationToken cancellationToken = default)
    {
        var requestType = request.GetType();
        var handlerType = typeof(IHandler<,>).MakeGenericType(requestType, typeof(TResponse));

        var handler = serviceProvider.GetService(handlerType) ??
                      throw new InvalidOperationException($"Handler nos found for {requestType}");
        var method = handlerType.GetMethod("HandleAsync") ??
                     throw new InvalidOperationException($"Method 'HandleAsync' not found in {handlerType}");

        var result = method.Invoke(handler, [request, cancellationToken]);

        if (result is not Task<TResponse> task)
            throw new InvalidOperationException("Method returns unexpected type");

        return await task;
    }
}