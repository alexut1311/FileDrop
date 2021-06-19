using FileDrop.BL.Interfaces;
using FileDrop.TL;
using FileDrop.TL.Helpers;
using FileDrop.TL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace FileDrop.Controllers
{
   [ApiController]
   [Authorize]
   [Route("[controller]")]
   public class FileController : ControllerBase
   {
      private readonly IS3Logic _s3Logic;
      private readonly IFileLogic _fileLogic;

      public FileController(IS3Logic s3Logic, IFileLogic fileLogic)
      {
         _s3Logic = s3Logic;
         _fileLogic = fileLogic;
      }

      [HttpGet]
      [Route("GetFiles")]
      public IEnumerable<FileDTO> GetFiles()
      {
         IList<FileDTO> filesDTO = _fileLogic.GetAllFiles(ControllerHelper.GetIntRight(User, "UserId"));
         return filesDTO;
      }

      [HttpPost]
      [Route("UploadFile")]
      public async Task<IActionResult> UploadFile()
      {
         List<IFormFile> files = Request.Form.Files.ToList();
         //Files are uploaded asynchronous, so they are uploaded one file per thread
         AddFileResponse result = await _s3Logic.UploadFilesAsync(ControllerHelper.GetStringRight(User, "UserBucketName"), files);
         IFormFile file = files.FirstOrDefault();
         if (result.StatusCode == HttpStatusCode.OK)
         {
            FileDTO fileDTO = new FileDTO
            {
               FileName = file.FileName,
               FileSize = FileHelper.FormatSize(file.Length),
               UploadDate = $"{DateTime.Now.ToLongDateString()} at {DateTime.Now.ToLongTimeString()}",
               StorageClass = "STANDARD",
               PreviewURL = result.PreSignedUrl.FirstOrDefault(),
               UserId = ControllerHelper.GetIntRight(User, "UserId")
            };
            _fileLogic.AddFileToDB(fileDTO);
         }
         return StatusCode((int)result.StatusCode, result.Message);
      }

      [HttpPost]
      [Route("DownloadFiles")]
      public async Task<IActionResult> DownloadFiles(string fileName)
      {
         S3Response result = await _s3Logic.GetFileWithoutPathAsync(ControllerHelper.GetStringRight(User, "UserBucketName"), fileName);

         return File(result.File, "application/octet-stream", fileName);
      }
   }
}
