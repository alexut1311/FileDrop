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
   }
}
