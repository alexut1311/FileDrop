﻿using FileDrop.Models;
using FileDrop.TL.Models;
using System.Threading.Tasks;

namespace FileDrop.ControllerHelpers.Interfaces
{
   public interface IAccountControllerHelper
   {
      Task<ApiResponse> LoginAsync(AccountViewModel accountViewModel);
   }
}
