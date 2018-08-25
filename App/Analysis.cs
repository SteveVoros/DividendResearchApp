using System;
using System.Collections.Generic;
using System.Linq;

namespace App
{
    internal class Analysis
    {
        public double AverageNumberOfDays { get; private set; }
        public double StandardDeviationOfDays { get; private set; }
        public int NumberOfTransactions { get; private set; }
        public int SumOfDays { get; private set; }
        public int NumberOfTransactionsOverLimit { get; private set; }
        public int NumberOfNoSells { get; private set; }
        public List<int> DaysList { get; private set; }
        public int MaxDaysLimit { get; set; }
        private Transactions _transactions;

        public Analysis(Transactions transactions, int MaxDaysLimit)
        {
            this._transactions = transactions;
            this.MaxDaysLimit = MaxDaysLimit;
        }

        public void Calculate()
        {
            CalculateAverageNumberofDays();
            CalculateStandardDeviationOfDays();
        }

        public void PrintResults()
        {
            Console.WriteLine("Number of transactions: {0}", NumberOfTransactions);
            Console.WriteLine("Number of transactions over limit: {0}", NumberOfTransactionsOverLimit);
            Console.WriteLine("Number of no sells: {0}", NumberOfNoSells);
            Console.WriteLine("Average number of days: {0:N2}", AverageNumberOfDays);
            Console.WriteLine("Standard deviation of days: {0:N2}", StandardDeviationOfDays);
            Console.WriteLine("List of Days:\r");
            DaysList.ForEach(d => Console.Write("{0} ", d));
            Console.WriteLine("\n\r");

        }

        private void CalculateAverageNumberofDays()
        {
            SumOfDays = 0;
            NumberOfTransactions = 0;
            NumberOfTransactionsOverLimit = 0;
            NumberOfNoSells = 0;
            DaysList = new List<int>();
            foreach (var stock in _transactions.Stocks)
            {
                foreach (var transaction in stock.Value)
                {
                    var buydate = transaction.Value.Buy.Date;
                    var sellDate = transaction.Value.Sell.Date;
                    if(sellDate == DateTime.MinValue)
                    {
                        NumberOfNoSells++;
                        continue;
                    }

                    var dateDifference = sellDate - buydate;
                    if(dateDifference.Days > MaxDaysLimit)
                    {
                        NumberOfTransactionsOverLimit++;
                        continue;
                    }

                    DaysList.Add(dateDifference.Days);
                    SumOfDays += dateDifference.Days;
                    NumberOfTransactions++;
                }
            }

            AverageNumberOfDays = (double) SumOfDays / NumberOfTransactions;
        }

        private void CalculateStandardDeviationOfDays()
        {   
            double avg = DaysList.Average();   
            double sum = DaysList.Sum(d => Math.Pow(d - avg, 2));  
            StandardDeviationOfDays = Math.Sqrt((sum) / (DaysList.Count()-1)); 
        }
    }
}