using FileDrop.TL;
using System.Collections.Generic;

namespace FileDrop.DAL.Repositories.Interfaces
{
   public interface IFileRepository
   {
      void AddFile(FileDTO fileDTO);
      IList<FileDTO> GetAllFiles();
   }
}
