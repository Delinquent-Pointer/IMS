using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using IMS.Data;
using IMS.Models;
using IMS.Attributes;
using Microsoft.AspNetCore.Authorization;
using System.Text.RegularExpressions;


namespace IMS.Pages {

    
    public class CreateProductModel:PageModel {


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
