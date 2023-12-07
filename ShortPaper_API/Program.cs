using Microsoft.EntityFrameworkCore;
using ShortPaper_API.Controllers;
using ShortPaper_API.Entities;
using ShortPaper_API.Services.Announcements;
using ShortPaper_API.Services.Files;
using ShortPaper_API.Services.Subjects;
using ShortPaper_API.Services.Users;

var builder = WebApplication.CreateBuilder(args);
var MyAllowSpecificOrigins = "AllowSpecificOrigin";

builder.Services.AddCors(options =>
{
    options.AddPolicy(MyAllowSpecificOrigins,
                      builder =>
                      {
                          builder
                            .WithOrigins("*")
                            .WithMethods("GET", "POST", "PUT", "DELETE")
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
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ISubjectService, SubjectService>();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<IAnnouncementService, AnnouncementService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseCors(MyAllowSpecificOrigins);

app.UseAuthorization();

app.MapControllers();

app.Run();
