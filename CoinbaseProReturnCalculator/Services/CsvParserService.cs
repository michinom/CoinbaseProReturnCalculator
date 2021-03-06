﻿using System.Collections.Generic;
using System.Globalization;
using System.IO;
using CoinbaseProReturnCalculator.Models;
using CsvHelper;

namespace CoinbaseProReturnCalculator.Services
{
    public class CsvParserService
    {
        public List<CoinbaseCsvModel> ReadCsvFile(string fileName)
        {
            var reader = new StreamReader($"Data/{fileName}");
            var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            {
                var records = new List<CoinbaseCsvModel>();
                csv.Read();
                csv.ReadHeader();
                while (csv.Read())
                {
                    var record = new CoinbaseCsvModel
                    {
                        Portfolio = csv.GetField("portfolio"),
                        TradeId = csv.GetField<int>("trade id"),
                        Product = csv.GetField("product"),
                        Side = csv.GetField("side"),
                        CreatedAt = csv.GetField("created at"),
                        Size = csv.GetField<decimal>("size"),
                        SizeUnit = csv.GetField("size unit"),
                        Price = csv.GetField<decimal>("price"),
                        Fee = csv.GetField<decimal>("fee"),
                        Total = csv.GetField<decimal>("total"),
                        Pftu = csv.GetField("price/fee/total unit")
                    };
                    records.Add(record);
                }
                return records;
            }
        }
    }
}
