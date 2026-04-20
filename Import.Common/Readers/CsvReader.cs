using System.Collections.Generic;
using System.Globalization;
using System.IO;
using CsvHelper.Configuration;
using Import.Common.Models;
using Logging;

namespace Import.Common.Readers
{
    internal class CsvReader : ICsvReader
    {
        private readonly ILogger _logger;

        public CsvReader(ILogger logger)
        {
            _logger = logger;
        }

        public List<Employee> GetEmployees(Stream? stream)
        {
            if (stream is null)
            {
                _logger.LogMessage("Error: File not found.");

                return [];
            }

            CsvConfiguration config = new(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                MissingFieldFound = null,
                HeaderValidated = null,
                ReadingExceptionOccurred = args =>
                {
                    _logger.LogMessage($"Skipping invalid row: {args.Exception.Context?.Parser?.RawRecord.Trim()}");
                    return false;
                }
            };

            using StreamReader streamReader = new(stream);
            using CsvHelper.CsvReader csv = new(streamReader, config);
            List<Employee> employees = [.. csv.GetRecords<Employee>()];

            return employees;
        }
    }
}
