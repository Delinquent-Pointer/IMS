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

namespace IMS.Pages {
  //[RequireLogin]
  [AllowAnonymous]
  public class LoginModel:PageModel {
    private readonly AppDbContext _context;

    public LoginModel(AppDbContext context,ILogger<LoginModel> logger) {
      _context = context;
      _logger = logger;
    }

    [BindProperty]
    public LoginInputModel Input { get; set; }
    public enum LoginErrorTypes { None, Redirect, NullPassword, AccountNotFound }
    public LoginErrorTypes LoginErrorType { get; set; } = LoginErrorTypes.None;
    public bool IsDatabaseOnline { get; set; } = false;

    public class LoginInputModel {
      [Required]
      public required string Username { get; set; }

      [Required]
      [DataType(DataType.Password)]
      public required string Password { get; set; }
    }

    private readonly ILogger<LoginModel> _logger;

    public async Task OnGetAsync() {
      // Simulate database still waking
      // IsDatabaseOnline = false;
      // Try waking up the DB (page refreshes every 15 seconds)
      IsDatabaseOnline = await PingDatabaseAsync();
      //IsDatabaseOnline = true;

      if(!IsDatabaseOnline) {
        // Skip all login error setup if DB is offline
        return;
      }

      // Only set login errors if DB is confirmed online
      String? loginError = HttpContext.Session.GetString("LoginRedirectError");
      if(loginError != null) {
        ModelState.AddModelError(string.Empty,loginError);
        LoginErrorType = LoginErrorTypes.Redirect;
        HttpContext.Session.Remove("LoginRedirectError");
      }
    }

    public async Task<IActionResult> OnPostAsync() {
      IsDatabaseOnline = true; //
      _logger.LogInformation("[Login] ModelState.IsValid = {IsValid}",ModelState.IsValid);
      _logger.LogInformation("[Login] Username = {Username}, PasswordLength = {PasswordLength}",Input?.Username,Input?.Password?.Length);

      if(!ModelState.IsValid)
        return Page();

      if(Input.Password == null) {
        ModelState.AddModelError(string.Empty,"Password cannot be null.");
        LoginErrorType = LoginErrorTypes.NullPassword;
        return Page();
      }

      try {
        _logger.LogInformation("[Login] Querying user: {Username}",Input.Username);

        string hashedPassword = HashPassword(Input.Password);

        var user = await _context.UserAccounts
          .FirstOrDefaultAsync(u => u.Username == Input.Username && u.Password_Hash == hashedPassword);

        if(user == null) {
          _logger.LogInformation("[Login] User not found or password incorrect.");
          ModelState.AddModelError(string.Empty,"Login Failed: Account info not recognised.");
          LoginErrorType = LoginErrorTypes.AccountNotFound;
          return Page();
        }

        if (!user.Verified) {
            _logger.LogInformation("[Login] User account not verified.");
            ModelState.AddModelError(string.Empty, "Login Failed: Account is not verified.");
            return Page();
        }

        var userDto = new UserDto {
          Account_Id = user.Account_Id,
          Username = user.Username,
          Is_IT_User = user.Is_IT_User
        };

        HttpContext.Session.SetString("User",System.Text.Json.JsonSerializer.Serialize(userDto));
        HttpContext.Session.SetInt32("ITPerms",user.Is_IT_User ? 1 : 0);

        return Redirect(user.Is_IT_User ? "/ITManagerLanding" : "/InventoryManagerLanding");
      } catch(Exception ex) {
        IsDatabaseOnline = false;
        _logger.LogError(ex,"[Login] Exception during login");
        HttpContext.Session.SetString("LoginRedirectError","Database went offline during login. Please wait and try again.");
        return RedirectToPage("/Login");
      }
    }

    private async Task<bool> PingDatabaseAsync() {
      try {
        // await _context.Database.ExecuteSqlRawAsync("SELECT 1");
        // return true;
        return await _context.Database.CanConnectAsync();
      } catch {
        return false;
      }
    }

    private static string HashPassword(string password) {
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