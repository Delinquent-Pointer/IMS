using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace IMS.Attributes
{
    public class SKUReqsAttribute : ValidationAttribute
    {

        //defining a custom attribute for SKU formatting
        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {

            if (value == null) return ValidationResult.Success!; //SKU is optional

            string SKU = value.ToString()!;

            Dictionary<string, string> SKUReqs = new Dictionary<string, string> {
                { @"^[A-Z0-9-]*$", "SKUs must contain only capital letters, digits, and dashes." },
                { @"^\S+(-\S+)+$", "An SKU must have 1 category and at least 1 subcategory." },
                { @"^\S{1,5}(-\S*)*$", "The category: (*AAAA*-BBBB) of an SKU must be 1-5 characters long." },
                { @"^\S+(-\S{1,10})+$", "subcategories: (AAAA-*BBBB*) of an SKU must be 1-10 characters long." },
                { @"^\S+(-\S+){1,3}$", "An SKU can have at most 3 subcategories: (AAA-BBB-CCC-DDD)." }
            };

            List<string> errors = SKUReqs
              .Where(req => !Regex.IsMatch(SKU, req.Key))
              .Select(req => req.Value)
              .ToList();

            if (errors.Count > 0) return new ValidationResult(string.Join("\n", errors));

            return ValidationResult.Success!;

        }
    }
}
