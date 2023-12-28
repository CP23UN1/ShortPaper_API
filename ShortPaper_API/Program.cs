using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.HttpOverrides;
using ShortPaper_API.Entities;
using ShortPaper_API.Services.Announcements;
using ShortPaper_API.Services.Students;
using ShortPaper_API.Services.Committees;
using ShortPaper_API.Services.Files;

var builder = WebApplication.CreateBuilder(args);
var MyAllowSpecificOrigins = "AllowSpecificOrigin";

builder.Services.AddCors(options =>
{
    options.AddPolicy(MyAllowSpecificOrigins,
                      builder =>
                      {
                          builder
                            .AllowAnyOrigin()
                            .AllowAnyMethod()
                            .AllowAnyHeader();
                      });
});

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure your DbContext
builder.Services.AddDbContext<ShortpaperDbContext>(options =>
{
    options.UseMySQL(builder.Configuration.GetConnectionString("ConnectionString"));
});

// Add Controller and Services Scoped
//builder.Services.AddControllers();

builder.Services.AddScoped<IAnnouncementService, AnnouncementService>();
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<ICommitteeService, CommitteeService>();
builder.Services.AddScoped<IFileService, FileService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UsePathBase("/un1");

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseCors(MyAllowSpecificOrigins);

app.UseAuthorization();

app.MapControllers();

app.Run();
