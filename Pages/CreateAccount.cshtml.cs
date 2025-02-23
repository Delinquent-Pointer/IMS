using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using IMS.Data;
using IMS.Models;

namespace IMS.Pages {
  public class CreateAccountModel:PageModel {
    private readonly AppDbContext _context;

    public CreateAccountModel(AppDbContext context) {
      _context = context;
    }

    [BindProperty]
    public CreateAccountInputModel Input { get; set; }

    public class CreateAccountInputModel {
      [Required]
      public required string Username { get; set; }

      [Required]
      [DataType(DataType.Password)]
      public required string Password { get; set; }

      [Required]
      [DataType(DataType.Password)]
      [Compare("Password",ErrorMessage = "The password and confirmation password do not match.")]
      public required string ConfirmPassword { get; set; }

      public bool Is_IT_User { get; set; }

      public string? AdminKey { get; set; }
    }

    public void OnGet() {
    }

    public async Task<IActionResult> OnPostAsync() {
      if(!ModelState.IsValid) {
        return Page();
      }

      // TODO: Implement company admin key validation might require a separate table in the database
      if(Input.Is_IT_User && Input.AdminKey != "YourCompanyAdminKey") {
        ModelState.AddModelError(string.Empty,"Invalid Company Admin Key.");
        return Page();
      }

      var hashedPassword = HashPassword(Input.Password);
      var user = new UserAccount {
        Username = Input.Username,
        Password_Hash = hashedPassword,
        Is_IT_User = Input.Is_IT_User
      };

      _context.UserAccounts.Add(user);
      await _context.SaveChangesAsync();

      return RedirectToPage("/Login");
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
