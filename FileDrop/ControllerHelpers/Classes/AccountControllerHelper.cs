using FileDrop.ControllerHelpers.Interfaces;
using FileDrop.Models;
using FileDrop.TL.Models;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FileDrop.ControllerHelpers.Classes
{
   public class AccountControllerHelper : IAccountControllerHelper
   {
      public async Task<ApiResponse> LoginAsync(AccountViewModel accountViewModel)
      {
         try
         {
            HttpClient client = new HttpClient
            {
               BaseAddress = new Uri("https://localhost:44343/")
            };

            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = await client.GetAsync($"Authentication/Login?username={accountViewModel.Username}&password={accountViewModel.Password}");
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

      public async Task<ApiResponse> RegisterAsync(AccountViewModel accountViewModel)
      {
         try
         {
            HttpClient client = new HttpClient
            {
               BaseAddress = new Uri("https://localhost:44343/")
            };

            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = await client.PostAsJsonAsync("Authentication/Register", accountViewModel);
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

      public ApplicationResult ValidateModel(AccountViewModel accountViewModel)
      {
         if (string.IsNullOrWhiteSpace(accountViewModel.FirstName))
         {
            return new ApplicationResult { IsCompletedSuccesfully = false, Message = "Invalid first name." };
         }

         if (string.IsNullOrWhiteSpace(accountViewModel.LastName))
         {
            return new ApplicationResult { IsCompletedSuccesfully = false, Message = "Invalid last name." };
         }

         if (string.IsNullOrWhiteSpace(accountViewModel.Username))
         {
            return new ApplicationResult { IsCompletedSuccesfully = false, Message = "Invalid username." };
         }

         if (string.IsNullOrWhiteSpace(accountViewModel.Password))
         {
            return new ApplicationResult { IsCompletedSuccesfully = false, Message = "Invalid password." };
         }

         if (string.IsNullOrWhiteSpace(accountViewModel.Email))
         {
            return new ApplicationResult { IsCompletedSuccesfully = false, Message = "Invalid email." };
         }
         else
         {
            bool isEmail = Regex.IsMatch(accountViewModel.Email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
            if (!isEmail)
            {
               return new ApplicationResult { IsCompletedSuccesfully = false, Message = "Incorrect format for email." };
            }
         }

         if (string.IsNullOrWhiteSpace(accountViewModel.RePassword))
         {
            return new ApplicationResult { IsCompletedSuccesfully = false, Message = "Invalid re password." };
         }
         else
         {
            if (accountViewModel.RePassword != accountViewModel.Password)
            {
               return new ApplicationResult { IsCompletedSuccesfully = false, Message = "Password must match." };
            }
         }

         return new ApplicationResult { IsCompletedSuccesfully = true, Message = "Success." };
      }
   }
}
