using StockForecasting.Modals;
using static StockForecasting.Workers.WorkersSyncContext;

namespace StockForecasting.Workers
{
    public abstract class BaseWorker : Progress<int>
    {
        private static readonly CancellationTokenSource cts = new();
        public static void CancelAll() => cts.Cancel();


        protected Thread _thread;
        protected readonly CancellationToken token = cts.Token;
        protected readonly int _jobLength;
        public bool JobCompleted { get; protected set; }

        internal BaseWorker(int jobLength = 0) => _jobLength = jobLength;

        protected static (Stock StockData, bool Preprocessed, bool Trained) GetInvokedStock()
        {
            return syncContext[invokedStockView.Id];
        }

    }
}
