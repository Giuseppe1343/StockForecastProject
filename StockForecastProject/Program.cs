using StockForecasting;
using StockForecasting.Modals;
using System.Security.Principal;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace StockForecastProject
{
    internal class Program
    {
        static LoadingAnimation load = new();
        static void Main(string[] args)
        {
            //Subscribe to the MessageReceived event to get the log messages
            MessageOutput.MessageReceived += (sender, e) =>
            {
                switch (e.Type)
                {
                    case LogLevel.Info:
                        ConsoleOut.WriteInfo(e.Message);
                        break;
                    case LogLevel.Error:
                        ConsoleOut.WriteError(e.Message);
                        break;
                    case LogLevel.Warning:
                        ConsoleOut.WriteWarning(e.Message);
                        break;
                }
            };


            var dataContext = new DataContext();
            if (dataContext.IsConnectionValid)
            {
                load.Start();
                var rawData = dataContext.GetStocks();
                DataContext.PreprocessData(rawData);
                load.Stop();
                ConsoleOut.WriteInfo("Data loaded successfully");
                Console.Clear();
                do
                {
                    float IR = 0.0f;
                    float MAE = 0.0f;
                    double RMSE = 0.0;
                    float UIR = 0.0f;
                    float UMAE = 0.0f;
                    double URMSE = 0.0;

                    foreach (var stock in rawData)
                    {
                        //Train the data
                        var model = new StockForecastMLModel(stock);
                        //if (model.WindowSize < 2 || model.WindowSize >= model.SeriesLength) 
                        //    model.WindowSize = 2;
                        //model.SeriesLength = Convert.ToInt32(stock.Data.TrainCount / s);
                        //if (model.SeriesLength <= model.WindowSize)
                        //{
                        //    reset = true;
                        //    model.SeriesLength = model.WindowSize + 1;
                        //}
                        model.Train();

                        IR += model.Evaluate(out var mAE, out var rMSE);
                        UIR += model.UpdateAndEvaluate(out var uMAE, out var uRMSE);

                        //ConsoleOut.WriteInfo($"MAE: {mAE} RMSE: {rMSE}");
                        //ConsoleOut.WriteWarning($"UMAE: {uMAE} URMSE: {uRMSE}");
                        MAE += mAE;
                        RMSE += rMSE;
                        UMAE += uMAE;
                        URMSE += uRMSE;

                        //var deviation = model.Test();
                        //totalDeviation += deviation;
                    }
                    IR /= rawData.Count;
                    MAE /= rawData.Count;
                    RMSE /= rawData.Count;
                    UIR /= rawData.Count;
                    UMAE /= rawData.Count;
                    URMSE /= rawData.Count;
                    ConsoleOut.WriteError("-------------------");
                    ConsoleOut.WriteInfo($"MAE: {MAE} RMSE: {RMSE} IR: {IR}");
                    ConsoleOut.WriteWarning($"UMAE: {UMAE} URMSE: {URMSE} UIR: {UIR}");
                    //MAE /= rawData.Count;
                    //RMSE /= rawData.Count;
                    //if (minMAE > MAE && minRMSE > RMSE)
                    //{
                    //    ConsoleOut.WriteWarning($"Found C: {b}");
                    //    minMAE = MAE;
                    //    minRMSE = RMSE;
                    //}
                    //Console.WriteLine($" Total: {totalDeviation}");
                    //Console.WriteLine($"Average MAE: {MAE}");
                    //Console.WriteLine($"Average RMSE: {RMSE}");
                } while (true);
                //TODO : Implement the logic
            }

            ConsoleOut.ProgramEnd();
        }
    }
}
