using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockForecastProject.Helpers
{
    internal class SFPHelpers
    {
        public static string GetConnectionString()
        {
            string appFolder = AppDomain.CurrentDomain.BaseDirectory;
            if(!File.Exists(appFolder + "config.txt"))
            {
                File.WriteAllText(appFolder + "config.txt", "Server= ;Database= ;User Id= ;Password= ;");
            }
            
            return File.ReadAllText(appFolder + "config.txt"); ;
        }
    }
}
