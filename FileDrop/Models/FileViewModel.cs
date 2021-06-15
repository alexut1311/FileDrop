using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileDrop.Models
{
   public class FileViewModel
   {
      public int Id { get; set; }
      public string FileName { get; set; }
      public string FileSize { get; set; }
   }
}
