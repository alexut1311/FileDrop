using FileDrop.TL.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FileDrop.Services.Interfaces
{
   public interface IS3Service
   {
      Task<S3Response> CreateBucketAsync(string bucketName);
      Task<S3Response> GetFileWithoutPathAsync(string bucketName, string fileName);
      Task<IList<S3ObjectModel>> ListObjectsAsync(string bucketName);
      Task<AddFileResponse> UploadFilesAsync(string bucketName, IList<IFormFile> formFiles);
      Task<S3Response> DeleteObjectAsync(string bucketName, string fileName);
      Task<IList<S3BucketModel>> ListBucketsAsync();
      Task<S3Response> DeleteBucketAsync(string bucketName);
      Task<S3Response> DoesBucketExists(string bucketName);
   }
}
