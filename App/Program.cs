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
            string appDirectory = Directory.GetCurrentDirectory();
            string filePath = appDirectory + "/../Data/Price/APU.csv";
            Console.WriteLine(filePath);
            if(!File.Exists(filePath))
            {
                Console.WriteLine("No file has been found.");
                return;
            }

            string CSVString = File.ReadAllText(filePath);
            CsvReader csv = new CsvReader(new StringReader(CSVString));
            csv.Configuration.RegisterClassMap<PriceListCSVMap>();
            csv.Configuration.Delimiter = ",";
            csv.Configuration.HeaderValidated = null;
            csv.Configuration.MissingFieldFound = null;
            List<PriceList> prices = csv.GetRecords<PriceList>().ToList();
            Console.WriteLine("Date has been imported.");

            foreach (var item in prices)
            {
                Console.WriteLine(item.Date);
                Console.WriteLine(item.High);
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
            Map(m => m.High).Name("High");
            Map(m => m.Low).Name("Low");
        }
    }
}
