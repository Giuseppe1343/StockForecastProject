using StockForecasting.Modals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Modals
{
    public class StockViewModelForecast
    {
        public float Actual { get; set; }
        public float Forecast { get; set; }
        public float LowerBound { get; set; }
        public float UpperBound { get; set; }

        public static List<StockViewModelForecast> GetListFromStock(Stock stock)
        {
            var forecasts = new List<StockViewModelForecast>();
            int i = 0;
            foreach (var actual in stock.Data.Test)
            {
                forecasts.Add(new StockViewModelForecast
                {
                    Actual = actual.Value,
                    Forecast = stock.Data.Predictions.Results[i],
                    LowerBound = stock.Data.Predictions.ConfidenceLower[i],
                    UpperBound = stock.Data.Predictions.ConfidenceUpper[i++]
                });
            }
            return forecasts;
        }
    }
}
