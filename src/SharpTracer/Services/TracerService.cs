// (c) 2020 Francesco Del Re <francesco.delre.87@gmail.com>
// This code is licensed under MIT license (see LICENSE.txt for details)
using Grpc.Core;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace SharpTracer.Services
{
    public class TracerService : Tracer.TracerBase
    {
        private readonly IQueueService _queueService;
        private readonly IHistoryService _historyService;
        private readonly ILogger<TracerService> _logger;

        public TracerService(ILogger<TracerService> logger, IQueueService queueService, IHistoryService historyService)
        {
            _logger = logger;
            _queueService = queueService;
            _historyService = historyService;
        }

        /// <summary>
        /// Insert a new trace.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task<TracerResponse> Trace(TracerRequest request, ServerCallContext context)
        {
            _queueService.QueueTraceRequest(request);

            _logger?.LogInformation($"Trace request received from {request.ServiceSourceId} to {request.ServiceDestinationId}");

            return Task.FromResult(new TracerResponse
            {
                Result = true
            });
        }

        /// <summary>
        /// Return the history related to the source.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task<HistoryResponse> History(HistoryRequest request, ServerCallContext context)
        {
            _logger?.LogInformation($"History request received for {request.SourceId}");

            return Task.FromResult(new HistoryResponse
            {
                History = { _historyService.History(request.SourceId) }
            });
        }
    }
}
