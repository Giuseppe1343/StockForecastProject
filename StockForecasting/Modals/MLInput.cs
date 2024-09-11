using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockForecasting.Modals
{
    public class MLInput
    {
        public float Value { get; set; }
        public MLInput(float value) => Value = value;
    }
}
