using Microsoft.ML;
using Microsoft.ML.Transforms.TimeSeries;
using StockForecasting.Modals;

namespace StockForecasting
{
    public class StockForecastModel
    {
        private readonly StockDemandForecastingData _data;
        private float _cLevel = 0.95f;
        private int _wSizeDivisor;
        private int _sLengthDivisor;
        public StockForecastModel(Stock stock, int wSizeDivisor = 3, int sLengthDivisor = 1)
        {
            _wSizeDivisor = wSizeDivisor;
            _sLengthDivisor = sLengthDivisor;
            _data = stock.DemandForecastingData;
        }

        public void TrainAndPredict()
        {
            TrainAndPredictInternal(TimeFrame.Daily);
            TrainAndPredictInternal(TimeFrame.Weekly);
            TrainAndPredictInternal(TimeFrame.Monthly);
        }
        private void TrainAndPredictInternal(in TimeFrame timeFrame)
        {
            var mlData = GetMLData(timeFrame);

            var ml = new MLContext();

            var dataView = ml.Data.LoadFromEnumerable(mlData.Train);

            var inputColumnName = nameof(MLInput.Value);
            var outputColumnName = nameof(MLOutput.Results);

            GetHyperParameters(mlData.TrainCount,out int wSize, out int sLength);

            var model = ml.Forecasting.ForecastBySsa(
                outputColumnName,
                inputColumnName,
                windowSize: wSize,
                seriesLength: sLength,
                trainSize: mlData.TrainCount,
                horizon: 1,
                confidenceLevel: _cLevel,
                confidenceLowerBoundColumn: nameof(MLOutput.ConfidenceLower),
                confidenceUpperBoundColumn: nameof(MLOutput.ConfidenceUpper)
                );

            var transformer = model.Fit(dataView);

            var forecastEngine = transformer.CreateTimeSeriesEngine<MLInput,MLOutput>(ml);

            int trueCount = 0;
            int falseCount = 0;

            MLOutput forecast = null;
            foreach (var item in mlData.Test)
            {
                forecast = forecastEngine.Predict();
                if(item.Value < forecast.ConfidenceUpper[0] && item.Value > forecast.ConfidenceLower[0])
                    trueCount++;
                else
                    falseCount++;

                forecastEngine.Predict(item);

            }
            mlData.SuccessPercentage = (float)trueCount / (trueCount + falseCount) * 100;
            mlData.PredictionCurrent = forecast;
        }
        private MLData GetMLData(in TimeFrame timeFrame)
        {
            return timeFrame switch
            {
                TimeFrame.Daily => _data.DailyData,
                TimeFrame.Weekly => _data.WeeklyData,
                TimeFrame.Monthly => _data.MonthlyData,
                _ => throw new NotImplementedException(),
            };
        }
        private void GetHyperParameters(in int trainSize, out int wSize, out int sLength)
        {
            wSize = trainSize / _wSizeDivisor;
            if (wSize < 2) wSize = 2;
            sLength = trainSize / _sLengthDivisor;
        }
    }
}
