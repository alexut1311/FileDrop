using FileDrop.ControllerHelpers.Interfaces;
using FileDrop.Models;
using FileDrop.TL.Helpers;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FileDrop.Controllers
{
   [ApiController]
   [Route("[controller]")]
   public class AccountController : ControllerBase
   {
      private readonly IAccountControllerHelper _accountControllerHelper;

      public AccountController(IAccountControllerHelper accountControllerHelper)
      {
         _accountControllerHelper = accountControllerHelper;
      }

      [HttpPost]
      [Route("Login")]
      public async Task<IActionResult> Login([FromBody] AccountViewModel accountViewModel)
      {
         if (accountViewModel == null || string.IsNullOrWhiteSpace(accountViewModel.UserName) || string.IsNullOrWhiteSpace(accountViewModel.UserName))
         {
            return BadRequest("Username or password cannot be null");
         }

         accountViewModel.Password = ControllerHelper.EncryptPassword(accountViewModel.Password);
         TL.Models.ApiResponse apiResult = await _accountControllerHelper.LoginAsync(accountViewModel);

         return StatusCode(apiResult.StatusCode, apiResult.Message);
      }
   }
}
