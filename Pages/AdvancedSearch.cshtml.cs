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
    public class AdvancedSearchModel:PageModel {

        private readonly AppDbContext _context;

        public AdvancedSearchModel(AppDbContext context) {
            _context = context;
        }

        [BindProperty]
        public AdvancedSearchInputModel Input { get; set; }
        public List<string?> Categories { get; set; } = new List<string?>();
        public List<string?> Locations { get; set; } =  new List<string?>();

        public class AdvancedSearchInputModel
        {
            public string? ProductName { get; set; }

            public string? Description { get; set; }

            public string? PriceOperator { get; set; }

            public decimal? Price { get; set; }

            public string? QuantityOperator { get; set; }
            public int? Quantity { get; set; }

            [SKUReqs]
            public string? SKU { get; set; }

            public string? Category { get; set; }

            public string? Location { get; set; }
        }
        public async Task OnGetAsync() {
            Categories = await _context.Products.Select(p => p.Category).Distinct().ToListAsync(); 
            Locations =  await _context.Products.Select(p => p.Location).Distinct().ToListAsync(); 
        }

        public async Task<IActionResult> OnPostAsync() {
            if(!ModelState.IsValid) return Page();
      
            string searchTerm = FormatQuery();

            return RedirectToPage("/Products", new { SearchType = "AdvQuery", SearchTerm = searchTerm });
        }
    
        private String FormatQuery() {
            var terms = new List<string>();

            if (!string.IsNullOrWhiteSpace(Input.ProductName)) terms.Add($"name:{Input.ProductName}");
            if (!string.IsNullOrWhiteSpace(Input.Description)) terms.Add($"desc:{Input.Description}");
            if (!string.IsNullOrWhiteSpace(Input.PriceOperator)) terms.Add($"PriceOp:{Input.PriceOperator}");
            if (Input.Price.HasValue) terms.Add($"price={Input.Price}");
            if (!string.IsNullOrWhiteSpace(Input.QuantityOperator)) terms.Add($"QtyOp:{Input.QuantityOperator}");
            if (Input.Quantity.HasValue) terms.Add($"qty={Input.Quantity}");
            if (!string.IsNullOrWhiteSpace(Input.SKU)) terms.Add($"sku:{Input.SKU}");
            if (!string.IsNullOrWhiteSpace(Input.Category)) terms.Add($"cat:{Input.Category}");
            if (!string.IsNullOrWhiteSpace(Input.Location)) terms.Add($"loc:{Input.Location}");

            return string.Join(";", terms);
        }
    
    }
}
