using FileDrop.DAL.Entities;
using FileDrop.DAL.Repositories.Interfaces;
using FileDrop.TL;
using System.Collections.Generic;
using System.Linq;

namespace FileDrop.DAL.Repositories.Classes
{
   public class FileRepository : IFileRepository
   {
      private readonly ApplicationDBContext _applicationDBContext;

      public FileRepository(ApplicationDBContext applicationDBContext)
      {
         _applicationDBContext = applicationDBContext;
      }

      public void AddFile(FileDTO fileDTO)
      {
         CloudFile existingFile = _applicationDBContext.Files.FirstOrDefault(x => x.FileName == fileDTO.FileName && x.UserId == fileDTO.UserId);
         if (existingFile != null)
         {
            existingFile.FileSize = fileDTO.FileSize;
            existingFile.UploadDate = fileDTO.UploadDate;
            existingFile.StorageClass = fileDTO.StorageClass;
            existingFile.PreviewURL = fileDTO.PreviewURL;
         }
         else
         {
            CloudFile newFile = new CloudFile
            {
               FileName = fileDTO.FileName,
               FileSize = fileDTO.FileSize,
               UploadDate = fileDTO.UploadDate,
               StorageClass = fileDTO.StorageClass,
               PreviewURL = fileDTO.PreviewURL,
               UserId = fileDTO.UserId,
            };
            _applicationDBContext.Files.Add(newFile);
         }

         _applicationDBContext.SaveChanges();
      }

      public IList<FileDTO> GetAllFiles(int userId)
      {
         return _applicationDBContext.Files.Where(x => x.UserId == userId).Select(file => new FileDTO
         {
            Id = file.Id,
            FileName = file.FileName,
            FileSize = file.FileSize,
            StorageClass = file.StorageClass,
            UploadDate = file.UploadDate,
            PreviewURL = file.PreviewURL,
            UserId = file.UserId,
         }).ToList();
      }
   }
}
