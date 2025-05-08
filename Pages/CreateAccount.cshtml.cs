using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using IMS.Data;
using IMS.Models;
using IMS.Attributes;
using Microsoft.AspNetCore.Authorization;
using System.Text.RegularExpressions;
using IMS.Interfaces;

namespace IMS.Pages {
    
    [AllowAnonymous]
    public class CreateAccountModel:PageModel, IHashPasswords {
    
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
      [MinLength(8,ErrorMessage = "Password must be at least 8 characters long.")]
      [PasswordReqs]
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


      var hashedPassword = IHashPasswords.HashPassword(Input.Password);
      var user = new UserAccount {
        Username = Input.Username,
        Password_Hash = hashedPassword,
        Is_IT_User = Input.Is_IT_User
      };

      if(await UsernameExists(user.Username)) {
        ModelState.AddModelError(string.Empty,"This Username is taken, try another.");
        return Page();
      }

      if(Input.Is_IT_User) {
        return await HandleITUser(user);
       
      }
      else{
        _context.UserAccounts.Add(user);
        await _context.SaveChangesAsync();
        return RedirectToPage("/Login");
      }

    }

    private async Task<bool> UsernameExists(string username) {
      return await _context.UserAccounts.AnyAsync(u => u.Username == username);
    }

    private async Task<IActionResult> HandleITUser(UserAccount user){
       if(Input.AdminKey == null){
        ModelState.AddModelError(string.Empty,"Company Admin Key is required for IT Users.");
        return Page();
        }


        AdminKeys? adminKey = await _context.AdminKeys.FirstOrDefaultAsync(a => a.AdminKey == Input.AdminKey);
        if(adminKey == null) {
          ModelState.AddModelError(string.Empty,"Invalid Admin Key.");
          return Page();
        }

        //have to do this to set the internal id, because it is required for the AdminKeys table
        _context.UserAccounts.Add(user);
        await _context.SaveChangesAsync(); 

        AdminKeys newKey = new AdminKeys{
        Account_Id = user.Account_Id
        };

        _context.AdminKeys.Add(newKey);
        await _context.SaveChangesAsync();
        return RedirectToPage("/Login");
    }
  }
}
