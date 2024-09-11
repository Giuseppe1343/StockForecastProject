using StockForecasting.Modals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static StockForecasting.Workers.WorkersSyncContext;

namespace StockForecasting.Workers
{
    public class PreprocessWorker : BaseWorker
    {
        public PreprocessWorker(int jobLength) : base(jobLength)
        {
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
        private static bool PreprocessData((Stock StockData, bool Preprocessed, bool Trained) data)
        {
            if (data.Preprocessed) return false;

            DataContext.PreprocessData(data.StockData);
            syncContext[data.StockData.Id] = (data.StockData, true, false);
            return true;
        }
    }
}