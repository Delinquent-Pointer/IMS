using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using IMS.Data;
using IMS.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IMS.Pages{
  public class ProductsModel:PageModel {
    private readonly AppDbContext _appDbContext;

    public ProductsModel(AppDbContext appDbContext) {
      _appDbContext = appDbContext;
    }

    public IList<Product> Products { get; set; } = new List<Product>();

    [BindProperty(SupportsGet = true)]
    public string? SearchTerm { get; set; }

    public async Task OnGetAsync() {
      var query = _appDbContext.Products.AsQueryable();

      if(!string.IsNullOrEmpty(SearchTerm)) {
        query = query.Where(p => EF.Functions.Like(p.Name,$"%{SearchTerm}%"));
      }

      Products = await query.ToListAsync();
    }
  }
}