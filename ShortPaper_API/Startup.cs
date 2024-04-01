using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;
using Microsoft.AspNetCore.HttpOverrides;
using ShortPaper_API.Entities;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Server.Kestrel.Core;

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
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
            });

            services.Configure<FormOptions>(options =>
            {
                options.MultipartBodyLengthLimit = long.MaxValue; // Set to the maximum value or an appropriate limit
            });

            services.Configure<KestrelServerOptions>(options =>
            {
                options.Limits.RequestHeadersTimeout = TimeSpan.FromMinutes(5); // Set an appropriate timeout
            });

            // Add DbContext
            services.AddDbContext<ShortpaperDbContext>(options =>
            {
                options.UseMySQL(Configuration.GetConnectionString("ConnectionString"));
            });

            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            });

            services.AddControllers();

            // Add Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ShortPaper API", Version = "v1" });
            });

            // Add Services Scoped
            //services.AddScoped<IUserService, UserService>();
            //services.AddScoped<IFileService, FileService>();

            // In ConfigureServices method
            services.AddCors();

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
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "ShortPaper API V1");
                    // Optionally, you can configure the Swagger UI route
                    c.RoutePrefix = "api/docs"; // This will serve Swagger UI at http://localhost:<port>/api/docs
                });
            }
            else
            {
                app.UseHsts();
            }

            // Use CORS before other middleware
            app.UseCors(options => options.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            //app.UsePathBase("/un1");

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
