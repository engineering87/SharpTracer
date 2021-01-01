// (c) 2020 Francesco Del Re <francesco.delre.87@gmail.com>
// This code is licensed under MIT license (see LICENSE.txt for details)
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace SharpTracer.Services
{
    public class OrderingService : BackgroundService
    {
        private readonly IQueueService _queueService;
        private readonly IHistoryService _historyService;
        private readonly ILogger<OrderingService> _logger;

        private readonly Dictionary<string, List<TracerRequest>> _orderedRequests;

        public OrderingService(ILogger<OrderingService> logger, IQueueService queueService, IHistoryService historyService)
        {
            _logger = logger;
            _queueService = queueService;
            _historyService = historyService;
            _orderedRequests = new Dictionary<string, List<TracerRequest>>();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (false == stoppingToken.IsCancellationRequested)
            {
                var request = await _queueService.DequeueTraceRequestAsync(stoppingToken);
                if (request != null)
                {
                    if (_orderedRequests.TryGetValue(request.ServiceSourceId, out var history))
                    {
                        // update and order the history of the node
                        history.Add(request);

                        history = history
                            .OrderBy(x => x.ServiceSourceId)
                            .ThenBy(x => x.Timestamp)
                            .ToList();

                        _orderedRequests[request.ServiceSourceId] = history;
                    }
                    else
                    {
                        // add the new node
                        _orderedRequests.Add(request.ServiceSourceId, new List<TracerRequest> { request });
                    }

                    // set the current history
                    _historyService.SetHistory(request.ServiceSourceId, _orderedRequests[request.ServiceSourceId]);
                }
            }
        }
    }
}
