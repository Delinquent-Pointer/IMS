using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using IMS.Data;
using IMS.DataTransferObj;
using IMS.Models;
using IMS.Attributes;

namespace IMS.Pages{
  //[RequireLogin]
  [AllowAnonymous]
  public class LoginModel:PageModel{
    private readonly AppDbContext _context;

    public LoginModel(AppDbContext context){
      _context = context;
    }

    [BindProperty]
    public LoginInputModel Input {get; set;}


    public enum LoginErrorTypes{None, Redirect, NullPassword, AccountNotFound}

    public LoginErrorTypes LoginErrorType {get; set;} = LoginErrorTypes.None;

    public class LoginInputModel {
      [Required]
      public required string Username {get; set;}

      [Required]
      [DataType(DataType.Password)]
      public required string Password {get; set;}
    }

    public void OnGet(){
      String? loginError = HttpContext.Session.GetString("LoginRedirectError");
      if(loginError != null) {
        ModelState.AddModelError(string.Empty, loginError);
        LoginErrorType = LoginErrorTypes.Redirect;
        HttpContext.Session.Remove("LoginRedirectError");
      }
    }

    public async Task<IActionResult> OnPostAsync() {
      if(!ModelState.IsValid){
        return Page();
      }
      
      if (Input.Password == null){
        ModelState.AddModelError(string.Empty, "Password cannot be null.");
        LoginErrorType = LoginErrorTypes.NullPassword;
        return Page();
      }

      string hashedPassword = HashPassword(Input.Password);
      UserAccount? user = await _context.UserAccounts
          .FirstOrDefaultAsync(u => u.Username == Input.Username && u.Password_Hash == hashedPassword);

      if(user == null){
        ModelState.AddModelError(string.Empty,"Login Failed: Account info not recognised.");
        LoginErrorType = LoginErrorTypes.AccountNotFound;
        return Page();
      }

      UserDto userDto = new UserDto{
        Account_Id = user.Account_Id,
        Username = user.Username,
        Is_IT_User = user.Is_IT_User
      };

      String userJSON = System.Text.Json.JsonSerializer.Serialize(userDto);
      HttpContext.Session.SetString("User", userJSON);
      HttpContext.Session.SetInt32("ITPerms", user.Is_IT_User ? 1 : 0);

      if(user.Is_IT_User){
        return Redirect("/ITManagerLanding");
      }

      return Redirect("/InventoryManagerLanding");
    }

    private static string HashPassword(string password){
      using(var sha256 = SHA256.Create()) {
        byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        StringBuilder builder = new StringBuilder();
        for(int i = 0;i < bytes.Length;i++) {
          builder.Append(bytes[i].ToString("x2"));
        }
        return builder.ToString();
      }
    }
  }
}