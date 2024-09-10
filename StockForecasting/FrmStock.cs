using StockForecasting.Modals;
using StockForecasting.Worker;
using StockForecasting.Workers;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace StockForecasting
{
    public partial class FrmStock : Form
    {
        private DataContext _dataContext;
        private List<StockViewModel> _stocks;
        private DataWorker _dataWorker;
        private PreprocessWorker _preprocessWorker;
        private PredictionWorker _predictionWorker;
        public FrmStock()
        {
            InitializeComponent();
            _dataContext = new DataContext();
            if (!_dataContext.IsConnectionValid)
                Close();

            _stocks = _dataContext.GetGridSourceStocks();
            grdStocks.DataSource = _stocks;
            grdStocks.MultiSelect = false;
            grdStocks.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grdStocks.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            grdStocks.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

            barDataIndicator.Maximum = _stocks.Count;
            barPreprocessIndicator.Maximum = _stocks.Count;
            barPredictionIndicator.Maximum = _stocks.Count;

            _dataWorker = new DataWorker(_stocks);
            _dataWorker.ProgressChanged += DataWorker_ProgressChanged;
            _preprocessWorker = new PreprocessWorker(_stocks.Count);
            _preprocessWorker.ProgressChanged += PreprocessWorker_ProgressChanged;
            _predictionWorker = new PredictionWorker(_stocks.Count);
            _predictionWorker.ProgressChanged += PredictionWorker_ProgressChanged;

            FormClosing += (s, e) => BaseWorker.CancelAll();
        }
        private void PredictionWorker_ProgressChanged(object? sender, int e)
        {
            barPredictionIndicator.Value = e;
            lblPredictionIndicator.Text = $"Tahmin: {e}/{_stocks.Count}";
        }
        private void PreprocessWorker_ProgressChanged(object? sender, int e)
        {
            barPreprocessIndicator.Value = e;
            lblPreprocessIndicator.Text = $"��lem: {e}/{_stocks.Count}";
        }
        private void DataWorker_ProgressChanged(object? sender, int e)
        {
            barDataIndicator.Value = e;
            lblDataIndicator.Text = $"Veri: {e}/{_stocks.Count}";
        }

        private void grdStocks_CellMouseDoubleClickAsync(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex == -1)
                return;

            var id = (int)grdStocks.Rows[e.RowIndex].Cells[0].Value;
            var name = (string)grdStocks.Rows[e.RowIndex].Cells[1].Value;

            var stok = InvokeStock(id, name);

            stok.GetAwaiter().OnCompleted(() => InfoOutput($"Stok: {stok.Result.Name}, Veri Say�s�: {stok.Result.Transactions.Count}, {Environment.NewLine}, G�nl�k Tahmin: {stok.Result.PredictionDaily}, G�nl�k Ger�ek: {stok.Result.ActualDaily}, {Environment.NewLine}, Haftal�k Tahmin: {stok.Result.PredictionWeekly}, Haftal�k Ger�ek: {stok.Result.ActualWeekly}, {Environment.NewLine}, Ayl�k Tahmin: {stok.Result.PredictionMonthly}, Ayl�k Ger�ek: {stok.Result.ActualMonthly}"));
        }
        private async Task<Stock> InvokeStock(int id,string name)
        {
             await Task.Run(() => WorkersSyncContext.InvokeSyncContext(id, name));
            return WorkersSyncContext.syncContext[id].StockData;
        }
    }
}
