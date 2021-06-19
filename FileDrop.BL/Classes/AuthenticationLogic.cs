using FileDrop.BL.Helpers.Interfaces;
using FileDrop.BL.Interfaces;
using FileDrop.TL.Helpers;
using FileDrop.TL.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FileDrop.BL.Classes
{
   public class AuthenticationLogic : IAuthenticationLogic
   {
      public IJWTokenHelper _jWTokenHelper;
      private readonly string fileDropAuthenticationToken = "fileDropAuthenticationToken";
      private readonly string fileDropAuthenticationRefreshToken = "fileDropAuthenticationRefreshToken";

      public AuthenticationLogic(IJWTokenHelper jWTokenHelper)
      {
         _jWTokenHelper = jWTokenHelper;
      }

      public async Task AuthenticateUserAsync(HttpContext httpContext)
      {
         string userToken = GetCookie(httpContext, fileDropAuthenticationToken);
         if (!string.IsNullOrEmpty(userToken))
         {
            SetAuthenticationCookies(httpContext, userToken);
            SetAuthentication(httpContext, userToken);
         }
         else
         {
            string userRefreshToken = GetCookie(httpContext, fileDropAuthenticationRefreshToken);
            if (!string.IsNullOrEmpty(userRefreshToken))
            {
               ApiResponse apiResponse = await GetJWTokenAsync(userRefreshToken);
               if (apiResponse.IsCompletedSuccesfully && apiResponse.StatusCode == 200)
               {
                  SaveJWToken(httpContext, apiResponse);
                  SetAuthentication(httpContext, apiResponse.Message);
               }
               else
               {
                  RemoveCookie(httpContext, fileDropAuthenticationRefreshToken, userRefreshToken);
               }
            }
         }
      }

      public void SaveJWToken(HttpContext httpContext, ApiResponse apiResult)
      {
         if (apiResult.IsCompletedSuccesfully && apiResult.StatusCode == 200)
         {
            string userToken = apiResult.Message;
            if (!string.IsNullOrEmpty(userToken))
            {
               SetAuthenticationCookies(httpContext, userToken);
            }
         }
      }

      private void SetAuthentication(HttpContext httpContext, string userToken)
      {
         ValidateTokenModel validtoken = _jWTokenHelper.ValidateToken(userToken);
         if (validtoken.IsValid)
         {
            SetAuthenticationHeader(httpContext, userToken);
         }
         else
         {
            RemoveCookie(httpContext, fileDropAuthenticationToken, userToken);
         }
      }

      private void SetAuthenticationHeader(HttpContext httpContext, string userToken)
      {
         if (!string.IsNullOrEmpty(userToken))
         {
            string authHeader = $"Bearer {userToken}";
            if (httpContext.Request.Headers.ContainsKey("Authorization"))
            {
               httpContext.Request.Headers["Authorization"] = authHeader;
            }
            else
            {
               httpContext.Request.Headers.Add("Authorization", authHeader);
            }
         }
      }

      private async Task<ApiResponse> GetJWTokenAsync(string userRefreshToken)
      {
         try
         {
            HttpClient client = new HttpClient
            {
               BaseAddress = new Uri("https://localhost:44343/")
            };

            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = await client.GetAsync($"Authentication/RefreshLogin?refreshToken={userRefreshToken}");
            client.Dispose();
            if (response.IsSuccessStatusCode)
            {
               ApiResponse apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse>();
               apiResponse.StatusCode = (int)response.StatusCode;
               return apiResponse;
            }
            else
            {
               string errorMessage = await response.Content.ReadFromJsonAsync<string>();
               return new ApiResponse { StatusCode = (int)response.StatusCode, Message = errorMessage };
            }

         }
         catch (Exception)
         {
            throw;
         }
      }

      private void SetAuthenticationCookies(HttpContext httpContext, string userToken)
      {
         ValidateTokenModel validtoken = _jWTokenHelper.ValidateToken(userToken);
         if (validtoken.IsValid)
         {
            JwtSecurityToken token = (JwtSecurityToken)validtoken.Token;
            string userRefreshToken = GetRefreshToken(token);
            SetJWTokenCookie(httpContext, userToken);
            SetRefreshTokenCookie(httpContext, userRefreshToken);
         }
         else
         {
            RemoveCookie(httpContext, fileDropAuthenticationToken, userToken);
         }
      }

      private string GetRefreshToken(JwtSecurityToken token)
      {
         _ = token ?? throw new ArgumentNullException(nameof(token));

         try
         {
            List<Claim> userClaims = token.Claims.ToList();

            return userClaims.FirstOrDefault(x => x.Type == "RefreshToken").Value.ToString();
         }
         catch (Exception e)
         {
            Console.WriteLine("GetRefreshToken exception " + e.Message);
            throw;
         }
      }

      private void SetRefreshTokenCookie(HttpContext httpContext, string userRefreshToken)
      {
         DateTime expirationDate = DateTime.Now.AddDays(1);
         SetCookie(httpContext, fileDropAuthenticationRefreshToken, userRefreshToken, expirationDate);
      }

      private void SetJWTokenCookie(HttpContext httpContext, string userToken)
      {
         DateTime expirationDate = DateTime.Now.AddMinutes(GetJWTokenExpirationMinutes());
         SetCookie(httpContext, fileDropAuthenticationToken, userToken, expirationDate);
      }

      private int GetJWTokenExpirationMinutes()
      {
         int.TryParse(ConfigurationHelper.GetKey("ExpirationMinutes", "appSettings"), out int expirationMinutes);
         return expirationMinutes;
      }

      private void SetCookie(HttpContext httpContext, string name, string value, DateTime expirationDate)
      {
         CookieOptions option = new CookieOptions()
         {
            Expires = expirationDate,
         };

         httpContext.Response.Cookies.Append(name, value, option);
      }

      private string GetCookie(HttpContext httpContext, string name)
      {
         return httpContext.Request.Cookies[name];
      }

      private void RemoveCookie(HttpContext httpContext, string name, string value)
      {
         CookieOptions option = new CookieOptions()
         {
            Expires = DateTime.Now.AddYears(-1),
         };

         httpContext.Response.Cookies.Append(name, value, option);
      }
   }
}
