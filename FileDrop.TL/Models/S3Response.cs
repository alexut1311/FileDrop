using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FileDrop.TL.Models
{
   public class S3Response
   {
      #region Properties

      public string Message { get; set; }
      public HttpStatusCode StatusCode { get; set; }

      #endregion
   }
}
