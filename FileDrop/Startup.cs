using Amazon.S3;
using FileDrop.BL.Classes;
using FileDrop.BL.Interfaces;
using FileDrop.DAL;
using FileDrop.DAL.Repositories.Classes;
using FileDrop.DAL.Repositories.Interfaces;
using FileDrop.Services.Classes;
using FileDrop.Services.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FileDrop
{
   public class Startup
   {
      public Startup(IConfiguration configuration)
      {
         Configuration = configuration;
      }

      public IConfiguration Configuration { get; }

      // This method gets called by the runtime. Use this method to add services to the container.
      public void ConfigureServices(IServiceCollection services)
      {
         services.AddDefaultAWSOptions(Configuration.GetAWSOptions());
         services.AddAWSService<IAmazonS3>();

         string connectionString = "Server=DESKTOP-CLJCC9A\\SQLEXPRESS;Database=FileDropDb;Trusted_Connection=True;MultipleActiveResultSets=true";
         services.AddDbContext<ApplicationDBContext>(options => options.UseSqlServer(connectionString));

         services.AddTransient<IS3Logic, S3Logic>();
         services.AddTransient<IFileLogic, FileLogic>();

         services.AddSingleton<IS3Service, S3Service>();

         services.AddTransient<IFileRepository, FileRepository>();

         services.AddControllersWithViews();

         // In production, the React files will be served from this directory
         services.AddSpaStaticFiles(configuration =>
         {
            configuration.RootPath = "ClientApp/build";
         });
      }

      // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
      public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
      {
         if (env.IsDevelopment())
         {
            app.UseDeveloperExceptionPage();
         }
         else
         {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
         }

         app.UseHttpsRedirection();
         app.UseStaticFiles();
         app.UseSpaStaticFiles();
         
         app.UseRouting();

         app.UseEndpoints(endpoints =>
         {
            endpoints.MapControllerRoute(
                   name: "default",
                   pattern: "{controller}/{action=Index}/{id?}");
         });

         app.UseSpa(spa =>
         {
            spa.Options.SourcePath = "ClientApp";

            if (env.IsDevelopment())
            {
               spa.UseReactDevelopmentServer(npmScript: "start");
            }
         });
      }
   }
}
