using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using Backend.Data;
using Backend.Services;

var builder = WebApplication.CreateBuilder(args);

// --------------------
// Add services
// --------------------
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("InsightAICors", policy =>
    {
        policy.WithOrigins(
             "http://localhost:5173",
             "https://localhost:7112"
        )
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials();

    });
});

//builder.Services.AddSwaggerGen(c =>
//{
//  c.AddSecurityDefinition("Bearer", new()
//  {
//    Name = "Authorization",
//    Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
//    Scheme = "Bearer",
//    BearerFormat = "JWT",
//    In = Microsoft.OpenApi.Models.ParameterLocation.Header
//  });

//  c.AddSecurityRequirement(new()
//    {
//        {
//            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
//            {
//                Reference = new Microsoft.OpenApi.Models.OpenApiReference
//                {
//                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
//                    Id = "Bearer"
//                }
//            },
//            Array.Empty<string>()
//        }
//    });
//});

// --------------------
// EF Core
// --------------------
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

// --------------------
// JWT Authentication
// --------------------
//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//    .AddJwtBearer(options =>
//    {
//      options.TokenValidationParameters = new TokenValidationParameters
//      {
//        ValidateIssuer = true,
//        ValidateAudience = true,
//        ValidateLifetime = true,
//        ValidateIssuerSigningKey = true,
//        ValidIssuer = builder.Configuration["Jwt:Issuer"],
//        ValidAudience = builder.Configuration["Jwt:Audience"],
//        IssuerSigningKey = new SymmetricSecurityKey(
//              Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)
//          )
//      };
//    });

//builder.Services.AddAuthorization();

// --------------------
// Application Services
// --------------------
builder.Services.AddSingleton<OpenAIService>();
builder.Services.AddScoped<PromptExecutionService>();
builder.Services.AddScoped<FileTypeService>();
builder.Services.AddScoped<RegionService>();
builder.Services.AddScoped<ApplicationService>();
builder.Services.AddScoped<PromptService>();
builder.Services.AddScoped<PromptResponseService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<UserPromptService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<ResponseTypeService>();
builder.Services.AddScoped<UserApplicationMappingService>();

// --------------------
// Build app
// --------------------
var app = builder.Build();

// --------------------
// Swagger
// --------------------
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// --------------------
// CORS MUST COME EARLY
// --------------------
app.UseCors("InsightAICors");


// --------------------
// Routing
// --------------------
app.UseRouting();

// --------------------
// OPTIONAL: HTTPS redirect (safe now)
// --------------------
app.UseHttpsRedirection();

// --------------------
// Auth (when enabled)
// --------------------
// app.UseAuthentication();
// app.UseAuthorization();

// --------------------
// Endpoints
// --------------------
app.MapControllers();

app.Run();

