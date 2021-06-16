using FileDrop.TL.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileDrop.BL.Interfaces
{
   public interface IS3Logic
   {
      Task<IList<S3ObjectModel>> GetFilesByBucketNameAsync(string bucketName);
      Task<AddFileResponse> UploadFilesAsync(string bucketName, IList<IFormFile> formFiles);
      Task<S3Response> GetFileWithoutPathAsync(string bucketName, string fileName);
   }
}
