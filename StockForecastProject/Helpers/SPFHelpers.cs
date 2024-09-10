using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockForecastProject.Helpers
{
    internal class SPFHelpers
    {
        public static string GetConnectionString()
        {
            string configPath = AppDomain.CurrentDomain.BaseDirectory + "config.txt";
            if(!File.Exists(configPath))
                File.WriteAllText(configPath, "Server= ;Database= ;User Id= ;Password= ;Trust Server Certificate=True;");
            return File.ReadAllText(configPath); ;
        }
    }
}
