// (c) 2020 Francesco Del Re <francesco.delre.87@gmail.com>
// This code is licensed under MIT license (see LICENSE.txt for details)
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace SharpTracer.Services
{
    public interface IQueueService
    {
        void QueueTraceRequest(TracerRequest request);
        Task<TracerRequest> DequeueTraceRequestAsync(CancellationToken cancellationToken);
    }

    public class QueueService : IQueueService
    {
        private readonly ConcurrentQueue<TracerRequest> _queueTraceRequests =
            new ConcurrentQueue<TracerRequest>();

        private readonly SemaphoreSlim _signal = new SemaphoreSlim(0);

        public void QueueTraceRequest(TracerRequest request)
        {
            if (request != null)
            {
                _queueTraceRequests.Enqueue(request);
                _signal.Release();
            }
        }

        public async Task<TracerRequest> DequeueTraceRequestAsync(CancellationToken cancellationToken)
        {
            await _signal.WaitAsync(cancellationToken);
            _queueTraceRequests.TryDequeue(out var workItem);

            return workItem;
        }
    }
}
