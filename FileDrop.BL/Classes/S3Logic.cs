using FileDrop.BL.Interfaces;
using FileDrop.Services.Interfaces;
using FileDrop.TL.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FileDrop.BL.Classes
{
   public class S3Logic : IS3Logic
   {
      private readonly IS3Service _s3Service;

      public S3Logic(IS3Service s3Service)
      {
         _s3Service = s3Service;
      }

      public async Task<IList<S3ObjectModel>> GetFilesByBucketNameAsync(string bucketName)
      {
         if (string.IsNullOrWhiteSpace(bucketName))
         {
            return null;
         }

         return await _s3Service.ListObjectsAsync(bucketName);
      }

      public async Task<S3Response> GetFileWithoutPathAsync(string bucketName, string fileName)
      {
         return await _s3Service.GetFileWithoutPathAsync(bucketName, fileName);
      }

      public async Task<AddFileResponse> UploadFilesAsync(string bucketName, IList<IFormFile> formFiles)
      {
         return await _s3Service.UploadFilesAsync(bucketName, formFiles);
      }
   }
}
