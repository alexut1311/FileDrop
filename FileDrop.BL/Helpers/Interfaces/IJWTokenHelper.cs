using FileDrop.TL.Models;

namespace FileDrop.BL.Helpers.Interfaces
{
   public interface IJWTokenHelper
   {
      ValidateTokenModel ValidateToken(string userToken);
   }
}
