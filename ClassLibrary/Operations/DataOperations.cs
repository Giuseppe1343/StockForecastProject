using Dapper;
using Microsoft.Data.SqlClient;
using StockForecasting.Modals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace StockForecasting
{
    public class DataContext
    {
        //Read the connection string from the config.txt file
        private string _connectionString = GetConnectionString();
        public bool IsConnectionValid { get; }
        public DataContext()
        {
            //Check if the connection string is valid
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    if (connection.State == System.Data.ConnectionState.Open)
                        IsConnectionValid = true;
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                MessageOutput.ErrorOutput(ex.ToString());
            }

            if (IsConnectionValid)
                MessageOutput.InfoOutput("Veritabanı bağlantısı başarılı.");
            else
                MessageOutput.WarningOutput("Veritabanı bağlantısı başarısız. Lütfen config.txt dosyasını kontrol edin.");
        }

        public Stock? GetStock(int id)
        {
            Stock? stock = null;
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    stock = connection.QueryFirstOrDefault<Stock>("SELECT TOP 1 stokno AS [Id], aciklama AS [Name] FROM MergedTable WHERE stokno = @id;", new { id });
                    if (stock is not null)
                    {
                        stock.Transactions = connection.Query<StockTransaction>("SELECT tarih AS [TDate], SUM(COALESCE(miktar,0.0)) AS [TAmount]  FROM model_data.dbo.MergedTable WHERE tipi = 761 AND stokno = @id AND tarih >= '2023-01-01' GROUP BY tarih  ORDER BY tarih", new { id }).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageOutput.ErrorOutput(ex.ToString());
            }
            return stock;
        }
        public List<StockViewModel> GetGridSourceStocks()
        {
            var stocks = new List<StockViewModel>();
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    stocks = connection.Query<StockViewModel>(@"
                        SELECT stokno AS [Id] ,(SELECT TOP 1 aciklama FROM MergedTable x WHERE x.stokno=R.stokno) AS [Name]
                        FROM ( 
	                        SELECT stokno, COUNT(Tb.TAmount) AS [Count]
	                        FROM (
		                        SELECT stokno, tarih, COUNT(miktar) AS [TAmount]
		                        FROM model_data.dbo.MergedTable
		                        WHERE tipi = 761 AND tarih >= '2023-01-01' AND stokno IS NOT NULL
		                        GROUP BY tarih,stokno
	                        ) Tb
	                        GROUP BY stokno
                        ) R
                        WHERE [Count] > 500
                    ").ToList();
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                MessageOutput.ErrorOutput(ex.ToString());
            }
            return stocks;
        }
        public List<Stock> GetStocks()
        {
            var stocks = new List<Stock>();
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    var validStocksId = connection.Query<int>(@"
                        SELECT stokno AS [Id] 
                        FROM ( 
	                        SELECT stokno, COUNT(Tb.TAmount) AS [Count]
	                        FROM (
		                        SELECT stokno, tarih, COUNT(miktar) AS [TAmount]
		                        FROM model_data.dbo.MergedTable
		                        WHERE tipi = 761 AND tarih >= '2023-01-01' AND stokno IS NOT NULL
		                        GROUP BY tarih,stokno
	                        ) Tb
	                        GROUP BY stokno
                        ) R
                        WHERE [Count] > 500
                    ");
                    foreach (var id in validStocksId)
                    {
                        var stock = connection.QueryFirstOrDefault<Stock>("SELECT TOP 1 stokno AS [Id], aciklama AS [Name] FROM MergedTable WHERE stokno = @id;", new { id });
                        if (stock is not null)
                        {
                            stock.Transactions = connection.Query<StockTransaction>("SELECT tarih AS [TDate], SUM(COALESCE(miktar,0.0)) AS [TAmount]  FROM MergedTable WHERE tipi = 761 AND stokno = @id AND tarih >= '2023-01-01' GROUP BY tarih  ORDER BY tarih", new { id }).ToList();
                            stocks.Add(stock);
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                MessageOutput.ErrorOutput(ex.ToString());
            }
            return stocks;
        }

        public static void PreprocessData(List<Stock> stocks)
        {
            foreach (var stock in stocks)
            {
                PreprocessData(stock);
            }
        }

        public static void PreprocessData(Stock stock)
        {
            var transactions = stock.Transactions;
            for (int i = transactions.Count  - 1; i >= 1; i--)
            {
                if (transactions[i].TDate.Date.AddDays(-1) != transactions[i - 1].TDate.Date)
                {
                    transactions.Insert(i, new StockTransaction
                    {
                        TDate = transactions[i].TDate.Date.AddDays(-1),
                        TAmount = 0
                    });
                    i++;
                }
            }
            var dailyData = new List<MLInput>();
            var weeklyData = new List<MLInput>();
            var monthlyData = new List<MLInput>();

            var nextWeek = transactions[0].TDate.Date.AddDays(7);
            var nextMonth = transactions[0].TDate.Date.AddMonths(1);

            double weeklySum = 0;
            double monthlySum = 0;

            foreach (var item in transactions)
            {
                dailyData.Add(new MLInput((float)item.TAmount));

                if (item.TDate.Date >= nextWeek)
                {
                    weeklyData.Add(new MLInput((float)weeklySum));
                    nextWeek = item.TDate.Date.AddDays(7);
                    weeklySum = 0;
                }
                weeklySum += item.TAmount;

                if (item.TDate.Date >= nextMonth)
                {
                    monthlyData.Add(new MLInput((float)monthlySum));
                    nextMonth = item.TDate.Date.AddMonths(1);
                    monthlySum = 0;
                }
                monthlySum += item.TAmount;
            }

            stock.DemandForecastingData = new StockDemandForecastingData
            {
                DailyData = new MLData(dailyData),
                WeeklyData = new MLData(weeklyData),
                MonthlyData = new MLData(monthlyData)
            };
        }
    }
}
