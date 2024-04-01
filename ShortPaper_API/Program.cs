using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.HttpOverrides;
using ShortPaper_API.Entities;
using ShortPaper_API.Services.Announcements;
using ShortPaper_API.Services.Students;
using ShortPaper_API.Services.Committees;
using ShortPaper_API.Services.Files;
using ShortPaper_API.Services.Articles;
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
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

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

// Auth Service
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
})
.AddCookie()
.AddOpenIdConnect(options =>
{
    options.MetadataAddress = "https://login.sit.kmutt.ac.th/realms/student-project/.well-known/openid-configuration";
    options.Authority = "https://login.sit.kmutt.ac.th"; // Keycloak server URL
    options.Authority = "account";
    options.RequireHttpsMetadata = true;
    options.ClientId = "auth-cp23un1"; // Your client ID
    options.ClientSecret = "Inz9aNsu1zJNDkbt0T1uHM75hMaSnQgm"; // Your client secret
    options.ResponseType = OpenIdConnectResponseType.Code;
    options.SaveTokens = true;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = "account",
        ValidIssuer = "https://login.sit.kmutt.ac.th/realms/student-project"
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("all", policy => policy.RequireAssertion(context =>
    {
        return context.User.HasClaim("groupid", "904") || context.User.HasClaim("groupid", "902") || context.User.HasClaim("groupid", "901");
    }));

    options.AddPolicy("studentoradmin", policy => policy.RequireAssertion(context =>
    {
        return context.User.HasClaim("groupid", "904") || context.User.HasClaim("groupid", "902");
    }));

    options.AddPolicy("studentorcommittee", policy => policy.RequireAssertion(context =>
    {
        return context.User.HasClaim("groupid", "904") || context.User.HasClaim("groupid", "901");
    }));

    options.AddPolicy("adminorcommittee", policy => policy.RequireAssertion(context =>
    {
        return context.User.HasClaim("groupid", "902") || context.User.HasClaim("groupid", "901");
    }));

    options.AddPolicy("committee", policy => policy.RequireClaim("groupid", "901"));
    options.AddPolicy("admin", policy => policy.RequireClaim("groupid", "902"));
    options.AddPolicy("student", policy => policy.RequireClaim("groupid", "904"));
});

builder.Services.AddHttpClient();

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
builder.Services.AddScoped<IArticleService, ArticleService>();
builder.Services.AddSingleton<FileStoreService>(provider =>
{
    string basePath = Directory.GetCurrentDirectory();
    string uploadsDirectory = "uploads"; // Provide the uploads directory here
    return new FileStoreService(basePath, uploadsDirectory);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "ShortPaper API V1");
    });
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
