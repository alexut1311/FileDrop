using FileDrop.BL.Interfaces;
using FileDrop.Models;
using FileDrop.TL;
using FileDrop.TL.Helpers;
using FileDrop.TL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace FileDrop.Controllers
{
   [ApiController]
   [Route("[controller]")]
   public class FileController : ControllerBase
   {
      private readonly IS3Logic _s3Logic;
      private readonly IFileLogic _fileLogic;
      private readonly static string bucketName = "create-bucket-api-test6";

      public FileController(IS3Logic s3Logic, IFileLogic fileLogic)
      {
         _s3Logic = s3Logic;
         _fileLogic = fileLogic;
      }

      [HttpGet]
      [Route("GetFiles")]
      public IEnumerable<FileDTO> GetFiles()
      {
         IList<FileDTO> filesDTO = _fileLogic.GetAllFiles();
         return filesDTO;
      }

      [HttpPost]
      [Route("UploadFile")]
      public async Task<IActionResult> UploadFile()
      {
         List<IFormFile> files = Request.Form.Files.ToList();
         //Files are uploaded asynchronous, so they are uploaded one file per thread
         var result = await _s3Logic.UploadFilesAsync(bucketName, files);
         var file = files.FirstOrDefault();
         if (result.StatusCode == HttpStatusCode.OK)
         {
            FileDTO fileDTO = new FileDTO
            {
               FileName = file.FileName,
               FileSize = FileHelper.FormatSize(file.Length),
               UploadDate = $"{DateTime.Now.ToLongDateString()} at {DateTime.Now.ToLongTimeString()}",
               StorageClass = "STANDARD",
               PreviewURL = result.PreSignedUrl.FirstOrDefault(),
               UserId = 1
            };
            _fileLogic.AddFileToDB(fileDTO);
         }
         return StatusCode((int)result.StatusCode, result.Message);
      }

      [HttpPost]
      [Route("DownloadFiles")]
      public async Task<IActionResult> DownloadFiles(string fileName)
      {
         var result = await _s3Logic.GetFileWithoutPathAsync(bucketName, fileName);

         return File(result.File, "application/octet-stream", fileName);
      }
   }
}
