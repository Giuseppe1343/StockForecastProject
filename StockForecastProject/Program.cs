using Stok_Tahmin_Modeli.Helpers;

namespace StockForecastProject
{
    internal class Program
    {
        static LoadingAnimation _load = new();

        public static MultiLineColoredText output = new(); 
        static void Main(string[] args)
        {
            _load.Start();
            var dataContext = new DataContext();
            if (dataContext.IsConnectionValid)
            {
                //TODO : Implement the logic
            }
            _load.Stop();
            output.WriteAllLines();
            ConsoleOut.ProgramEnd();
        }
    }
}
