using FileDrop.BL.Interfaces;
using FileDrop.ControllerHelpers.Interfaces;
using FileDrop.Models;
using FileDrop.TL.Helpers;
using FileDrop.TL.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FileDrop.Controllers
{
   [ApiController]
   [Route("[controller]")]
   public class AccountController : ControllerBase
   {
      private readonly IAccountControllerHelper _accountControllerHelper;
      private readonly IS3Logic _s3Logic;

      public AccountController(IAccountControllerHelper accountControllerHelper, IS3Logic s3Logic)
      {
         _accountControllerHelper = accountControllerHelper;
         _s3Logic = s3Logic;
      }

      [HttpPost]
      [Route("Login")]
      public async Task<ApiResponse> Login([FromBody] AccountViewModel accountViewModel)
      {
         if (accountViewModel == null || string.IsNullOrWhiteSpace(accountViewModel.Username) || string.IsNullOrWhiteSpace(accountViewModel.Password))
         {
            return new ApiResponse
            {
               IsCompletedSuccesfully = false,
               StatusCode = 400,
               Message = "Username or password cannot be null"
            };
         }

         accountViewModel.Password = ControllerHelper.EncryptPassword(accountViewModel.Password);
         ApiResponse apiResult = await _accountControllerHelper.LoginAsync(accountViewModel);

         return apiResult;
      }

      [HttpPost]
      [Route("Register")]
      public async Task<ApiResponse> Register([FromBody] AccountViewModel accountViewModel)
      {
         ApplicationResult validateModel = _accountControllerHelper.ValidateModel(accountViewModel);
         if (!validateModel.IsCompletedSuccesfully)
         {
            return new ApiResponse
            {
               StatusCode = 400,
               IsCompletedSuccesfully = false,
               Message = validateModel.Message
            };
         }

         accountViewModel.Password = ControllerHelper.EncryptPassword(accountViewModel.Password);
         accountViewModel.RoleName = "User";
         S3Response response = await _s3Logic.CreateUserBucketAsync(accountViewModel.Username);
         if ((int)response.StatusCode == 200)
         {
            accountViewModel.UserBucketName = response.Message;
         }
         else
         {
            return new ApiResponse { StatusCode = 500, Message = response.Message };
         }

         ApiResponse apiResult = await _accountControllerHelper.RegisterAsync(accountViewModel);

         return apiResult;
      }
      [HttpGet]
      [Route("IsUserLoggedIn")]
      public bool IsUserLoggedIn()
      {
         return Request.Cookies["fileDropAuthenticationToken"] != null;
      }
   }
}
