using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using IMS.Data;
using IMS.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace IMS.Pages {
  public class ITManagerLandingModel:PageModel {
    private readonly AppDbContext _context;

    public ITManagerLandingModel(AppDbContext context) {
      _context = context;
    }

    public IList<UserAccount> AllUserAccounts { get; set; } = new List<UserAccount>();

    public async Task OnGetAsync() {
      AllUserAccounts = await _context.UserAccounts
                          .OrderBy(u => u.Username)
                          .ToListAsync();
    }

    public async Task<IActionResult> OnPostVerifyAsync(int id) {
      var account = await _context.UserAccounts.FindAsync(id);
      if(account != null) {
        account.Verified = true;
        await _context.SaveChangesAsync();
      }
      return RedirectToPage();
    }

    public async Task<IActionResult> OnPostDeleteAsync(int id) {
      var account = await _context.UserAccounts.FindAsync(id);
      if(account != null) {
        _context.UserAccounts.Remove(account);
        await _context.SaveChangesAsync();
      }
      return RedirectToPage();
    }

    public IActionResult OnPostLogout() {
      HttpContext.Session.Clear();
      return RedirectToPage("/Login");
    }
  }
}
