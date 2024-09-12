using ClassLibrary.Modals;
using StockForecasting.Modals;
using Syncfusion.Windows.Forms.Chart;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockForecasting
{
    public partial class FrmStockForecast : Form
    {
        public FrmStockForecast(Stock stock)
        {
            InitializeComponent();
            var stockForecasts = StockViewModelForecast.GetListFromStock(stock);
            lblDescription.Text = $"({stock.Id}) Numaralı {stock.Name} Stoğu için Tahmin Detayı";

            ChartSeries actSeries = new ChartSeries("Actual", ChartSeriesType.Line);
            ChartSeries foreSeries = new ChartSeries("Forecast", ChartSeriesType.Line);
            ChartSeries series = new ChartSeries("Bounds", ChartSeriesType.RangeArea);
            for (int i = 0; i < stockForecasts.Count; i++)
            {
                actSeries.Points.Add(i, stockForecasts[i].Actual);
                foreSeries.Points.Add(i, stockForecasts[i].Forecast);
                series.Points.Add(i, stockForecasts[i].LowerBound, stockForecasts[i].UpperBound);
            }
            chartControl1.Series.Add(actSeries);
            chartControl1.Series.Add(foreSeries);
            chartControl1.Series.Add(series);
            chartControl1.Palette = ChartColorPalette.Office2016;
        }
    }
}
