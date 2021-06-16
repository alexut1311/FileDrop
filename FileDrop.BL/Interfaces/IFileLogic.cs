using FileDrop.TL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileDrop.BL.Interfaces
{
   public interface IFileLogic
   {
      void AddFileToDB(FileDTO fileDTO);
      IList<FileDTO> GetAllFiles();
   }
}
