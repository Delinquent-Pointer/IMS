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
using IMS.DataTransferObj;

namespace IMS.Pages {
  public class InventoryManagerLandingModel:PageModel {
    private readonly AppDbContext _context;

    public InventoryManagerLandingModel(AppDbContext context) {
      _context = context;
    }

    public IList<Product> Products { get; set; } = new List<Product>();
    public IList<Product> LowStockProducts { get; set; } = new List<Product>();
    public IList<Product> WatchedLowProducts { get; set; } = new List<Product>();

    [BindProperty(SupportsGet = true)]
    public string? SearchTerm { get; set; }

    [BindProperty]
    public string? NewNote { get; set; }

    [BindProperty]
    public int EditNoteId { get; set; }

    [BindProperty]
    public string? EditNoteContent { get; set; }

    [BindProperty]
    public int UniversalThreshold { get; set; } = 0;
    public DateTime CurrentTime { get; private set; }
    // public IList<Note> Notes { get; set; } = new List<Note>();
    public IList<NoteDTO> Notes { get; set; } = new List<NoteDTO>();

    public IList<CalendarEvent> CalendarEvents { get; set; } = new List<CalendarEvent>();
    public async Task<IActionResult> OnPostSetThresholdAsync() {
      HttpContext.Session.SetInt32("UniversalThreshold",UniversalThreshold);
      return RedirectToPage();
    }
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

      int userId = GetCurrentUserId();

      // Notes
      Notes = await _context.Notes
          .Where(n => n.Account_Id == userId)
          .OrderByDescending(n => n.Timestamp)
          .Select(n => new NoteDTO {
            Id = n.Id,
            Content = n.Content,
            Timestamp = n.Timestamp.ToString("yyyy-MM-ddTHH:mm:ss")
          })
          .ToListAsync();

      // Universal Threshold
      UniversalThreshold = HttpContext.Session.GetInt32("UniversalThreshold") ?? 0;
      ViewData["Threshold"] = UniversalThreshold;

      LowStockProducts = Products
          .Where(p => p.Quantity < UniversalThreshold)
          .ToList();

      // Watched Threshold
      var watchedItems = await _context.WatchedProducts
          .Include(w => w.Product)
          .Where(w => w.Account_Id == userId)
          .ToListAsync();

      WatchedLowProducts = watchedItems
          .Where(w => w.Product.Quantity < w.Threshold)
          .Select(w => w.Product)
          .ToList();
    }

    public async Task<IActionResult> OnPostAddNoteAsync() {
      if(!string.IsNullOrEmpty(NewNote)) {
        int userId = GetCurrentUserId();

        _context.Notes.Add(new Note {
          Content = NewNote,
          Timestamp = DateTime.Now,
          Account_Id = userId
        });

        await _context.SaveChangesAsync();
      }

      return RedirectToPage();
    }

    public async Task<IActionResult> OnPostDeleteNoteAsync(int id) {
      int userId = GetCurrentUserId();

      var note = await _context.Notes
          .Where(n => n.Id == id && n.Account_Id == userId)
          .FirstOrDefaultAsync();

      if(note != null) {
        _context.Notes.Remove(note);
        await _context.SaveChangesAsync();
      }

      return RedirectToPage();
    }

    public async Task<IActionResult> OnPostEditNoteAsync() {
      if(!string.IsNullOrEmpty(EditNoteContent)) {
        int userId = GetCurrentUserId();

        var note = await _context.Notes
            .Where(n => n.Id == EditNoteId && n.Account_Id == userId)
            .FirstOrDefaultAsync();

        if(note != null) {
          note.Content = EditNoteContent;
          note.Timestamp = DateTime.Now;
          await _context.SaveChangesAsync();
        }
      }

      return RedirectToPage();
    }

    public IActionResult OnPostLogout() {
      HttpContext.Session.Clear();
      return RedirectToPage("/Login");
    }

    public async Task<JsonResult> OnGetCalendarEventsAsync() {
      int userId = GetCurrentUserId();

      var events = await _context.CalendarEvents
          .Where(e => e.Account_Id == userId)
          .Select(e => new {
            e.Id,
            e.Title,
            Date = e.StartDate.ToString("yyyy-MM-dd"),
            e.Description
          }).ToListAsync();

      return new JsonResult(events);
    }

    private UserDto? CurrentUser { get; set; }

    private int GetCurrentUserId() {
      if(CurrentUser is null) {
        var userJson = HttpContext.Session.GetString("User");
        if(string.IsNullOrEmpty(userJson))
          throw new Exception("User is not logged in.");

        CurrentUser = System.Text.Json.JsonSerializer.Deserialize<UserDto>(userJson);
      }
      // tells the compiler: “I know this isn't null — trust me.”
      var user = CurrentUser!;
      return user.Account_Id;
    }


  }
}
