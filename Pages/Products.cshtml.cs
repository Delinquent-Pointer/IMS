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
using Microsoft.IdentityModel.Tokens;
using IMS.Interfaces;

namespace IMS.Pages {
    public class ProductsModel : PageModel, IHandleProductCsv {
        private readonly AppDbContext _appDbContext;

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
                case "AdvQuery":
                    try {
                        query = ParseAdvQuery(SearchTerm);
                    }
                    catch(ArgumentException e)
                    {
                        ModelState.AddModelError(string.Empty, e.Message);
                        ViewData["Errors"] = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                        await PopulateProductsList();
                        return;
                    }
                    break;
                    default:
                        ModelState.AddModelError(string.Empty, $"Unrecognized Search Category:{SearchType}");
                        ViewData["Errors"] = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                    await PopulateProductsList();
                        return;

                    }

            Products = await query.ToListAsync();
        }

        private IQueryable<Product> ParseAdvQuery(string searchTerm)
        {
            var query = _appDbContext.Products.AsQueryable();
            // Split the search term into key-value pairs
            Dictionary<string, string> terms = searchTerm.Split(new[] {';'}, StringSplitOptions.RemoveEmptyEntries)
                .Select(term => term.Split(':'))
                .Where(pair => pair.Length==2)
                .ToDictionary(pair => pair[0], pair => pair[1]);

            foreach (var term in terms)
            {
                string key = term.Key.Trim();
                string value = term.Value.Trim();

                switch (key.ToLower())
                {
                    case "name":
                        query = query.Where(p => EF.Functions.Like(p.Name, $"%{value}%"));
                        break;
                    case "desc" or "description":
                        query = query.Where(p => EF.Functions.Like(p.Description, $"%{value}%"));
                        break; 
                    case "price":
                        if (decimal.TryParse(value, out decimal price)) {
                            switch (terms.TryGetValue("priceop", out string? priceOp) ? priceOp : "=")
                            {
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
                                case "=":
                                    query = query.Where(p => p.Price == price);
                                    break;
                                default:
                                    throw new ArgumentException($"Invalid price operator: {priceOp}");
                            }
                        }
                        break;
                    case "qty" or "quantity":
                        if (int.TryParse(value, out int quantity)){
                            switch(terms.TryGetValue("qtyop", out string? qtyOp) ? qtyOp : "=" )
                            {
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
                                case "=":
                                    query = query.Where(p => p.Quantity == quantity);
                                    break;
                                default:
                                    throw new ArgumentException($"Invalid quantity operator: {qtyOp}");
                            }
                        }

                        break;
                    case "sku":
                        query = query.Where(p => EF.Functions.Like(p.SKU, $"%{value}%"));
                        break;
                    case "cat" or "category":
                        query = query.Where(p => EF.Functions.Like(p.Category, $"%{value}%"));
                        break;
                    case "loc" or "location":
                        query = query.Where(p => EF.Functions.Like(p.Location, $"%{value}%"));
                        break;
                    default:
                        throw new ArgumentException($"Invalid search category: {key}");
                }
            }

            return query;
        }

        //Delete functionality
        public async Task<IActionResult> OnPostDeleteAsync(int id) {
            var product = await _appDbContext.Products.FindAsync(id);

            if (product == null) {
                return NotFound();
            }

            _appDbContext.Products.Remove(product);
            _appDbContext.SaveChangesAsync();
            

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

            try
            {
               List<Product> products = IHandleProductCsv.ConvertFromCsv(file);
                _appDbContext.Products.AddRange(products);
                await _appDbContext.SaveChangesAsync();

                return RedirectToPage("/Products");
            }
            catch (ProductCsvException ex)
            {
               //foreach (string err in ex.Errors) ModelState.AddModelError(string.Empty, err);
                ViewData["Errors"] = ex.Errors;
                await PopulateProductsList();
                return Page();
            }
            
        }

        public async Task<IActionResult> OnPostDownloadCSVAsync(List<int> productIds)
        {
            //previous product list is entered as hidden form, this will allow downloading of narrowed search results
            List<Product> Products = await _appDbContext.Products
                .Where(p => productIds.Contains(p.Id))
                .ToListAsync();

            //if the result list was empty we grab the whole list
            if (Products.IsNullOrEmpty())
            {
                await PopulateProductsList();
            }

            return IHandleProductCsv.ConvertToCsv(Products);
        }
    }
}
