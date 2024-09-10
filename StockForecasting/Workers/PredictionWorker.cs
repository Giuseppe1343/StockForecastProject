using StockForecasting.Modals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static StockForecasting.Workers.WorkersSyncContext;

namespace StockForecasting.Workers
{
    internal class PredictionWorker : BaseWorker
    {
        public PredictionWorker(int jobLength) : base(jobLength)
        { 
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

                    if (TrainAndPredict(sync.Value))
                        OnReport(++currProgress);

                    if (trainData.WaitOne(0))
                    {
                        if(TrainAndPredict(GetInvokedStock()))
                            OnReport(++currProgress);
                        predictData.Set();
                    }
                }
            }
            JobCompleted = true;
        }
        private static bool TrainAndPredict((Stock StockData, bool Preprocessed, bool Trained) data)
        {
            if (data.Trained) return false;

            var forecastModel = new StockForecastModel(data.StockData);
            forecastModel.TrainAndPredict();
            syncContext[data.StockData.Id] = (data.StockData, true, true);
            return true;
        }
    }
}
