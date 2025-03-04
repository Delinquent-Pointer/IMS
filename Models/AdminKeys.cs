using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;

namespace IMS.Models {
  public class AdminKeys {
  
     private static readonly char[] chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmnopqrstuvwxyz".ToCharArray();

    public static string GenKey(int length) {
       byte[] data = new byte[length];
      RandomNumberGenerator rng = RandomNumberGenerator.Create();
       
       rng.GetBytes(data);

        StringBuilder key = new StringBuilder(length);
        foreach (byte b in data)
        {
            key.Append(chars[b % chars.Length]);
        }

        rng.Dispose();

        return key.ToString();
    }
    [Key]
    public int It_Id { get; set; }

    [Required]
    public int Account_Id { get; set; }

    [Required]
    public string AdminKey {set; get;} = GenKey(8);
   
  }


}
