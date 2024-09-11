namespace StockForecasting.Modals
{

    public class MLOutput
    {
        public float[] Results { get; set; }
        public float[] ConfidenceLower { get; set; }
        public float[] ConfidenceUpper { get; set; }
    }
}
