using System;

namespace App
{
    public class PriceList
    {
        public DateTime Date { get; set; }
        public decimal Open { get; set; }
        public decimal Close { get; set; }
        public decimal AdjustedClose { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
    }
}