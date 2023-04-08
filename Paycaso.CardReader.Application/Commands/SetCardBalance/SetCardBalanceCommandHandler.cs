using MediatR;

namespace Paycaso.CardReader.Application.Commands.SetCardBalance
{
    public class SetCardBalanceCommandHandler : IRequestHandler<SetCardBalanceCommand>
    {
        public SetCardBalanceCommandHandler()
        { 
        
        }

        public Task Handle(SetCardBalanceCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
