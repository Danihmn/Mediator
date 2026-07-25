namespace Mediator.Abstractions;

public interface IMediator
{
    /// <summary>
    ///  Define um contrato responsável por mediar o envio para o Handler e retorno para quem mandou a request
    /// </summary>
    /// <param name="request">Request enviada</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <typeparam name="TResponse">Response devolvida para quem enviou a request</typeparam>
    /// <returns>Response para quem enviou a request</returns>
    Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default);
}