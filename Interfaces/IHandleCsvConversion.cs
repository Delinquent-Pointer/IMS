using CsvHelper;
using System.Globalization;
using IMS.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using CsvHelper.Configuration;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;

namespace IMS.Interfaces
{
    public interface IHandleProductCsv
    {
        private class ProductMap : ClassMap<Product>
        {
            public ProductMap(bool export = false)
            {
                Map(m => m.Name).Default(string.Empty);
                Map(m => m.Description).Default(string.Empty);
                Map(m => m.Price).Default(0.00m);
                Map(m => m.Quantity).Default(0);
                Map(m => m.ReorderLevel).Default(0);
                Map(m => m.SKU).Default(string.Empty);
                Map(m => m.Category).Default(string.Empty);
                Map(m => m.Location).Default(string.Empty);
                if (export)
                {
                    //convert binary to base64 when used for CSV export
                    Map(m => m.Image).Convert(args =>
                        args.Value.Image != null && args.Value.Image.Length > 0
                            ? Convert.ToBase64String(args.Value.Image)
                            : "");
                }
                else
                {
                    //convert uploaded Base64 to byte array when used for CSV import
                    Map(m => m.Image).Convert(args =>
                    {
                        var base64 = args.Row.GetField("Image");
                        return string.IsNullOrEmpty(base64)
                            ? Array.Empty<byte>()
                            : Convert.FromBase64String(base64);
                    });
                }
            }
        }

        public static List<Product> ConvertFromCsv(IFormFile file)
        {
            using (var reader = new StreamReader(file.OpenReadStream()))
            {

                CsvReader csv = new(reader, new CsvHelper.Configuration.CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    Delimiter = ",",
                    PrepareHeaderForMatch = args => args.Header.Trim(),
                    HasHeaderRecord = true,
                    MissingFieldFound = null, // Ignore missing fields
                    HeaderValidated = null, // Ignore header validation
                });

                csv.Read(); //read the first row
                csv.ReadHeader(); //read the first row as the header row

                // Validate the header
                string[]? headerRecord = csv.HeaderRecord;
                var headerResult = ValidateCsvHeader(headerRecord);
                if (!headerResult.isValid)
                {
                    throw new ProductCsvException(headerResult.errors);

                }


                csv.Context.RegisterClassMap(new ProductMap()); //handles converting empty csv fields to default values. 

                //attempt to read the records as Product objects
                List<Product> records;
                try
                {
                    records = csv.GetRecords<Product>().ToList();
                }
                catch (Exception ex)
                {
                    throw new ProductCsvException($"CSV parsing error: {ex.Message}");

                }

                // Validate records before saving, if any records are invalid, return to the page with errors
                var recordResult = ValidateRecords(records);
                if (!recordResult.isValid)
                {
                    throw new ProductCsvException(recordResult.errors);
                }
                // If all records are valid, return the list of products
                return records;
            }
        }


        private static (bool isValid, List<string> errors) ValidateRecords(List<Product> records)
        {
            bool isValid = true;
            List<string> errors = new();
            for (int i = 0; i < records.Count; i++)
            //foreach (var record in records)
            {
                Product record = records[i];
                int index = i + 1; // 1-based index for error messages
                if (string.IsNullOrEmpty(record.Name))
                {
                    errors.Add( $"Item {index}: Name is a required field and cannot be empty.");
                    isValid = false;
                }

                if (record.Price < 0)
                {
                    errors.Add( $"Item {index}: An item's Price cannot be negative.");
                    isValid = false;
                }
                if (record.Quantity < 0)
                {
                    Console.WriteLine($"Validation failed for Quantity at index: {index}");
                    errors.Add( $"Item {index}: An item's Quantity cannot be negative.");
                    isValid = false;
                }
                if (record.ReorderLevel < 0)
                {
                    errors.Add( $"Item {index}: An item's Reorder Level cannot be negative.");
                    isValid = false;
                }

                if (!string.IsNullOrEmpty(record.SKU))
                {
                    if (!Regex.IsMatch(record.SKU, @"^[A-Z0-9-]*$") ||
                        !Regex.IsMatch(record.SKU, @"^\S+(-\S+)+$") ||
                        !Regex.IsMatch(record.SKU, @"^\S{1,5}(-\S*)*$") ||
                        !Regex.IsMatch(record.SKU, @"^\S+(-\S{1,10})+$") ||
                        !Regex.IsMatch(record.SKU, @"^\S+(-\S+){1,3}$"))
                    {
                        errors.Add( $"Item {index}: Incorrectly formatted SKU.");
                        isValid = false;
                    }
                }
            }
            return (isValid, errors);
        }

       private static (bool isValid, List<string> errors) ValidateCsvHeader(string[]? headerRecord)
        {
            bool isValid = true;
            List<string> errors = new();
            if (headerRecord == null)
            {
                errors.Add("No header record found.");
                return (false, errors);
            }
            var allowedHeaders = new List<string> {
                "Name", "Description", "Price", "Quantity", "ReorderLevel", "SKU", "Category", "Location", "Image"
            };

            //Name header is required
            if (!headerRecord.Contains("Name", StringComparer.OrdinalIgnoreCase))
            {
                errors.Add( "The 'Name' header is required but missing.");
                isValid = false;
            }

            // all other headers besides Name are optional, but if they are present, they must match the allowedHeaders list
            var invalidHeaders = headerRecord
                .Where(header => !allowedHeaders.Contains(header, StringComparer.OrdinalIgnoreCase))
                   .ToList();

            if (invalidHeaders.Any())
            {
                errors.Add( $"Invalid headers found: {string.Join(", ", invalidHeaders.Select(header => $"\"{header}\""))}.");
                isValid = false;
            }


            var duplicateHeaders = headerRecord
                .GroupBy(header => header, StringComparer.OrdinalIgnoreCase)
                .Where(group => group.Count() > 1)
                .Select(group => group.Key);

            if (duplicateHeaders.Any())
            {
                errors.Add( $"Duplicate headers found: {string.Join(", ", duplicateHeaders.Select(header => $"\"{header}\""))}.");
                isValid = false; // Duplicate headers found
            }

            return (isValid, errors);
        }



        static FileContentResult ConvertToCsv (List<Product> Products)
        {
            using (StringWriter writer = new StringWriter())
            using (CsvWriter csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.Context.RegisterClassMap(new ProductMap(true));
                csv.WriteRecords(Products);

                string fileContent = writer.ToString();
                string fileName = $"Products_{DateTime.Now:yyyyMMdd_HHmmss}.csv";
                string contentType = "text/csv";
                return new FileContentResult( new System.Text.UTF8Encoding().GetBytes(fileContent), contentType) {FileDownloadName = fileName};
            }
        }



    }



}
