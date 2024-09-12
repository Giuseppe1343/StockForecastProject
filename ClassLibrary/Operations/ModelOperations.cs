using Microsoft.ML;
using Microsoft.ML.Transforms.TimeSeries;
using StockForecasting.Modals;

namespace StockForecasting
{
    public class StockForecastMLModel
    {
        private readonly MLData _data;

        private TimeSeriesPredictionEngine<MLInput, MLOutput> _forecastEngine;

        public int Horizon { get; set; } = 1;
        public float ConfidenceLevel { get; set; } = 0.80f;
        public int WindowSize { get; set; } = 7;

        public StockForecastMLModel(Stock stock) => _data = stock.Data;

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
                seriesLength: _data.TrainCount,
                trainSize: _data.TrainCount,
                horizon: Horizon,
                confidenceLevel: ConfidenceLevel,
                confidenceLowerBoundColumn: nameof(MLOutput.ConfidenceLower),
                confidenceUpperBoundColumn: nameof(MLOutput.ConfidenceUpper)
                );

            var transformer = model.Fit(dataView);

            _forecastEngine = transformer.CreateTimeSeriesEngine<MLInput, MLOutput>(ml);
        }

        public void Predict(bool feedback = true)
        {
            if (feedback)
            {
                _data.Predictions = new MLOutput() { Results = new float[_data.TestCount], ConfidenceLower = new float[_data.TestCount], ConfidenceUpper = new float[_data.TestCount] };
                int i = 0;
                foreach (var actual in _data.Test)
                {
                    var forecast = _forecastEngine.Predict();
                    _data.Predictions.Results[i] = forecast.Results[0];
                    _data.Predictions.ConfidenceLower[i] = forecast.ConfidenceLower[0];
                    _data.Predictions.ConfidenceUpper[i++] = forecast.ConfidenceUpper[0];
                    _forecastEngine.Predict(actual);
                }
            }
            else
            {
                _data.Predictions = _forecastEngine.Predict(_data.TestCount);
            }
        }

        public float UpdateAndEvaluate(out float MAE, out double RMSE)
        {
            int insideInterval = 0;
            MAE = 0;
            RMSE = 0;
            foreach (var item in _data.Test)
            {
                var res = _forecastEngine.Predict();
                var actualValue = item.Value;
                if (res.ConfidenceLower[0] <= actualValue && actualValue <= res.ConfidenceUpper[0])
                    insideInterval++;
                var predictedValue = res.Results[0];

                MAE += Math.Abs(actualValue - predictedValue);
                RMSE += Math.Pow(actualValue - predictedValue, 2);
                _forecastEngine.Predict(item);
            }
            MAE /= _data.TestCount;
            RMSE = Math.Sqrt(RMSE / _data.TestCount);

            return (float)insideInterval / _data.TestCount;
        }

        public float Evaluate(out float MAE, out double RMSE)
        {
            var forecast = _forecastEngine.Predict(_data.TestCount);
            var actual = _data.Test.Select(x => x.Value).ToArray();

            int insideInterval = actual.Count(actual =>
                forecast.ConfidenceLower.Zip(forecast.ConfidenceUpper, (lower, upper) => (lower, upper))
                    .Any(bounds => actual >= bounds.lower && actual <= bounds.upper));

            var metrics = actual.Zip(forecast.Results, (actualValue, forecastValue) => actualValue - forecastValue);

            MAE = metrics.Average(error => Math.Abs(error));
            RMSE = Math.Sqrt(metrics.Average(error => Math.Pow(error, 2)));

            return (float)insideInterval / _data.TestCount;
        }
    }
}
