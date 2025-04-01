using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using IMS.Data;
using IMS.Models;
using Microsoft.AspNetCore.Authorization;
using System.Text.RegularExpressions;


namespace IMS.Pages {

    
    public class CreateProductModel:PageModel {

    private class SKUReqsAttribute : ValidationAttribute {
        
        //defining a custom attribute for SKU formatting
        protected override ValidationResult IsValid(object? value, ValidationContext validationContext) {
          
          if(value == null) return ValidationResult.Success!; //SKU is optional

          string SKU = value.ToString()!;

          Dictionary<string, string> SKUReqs = new Dictionary<string, string> {
            { @"^[A-Z0-9-]*$", "SKUs must contain only capital letters, digits, and dashes." },
            { @"^\S+(-\S+)+$", "An SKU must have 1 category and at least 1 subcategory." },
            { @"^\S{1,5}(-\S*)*$", "The category: (*AAAA*-BBBB) of an SKU must be 1-5 characters long." },
            { @"^\S+(-\S{1,10})+$", "subcategories: (AAAA-*BBBB*) of an SKU must be 1-10 characters long." },
            { @"^\S+(-\S+){1,3}$", "An SKU can have at most 3 subcategories: (AAA-BBB-CCC-DDD)." }
        };

          List<string> errors = SKUReqs
            .Where(req => !Regex.IsMatch(SKU, req.Key))
            .Select(req => req.Value)
            .ToList();
          
          if(errors.Count > 0) return new ValidationResult(string.Join("\n", errors));
          
          return ValidationResult.Success!;
          
        }
    }

    private readonly AppDbContext _context;

    public CreateProductModel(AppDbContext context) {
      _context = context;
    }

    [BindProperty]
    public CreateProductInputModel Input { get; set; }
    [BindProperty]
    public IFormFile? ImageFile { get; set; }

    public List<string?> Categories { get; set; } = new List<string?>();
    public List<string?> Locations { get; set; } =  new List<string?>();

    public class CreateProductInputModel {
     
      [Required]
      public string ProductName { get; set; }

      public string? Description { get; set; }

      public decimal? Price { get; set; }

      public int? Quantity { get; set; }

      public int? ReorderLevel { get; set; }

      [SKUReqs]
      public string? SKU { get; set; }

      public string? Category { get; set; }

      public string? Location { get; set; }
      public IFormFile? ImageFile { get; set; }
    }
    public async Task OnGetAsync() {
      Categories = await _context.Products.Select(p => p.Category).Distinct().ToListAsync(); 
      Locations =  await _context.Products.Select(p => p.Location).Distinct().ToListAsync(); 
    }

    public async Task<IActionResult> OnPostAsync() {
      if(!ModelState.IsValid) {
        return Page();
      }
      byte[]? imageData = null;
      if (ImageFile != null) {
          using (var memoryStream = new MemoryStream()) {
              await ImageFile.CopyToAsync(memoryStream);
              imageData = memoryStream.ToArray(); 
          }
      }

      //Handles nulls to allow empty fields on the input form
      var product = new Product {
        Name = Input.ProductName,
        Description = Input.Description ?? "",
        Price = Input.Price ?? 0.00m,
        Quantity = Input.Quantity ?? 0,
        ReorderLevel = Input.ReorderLevel ?? 0,
        SKU = Input.SKU ?? "",
        Category = Input.Category ?? "",
        Location = Input.Location ?? "",
        Image = imageData
      };

      _context.Products.Add(product);
      await _context.SaveChangesAsync();
      return RedirectToPage("/InventoryManagerLanding");
    }

    
  }
}
