using StockForecasting.Modals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static StockForecasting.Workers.WorkersSyncContext;

namespace StockForecasting.Workers
{
    internal class PredictionWorker : Progress<int>
    {
        private readonly Thread _thread;
        private readonly CancellationToken token = cts.Token;
        private readonly int _jobLength;
        
        public bool JobCompleted { get; private set; }

        public PredictionWorker(int jobLength)
        {
            _jobLength = jobLength;
            _thread = new Thread(PredictionWorkerMain);
            _thread.Start();
        }

        private void PredictionWorkerMain()
        {
            int currProgress = 0;
            while (currProgress != _jobLength && !token.IsCancellationRequested)
            {
                foreach (var sync in syncContext.Where(x => x.Value.Preprocessed == true && x.Value.Trained == false))
                {
                    if (token.IsCancellationRequested)
                        return;
                    if (DUMMYTrainData(sync.Value))
                        OnReport(++currProgress);

                    if (trainData.WaitOne(0))
                    {
                        if(DUMMYTrainData(GetInvokedStock()))
                            OnReport(++currProgress);
                        predictData.Set();
                    }
                }
            }
            JobCompleted = true;
        }
        private bool DUMMYTrainData((Stock StockData, bool Preprocessed, bool Trained) data)
        {
            if (data.Trained) return false;
            syncContext[data.StockData.Id] = (data.StockData, true, true);
            //Thread.Sleep(1000);
            return true;
        }
        private (Stock StockData, bool Preprocessed, bool Trained) GetInvokedStock()
        {
            return syncContext[invokedStockView.Id];
        }
    }
}
