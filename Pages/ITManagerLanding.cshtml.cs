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

    public IList<UserAccountViewModel> AllUserAccounts { get; set; } = new List<UserAccountViewModel>();

    public async Task OnGetAsync() {
      AllUserAccounts = await (
          from account in _context.UserAccounts
          join profile in _context.Set<UserProfile>()
          on account.Account_Id equals profile.Account_Id into accountProfile
          from profile in accountProfile.DefaultIfEmpty()
          select new UserAccountViewModel {
            Account_Id = account.Account_Id,
            Username = account.Username,
            Is_IT_User = account.Is_IT_User,
            Verified = account.Verified,
            FirstName = profile != null ? profile.FirstName : "",
            LastName = profile != null ? profile.LastName : "",
            Email = profile != null ? profile.Email : "",
            PhoneNumber = profile != null ? profile.PhoneNumber : "",
            TimeZone = profile != null ? profile.TimeZone : "",
            State = profile != null ? profile.State : "",
            City = profile != null ? profile.City : "",
            CreatedAt = profile != null ? profile.CreatedAt.ToString("g") : ""
          }).OrderBy(u => u.Username).ToListAsync();
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
