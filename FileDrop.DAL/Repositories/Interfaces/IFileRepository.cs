﻿using FileDrop.TL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileDrop.DAL.Repositories.Interfaces
{
   public interface IFileRepository
   {
      void AddFile(FileDTO fileDTO);
      IList<FileDTO> GetAllFiles();
   }
}
