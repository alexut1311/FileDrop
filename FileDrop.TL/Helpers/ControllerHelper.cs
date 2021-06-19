using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace FileDrop.TL.Helpers
{
   public class ControllerHelper
   {
      public static string EncryptPassword(string password)
      {
         byte[] data = Encoding.ASCII.GetBytes(password);
         data = new SHA256Managed().ComputeHash(data);
         string hash = Encoding.ASCII.GetString(data);
         return hash;
      }
      public static string GetStringRight(ClaimsPrincipal user, string stringRightName)
      {
         try
         {
            List<Claim> userClaims = user.Claims.ToList();

            string rightClaim = userClaims.FirstOrDefault(x => x.Type == stringRightName).Value;

            return rightClaim;
         }
         catch
         {
            return null;
         }
      }
      public static int GetIntRight(ClaimsPrincipal user, string intRightName)
      {
         List<Claim> userClaims = user.Claims.ToList();

         string rightClaim = userClaims.FirstOrDefault(x => x.Type == intRightName).Value;
         bool tryGetRight = int.TryParse(rightClaim, out int right);
         if (tryGetRight == false)
         {
            throw new Exception($"Cannot parse {intRightName} {rightClaim} to int.");
         }

         return right;
      }
   }
}
