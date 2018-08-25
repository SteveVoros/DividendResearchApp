using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace App
{
    class Program
    {
        static void Main(string[] args)
        {
            var data = new Data();
            var transactions = new Transactions(data);
            var historicalAnalysis = new Analysis(transactions, 100);
            historicalAnalysis.Calculate();
            historicalAnalysis.PrintResults();
        }
    }
}
