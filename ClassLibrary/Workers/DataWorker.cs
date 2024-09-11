using Dapper;
using Microsoft.Data.SqlClient;
using StockForecasting.Modals;
using StockForecasting.Workers;
using static StockForecasting.Workers.WorkersSyncContext;

namespace StockForecasting.Worker
{
    public class DataWorker : BaseWorker
    {
        public DataWorker(List<StockViewModel> viewModels) 
        {
            _thread = new Thread(() => DataWorkerMain(new List<StockViewModel>(viewModels)));
            _thread.Start();
        }
        private void DataWorkerMain(List<StockViewModel> viewModels)
        {
            int currProgress = 0;
            try
            {
                using (var connection = new SqlConnection(GetConnectionString()))
                {
                    connection.Open();
                    foreach (var viewModel in viewModels)
                    {
                        if (token.IsCancellationRequested)
                            return;

                        if (!syncContext.ContainsKey(viewModel.Id))
                            AddStock(connection, viewModel.Id, viewModel.Name);

                        OnReport(++currProgress);

                        if (readData.WaitOne(0))
                        {
                            if (!syncContext.ContainsKey(invokedStockView.Id))
                                AddStock(connection, invokedStockView.Id, invokedStockView.Name);
                            preprocessData.Set();
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                MessageOutput.ErrorOutput(ex.ToString());
                CancelAll();
            }
            JobCompleted = true;
        }
        private static void AddStock(SqlConnection connection, in int id, in string name)
        {
            var stock = new Stock { Id = id, Name = name };
            stock.Transactions = connection.Query<StockTransaction>("SELECT tarih AS [TDate], SUM(COALESCE(miktar,0.0)) AS [TAmount]  FROM MergedTable WHERE tipi = 761 AND stokno = @id AND tarih >= '2023-01-01' GROUP BY tarih  ORDER BY tarih", new { id = stock.Id }).ToList();
            syncContext[stock.Id] = (stock, false, false);
        }
    }
}
