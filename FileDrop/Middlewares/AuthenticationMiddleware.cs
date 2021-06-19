using FileDrop.BL.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace FileDrop.Middlewares
{
   public class AuthenticationMiddleware
   {
      private readonly RequestDelegate _next;

      public AuthenticationMiddleware(RequestDelegate next)
      {
         _next = next;
      }

      // IMyScopedService is injected into Invoke
      public async Task Invoke(HttpContext httpContext, IAuthenticationLogic authenticationLogic)
      {
         await authenticationLogic.AuthenticateUserAsync(httpContext);
         await _next(httpContext);
      }
   }
}
