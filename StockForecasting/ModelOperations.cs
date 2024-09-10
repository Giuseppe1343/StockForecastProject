using Microsoft.ML;
using Microsoft.ML.Transforms.TimeSeries;
using StockForecasting.Modals;

namespace StockForecasting
{
    public class StockForecastModel
    {
        private readonly Stock _stock;
        public StockForecastModel(Stock stock) => _stock = stock;

        public void TrainAndPredict()
        {
            TrainAndPredictInternal(TimeFrame.Daily);
            TrainAndPredictInternal(TimeFrame.Weekly);
            TrainAndPredictInternal(TimeFrame.Monthly);
        }
        private void TrainAndPredictInternal(in TimeFrame timeFrame)
        {
            var ml = new MLContext();

            var dataView = ml.Data.LoadFromEnumerable(PrepareData(timeFrame));

            var inputColumnName = nameof(TrainRow.Value);
            var outputColumnName = nameof(PredictedResult.Results);

            GetHyperParameters(timeFrame, out int wSize, out int sLength, out int tSize);

            var model = ml.Forecasting.ForecastBySsa(
                outputColumnName,
                inputColumnName,
                windowSize: wSize,
                seriesLength: sLength,
                trainSize: tSize,
                horizon: 1);

            var transformer = model.Fit(dataView);

            var forecastEngine = transformer.CreateTimeSeriesEngine<TrainRow,PredictedResult>(ml);

            var forecast = forecastEngine.Predict();

            SetPrediction(timeFrame, forecast.Results[0]);
        }
        private IEnumerable<TrainRow> PrepareData(in TimeFrame timeFrame)
        {
            return timeFrame switch
            {
                TimeFrame.Daily => _stock.TrainDaily,
                TimeFrame.Weekly => _stock.TrainWeekly,
                TimeFrame.Monthly => _stock.TrainMonthly,
                _ => throw new NotImplementedException(),
            };
        }
        private void GetHyperParameters(in TimeFrame timeFrame, out int wSize, out int sLength, out int tSize)
        {
            int count = 0;
            switch (timeFrame)
            {
                case TimeFrame.Daily:
                    count = _stock.TrainDaily.Count();
                    break;
                case TimeFrame.Weekly:
                    count = _stock.TrainWeekly.Count();
                    break;
                case TimeFrame.Monthly:
                    count = _stock.TrainMonthly.Count();
                    break;
            }
            wSize = count / 3;
            sLength = count / 2;
            tSize = count;
        }
        private void SetPrediction(in TimeFrame timeFrame, float prediction)
        {
            switch (timeFrame)
            {
                case TimeFrame.Daily:
                    _stock.PredictionDaily = prediction;
                    break;
                case TimeFrame.Weekly:
                    _stock.PredictionWeekly = prediction;
                    break;
                case TimeFrame.Monthly:
                    _stock.PredictionMonthly = prediction;
                    break;
            }
        }
    }
}
