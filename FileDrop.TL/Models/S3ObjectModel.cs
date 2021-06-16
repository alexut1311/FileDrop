namespace FileDrop.TL.Models
{
   public class S3ObjectModel
   {
      public int Id { get; set; }
      public string Name { get; set; }
      public string Size { get; set; }
      public string LastModified { get; set; }
      public string OwnerDisplayName { get; set; }
      public string OwnerId { get; set; }
      public string StorageClass { get; set; }

   }
}
