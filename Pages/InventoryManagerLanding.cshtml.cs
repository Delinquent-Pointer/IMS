using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using IMS.Data;
using IMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IMS.Pages {
  public class InventoryManagerLandingModel:PageModel {
    private readonly AppDbContext _context;

    public InventoryManagerLandingModel(AppDbContext context) {
      _context = context;
    }

    public IList<Product> Products { get; set; } = new List<Product>();

    [BindProperty(SupportsGet = true)]
    public string? SearchTerm { get; set; }

    [BindProperty]
    public string? NewNote { get; set; }

    [BindProperty]
    public string? SelectedDate { get; set; }

    public DateTime CurrentTime { get; private set; }
    public IList<Note> Notes { get; set; } = new List<Note>();

    public List<CalendarEvent> CalendarEvents { get; set; } = new List<CalendarEvent>();

    public async Task OnGetAsync() {
      CurrentTime = DateTime.Now;

      var query = _context.Products.AsQueryable();

      if(!string.IsNullOrEmpty(SearchTerm)) {
        query = query.Where(p => EF.Functions.Like(p.Name,$"%{SearchTerm}%") ||
                                 EF.Functions.Like(p.Description,$"%{SearchTerm}%") ||
                                 EF.Functions.Like(p.Category,$"%{SearchTerm}%") ||
                                 EF.Functions.Like(p.SKU,$"%{SearchTerm}%") ||
                                 EF.Functions.Like(p.Location,$"%{SearchTerm}%") ||
                                 p.ReorderLevel.ToString().Contains(SearchTerm) ||
                                 p.Quantity.ToString().Contains(SearchTerm) ||
                                 p.Price.ToString().Contains(SearchTerm));
      }

      Products = await query.ToListAsync();

      CalendarEvents = await _context.CalendarEvents.ToListAsync();
    }

    public JsonResult OnGetCalendarEvents() {
      var events = _context.CalendarEvents
          .Select(e => new {
            title = e.Title,
            start = e.StartDate.ToString("yyyy-MM-ddTHH:mm:ss"),
            end = e.EndDate.HasValue ? e.EndDate.Value.ToString("yyyy-MM-ddTHH:mm:ss") : null,
            description = e.Description
          }).ToList();

      return new JsonResult(events);
    }

    public async Task<IActionResult> OnPostLogoutAsync() {
      HttpContext.Session.Clear();
      return RedirectToPage("/Login");
    }
  }
}
