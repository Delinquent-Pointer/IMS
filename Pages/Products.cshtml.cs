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
    public string? SearchType { get; set; }

    [BindProperty(SupportsGet = true)]
    public string? SearchTerm { get; set; }

    [BindProperty(SupportsGet = true)]
    public string? Operator { get; set; }

    public async Task OnGetAsync() {
      var query = _appDbContext.Products.AsQueryable();

      if(string.IsNullOrEmpty(SearchTerm)) {
        Products = await query.ToListAsync();
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
          if(int.TryParse(SearchTerm, out int quantity)) {
            switch(Operator) {
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
          if(decimal.TryParse(SearchTerm, out decimal price)) {
            switch(Operator) {
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
  }
}