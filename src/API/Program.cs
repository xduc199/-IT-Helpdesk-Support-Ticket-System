using System.Text;
using Core.Interfaces;
using API.Middleware;
using Infrastructure.Data;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using API.Hubs;

var builder = WebApplication.CreateBuilder(args);

// =======================
// DB CONTEXT
// =======================
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

// =======================
// REPOSITORIES
// =======================
builder.Services.AddScoped<ITicketRepository, TicketRepository>();
builder.Services.AddScoped<ITagRepository, TagRepository>();
builder.Services.AddScoped<IAuditLogRepository, AuditLogRepository>();

// =======================
// SERVICES
// =======================
builder.Services.AddScoped<TokenService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<ITicketService, TicketService>();
builder.Services.AddScoped<ITagService, TagService>();

// =======================
// SIGNALR
// =======================
builder.Services.AddSignalR();

// =======================
// CONTROLLERS + JSON CONFIG
// =======================
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler =
            System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;

        options.JsonSerializerOptions.DefaultIgnoreCondition =
            System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
    });

// =======================
// EXCEPTION HANDLER
// =======================
builder.Services.AddExceptionHandler<AppExceptionHandler>();
builder.Services.AddProblemDetails();

// =======================
// CORS
// =======================
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// =======================
// JWT AUTH
// =======================
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)
        ),

        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],

        ValidateAudience = true,
        ValidAudience = builder.Configuration["Jwt:Audience"],

        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero,

        // ⚠ QUAN TRỌNG: phải khớp TokenService
        RoleClaimType = "role",
        NameClaimType = "unique_name"
    };
});

builder.Services.AddAuthorization();

// =======================
// OPENAPI + SCALAR
// =======================
builder.Services.AddOpenApi();

var app = builder.Build();

// =======================
// STATIC FILES (INDEX HTML)
// =======================
app.UseDefaultFiles(new DefaultFilesOptions
{
    DefaultFileNames = new List<string> { "index.html" }
});

app.UseStaticFiles();

// =======================
// PIPELINE ORDER (QUAN TRỌNG)
// =======================
app.UseExceptionHandler();

app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

// =======================
// SIGNALR
// =======================
app.MapHub<TicketHub>("/ticketHub");

// =======================
// OPENAPI + SCALAR (DEV ONLY)
// =======================
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi("/openapi/{documentName}.json");

    app.MapScalarApiReference(options =>
    {
        options
            .WithTitle("HelpDesk IT Support System - Nhóm 14")
            .WithTheme(ScalarTheme.Moon);
    });
}

// =======================
// CONTROLLERS
// =======================
app.MapControllers();

app.Run();