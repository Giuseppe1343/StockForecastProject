using StockForecasting.Modals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static StockForecasting.Workers.WorkersSyncContext;

namespace StockForecasting.Workers
{
    internal class PreprocessWorker : Progress<int>
    {
        private readonly Thread _thread;
        private readonly CancellationToken token = cts.Token;
        private readonly int _jobLength;
        public bool JobCompleted { get; private set; }

        public PreprocessWorker(int jobLength)
        {
            _jobLength = jobLength;
            _thread = new Thread(PreprocessWorkerMain);
            _thread.Start();
        }
        private void PreprocessWorkerMain()
        {
            int currProgress = 0;
            while (currProgress != _jobLength && !token.IsCancellationRequested)
            {
                foreach (var sync in syncContext.Where(x => x.Value.Preprocessed == false))
                {
                    if (token.IsCancellationRequested)
                        return;
                    if (PreprocessData(sync.Value))
                        OnReport(++currProgress);

                    if (preprocessData.WaitOne(0))
                    {
                        if(PreprocessData(GetInvokedStock()))
                            OnReport(++currProgress);
                        trainData.Set();
                    }
                }
                Thread.Sleep(1000);
            }
            JobCompleted = true;
        }
        private bool PreprocessData((Stock StockData, bool Preprocessed, bool Trained) data)
        {
            if (data.Preprocessed) return false;
            DataContext.PreprocessData(data.StockData);
            syncContext[data.StockData.Id] = (data.StockData, true, false);
            return true;
        }
        private (Stock StockData, bool Preprocessed, bool Trained) GetInvokedStock()
        {
            return syncContext[invokedStockView.Id];
        }
    }
}