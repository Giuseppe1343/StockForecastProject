using Dapper;
using Microsoft.Data.SqlClient;
using StockForecasting.Modals;
using static StockForecasting.Workers.WorkersSyncContext;

namespace StockForecasting.Worker
{
    internal class DataWorker : Progress<int>
    {
        private readonly Thread _thread;
        private readonly CancellationToken token = cts.Token;
        public bool JobCompleted { get; private set; }

        public DataWorker(List<StockViewModel> viewModels) 
        {
            _thread = new Thread(() => DataWorkerMain(viewModels));
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
                        if (IsNewStock(viewModel.Id))
                        {
                            AddStock(connection, viewModel.Id, viewModel.Name);
                        }
                        OnReport(++currProgress);

                        if (readData.WaitOne(0))
                        {
                            if (IsNewStock(invokedStockView.Id))
                                AddStock(connection, invokedStockView.Id, invokedStockView.Name);
                            preprocessData.Set();
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                ErrorOutput(ex.ToString());
            }
            JobCompleted = true;
        }
        private bool IsNewStock(int id)
        {
            return !syncContext.ContainsKey(id);
        }
        private static void AddStock(SqlConnection connection, in int id, in string name)
        {
            var stock = new Stock { Id = id, Name = name };
            stock.Transactions = connection.Query<TransactionRow>("SELECT tarih AS [TDate], SUM(COALESCE(miktar,0.0)) AS [TAmount]  FROM MergedTable WHERE tipi = 761 AND stokno = @id AND tarih >= '2023-01-01' GROUP BY tarih  ORDER BY tarih", new { id = stock.Id }).ToList();
            syncContext[stock.Id] = (stock, false, false);
        }
    }
}
