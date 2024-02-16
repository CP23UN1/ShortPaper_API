using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.HttpOverrides;
using ShortPaper_API.Entities;
using ShortPaper_API.Services.Announcements;
using ShortPaper_API.Services.Students;
using ShortPaper_API.Services.Committees;
using ShortPaper_API.Services.Files;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using ShortPaper_API.Services.Shortpapers;
using ShortPaper_API.Services.Subjects;
using ShortPaper_API.Services.Comments;
using ShortPaper_API.Services.Authentications;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Configuration;
using System.Text;
using Microsoft.OpenApi.Models;
using ShortPaper_API.Helper;

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

builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = long.MaxValue; // Set to the maximum value or an appropriate limit
});

builder.Services.Configure<KestrelServerOptions>(options =>
{
    options.Limits.RequestHeadersTimeout = TimeSpan.FromMinutes(5); // Set an appropriate timeout
});

// Configure JWT authentication
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var key = Encoding.ASCII.GetBytes(jwtSettings["Secret"]);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };
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

// Add Swagger generation
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "ShortPaper API", Version = "v1" });

    // Configure JWT Bearer authentication
    var jwtSecurityScheme = new OpenApiSecurityScheme
    {
        Scheme = "Bearer",
        BearerFormat = "JWT",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Description = "JWT Authorization header using the Bearer scheme. Example: 'Bearer {token}'"
    };
    c.AddSecurityDefinition("Bearer", jwtSecurityScheme);

    // Add JWT token input to Swagger UI
    c.OperationFilter<AuthorizeOperationFilter>();
});

// Add Controller and Services Scoped
//builder.Services.AddControllers();

builder.Services.AddScoped<IAnnouncementService, AnnouncementService>();
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<ICommitteeService, CommitteeService>();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<IShortpaperService, ShortpaperService>();
builder.Services.AddScoped<ISubjectService, SubjectService>();
builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddScoped<IAuthService, AuthService>();

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
