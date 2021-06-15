using FileDrop.BL.Interfaces;
using FileDrop.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileDrop.Controllers
{
   [ApiController]
   [Route("[controller]")]
   public class FileController : ControllerBase
   {
      private readonly IS3Logic _s3Logic;

      public FileController(IS3Logic s3Logic)
      {
         _s3Logic = s3Logic;
      }

      [HttpGet]
      [Route("GetFiles")]
      public async Task<IEnumerable<FileViewModel>> GetFiles()
      {
         var ceva = await _s3Logic.GetFilesByBucketNameAsync("create-bucket-api-test6");
         return new List<FileViewModel> {
            new FileViewModel {
               Id=1,
               FileName = "text.txt", 
               FileSize = "15 de mega" 
            } 
         };
      }
   }
}
