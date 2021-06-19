using Microsoft.IdentityModel.Tokens;

namespace FileDrop.TL.Models
{
   public class ValidateTokenModel
   {
      public SecurityToken Token { get; set; }
      public bool IsValid { get; set; }
   }
}
