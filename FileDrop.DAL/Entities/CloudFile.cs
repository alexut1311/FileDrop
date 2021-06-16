using System.ComponentModel.DataAnnotations;

namespace FileDrop.DAL.Entities
{
   public class CloudFile
   {
      [Key]
      public int Id { get; set; }
      [Required]
      public string FileName { get; set; }
      [Required]
      public string FileSize { get; set; }
      [Required]
      public string UploadDate { get; set; }
      [Required]
      public string StorageClass { get; set; }
      public string PreviewURL { get; set; }
      [Required]
      public int UserId { get; set; }
   }
}
