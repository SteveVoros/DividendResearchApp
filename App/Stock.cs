using System;
using System.Collections.Generic;

namespace App
{
    public class Stock
    {
        public SortedDictionary<DateTime, PriceData> HistoricalPriceData { get; set; }
        public SortedDictionary<DateTime, DividendData> HistoricalDividendData { get; set; }
        public string Symbol { get; set; }
    }
}