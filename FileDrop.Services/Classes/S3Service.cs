using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Amazon.S3.Util;
using FileDrop.Services.Interfaces;
using FileDrop.TL.Helpers;
using FileDrop.TL.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Threading.Tasks;

namespace FileDrop.Services.Classes
{
   public class S3Service : IS3Service
   {

      #region Members

      private readonly IAmazonS3 _client;

      #endregion

      #region Constructors

      public S3Service(IAmazonS3 client)
      {
         _client = client;
      }

      #endregion

      #region Interface Members

      public async Task<S3Response> CreateBucketAsync(string bucketName)
      {
         try
         {
            if (bucketName == null)
            {
               return new S3Response()
               {
                  Message = $"Bucket name cannot be empty",
                  StatusCode = HttpStatusCode.BadRequest
               };
            }
            if (await AmazonS3Util.DoesS3BucketExistV2Async(_client, bucketName) == false)
            {
               PutBucketRequest putBucketRequest = new PutBucketRequest() { BucketName = bucketName, UseClientRegion = true };

               PutBucketResponse response = await _client.PutBucketAsync(putBucketRequest);

               return new S3Response()
               {
                  Message = $"Created bucked \"{bucketName}\" with requestId = {response.ResponseMetadata.RequestId}",
                  StatusCode = response.HttpStatusCode
               };
            }

            return new S3Response() { Message = $"Bucket {bucketName} already exists", StatusCode = HttpStatusCode.Conflict };
         }
         catch (AmazonS3Exception amazonS3Exception)
         {
            if (amazonS3Exception.ErrorCode != null &&
                (amazonS3Exception.ErrorCode.Equals("InvalidAccessKeyId") || amazonS3Exception.ErrorCode.Equals("InvalidSecurity")))
            {
               Console.WriteLine();
               return new S3Response() { Message = "Please check the provided AWS Credentials", StatusCode = amazonS3Exception.StatusCode };
            }
            else
            {
               return new S3Response()
               {
                  Message = "An error occurred when listing buckets with message " + amazonS3Exception.ErrorCode,
                  StatusCode = amazonS3Exception.StatusCode
               };
            }
         }
         catch (Exception e)
         {
            return new S3Response() { Message = e.Message, StatusCode = HttpStatusCode.InternalServerError };
         }
      }

      public async Task<S3Response> DeleteBucketAsync(string bucketName)
      {
         try
         {
            await _client.DeleteBucketAsync(bucketName);

            return new S3Response() { Message = $"Bucket \"{bucketName}\" was deleted.", StatusCode = HttpStatusCode.OK };
         }
         catch (AmazonS3Exception amazonS3Exception)
         {
            if (amazonS3Exception.ErrorCode != null &&
                (amazonS3Exception.ErrorCode.Equals("InvalidAccessKeyId") || amazonS3Exception.ErrorCode.Equals("InvalidSecurity")))
            {
               Console.WriteLine();
               return new S3Response() { Message = "Please check the provided AWS Credentials", StatusCode = amazonS3Exception.StatusCode };
            }
            else
            {
               return new S3Response()
               {
                  Message = "An error occurred when deleting the bucket, message " + amazonS3Exception.ErrorCode,
                  StatusCode = amazonS3Exception.StatusCode
               };
            }
         }
         catch (Exception e)
         {
            return new S3Response() { Message = e.Message, StatusCode = HttpStatusCode.InternalServerError };
         }
      }

      public async Task<S3Response> DeleteObjectAsync(string bucketName, string fileName)
      {
         try
         {
            DeleteObjectRequest deleteObjectRequest = new DeleteObjectRequest { BucketName = bucketName, Key = fileName };

            Console.WriteLine("Deleting an object");

            await _client.DeleteObjectAsync(deleteObjectRequest);

            return new S3Response() { Message = $"Deleted the file \"{fileName}\" with success", StatusCode = HttpStatusCode.OK };
         }
         catch (AmazonS3Exception amazonS3Exception)
         {
            if (amazonS3Exception.ErrorCode != null &&
                (amazonS3Exception.ErrorCode.Equals("InvalidAccessKeyId") || amazonS3Exception.ErrorCode.Equals("InvalidSecurity")))
            {
               Console.WriteLine();
               return new S3Response() { Message = "Please check the provided AWS Credentials", StatusCode = amazonS3Exception.StatusCode };
            }
            else
            {
               return new S3Response()
               {
                  Message = "An error occurred when deleting the file, message " + amazonS3Exception.ErrorCode,
                  StatusCode = amazonS3Exception.StatusCode
               };
            }
         }
         catch (Exception e)
         {
            return new S3Response() { Message = e.Message, StatusCode = HttpStatusCode.InternalServerError };
         }
      }

