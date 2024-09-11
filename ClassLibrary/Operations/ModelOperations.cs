using Microsoft.ML;
using Microsoft.ML.Transforms.TimeSeries;
using StockForecasting.Modals;

namespace StockForecasting
{
    public class StockForecastModel
    {
        private readonly MLData _data;

        private TimeSeriesPredictionEngine<MLInput, MLOutput> _forecastEngine;

        private int Horizon { get; set; } = 1;
        public float ConfidenceLevel { get; set; } = 0.95f;
        public int WindowSize { get; set; }
        public int SeriesLength { get; set; }
        public StockForecastModel(Stock stock)
        {
            _data = stock.Data;
            WindowSize = _data.TrainCount / 3;
            SeriesLength = _data.TrainCount / 2;
        }

        public void Train()
        {
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

            _forecastEngine = transformer.CreateTimeSeriesEngine<MLInput,MLOutput>(ml);
        }
        public MLOutput Predict(MLInput input)
        {
            return _forecastEngine.Predict(input);
        }

    }
}
