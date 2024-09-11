using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockForecasting.Modals
{

    public class MLOutput
    {
        public float[] Results { get; set; }
        public float[] ConfidenceLower { get; set; }
        public float[] ConfidenceUpper { get; set; }


        public static implicit operator (float, float)(MLOutput results)
        {
            return (results.ConfidenceLower[0], results.ConfidenceUpper[0]);
        }
    }
}
