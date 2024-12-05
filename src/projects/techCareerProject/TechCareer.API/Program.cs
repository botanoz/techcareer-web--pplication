using Autofac;
using Autofac.Extensions.DependencyInjection;
using Core.CrossCuttingConcerns.Serilog;
using Core.CrossCuttingConcerns.Serilog.Loggers;
using Core.Security;
using Core.Security.Encryption;
using Core.Security.JWT;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;
using System.Text.Json.Serialization;
using TechCareer.DataAccess;
using TechCareer.Service.DependencyResolvers.Autofac;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration) // appsettings.json'dan oku
    .Enrich.FromLogContext()
    .WriteTo.Console() // Konsola log yaz
    .WriteTo.File(
        path: builder.Configuration["SerilogLogConfigurations:FileLogConfiguration:FolderPath"] + "log-.txt",
        rollingInterval: RollingInterval.Day,
        restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information
    )
    .WriteTo.MSSqlServer(
        connectionString: builder.Configuration["SerilogLogConfigurations:MsSqlConfiguration:ConnectionString"],
        sinkOptions: new MSSqlServerSinkOptions
        {
            TableName = builder.Configuration["SerilogLogConfigurations:MsSqlConfiguration:TableName"],
            AutoCreateSqlTable = Convert.ToBoolean(builder.Configuration["SerilogLogConfigurations:MsSqlConfiguration:AutoCreateSqlTable"])
        },
        columnOptions: new ColumnOptions(),
        restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information
    )
    .CreateLogger();

builder.Host.UseSerilog(); // Serilog'u Host ile baðla

// Veri eriþimi servislerini kaydet
builder.Services.AddDataAccessServices(builder.Configuration);

// Logger servisini kaydet (FileLogger olarak kullanmak için)
builder.Services.AddSingleton<LoggerServiceBase, FileLogger>();

// JWT kimlik doðrulama yapýlandýrmasý
const string tokenOptionsConfigurationSection = "TokenOptions";
var tokenOptions = builder.Configuration.GetSection(tokenOptionsConfigurationSection).Get<TokenOptions>()
                   ?? throw new InvalidOperationException($"\"{tokenOptionsConfigurationSection}\" section cannot found in configuration.");

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidIssuer = tokenOptions.Issuer,
            ValidAudience = tokenOptions.Audience,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = SecurityKeyHelper.CreateSecurityKey(tokenOptions.SecurityKey)
        };
    });

// Diðer servis ve middleware baðýmlýlýklarý
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
        options.JsonSerializerOptions.MaxDepth = 64;
        options.JsonSerializerOptions.WriteIndented = true;
    });

// Swagger yapýlandýrmasý
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer' [space] and then your token in the text input below. Example: \"Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9\""
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            new string[] {}
        }
    });
});

// Autofac entegrasyonu
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(container =>
{
    container.RegisterModule(new AutofacBusinessModule());
});

// Uygulamayý oluþtur
var app = builder.Build();

// Middleware eklemeleri
app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

// Request ve response loglama middleware
app.Use(async (context, next) =>
{
    var logger = app.Services.GetRequiredService<LoggerServiceBase>();
    logger.Info($"Request: {context.Request.Method} {context.Request.Path}");
    await next.Invoke();
    logger.Info($"Response: {context.Response.StatusCode}");
});

app.MapControllers();
app.Run();
