// (c) 2020 Francesco Del Re <francesco.delre.87@gmail.com>
// This code is licensed under MIT license (see LICENSE.txt for details)
using System.Collections.Generic;

namespace SharpTracer.Services
{
    public interface IHistoryService
    {
        void SetHistory(string sourceId, List<TracerRequest> history);
        List<TracerRequest> History(string sourceId);
    }

    public class HistoryService : IHistoryService
    {
        private readonly Dictionary<string, List<TracerRequest>> _historyDictionary;

        public HistoryService()
        {
            _historyDictionary = new Dictionary<string, List<TracerRequest>>();
        }

        /// <summary>
        /// Copy the current history.
        /// </summary>
        /// <param name="sourceId"></param>
        /// <param name="history"></param>
        public void SetHistory(string sourceId, List<TracerRequest> history)
        {
            if (_historyDictionary.TryGetValue(sourceId, out _))
            {
                // update the history
                _historyDictionary[sourceId] = history;
            }
            else
            {
                // add the history
                _historyDictionary.Add(sourceId, history);
            }
        }

        /// <summary>
        /// Return the ordered history based on source id.
        /// </summary>
        /// <param name="sourceId"></param>
        /// <returns></returns>
        public List<TracerRequest> History(string sourceId)
        {
            return _historyDictionary[sourceId];
        }
    }
}
