using StockForecasting;
using StockForecasting.Modals;

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
                load.Stop();
                ConsoleOut.WriteInfo("Data loaded successfully");

                do
                {
                    //Copy the master data to a new list
                    var data = new List<Stock>(rawData);

                    //Preprocess the all data
                    DataContext.PreprocessData(data);

                    int totalDeviation = 0;
                    //Train the data
                    var MAE = 0.0f;
                    var RMSE = 0.0f;
                    foreach (var stock in data)
                    {
                        //Train the data
                        var model = new StockForecastModel(stock);
                        model.WindowSize = 7;
                        model.SeriesLength = 30;
                        //model.Horizon = 1;

                        model.Train();

                        var tuple = model.Evaluate();
                        MAE += tuple.Item1;
                        RMSE += tuple.Item2;

                        //var deviation = model.Test();
                        //totalDeviation += deviation;
                    }
                    totalDeviation = totalDeviation / data.Count;
                    MAE /= data.Count;
                    RMSE /= data.Count;
                    //Console.WriteLine($" Total: {totalDeviation}");
                    Console.WriteLine($"Average MAE: {MAE}");
                    Console.WriteLine($"Average RMSE: {RMSE}");
                } while (false);
                //TODO : Implement the logic
            }

            ConsoleOut.ProgramEnd();
        }
    }
}
