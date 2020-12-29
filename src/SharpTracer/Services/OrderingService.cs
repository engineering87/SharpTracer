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
        private List<TracerRequest> _orderedRequests;

        public OrderingService(ILogger<OrderingService> logger, IQueueService queueService, IHistoryService historyService)
        {
            _logger = logger;
            _queueService = queueService;
            _historyService = historyService;
            _orderedRequests = new List<TracerRequest>();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (false == stoppingToken.IsCancellationRequested)
            {
                var request = await _queueService.DequeueTraceRequestAsync(stoppingToken);
                if (request != null)
                {
                    // add the request to the list
                    _orderedRequests.Add(request);
                    // order the current list
                    OrderRequests();
                    // set the current history
                    _historyService.SetHistory(_orderedRequests);
                }
            }
        }

        /// <summary>
        /// Order the TracerRequest by source node and local timestamp
        /// </summary>
        private void OrderRequests()
        {
            _orderedRequests = _orderedRequests
                .OrderBy(x => x.ServiceSourceId)
                .ThenBy(x => x.Timestamp)
                .ToList();
        }
    }
}
