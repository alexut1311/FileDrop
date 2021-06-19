using Amazon.S3;
using FileDrop.BL.Classes;
using FileDrop.BL.Helpers.Classes;
using FileDrop.BL.Helpers.Interfaces;
using FileDrop.BL.Interfaces;
using FileDrop.ControllerHelpers.Classes;
using FileDrop.ControllerHelpers.Interfaces;
using FileDrop.DAL;
using FileDrop.DAL.Repositories.Classes;
using FileDrop.DAL.Repositories.Interfaces;
using FileDrop.Middlewares;
using FileDrop.Services.Classes;
using FileDrop.Services.Interfaces;
using FileDrop.TL.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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
         services.AddTransient<IAuthenticationLogic, AuthenticationLogic>();
         services.AddTransient<IJWTokenHelper, JWTokenHelper>();

         services.AddSingleton<IS3Service, S3Service>();

         services.AddTransient<IFileRepository, FileRepository>();

         services.AddTransient<IAccountControllerHelper, AccountControllerHelper>();

         services.AddAuthentication(x =>
         {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
         }).AddJwtBearer(x =>
         {
            x.RequireHttpsMetadata = false;
            x.SaveToken = false;
            x.TokenValidationParameters = new TokenValidationParameters
            {
               ValidateIssuer = true,
               ValidIssuer = ConfigurationHelper.GetKey("Issuer", "jwtSettings"),
               ValidateAudience = true,
               ValidAudience = ConfigurationHelper.GetKey("Audience", "jwtSettings"),
               RequireExpirationTime = true,
               RequireSignedTokens = true,
               ValidateLifetime = true,
               ValidateIssuerSigningKey = true,
               IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(ConfigurationHelper.GetKey("SigningKey", "jwtSettings"))),
               TokenDecryptionKey = new SymmetricSecurityKey(Encoding.Default.GetBytes(ConfigurationHelper.GetKey("EncryptyingSecurityKey", "jwtSettings"))),
            };
         });

         services.AddControllersWithViews().AddNewtonsoftJson();

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

         app.UseMiddleware<AuthenticationMiddleware>();
         app.UseAuthentication();
         app.UseAuthorization();

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
