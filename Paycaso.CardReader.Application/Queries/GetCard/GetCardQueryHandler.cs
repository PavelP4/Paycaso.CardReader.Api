using MediatR;
using Paycaso.CardReader.Application.Models;
using Paycaso.CardReader.Application.Queues;

namespace Paycaso.CardReader.Application.Queries.GetCard
{
    public class GetCardQueryHandler : IRequestHandler<GetCardQuery, CardDto>
    {
        private readonly CardReaderCommandQueue _cardReaderCommandQueue;

        public GetCardQueryHandler(CardReaderCommandQueue cardReaderCommandQueue)
        {
            _cardReaderCommandQueue = cardReaderCommandQueue;
        }

        public Task<CardDto> Handle(GetCardQuery request, CancellationToken cancellationToken)
        {
            var tcs = new TaskCompletionSource<CardDto>();

            _cardReaderCommandQueue.Enqueue((tm, c) =>
            {
                tcs.SetResult(new CardDto());
            });

            return tcs.Task;
        }
    }
}