      public async Task<S3Response> GetFileWithoutPathAsync(string bucketName, string fileName)
      {
         try
         {
            string keyName = fileName;

            GetObjectRequest request = new GetObjectRequest() { BucketName = bucketName, Key = keyName };

            using (GetObjectResponse response = await _client.GetObjectAsync(request))
            {
               if (response.ResponseStream == null)
               {
                  return null;
               }
               return new S3Response()
               {
                  Message = "Please check the provided AWS Credentials",
                  File = response.ResponseStream,
                  ContentType = response.Headers.ContentType,
                  StatusCode = HttpStatusCode.OK
               };
            }
         }
         catch (AmazonS3Exception amazonS3Exception)
         {
            if (amazonS3Exception.ErrorCode != null &&
                (amazonS3Exception.ErrorCode.Equals("InvalidAccessKeyId") || amazonS3Exception.ErrorCode.Equals("InvalidSecurity")))
            {
               Console.WriteLine();
               return new S3Response() { Message = "Please check the provided AWS Credentials", StatusCode = amazonS3Exception.StatusCode };
            }
            else
            {
               return new S3Response()
               {
                  Message = "An error occurred when downloading the file, message " + amazonS3Exception.ErrorCode,
                  StatusCode = amazonS3Exception.StatusCode
               };
            }
         }
         catch (Exception e)
         {
            return new S3Response() { Message = e.Message, StatusCode = HttpStatusCode.InternalServerError };
         }
      }
      public async Task<IList<S3BucketModel>> ListBucketsAsync()
      {
         ListBucketsResponse response = await _client.ListBucketsAsync();

         int i = 1;
         IList<S3BucketModel> buckets = new List<S3BucketModel>();
         foreach (S3Bucket entry in response.Buckets)
         {
            buckets.Add(new S3BucketModel() { Id = i, Name = entry.BucketName, CreationDate = entry.CreationDate });
            i++;
         }

         return buckets;
      }

      public async Task<IList<S3ObjectModel>> ListObjectsAsync(string bucketName)
      {
         try
         {
            ListObjectsRequest request = new ListObjectsRequest() { BucketName = bucketName };
            ListObjectsResponse response = await _client.ListObjectsAsync(request);

            int i = 1;
            IList<S3ObjectModel> objects = new List<S3ObjectModel>();

            foreach (S3Object entry in response.S3Objects)
            {
               objects.Add(new S3ObjectModel()
               {
                  Id = i,
                  Name = entry.Key,
                  Size = FileHelper.FormatSize(entry.Size),
                  LastModified = entry.LastModified.ToString(CultureInfo.InvariantCulture),
                  OwnerDisplayName = entry.Owner.DisplayName,
                  OwnerId = entry.Owner.Id,
                  StorageClass = entry.StorageClass.Value
               });
               i++;
            }

            return objects;
         }
         catch (AmazonS3Exception amazonS3Exception)
         {
            if (amazonS3Exception.ErrorCode != null &&
                (amazonS3Exception.ErrorCode.Equals("InvalidAccessKeyId") || amazonS3Exception.ErrorCode.Equals("InvalidSecurity")))
            {
               Console.WriteLine("Please check the provided AWS Credentials");
            }
            else
            {
               Console.WriteLine("An error occurred when listing buckets with message {0}", amazonS3Exception.ErrorCode, amazonS3Exception);
            }

            return new List<S3ObjectModel>();
         }
      }

      public async Task<AddFileResponse> UploadFilesAsync(string bucketName, IList<IFormFile> formFiles)
      {
         try
         {
            List<string> response = new List<string>();

            foreach (IFormFile file in formFiles)
            {
               TransferUtilityUploadRequest uploadRequest = new TransferUtilityUploadRequest()
               {
                  InputStream = file.OpenReadStream(),
                  Key = file.FileName,
                  BucketName = bucketName,
                  CannedACL = S3CannedACL.NoACL,
                  ServerSideEncryptionMethod = ServerSideEncryptionMethod.AES256
               };

               using (TransferUtility fileTransferUtility = new TransferUtility(_client))
               {
                  await fileTransferUtility.UploadAsync(uploadRequest);
               }

               //Make signature link for object with filename file.FileName
               GetPreSignedUrlRequest expiryUrlRequest = new GetPreSignedUrlRequest()
               {
                  BucketName = bucketName,
                  Key = file.FileName,
                  Expires = DateTime.Now.AddDays(1)
               };

               string url = _client.GetPreSignedURL(expiryUrlRequest);

               response.Add(url);
            }

            return new AddFileResponse() { PreSignedUrl = response, Message = "Upload completed", StatusCode = HttpStatusCode.OK };
         }
         catch (AmazonS3Exception amazonS3Exception)
         {
            if (amazonS3Exception.ErrorCode != null &&
                (amazonS3Exception.ErrorCode.Equals("InvalidAccessKeyId") || amazonS3Exception.ErrorCode.Equals("InvalidSecurity")))
            {
               Console.WriteLine();
               return new AddFileResponse() { Message = "Please check the provided AWS Credentials", StatusCode = amazonS3Exception.StatusCode };
            }
            else
            {
               return new AddFileResponse()
               {
                  Message = "An error occurred when uploading the file, message " + amazonS3Exception.ErrorCode,
                  StatusCode = amazonS3Exception.StatusCode
               };
            }
         }
         catch (Exception e)
         {
            return new AddFileResponse() { Message = e.Message, StatusCode = HttpStatusCode.InternalServerError };
         }
      }

      #endregion

      #region Methods - Public

      public async Task<bool> DoesBucketExists(string bucketName)
      {
         return await AmazonS3Util.DoesS3BucketExistV2Async(_client, bucketName);
      }

      #endregion


   }
}
