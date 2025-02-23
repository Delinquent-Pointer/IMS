using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using IMS.Data;
using IMS.Models;

namespace IMS.Pages {
  public class LoginModel:PageModel {
    private readonly AppDbContext _context;

    public LoginModel(AppDbContext context) {
      _context = context;
    }

    [BindProperty]
    public LoginInputModel Input { get; set; }

    public class LoginInputModel {
      [Required]
      public required string Username { get; set; }

      [Required]
      [DataType(DataType.Password)]
      public required string Password { get; set; }
    }

    public void OnGet() {
    }

    public async Task<IActionResult> OnPostAsync() {
      if(!ModelState.IsValid) {
        return Page();
      }

      var hashedPassword = HashPassword(Input.Password);
      var user = await _context.UserAccounts
          .FirstOrDefaultAsync(u => u.Username == Input.Username && u.Password_Hash == hashedPassword);

      if(user == null) {
        ModelState.AddModelError(string.Empty,"Login Failed, Account info not recognised.");
        ViewData["LoginFailed"] = true;
        return Page();
      }

      // TODO: Implement authentication logic

      // TODO: Implement Switch statement to redirect to different pages based on user role
      return Redirect("/InventoryManagerLanding");
    }

    private static string HashPassword(string password) {
      using(var sha256 = SHA256.Create()) {
        var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        var builder = new StringBuilder();
        for(int i = 0;i < bytes.Length;i++) {
          builder.Append(bytes[i].ToString("x2"));
        }
        return builder.ToString();
      }
    }
  }
}