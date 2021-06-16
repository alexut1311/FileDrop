using FileDrop.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace FileDrop.DAL
{
   public class ApplicationDBContext : DbContext
   {
      public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
      {
      }

      public DbSet<CloudFile> Files { get; set; }
   }
}
