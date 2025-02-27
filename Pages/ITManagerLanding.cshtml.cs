using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using IMS.Data;
using IMS.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IMS.Pages {
  public class ITManagerLandingModel:PageModel {
    private readonly AppDbContext _context;

    public ITManagerLandingModel(AppDbContext context) {
      _context = context;
    }

    public IList<UserAccount> UnverifiedAccounts { get; private set; } = new List<UserAccount>();

    public async Task<IActionResult> OnGetAsync() {
      if(_context.UserAccounts == null) {
        return NotFound("Database context is null.");
      }

      UnverifiedAccounts = await _context.UserAccounts
          .Where(u => !u.Verified)
          .ToListAsync();

      return Page();
    }

    public async Task<IActionResult> OnPostDeleteAsync(int? id) {
      if(id == null) {
        return BadRequest("Invalid user ID.");
      }

      var user = await _context.UserAccounts.FindAsync(id);
      if(user == null) {
        return NotFound($"User with ID {id} not found.");
      }

      _context.UserAccounts.Remove(user);
      await _context.SaveChangesAsync();

      return RedirectToPage();
    }

    public async Task<IActionResult> OnPostVerifyAsync(int? id) {
      if(id == null) {
        return BadRequest("Invalid user ID.");
      }

      var user = await _context.UserAccounts.FindAsync(id);
      if(user == null) {
        return NotFound($"User with ID {id} not found.");
      }

      user.Verified = true;
      _context.UserAccounts.Update(user);
      await _context.SaveChangesAsync();

      return RedirectToPage();
    }
  }
}
