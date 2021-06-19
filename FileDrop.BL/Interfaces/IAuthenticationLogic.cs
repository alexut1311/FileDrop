using FileDrop.TL.Models;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace FileDrop.BL.Interfaces
{
   public interface IAuthenticationLogic
   {
      void SaveJWToken(HttpContext httpContext, ApiResponse apiResult);
      Task AuthenticateUserAsync(HttpContext httpContext);
   }
}
