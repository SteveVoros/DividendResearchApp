using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration;

namespace App
{
    public class Data
    {
        public List<Stock> Stocks { get; private set; }
        private readonly string _workingDirectory;
        private readonly string _priceDataDirectory;
        private readonly string _dividendDataDirectory;

        public Data()
        {
            _workingDirectory = Directory.GetCurrentDirectory();
            _priceDataDirectory = _workingDirectory + "/../Data/Price/";
            _dividendDataDirectory = _workingDirectory + "/../Data/Dividend/";
        }

        public void Import()
        {
            ImportPriceData();
            ImportDividendData();
        }

        private void ImportPriceData()
        {
            string[] files = Directory.GetFiles(_priceDataDirectory);

            foreach (var file in files)
            {
                List<PriceData> stockPrices = GetStockPriceData(file);
            }
        }

        private List<PriceData> GetStockPriceData(string filePath)
        {
            if (!File.Exists(filePath))
            {
                Console.Error.WriteLine("File was not found at location: {0}", filePath);
                return null;
            }

            string CSVString = File.ReadAllText(filePath);

            List<PriceData> prices;
            using (CsvReader csv = new CsvReader(new StringReader(CSVString)))
            {
                csv.Configuration.RegisterClassMap<PriceListCSVMap>();
                csv.Configuration.Delimiter = ",";
                csv.Configuration.HeaderValidated = null;
                csv.Configuration.MissingFieldFound = null;
                prices = csv.GetRecords<PriceData>().ToList();
            }

            return prices;
        }

        private List<DividendData> GetStockDividendData(string filePath)
        {
            if (!File.Exists(filePath))
            {
                Console.Error.WriteLine("File was not found at location: {0}", filePath);
                return null;
            }

            string CSVString = File.ReadAllText(filePath);

            List<DividendData> dividends;
            using (CsvReader csv = new CsvReader(new StringReader(CSVString)))
            {
                csv.Configuration.RegisterClassMap<DividendListCSVMap>();
                csv.Configuration.Delimiter = ",";
                csv.Configuration.HeaderValidated = null;
                csv.Configuration.MissingFieldFound = null;
                dividends = csv.GetRecords<DividendData>().ToList();
            }

            return dividends;
        }

        private void ImportDividendData()
        {

        }
    }

    public sealed partial class PriceListCSVMap : ClassMap<PriceData>
    {
        public PriceListCSVMap()
        {
            Map(m => m.Date).Name("Date");
            Map(m => m.Open).Name("Open");
            Map(m => m.Close).Name("Close");
            Map(m => m.AdjustedClose).Name("Adj Close");
            Map(m => m.High).Name("High");
            Map(m => m.Low).Name("Low");
            Map(m => m.Volume).Name("Volume");
        }
    }

    public sealed partial class DividendListCSVMap : ClassMap<DividendData>
    {
        public DividendListCSVMap()
        {
            Map(m => m.Date).Name("Date");
            Map(m => m.Dividend).Name("Dividends");
        }
    }
}