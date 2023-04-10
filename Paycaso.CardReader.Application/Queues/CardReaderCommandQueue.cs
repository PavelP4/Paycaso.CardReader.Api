using Paycaso.CardReader.Application.Managers;
using System.Collections.Concurrent;

namespace Paycaso.CardReader.Application.Queues
{
    public class CardReaderCommandQueue
    {
        private readonly ConcurrentQueue<Action<CardReaderManager, CancellationToken>> _queue = new();
        private readonly SemaphoreSlim _semaphore = new(0);

        public void Enqueue(Action<CardReaderManager, CancellationToken> action)
        {
            _queue.Enqueue(action);
            _semaphore.Release();
        }

        public async Task<Action<CardReaderManager, CancellationToken>?> DequeueOrWaitOne(CancellationToken cancellationToken)
        {
            await _semaphore.WaitAsync(cancellationToken);
            _queue.TryDequeue(out var action);

            return action;
        }
    }
}
