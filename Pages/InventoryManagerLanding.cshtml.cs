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
    public int EditNoteId { get; set; }

    [BindProperty]
    public string? EditNoteContent { get; set; }

    public DateTime CurrentTime { get; private set; }
    public IList<Note> Notes { get; set; } = new List<Note>();

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
      Notes = await _context.Notes.OrderByDescending(n => n.Timestamp).ToListAsync();
    }

    public async Task<IActionResult> OnPostAddNoteAsync() {
      if(!string.IsNullOrEmpty(NewNote)) {
        _context.Notes.Add(new Note {
          Content = NewNote,
          Timestamp = DateTime.Now
        });
        await _context.SaveChangesAsync();
      }
      return RedirectToPage();
    }

    public async Task<IActionResult> OnPostDeleteNoteAsync(int id) {
      var note = await _context.Notes.FindAsync(id);
      if(note != null) {
        _context.Notes.Remove(note);
        await _context.SaveChangesAsync();
      }
      return RedirectToPage();
    }

    public async Task<IActionResult> OnPostEditNoteAsync() {
      if(!string.IsNullOrEmpty(EditNoteContent)) {
        var note = await _context.Notes.FindAsync(EditNoteId);
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
  }
}
