using FileDrop.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
