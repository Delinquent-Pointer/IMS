using IMS.Data;
using IMS.Interfaces;
using IMS.DataTransferObj;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using IMS.Models;
using IMS.Services;
using IMS.Attributes;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc.Filters;

namespace IMS.Pages
{
    public class AccountSettingsModel : PageModel
    {
        //private readonly AppDbContext _appDbContext;
        private readonly UserAccountService _userAccountService;

        public AccountSettingsModel(UserAccountService userAccountService)
        {
            _userAccountService = userAccountService;
            //Debug.WriteLine("Constructor hit");
        }

        public class UsernameChangeInput
        {
            [Required]
            public string? Username { get; set; }
        }

        public class PasswordChangeInput
        {
            [Required]
            [PasswordReqs]
            public string? NewPassword { get; set; }

            [Required]
            [Compare("NewPassword")]
            public string? ConfirmPassword { get; set; }
        }

        public class AdminKeyChangeInput
        {
            [Required]
            public string? NewKey { get; set; }
        }


        [BindProperty]
        public AdminKeyChangeInput AdminKeyInput { get; set; }

        [BindProperty]
        public UsernameChangeInput UsernameInput { get; set; }

        [BindProperty]
        public PasswordChangeInput PasswordInput { get; set; }

        [BindProperty]
        [Required] //because the way we're manually adding this to the form with javascript, required will fail even when it is input, so we have to check it manually.
        public string CurrentPassword { get; set; }



        public bool HasPerms { get; set; } = false;
        public UserDto Account { get; set; }

        public string Key { get; set; } = "";

        public override async Task OnPageHandlerExecutionAsync(
            PageHandlerExecutingContext context,
            PageHandlerExecutionDelegate next)
        {
            var json = HttpContext.Session.GetString("User");
            if (string.IsNullOrEmpty(json))
            {
                HttpContext.Session.Clear();
                HttpContext.Session.SetString("LoginRedirectError", "An error has occured with your session information. Please login again.");
                context.Result = RedirectToPage("/Login");
                return;
            }
            Account = UserDto.FromJson(json);

            if (Account.Is_IT_User)
            {
                Key = (await _userAccountService.FindAdminKeyAsync(Account.Account_Id)).AdminKey;
            }

            HasPerms = (HttpContext.Session.GetInt32("ITPerms") ?? 0) == 1;

            await next.Invoke();
        }

        //needed to allow multiple different bound properties, without this any missing input from any form triggers modelstate invalidation
        private void RemoveUnrelatedInput(params string[] keys)
        {
            foreach (var key in keys)
                ModelState.Remove(key);
        }

        public async Task<IActionResult> OnPostChangeUsername()
        {

            RemoveUnrelatedInput("NewPassword", "ConfirmPassword", "NewKey");

            foreach (var key in ModelState.Keys)
            {
                var entry = ModelState[key];
                if (entry.Errors.Count > 0)
                {
                    Debug.WriteLine($"[ModelState] {key}: {string.Join(", ", entry.Errors.Select(e => e.ErrorMessage))}");
                }
            }

            if (!ModelState.IsValid)
                return Page();

            if (!await _userAccountService.VerifyPassword(Account.Account_Id, CurrentPassword))
            {
                ModelState.AddModelError("", "Incorrect password.");
                return Page();
            }

            if (!await _userAccountService.IsUsernameUniqueAsync(UsernameInput.Username))
            {
                ModelState.AddModelError("UsernameInput.Username", "Username already exists.");
                return Page();
            }

            await _userAccountService.UpdateUsernameAsync(Account.Account_Id, UsernameInput.Username);
            Account.Username = UsernameInput.Username;
            HttpContext.Session.SetString("User", Account.ToJson());
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostChangePassword()
        {
            RemoveUnrelatedInput("Username", "NewKey");

            if (!ModelState.IsValid)
            {
                TempData["ShowPasswordForm"] = true;
                return Page();
            }
            if (!await _userAccountService.VerifyPassword(Account.Account_Id, CurrentPassword))
            {
                ModelState.AddModelError("", "Incorrect password.");
                return Page();
            }
            await _userAccountService.UpdatePasswordAsync(Account.Account_Id, PasswordInput.NewPassword);
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostChangeAdminKey()
        {
            RemoveUnrelatedInput("Username", "NewPassword", "ConfirmPassword");
            if (!ModelState.IsValid)
                return Page();

            if (!await _userAccountService.VerifyPassword(Account.Account_Id, CurrentPassword))
            {
                ModelState.AddModelError("", "Incorrect password.");
                return Page();
            }
            await _userAccountService.UpdateAdminKeyAsync(Account.Account_Id, AdminKeyInput.NewKey);

            return RedirectToPage();
        }
    }
}
