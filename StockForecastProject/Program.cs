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

                    //Train the data
                    foreach (var stock in data)
                    {
                        //Train the data
                        var model = new StockForecastModel(stock);
                        model.TrainAndPredict();
                        ConsoleOut.WriteInfo($"Stock {stock.DemandForecastingData.DailyData.SuccessPercentage} trained successfull");
                        ConsoleOut.WriteInfo($"Stock {stock.DemandForecastingData.WeeklyData.SuccessPercentage} trained successfull");
                        ConsoleOut.WriteInfo($"Stock {stock.DemandForecastingData.MonthlyData.SuccessPercentage} trained successfull");
                    }
                    Console.ReadLine();
                } while (true);
                //TODO : Implement the logic
            }

            ConsoleOut.ProgramEnd();
        }
    }
}
