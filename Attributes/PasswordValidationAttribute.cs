using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace IMS.Attributes
{
    public class PasswordReqsAttribute : ValidationAttribute
    {

        //defining a custom attribute for password validation
        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {

            if (value == null) return new ValidationResult("Password is required."); //this should never happen since pw is already marked as required.

            string password = value.ToString()!;

            Dictionary<string, string> passwordReqs = new Dictionary<string, string> {
                { @"^\S*$", "Password cannot contain whitespace." },
                { @".*[A-Z]", "Password must contain at least one uppercase letter." },
                { @".*[a-z]", "Password must contain at least one lowercase letter." },
                { @".*\d", "Password must contain at least one digit." },
                { @".*[)(\]\[}{;:,?!.'""\/-]", "Password must contain at least one special character." }
            };

            List<string> errors = passwordReqs
              .Where(req => !Regex.IsMatch(password, req.Key))
              .Select(req => req.Value)
              .ToList();

            if (errors.Count > 0) return new ValidationResult(string.Join("\n", errors));

            return ValidationResult.Success!;

        }
    }
}
