using MediatR;
using Paycaso.CardReader.Application.Queues;

namespace Paycaso.CardReader.Application.Commands.SetCardBalance
{
    public class SetCardBalanceCommandHandler : IRequestHandler<SetCardBalanceCommand>
    {
        private readonly CardReaderCommandQueue _cardReaderCommandQueue;

        public SetCardBalanceCommandHandler(CardReaderCommandQueue cardReaderCommandQueue)
        {
            _cardReaderCommandQueue = cardReaderCommandQueue;
        }

        public Task Handle(SetCardBalanceCommand request, CancellationToken cancellationToken)
        {
            var tcs = new TaskCompletionSource();

            _cardReaderCommandQueue.Enqueue((tm, c) =>
            {
                tcs.SetResult();
            });

            return tcs.Task;
        }
    }
}
