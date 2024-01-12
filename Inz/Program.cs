using Inz.Context;
using Inz.Services;
using Microsoft.EntityFrameworkCore;
using FluentValidation.AspNetCore;
using System.Reflection;
using FluentValidation;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();

ValidatorOptions.Global.LanguageManager.Enabled = false;

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DbContextApi>(option => option.UseSqlServer(
builder.Configuration.GetSection("AzureSeverDB")["ConnectionString"]));
//builder.Services.AddDbContext<DbContextApi>(option => option.UseSqlServer(builder.Configuration.GetSection("ConnectionDbStrings")["localhostExpress2"]), ServiceLifetime.Transient);

builder.Services.AddCustomServices();

builder.Host.UseSerilog((ctx, lc)
    => lc.ReadFrom.Configuration(ctx.Configuration));

builder.Configuration.AddUserSecrets<Program>(true);

builder.Services.AddAuthService(builder.Configuration);

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpLogging();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();