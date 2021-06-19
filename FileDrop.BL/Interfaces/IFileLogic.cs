using FileDrop.TL;
using System.Collections.Generic;

namespace FileDrop.BL.Interfaces
{
   public interface IFileLogic
   {
      void AddFileToDB(FileDTO fileDTO);
      IList<FileDTO> GetAllFiles(int userId);
   }
}
