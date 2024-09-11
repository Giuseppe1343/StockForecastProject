using Microsoft.ML;
using Microsoft.ML.Transforms.TimeSeries;
using StockForecasting.Modals;

namespace StockForecasting
{
    public class StockForecastModel
    {
        private readonly MLData _data;

        private TimeSeriesPredictionEngine<MLInput, MLOutput> _forecastEngine;

        public int Horizon { get; set; } = 1;
        public float ConfidenceLevel { get; set; } = 0.95f;
        public int WindowSize { get; set; }
        public int SeriesLength { get; set; }
        public StockForecastModel(Stock stock)
        {
            _data = stock.Data;
            WindowSize = _data.TrainCount / 3;
            SeriesLength = _data.TrainCount / 2;
        }

        public void Train(float splitRatio = 0.8f)
        {
            _data.SetSplitPercentage(splitRatio);
            var ml = new MLContext();

            var dataView = ml.Data.LoadFromEnumerable(_data.Train);

            var inputColumnName = nameof(MLInput.Value);
            var outputColumnName = nameof(MLOutput.Results);

            var model = ml.Forecasting.ForecastBySsa(
                outputColumnName,
                inputColumnName,
                windowSize: WindowSize,
                seriesLength: SeriesLength,
                trainSize: _data.TrainCount,
                horizon: Horizon,
                confidenceLevel: ConfidenceLevel,
                confidenceLowerBoundColumn: nameof(MLOutput.ConfidenceLower),
                confidenceUpperBoundColumn: nameof(MLOutput.ConfidenceUpper)
                );

            var transformer = model.Fit(dataView);

            _forecastEngine = transformer.CreateTimeSeriesEngine<MLInput, MLOutput>(ml);
        }
        //public MLOutput Predict(TimeFrame timeFrame)
        //{
        //    switch (timeFrame)
        //    {
        //        case TimeFrame.Daily:
        //            return _forecastEngine.Predict(1);
        //        case TimeFrame.Weekly:
        //            return _forecastEngine.Predict(7);
        //            break;
        //        case TimeFrame.Monthly:
        //            return _forecastEngine.Predict(30);
        //            break;
        //    }
        //}

        public int Test()
        {
            int totalDeviation = 0;

            foreach (var item in _data.Test)
            {
                if (item.Value != 0)
                {
                    var res = _forecastEngine.Predict();
                    int predictedValue = Convert.ToInt32(res.Results[0]);
                    int actualValue = Convert.ToInt32(item.Value);

                    double tempDeviation = Math.Abs((double)(actualValue - predictedValue) / actualValue) * 100;

                    totalDeviation += Convert.ToInt32(tempDeviation);

                    _forecastEngine.Predict(item);
                }
            }

            int averageDeviation = totalDeviation / _data.TestCount;

            Console.WriteLine($"Ortalama Sapma: %{averageDeviation}");
            return averageDeviation;
        }

        public (float, float) Evaluate()
        {
            var forecast = _forecastEngine.Predict(_data.TestCount).Results;
            var actual = _data.Test.Select(x => x.Value).ToArray();
            var metrics = actual.Zip(forecast, (actualValue, forecastValue) => actualValue - forecastValue);

            var MAE = metrics.Average(error => Math.Abs(error));
            var RMSE = (float)Math.Sqrt(metrics.Average(error => Math.Pow(error, 2)));
            //MessageOutput.InfoOutput("MAE " + MAE.ToString());
            //MessageOutput.InfoOutput("RMSE " + RMSE.ToString());
            return (MAE, RMSE);
        }
    }


}
