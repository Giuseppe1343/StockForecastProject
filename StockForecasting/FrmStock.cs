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
            lblPreprocessIndicator.Text = $"Ýþlem: {e}/{_stocks.Count}";
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

            var stok = StockInvoker.Invoke(id, name);

            stok.GetAwaiter().OnCompleted(() =>
            {
                var stockName = stok.Result.Name;
                var dailyPrediction = stok.Result.DemandForecastingData.DailyData.PredictionCurrent;
                var dailyActual = stok.Result.DemandForecastingData.DailyData.ActualCurrent;
                var dailySuccess = stok.Result.DemandForecastingData.DailyData.SuccessPercentage;
                var weeklyPrediction = stok.Result.DemandForecastingData.WeeklyData.PredictionCurrent;
                var weeklyActual = stok.Result.DemandForecastingData.WeeklyData.ActualCurrent;
                var weeklySuccess = stok.Result.DemandForecastingData.WeeklyData.SuccessPercentage;
                var monthlyPrediction = stok.Result.DemandForecastingData.MonthlyData.PredictionCurrent;
                var monthlyActual = stok.Result.DemandForecastingData.MonthlyData.ActualCurrent;
                var monthlySuccess = stok.Result.DemandForecastingData.MonthlyData.SuccessPercentage;
                MessageBox.Show($"Stok Adý: {stockName}\n\nGünlük Tahmin: {dailyPrediction.ConfidenceLower[0]},{dailyPrediction.Results[0]},{dailyPrediction.ConfidenceUpper[0]}\nGünlük Gerçek: {dailyActual}\nGünlük Baþarý: %{dailySuccess}\n\nHaftalýk Tahmin: {weeklyPrediction.ConfidenceLower[0]},{weeklyPrediction.Results[0]},{weeklyPrediction.ConfidenceUpper[0]}\nHaftalýk Gerçek: {weeklyActual}\nHaftalýk Baþarý: %{weeklySuccess}\n\nAylýk Tahmin: {monthlyPrediction.ConfidenceLower[0]},{monthlyPrediction.Results[0]},{monthlyPrediction.ConfidenceUpper[0]}\nAylýk Gerçek: {monthlyActual}\nAylýk Baþarý: %{monthlySuccess}");
            });
        }
    }
}
