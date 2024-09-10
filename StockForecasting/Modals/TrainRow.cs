using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockForecasting.Modals
{
    public class TrainRow
    {
        public float Value { get; set; }
        public TrainRow(float value) => Value = value;
    }
}
