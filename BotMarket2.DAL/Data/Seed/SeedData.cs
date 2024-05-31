using BotMarket2.Common.Models;
using BotMarket2.Data;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using Microsoft.AspNetCore.Hosting;
using System.Globalization;
using System.Reflection;
using System.Text;

namespace BotMarket2.DAL.Data.Seed
{
    public sealed class HistoricalStockDataMap : ClassMap<HistoricalStockData>
    {
        public HistoricalStockDataMap()
        {
            Map(m => m.Date).Name("Date");
            Map(m => m.CloseLast).Name("Close/Last");
            Map(m => m.Volume).Name("Volume");
            Map(m => m.Open).Name("Open");
            Map(m => m.High).Name("High");
            Map(m => m.Low).Name("Low");
        }
    }
    public static class DatabaseInitializer
    {
        public static void SeedData(ApplicationDbContext context)
        {
            if (!context.HistoricalStockData.Any())
            {
                using Stream? stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("BotMarket2.DAL.Data.Seed.Raw.MSFT5YR.csv");
                if(stream != null)
                {
                    using (var reader = new StreamReader(stream))
                    using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
                    {
                        HasHeaderRecord = true,
                        HeaderValidated = null,
                        MissingFieldFound = null,
                    }))
                    {
                        csv.Context.TypeConverterCache.AddConverter<decimal>(new DecimalConverter());
                        csv.Context.RegisterClassMap<HistoricalStockDataMap>();
                        var records = csv.GetRecords<HistoricalStockData>().ToList();
                        foreach (var record in records)
                        {
                            record.Symbol = "MSFT";
                        }
                        context.HistoricalStockData.AddRange(records);
                        context.SaveChanges();
                    }
                }
                else
                {
                    throw new Exception($"Embedded resource not found.");
                }
            }
        }

        public class DecimalConverter : DefaultTypeConverter
        {
            public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
            {
                if (decimal.TryParse(text.Replace("$", "").Replace(",", ""), NumberStyles.Any, CultureInfo.InvariantCulture, out decimal value))
                {
                    return value;
                }
                return base.ConvertFromString(text, row, memberMapData);
            }
        }

        /// <summary>
        /// Reads an embedded resource csv file;
        /// </summary>
        public static string ReadEmbeddedResourceCsv(string resourceName)
        {
            using Stream? stream = Assembly.GetCallingAssembly().GetManifestResourceStream(resourceName);
            if (stream != null)
            {
                using StreamReader reader = new StreamReader(stream);
                StringBuilder lines = new StringBuilder();
                string? line;
                try
                {
                    while ((line = reader.ReadLine()) != null)
                    {
                        lines.Append(line);
                    }
                }
                catch (OutOfMemoryException ex)
                {
                    throw new Exception($"Error reading CSV file from embedded resource '{resourceName}': {ex.Message}", ex);
                }
                return lines.ToString();
            }
            else
            {
                throw new Exception($"Embedded resource '{resourceName}' not found.");
            }
        }
    }
}
