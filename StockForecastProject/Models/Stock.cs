using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace StockForecastProject.Models
{
    public class Stock
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<TRows> Transactions { get; set; }
        public double Prediction { get; set; }
    }
}
