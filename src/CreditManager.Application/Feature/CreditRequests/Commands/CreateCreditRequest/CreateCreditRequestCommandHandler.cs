using MediatR;

namespace CreditManager.Application.Feature.CreditRequests.Commands.CreateCreditRequest;

public class CreateCreditRequestCommandHandler : IRequestHandler<CreateCreditRequestCommand, Guid>
{
    public async Task<Guid> Handle(CreateCreditRequestCommand request, CancellationToken cancellationToken)
    {
        return Guid.NewGuid();
    }
}