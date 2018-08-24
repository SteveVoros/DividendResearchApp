using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

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
            CreateSells();
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

                    if (priceData.ContainsKey(date))
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
            foreach (var stock in Stocks)
            {
                var stockSymbol = stock.Key;
                foreach (var transaction in stock.Value)
                {
                    var buyDate = transaction.Value.Buy.Date;
                    var buyPrice = transaction.Value.Buy.Price;
                    var priceData = _stocksData[stockSymbol].HistoricalPriceData;

                    var startIndex = priceData.Keys.Where(d => d == buyDate).First();

                    int index = 0;
                    while(index < priceData.Count)
                    {
                        var dailyPriceHigh = priceData.ElementAt(index).Value.High;

                        if(dailyPriceHigh > buyPrice)
                        {
                            var sellDate = priceData.ElementAt(index).Key;
                            this.Stocks[stockSymbol][buyDate].Sell = new Sell {
                                Date = sellDate,
                                Price = dailyPriceHigh
                            };
                        }

                        index++;
                    }
                }
            }
        }
    }
}