using System;
using System.Collections.Generic;

namespace App
{
    internal class Transactions
    {
        private readonly Dictionary<string, Stock> _stocksData;
        public Dictionary<string, SortedDictionary<DateTime, Transaction>> Stocks { get; private set; }

        public Transactions()
        {
        }

        public Transactions(Data data)
        {
            this._stocksData = data.Stocks;
            this.Stocks = new Dictionary<string, SortedDictionary<DateTime, Transaction>>();
            CreateBuys();
        }

        private void CreateBuys()
        {
            foreach (var stockData in _stocksData)
            {
                var stockSymbol = stockData.Key;

                Stocks.Add(stockSymbol, new SortedDictionary<DateTime, Transaction>());

                var dividendData = stockData.Value.HistoricalDividendData;
                var priceData = stockData.Value.HistoricalPriceData;

                foreach (var dividend in dividendData)
                {
                    var date = dividend.Key;

                    if (priceData.GetValueOrDefault(date) != null)
                    {
                        var buyPrice = priceData[date].Close;
                        this.Stocks[stockSymbol].Add(date, new Transaction()
                        {
                            Buy = new Buy { Date = date, Price = buyPrice },
                            Sell = new Sell()
                        });
                    }
                }
            }
        }

        private void CreateSells()
        {

        }
    }
}