using Amazon.S3.Model;
using FileDrop.BL.Interfaces;
using FileDrop.DAL.Repositories.Interfaces;
using FileDrop.TL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileDrop.BL.Classes
{
   public class FileLogic: IFileLogic
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

      public IList<FileDTO> GetAllFiles()
      {
         return _fileRepository.GetAllFiles();
      }
   }
}
