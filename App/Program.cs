using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration;

namespace App
{
    class Program
    {
        static void Main(string[] args)
        {
            string dataDirectory = Directory.GetCurrentDirectory() + "/../Data/Price/";
            string[] files = Directory.GetFiles(dataDirectory);

            foreach (var file in files)
            {
                if (!File.Exists(file))
                {
                    Console.WriteLine("File at {0} was not found.", file);
                    continue;
                }

                string CSVString = File.ReadAllText(file);
                using (CsvReader csv = new CsvReader(new StringReader(CSVString)))
                {
                    csv.Configuration.RegisterClassMap<PriceListCSVMap>();
                    csv.Configuration.Delimiter = ",";
                    csv.Configuration.HeaderValidated = null;
                    csv.Configuration.MissingFieldFound = null;
                    List<PriceList> prices = csv.GetRecords<PriceList>().ToList();
                }
            }
        }
    }

    public sealed partial class PriceListCSVMap : ClassMap<PriceList>
    {
        public PriceListCSVMap()
        {
            Map(m => m.Date).Name("Date");
            Map(m => m.Open).Name("Open");
            Map(m => m.Close).Name("Close");
            Map(m => m.AdjustedClose).Name("Adj Close");
            Map(m => m.High).Name("High");
            Map(m => m.Low).Name("Low");
        }
    }
}
