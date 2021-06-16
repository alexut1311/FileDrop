using System.Collections.Generic;
using System.Net;

namespace FileDrop.TL.Models
{
   public class AddFileResponse
   {
      public IList<string> PreSignedUrl { get; set; }
      public string Message { get; set; }
      public HttpStatusCode StatusCode { get; set; }
   }
}
