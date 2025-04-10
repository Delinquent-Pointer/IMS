using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using IMS.Data;
using IMS.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using System.Text.RegularExpressions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace IMS.Pages {
    public class ProductsModel : PageModel {
        private readonly AppDbContext _appDbContext;

        private class ProductMap : ClassMap<Product>{
            ProductMap(){
                Map(m => m.Name).Default(string.Empty);
                Map(m => m.Description).Default(string.Empty);
                Map(m => m.Price).Default(0.00m);
                Map(m => m.Quantity).Default(0);
                Map(m => m.ReorderLevel).Default(0);
                Map(m => m.SKU).Default(string.Empty);
                Map(m => m.Category).Default(string.Empty);
                Map(m => m.Location).Default(string.Empty);
                Map(m => m.Image).Convert(args =>{
                    var base64String = args.Row.GetField("Image");
                    return string.IsNullOrEmpty(base64String) ? Array.Empty<byte>() : Convert.FromBase64String(base64String);
                });
            }
        }





        public ProductsModel(AppDbContext appDbContext) {
            _appDbContext = appDbContext;
        }

        public List<string?> Categories { get; set; }
        public List<string?> Locations { get; set; }

        [BindProperty]
        public EditProductInputModel Input { get; set; }

        public IList<Product> Products { get; set; } = new List<Product>();

        [BindProperty(SupportsGet = true)]
        public string? SearchType { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? SearchTerm { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? Operator { get; set; }

        public class EditProductInputModel {
            public int Id { get; set; } // Add Id for the product
            [Required]
            public string ProductName { get; set; }
            public string? Description { get; set; }
            public decimal? Price { get; set; }
            public int? Quantity { get; set; }
            public int? ReorderLevel { get; set; }
            public string? SKU { get; set; }
            public string? Category { get; set; }
            public string? Location { get; set; }
            public IFormFile? ImageFile { get; set; }
        }

        // Fetch data for editing the product
        public async Task<IActionResult> OnGetEditAsync(int id) {
            var product = await _appDbContext.Products.FindAsync(id);

            if (product == null) {
                return NotFound();
            }

            // Populate the input model with product data
            Input = new EditProductInputModel {
                Id = product.Id,
                ProductName = product.Name,
                Description = product.Description,
                Price = product.Price,
                Quantity = product.Quantity,
                ReorderLevel = product.ReorderLevel,
                SKU = product.SKU,
                Category = product.Category,
                Location = product.Location
            };
            if (product.Image != null && product.Image.Length > 0)
            {
                string base64Image = Convert.ToBase64String(product.Image);
                ViewData["ProductImage"] = $"data:image/png;base64,{base64Image}";
            }

            // Load categories and locations for the dropdown options
            Categories = await _appDbContext.Products.Select(p => p.Category).Distinct().ToListAsync();
            Locations = await _appDbContext.Products.Select(p => p.Location).Distinct().ToListAsync();

            return Page();
        }

        // Handle the form submission to update the product
        public async Task<IActionResult> OnPostAsync() {
            if (!ModelState.IsValid) {
                await PopulateProductsList();
                return Page();
            }

            var product = await _appDbContext.Products.FindAsync(Input.Id);

            if (product == null) {
                return NotFound();
            }

            // Update product properties
            product.Name = Input.ProductName;
            product.Description = Input.Description ?? "";
            product.Price = Input.Price ?? 0.00m;
            product.Quantity = Input.Quantity ?? 0;
            product.ReorderLevel = Input.ReorderLevel ?? 0;
            product.SKU = Input.SKU ?? "";
            product.Category = Input.Category ?? "";
            product.Location = Input.Location ?? "";
            if (Input.ImageFile != null && Input.ImageFile.Length > 0)
            {
                using var memoryStream = new MemoryStream();
                await Input.ImageFile.CopyToAsync(memoryStream);
                product.Image = memoryStream.ToArray(); // Save as binary
            }

            _appDbContext.Products.Update(product);
            await _appDbContext.SaveChangesAsync();

            return RedirectToPage("/Products");
        }

        
        private async Task PopulateProductsList() {
            Products = await _appDbContext.Products.AsQueryable().ToListAsync();
        }

        // Handle the search functionality
        public async Task OnGetAsync() {
            var query = _appDbContext.Products.AsQueryable();

            if (string.IsNullOrEmpty(SearchTerm)) {
                await PopulateProductsList();
                return;
            }

            switch (SearchType) {
                case "Name":
                    query = query.Where(p => EF.Functions.Like(p.Name, $"%{SearchTerm}%"));
                    break;
                case "Description":
                    query = query.Where(p => EF.Functions.Like(p.Description, $"%{SearchTerm}%"));
                    break;
                case "Category":
                    query = query.Where(p => EF.Functions.Like(p.Category, $"%{SearchTerm}%"));
                    break;
                case "SKU":
                    query = query.Where(p => EF.Functions.Like(p.SKU, $"%{SearchTerm}%"));
                    break;
                case "Location":
                    query = query.Where(p => EF.Functions.Like(p.Location, $"%{SearchTerm}%"));
                    break;
                case "Quantity":
                    if (int.TryParse(SearchTerm, out int quantity)) {
                        switch (Operator) {
                            case "=":
                                query = query.Where(p => p.Quantity == quantity);
                                break;
                            case ">":
                                query = query.Where(p => p.Quantity > quantity);
                                break;
                            case "<":
                                query = query.Where(p => p.Quantity < quantity);
                                break;
                            case ">=":
                                query = query.Where(p => p.Quantity >= quantity);
                                break;
                            case "<=":
                                query = query.Where(p => p.Quantity <= quantity);
                                break;
                        }
                    }
                    break;
                case "Price":
                    if (decimal.TryParse(SearchTerm, out decimal price)) {
                        switch (Operator) {
                            case "=":
                                query = query.Where(p => p.Price == price);
                                break;
                            case ">":
                                query = query.Where(p => p.Price > price);
                                break;
                            case "<":
                                query = query.Where(p => p.Price < price);
                                break;
                            case ">=":
                                query = query.Where(p => p.Price >= price);
                                break;
                            case "<=":
                                query = query.Where(p => p.Price <= price);
                                break;
                        }
                    }
                    break;
                default:
                    break;
            }

            Products = await query.ToListAsync();
        }

        //Delete functionality
        public async Task<IActionResult> OnPostDeleteAsync(int id) {
            var product = await _appDbContext.Products.FindAsync(id);

            if (product == null) {
                return NotFound();
            }

            _appDbContext.Products.Remove(product);
            
            _appDbContext.ProductsBin.Add(new ProductBin {
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Quantity = product.Quantity,
                ReorderLevel = product.ReorderLevel,
                SKU = product.SKU,
                Category = product.Category,
                Location = product.Location,
                Image = product.Image,
                DeleteDate = DateOnly.FromDateTime(DateTime.Now),
                DeleteTime = TimeOnly.FromDateTime(DateTime.Now)
            });
            await _appDbContext.SaveChangesAsync();

            return RedirectToPage("/Products");
        }

        public async Task<IActionResult> OnPostUploadCSVAsync(IFormFile file) {
            ModelState.Clear();
            if (file == null || file.Length == 0) {
                ModelState.AddModelError(string.Empty, "Please upload a valid CSV file.");
                ViewData["Errors"] = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                await PopulateProductsList();
                return Page();
            }

            using (var reader = new StreamReader(file.OpenReadStream())) {
                CsvReader csv = new(reader, new CsvHelper.Configuration.CsvConfiguration(CultureInfo.InvariantCulture) {
                    Delimiter = ",",
                    HasHeaderRecord = true,
                    MissingFieldFound = null, // Ignore missing fields
                    HeaderValidated = null, // Ignore header validation
                });

                csv.Context.RegisterClassMap<ProductMap>(); //handles converting empty csv fields to default values. 
                List<Product> records = csv.GetRecords<Product>().ToList();

                // Validate records before saving, if records are invalid, return to the page with errors
                if (!ValidateRecords(records))
                {
                    ViewData["Errors"] = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                    await PopulateProductsList();
                    return Page();
                }
                _appDbContext.Products.AddRange(records);
                await _appDbContext.SaveChangesAsync();
            }

            return RedirectToPage("/Products");
        }

        private bool ValidateRecords(List<Product> records) {
            bool isValid = true;
            
            for(int i = 0; i < records.Count; i++)
            //foreach (var record in records)
            {
                Product record = records[i];
                int index = i + 1; // 1-based index for error messages
                if (string.IsNullOrEmpty(record.Name)){
                    ModelState.AddModelError(string.Empty, $"Item {index}: Name is a required field and cannot be empty.");
                    isValid = false;
                }

                if (record.Price < 0)
                {
                    ModelState.AddModelError(string.Empty, $"Item {index}: An item's Price cannot be negative.");
                    isValid = false;
                }
                if (record.Quantity < 0)
                {
                    Console.WriteLine($"Validation failed for Quantity at index: {index}");
                    ModelState.AddModelError(string.Empty, $"Item {index}: An item's Quantity cannot be negative.");
                    isValid = false;
                }
                if (record.ReorderLevel < 0)
                {
                    ModelState.AddModelError(string.Empty, $"Item {index}: An item's Reorder Level cannot be negative.");
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
                        ModelState.AddModelError(string.Empty, $"Item {index}: Incorrectly formatted SKU.");
                        isValid = false;
                    }
                }
            }
            return isValid;    
        }
    }
}
