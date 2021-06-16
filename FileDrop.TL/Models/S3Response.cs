using System.IO;
using System.Net;

namespace FileDrop.TL.Models
{
   public class S3Response
   {
      #region Properties

      public string Message { get; set; }
      public HttpStatusCode StatusCode { get; set; }
      public Stream File { get; set; }
      public string ContentType { get; set; }

      #endregion
   }
}
