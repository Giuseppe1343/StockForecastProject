using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace StockForecasting.Modals
{
    public class StockDemandForecastingData
    {
        public MLData DailyData { get; set; }
        public MLData WeeklyData { get; set; }
        public MLData MonthlyData { get; set; }
    }
}
