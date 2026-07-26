# Mediator

Projeto de estudo/prática da implementação do padrão **Mediator** do zero (sem bibliotecas como MediatR), integrado a uma estrutura em **Clean Architecture**.

## Estrutura da solução

A solution (`Mediator.slnx`) organiza os projetos assim:

- **Mediator.Abstractions** — contratos do Mediator:
  - `IRequest<TResponse>` — marca uma request que espera uma resposta do tipo `TResponse`.
  - `IHandler<TRequest, TResponse>` — contrato do handler que processa uma `TRequest` e retorna `TResponse`.
  - `IMediator` — contrato com `SendAsync<TResponse>(request, cancellationToken)` para enviar a request ao handler correspondente.
- **Mediator** — implementação:
  - `Mediator.cs` — implementação de `IMediator`. Resolve o handler em runtime via reflection: monta o tipo genérico `IHandler<,>` combinando o tipo da request com o tipo da resposta, busca a instância no `IServiceProvider`, localiza o método `HandleAsync` via reflection e o invoca.
  - `Extensions/MediatorExtension.cs` — método de extensão `AddMediator(this IServiceCollection, params Assembly[])` que registra `IMediator` no DI e varre as assemblies informadas em busca de classes concretas que implementam `IHandler<,>`, registrando cada uma automaticamente como `Transient`.
- **Mediator.Samples** — console app de exemplo isolado, demonstrando o uso básico do Mediator: registra o mediator via DI, define uma `CreateAccountCommand` (`IRequest<string>`) e um `CreateAccountHandler` (`IHandler<CreateAccountCommand, string>`) no próprio `Program.cs`, e envia a request via `mediator.SendAsync`.
- **Pasta "Clean Architecture"** (dentro da solution) — exemplo aplicando o Mediator em camadas:
  - **Domain** — entidade `Account` (`Id`, `Name`).
  - **Application** — casos de uso organizados por pasta (`UseCases/Account/Create`), cada um com sua `Request` (record que implementa `IRequest<TResponse>`) e `Handler` (implementa `IHandler<Request, TResponse>`); depende de abstrações como `IAccountRepository`; expõe `AddApplication()` que chama `AddMediator` passando a própria assembly.
  - **Infrastructure** — implementação concreta de `IAccountRepository` (`AccountRepository`, que atualmente só simula o save com `Console.WriteLine`); expõe `AddInfrastructure()` para registrar os repositórios no DI.
  - **Api** — Web API (`Program.cs`) que registra `AddApplication()` e `AddInfrastructure()` e expõe o endpoint `POST /accounts`, que recebe o `Request` do caso de uso `Create`, envia via `IMediator.SendAsync` e retorna o resultado.
- **ClassLibrary1** — projeto padrão gerado pelo template do .NET (`Class1` vazio), sem uso no fluxo do Mediator.

## Fluxo de uma requisição

1. Uma `Request` (implementando `IRequest<TResponse>`) é enviada via `IMediator.SendAsync`.
2. O `Mediator` descobre em runtime, via reflection, qual `IHandler<TRequest, TResponse>` corresponde ao tipo da request e da resposta.
3. O handler correspondente é resolvido no container de DI (registrado automaticamente por `AddMediator`) e seu `HandleAsync` é invocado.
4. O resultado é retornado ao chamador original.

## Observações

- Não há testes automatizados no repositório no momento.
- O registro de handlers é automático: basta declarar uma classe que implemente `IHandler<TRequest, TResponse>` na assembly informada em `AddMediator` que ela é registrada no DI como `Transient`.
