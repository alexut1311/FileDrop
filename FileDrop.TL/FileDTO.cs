namespace FileDrop.TL
{
   public class FileDTO
   {
      public int Id { get; set; }
      public string FileName { get; set; }
      public string FileSize { get; set; }
      public string UploadDate { get; set; }
      public string StorageClass { get; set; }
      public string PreviewURL { get; set; }
      public int UserId { get; set; }
   }
}
