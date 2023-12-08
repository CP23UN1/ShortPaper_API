using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ShortPaper_API.Entities;
using ShortPaper_API.Services.Files;
using ShortPaper_API.Services.Users;
using Microsoft.Extensions.Logging;

namespace ShortPaper_API
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
            //Add Cors Config
            //services.AddCors(options =>
            //{
            //    options.AddDefaultPolicy(builder =>
            //    {
            //        builder.AllowAnyOrigin()
            //               .AllowAnyMethod()
            //               .AllowAnyHeader();
            //    });
            //});

            // Add DbContext
            services.AddDbContext<ShortpaperDbContext>(options =>
            {
                options.UseMySQL(Configuration.GetConnectionString("ConnectionString"));
            });

            services.AddControllers();

            // Add Services Scoped
            //services.AddScoped<IUserService, UserService>();
            //services.AddScoped<IFileService, FileService>();

            // In ConfigureServices method
            // Add Cors Config
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
            });

            services.AddScoped<IFileService, FileService>();
            services.AddLogging(loggingBuilder =>
            {
                // Add any specific logging configuration here
                loggingBuilder.AddConsole();  // Example: Log to the console
            });

            //services.AddHttpsRedirection(options =>
            //{
            //    options.HttpsPort = 443;
            //});
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
                app.UseHsts();
            }

            // Use CORS before other middleware
            app.UseCors(options => options.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            //app.UseHttpsRedirection();
        }
    }
}
