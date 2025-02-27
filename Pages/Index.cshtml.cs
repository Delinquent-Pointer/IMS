using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IMS.Data;
using IMS.Models;
using Microsoft.AspNetCore.Authorization;
using IMS.Attributes;

[RequireITPerms]
public class IndexModel:PageModel {
  private readonly AppDbContext _context;

  public IndexModel(AppDbContext context) {
    _context = context;
  }

  public IList<Product> Products { get; set; } = new List<Product>();

  public async Task OnGetAsync(string searchTerm) {
    if(string.IsNullOrWhiteSpace(searchTerm)) {
      Products = await _context.Products.ToListAsync();
    } else {
      Products = await _context.Products
          .Where(p => (p.Name != null && p.Name.Contains(searchTerm))
                   || (p.Description != null && p.Description.Contains(searchTerm)))
          .ToListAsync();
    }
  }

}
