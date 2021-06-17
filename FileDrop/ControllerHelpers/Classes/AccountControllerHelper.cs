using FileDrop.ControllerHelpers.Interfaces;
using FileDrop.Models;
using FileDrop.TL.Models;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
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

            HttpResponseMessage response = await client.GetAsync($"Authentication/Login?username={accountViewModel.UserName}&password={accountViewModel.Password}");
            if (response.IsSuccessStatusCode)
            {
               ApiResponse apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse>();
               return apiResponse;
            }
            client.Dispose();
            return new ApiResponse { StatusCode = (int)response.StatusCode, Message = response.ReasonPhrase };

         }
         catch (Exception)
         {
            throw;
         }
      }
   }
}
