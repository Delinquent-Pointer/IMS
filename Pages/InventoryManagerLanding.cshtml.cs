using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using IMS.Data;
using IMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Globalization;

namespace IMS.Pages {
  public class InventoryManagerLandingModel:PageModel {
    private readonly AppDbContext _context;

    public InventoryManagerLandingModel(AppDbContext context) {
      _context = context;
    }

    // List of products for the search section
    public IList<Product> Products { get; set; } = new List<Product>();

    // Search term for filtering products
    [BindProperty(SupportsGet = true)]
    public string? SearchTerm { get; set; }

    // Holds the new note text posted from the form
    [BindProperty]
    public string? NewNote { get; set; }

    public DateTime CurrentTime { get; private set; }

    // List for notes (for demo purposes; in production, persist these to a database)
    public IList<Note> Notes { get; set; } = new List<Note>();

    public async Task OnGetAsync() {
      string? error = HttpContext.Session.GetString("ITRedirectError");
      if(error != null) {
        ModelState.AddModelError(string.Empty,error);
        HttpContext.Session.Remove("ITRedirectError");
      }
      
      CurrentTime = DateTime.Now;

      // Product search
      var query = _context.Products.AsQueryable();
      if(!string.IsNullOrEmpty(SearchTerm)) {
        query = query.Where(p => EF.Functions.Like(p.Name,$"%{SearchTerm}%") ||
                                 EF.Functions.Like(p.Description,$"%{SearchTerm}%"));
      }
      Products = await query.ToListAsync();

      // Load dummy notes if none exist (for demonstration)
      if(Notes.Count == 0) {
        Notes.Add(new Note { Content = "Dashboard initialized.",Timestamp = DateTime.Now.AddMinutes(-30) });
        Notes.Add(new Note { Content = "Inventory levels checked.",Timestamp = DateTime.Now.AddMinutes(-10) });
      }
    }

    public async Task<IActionResult> OnPostLogout() {
      HttpContext.Session.Clear();
      return RedirectToPage("/Login");
    }

    public async Task<IActionResult> OnPostAsync() {
      if(!string.IsNullOrEmpty(NewNote)) {
        Notes.Add(new Note { Content = NewNote,Timestamp = DateTime.Now });
      }
      return RedirectToPage();
    }
  }

  // Simple note model for demo purposes
  public class Note {
    public string? Content { get; set; }
    public DateTime Timestamp { get; set; }
  }
}
