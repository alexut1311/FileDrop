using FileDrop.BL.Interfaces;
using FileDrop.DAL.Repositories.Interfaces;
using FileDrop.TL;
using System.Collections.Generic;

namespace FileDrop.BL.Classes
{
   public class FileLogic : IFileLogic
   {
      private readonly IFileRepository _fileRepository;

      public FileLogic(IFileRepository fileRepository)
      {
         _fileRepository = fileRepository;
      }
      public void AddFileToDB(FileDTO fileDTO)
      {
         _fileRepository.AddFile(fileDTO);
      }

      public IList<FileDTO> GetAllFiles(int userId)
      {
         return _fileRepository.GetAllFiles(userId);
      }
   }
}
