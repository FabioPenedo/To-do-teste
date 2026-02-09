// ----------------------
// Configuração do Serilog
// ----------------------
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;
using System.Text;
using System.Text.Json.Serialization;
using To_do_teste.src.Data;
using To_do_teste.src.DTOs;
using To_do_teste.src.Interfaces;
using To_do_teste.src.Middlewares;
using To_do_teste.src.Repositories;
using To_do_teste.src.Services;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
    .MinimumLevel.Override("System", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.Console(
        outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss}] [{Level:u3}] [User:{UserId}] [ReqId:{RequestId}] {Message:lj}{NewLine}{Exception}")
    .WriteTo.File(
        path: "logs/todo-teste-.log",
        rollingInterval: RollingInterval.Day,
        retainedFileCountLimit: 30,
        outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss}] [{Level:u3}] [User:{UserId}] [ReqId:{RequestId}] {Message:lj}{NewLine}{Exception}")
    .CreateLogger();

try
{
    Log.Information("application.startup Iniciando aplicação To-do-list");

    var builder = WebApplication.CreateBuilder(args);

    // Usar Serilog como provider de logs
    builder.Host.UseSerilog();

    // ----------------------
    // Configure DbContext
    // ----------------------
    builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

    // ----------------------
    // Configure JWT
    // ----------------------

    builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JWT"));

    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(options =>
    {
        options.SaveToken = true;
        var jwt = builder.Configuration.GetSection("JWT").Get<JwtSettings>()!;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ClockSkew = TimeSpan.Zero,
            ValidIssuer = jwt.Issuer,
            ValidAudience = jwt.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Secret))
        };
    });

    // ----------------------
    // Configure CORS
    // ----------------------
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("Prod", policy =>
        {
            policy
                .WithOrigins("https://seu-dominio.com")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials()
                .WithExposedHeaders("x-new-access-token");
        });

        options.AddPolicy("Dev", policy =>
        {
            policy
                .WithOrigins("http://localhost:5173")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials()
                .WithExposedHeaders("x-new-access-token");
        });
    });

    builder.Services.Configure<CookiePolicyOptions>(options =>
    {
        options.MinimumSameSitePolicy = SameSiteMode.None;
    });

    // ----------------------
    // Repositórios e Services
    // ----------------------

    builder.Services.AddScoped<ITokenService, TokenService>();
    builder.Services.AddScoped<AuthService>();
    builder.Services.AddScoped<TaskService>();
    builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
    builder.Services.AddScoped<IUserRepository, UserRepository>();
    builder.Services.AddScoped<ITaskRepository, TaskRepository>();


    /// ----------------------
    // Controllers
    // ----------------------
    builder.Services.AddControllers()
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });


    // ----------------------
    // Swagger / OpenAPI
    // ----------------------
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(c =>
    {
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "Bearer {seu access token}"
        });

        c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                Array.Empty<string>()
            }
        });
    });

    var app = builder.Build();

    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        db.Database.Migrate();
    }

    app.UseHttpsRedirection();

    // ----------------------
    // Pipeline HTTP
    // ----------------------
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Minha API V1");
            c.RoutePrefix = string.Empty;
            c.ConfigObject.AdditionalItems["withCredentials"] = true;
        });

        app.UseCors("Dev");
    }
    else
    {
        app.UseCors("Prod");
    }

    app.UseCookiePolicy();

    app.UseAuthentication();

    app.UseMiddleware<ToDoMiddleware>();

    app.UseMiddleware<JwtRefreshMiddleware>();

    app.UseMiddleware<LoggingMiddleware>();

    app.ConfigureExceptionHandler(app.Logger);

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "application.startup.failed Aplicação falhou ao iniciar ExceptionType={ExceptionType}", ex.GetType().Name);
}
finally
{
    Log.CloseAndFlush();
}


