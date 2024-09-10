using StockForecasting.Modals;
using StockForecasting.Worker;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockForecasting.Workers
{
    internal static class WorkersSyncContext
    {
        internal static readonly ConcurrentDictionary<int, (Stock StockData, bool Preprocessed, bool Trained)> syncContext = new();

        internal static (int Id,string Name) invokedStockView; 

        internal static readonly AutoResetEvent readData = new(false);
        internal static readonly AutoResetEvent preprocessData = new(false);
        internal static readonly AutoResetEvent trainData = new(false);
        internal static readonly AutoResetEvent predictData = new(false);

        internal static void InvokeSyncContext(int id, string name)
        {
            invokedStockView = (id, name);

            if (!syncContext.ContainsKey(id))
                readData.Set();
            else if (syncContext[id].Preprocessed == false)
                preprocessData.Set();
            else if (syncContext[id].Trained == false)
                trainData.Set();
            else
                return;

            predictData.WaitOne();
        }
    }
}
