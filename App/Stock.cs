using System;
using System.Collections.Generic;

namespace App
{
    public class Stock
    {
        public SortedDictionary<DateTime, Data> HistoricalPriceData { get; set; }
        public string Name { get; set; }
    }
}