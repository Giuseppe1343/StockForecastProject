namespace StockForecasting.Modals
{
    public class Stock
    {
        //Model Bound
        public int Id { get; set; }
        public string Name { get; set; }
        public List<StockTransaction> Transactions { get; set; }
        
        //Calculated
        public StockDemandForecastingData DemandForecastingData { get; set; }

    }
}
