// (c) 2020 Francesco Del Re <francesco.delre.87@gmail.com>
// This code is licensed under MIT license (see LICENSE.txt for details)
using System.Collections.Generic;
using System.Linq;

namespace SharpTracer.Services
{
    public interface IHistoryService
    {
        void SetHistory(List<TracerRequest> history);
        List<TracerRequest> History(string sourceId);
    }

    public class HistoryService : IHistoryService
    {
        private List<TracerRequest> _history;

        /// <summary>
        /// Copy the current history.
        /// </summary>
        /// <param name="history"></param>
        public void SetHistory(List<TracerRequest> history)
        {
            _history = history?.Select(item => (TracerRequest)item.Clone()).ToList(); ;
        }

        /// <summary>
        /// Return the ordered history of a single node.
        /// </summary>
        /// <param name="sourceId"></param>
        /// <returns></returns>
        public List<TracerRequest> History(string sourceId)
        {
            return _history?.FindAll(x => x.ServiceSourceId == sourceId);
        }
    }
}
