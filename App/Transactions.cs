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
                    var buyDate = GetBuyDate(dividend.Key, priceData);
                    // var buyDate = dividend.Key;

                    if (buyDate != DateTime.MinValue && !this.Stocks[stockSymbol].ContainsKey(buyDate) && priceData.ContainsKey(buyDate))
                    {
                        var buyPrice = priceData[buyDate].Close;
                        this.Stocks[stockSymbol].Add(buyDate, new Transaction()
                        {
                            Buy = new Buy { Date = buyDate, Price = buyPrice },
                            Sell = new Sell()
                        });
                    }
                }
            }
        }

        private DateTime GetBuyDate(DateTime exDividendDate, SortedDictionary<DateTime, PriceData> priceData)
        {
            var dayBeforeExDividendDate = exDividendDate.AddDays(-1);

            if (priceData.ContainsKey(dayBeforeExDividendDate))
                return dayBeforeExDividendDate;

            var firstDateInPriceData = priceData.First().Key;
            while (!priceData.ContainsKey(dayBeforeExDividendDate))
            {
                if (firstDateInPriceData > dayBeforeExDividendDate)
                    return DateTime.MinValue;

                dayBeforeExDividendDate = dayBeforeExDividendDate.AddDays(-1);
            }

            return dayBeforeExDividendDate;
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

                    int index = 0;
                    while (index < priceData.Count)
                    {
                        var priceDataAtIndex = priceData.ElementAt(index);
                        var dateAtIndex = priceDataAtIndex.Key;
                        if (dateAtIndex <= buyDate)
                        {
                            index++;
                            continue;
                        }

                        var dailyPriceHigh = priceDataAtIndex.Value.High;

                        if (dailyPriceHigh > buyPrice)
                        {
                            this.Stocks[stockSymbol][buyDate].Sell.Date = dateAtIndex;
                            this.Stocks[stockSymbol][buyDate].Sell.Price = dailyPriceHigh;
                            break;
                        }

                        index++;
                    }
                }
            }
        }
    }
}