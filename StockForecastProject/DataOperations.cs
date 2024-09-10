using Dapper;
using Microsoft.Data.SqlClient;
using StockForecastProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockForecastProject
{
    public class DataContext
    {
        //Read the connection string from the config.txt file
        private string _connectionString = SPFHelpers.GetConnectionString();
        public bool IsConnectionValid { get; }

        public DataContext(string? connectionString = null)
        {
            if(!string.IsNullOrWhiteSpace(connectionString))
                _connectionString = connectionString;
            
            Program.output.AddText("Veritabanı bağlantısı kontrol ediliyor...");
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
                Program.output.AddError(ex.ToString());
            }

            if (IsConnectionValid)
                Program.output.AddInfo("Veritabanı bağlantısı başarılı.");
            else
                Program.output.AddWarning("Veritabanı bağlantısı başarısız. Lütfen config.txt dosyasını kontrol edin.");
        }

        public Stock? GetStock(int id)
        {
            Stock? stock = null;
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    stock = connection.QueryFirstOrDefault<Stock>("SELECT TOP 1 stokno AS [Id],aciklama AS [Name] FROM MergedTable WHERE stokno = @id;", new { id });
                    if (stock is not null)
                    {
                        stock.Transactions = connection.Query<TRows>("SELECT tarih AS [TDate], SUM(COALESCE(miktar,0.0)) AS [TAmount]  FROM model_data.dbo.MergedTable WHERE tipi = 761 AND stokno = @id AND tarih >= '2023-01-01' GROUP BY tarih  ORDER BY tarih", new { id }).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                Program.output.AddError(ex.ToString());
            }
            return stock;
        }

        public static List<Stock> GetStocks()
        {
            using (var connection = new SqlConnection(/*_connectionString*/))
            {
                connection.Open();
                var stocks = connection.Query<Stock>("SELECT * FROM Stocks").ToList();
                foreach (var stock in stocks)
                {
                    stock.Transactions = connection.Query<TRows>("SELECT * FROM MergedTable WHERE StockId = @Id", new { stock.Id }).ToList();
                }
                return stocks;
            }
        }
    }
}
