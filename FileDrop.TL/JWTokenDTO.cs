using System.Collections.Generic;

namespace FileDrop.TL
{
   public class JWTokenDTO
   {
      public int ExpirationMinutes { get; set; }
      public string SigningKey { get; set; }
      public string EncryptyingSecurityKey { get; set; }
      public string Issuer { get; set; }
      public string Audience { get; set; }
      public Dictionary<string, object> UserClaims { get; set; }
   }
}
