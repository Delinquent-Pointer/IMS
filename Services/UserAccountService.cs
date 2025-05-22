using IMS.Data;
using IMS.Models;
using IMS.Interfaces;
using Microsoft.EntityFrameworkCore;
namespace IMS.Services
{
    public class UserAccountService: IHashPasswords
    {
        private readonly AppDbContext _context;
        public UserAccountService(AppDbContext context)
        {
            _context = context;
        }


        public async Task<bool> UploadUserAccountAsync(string username, string password, bool isITUser, string? adminKey = "")
        {
            if(!await IsUsernameUniqueAsync(username)) throw new ArgumentException("Username already exists.");

            var hashedPassword = IHashPasswords.HashPassword(password);
            var user = new UserAccount
            {
                Username = username,
                Password_Hash = hashedPassword,
                Is_IT_User = isITUser
            };
            if (isITUser)
            {
                var key = new AdminKeys
                {
                    Account_Id = user.Account_Id,
                };
                if (!string.IsNullOrEmpty(adminKey)) key.AdminKey = adminKey;

                await _context.AdminKeys.AddAsync(key);
            }
            await _context.UserAccounts.AddAsync(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UploadUserAccountAsync(UserAccount user, AdminKeys? adminKey = null)
        {

            if (!await IsUsernameUniqueAsync(user.Username)) throw new ArgumentException("Username already exists.");

            if (user.Is_IT_User)
            {
                if (adminKey == null)
                {
                    adminKey = new AdminKeys
                    {
                        Account_Id = user.Account_Id
                    };
                }
                await _context.AdminKeys.AddAsync(adminKey);
            }
            await _context.UserAccounts.AddAsync(user);
            await _context.SaveChangesAsync();
            return true;
        }


        public async Task<bool> VerifyPassword(int accountId, string password)
        {
            var user = await _context.UserAccounts.FindAsync(accountId);
            if (user == null) throw new ArgumentException("User not found.");


            return IHashPasswords.VerifyPassword(password, user.Password_Hash);
        }

        public async Task<bool> UpdateUsernameAsync(int accountId, string newUsername)
        {
            var user = await _context.UserAccounts.FindAsync(accountId);
            if (user == null) throw new ArgumentException("User not found.");

            if (!await IsUsernameUniqueAsync(newUsername)) throw new ArgumentException("Username already exists.");
            
            user.Username = newUsername;
            await _context.SaveChangesAsync();
            
            return true;
        }

        public async Task<bool> UpdatePasswordAsync(int accountId, string newPassword)
        {
            var user = await _context.UserAccounts.FindAsync(accountId);
            if (user == null) throw new ArgumentException("User not found.");
            
            user.Password_Hash = IHashPasswords.HashPassword(newPassword);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateAdminKeyAsync(int accountId, string newKey)
        {
            var adminKey = await _context.AdminKeys.FirstOrDefaultAsync(a => a.Account_Id == accountId);
            if (adminKey == null) throw new ArgumentException("Admin key not found.");
            
            adminKey.AdminKey = newKey;
            await _context.SaveChangesAsync();
            
            return true;
        }

        public async Task<AdminKeys> FindAdminKeyAsync(int userId)
        {
            var adminKey = await _context.AdminKeys.FirstOrDefaultAsync(a => a.Account_Id == userId);
            if (adminKey == null) throw new ArgumentException("Admin key not found.");

            return adminKey;
        }


        public async Task<bool> DeleteUserAccountAsync(int accountId)
        {
            var user = await _context.UserAccounts.FindAsync(accountId);
            if (user == null) throw new ArgumentException("User not found.");

            _context.UserAccounts.Remove(user);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> IsUsernameUniqueAsync(string username)
        {
            return ! await _context.UserAccounts.AnyAsync(u => u.Username.ToLower() == username.Trim().ToLower());
        }




    }
}
