using Inz.Context;
using Inz.Repository;
using Inz.Services;
using Microsoft.EntityFrameworkCore;
using FluentValidation.AspNetCore;
using System.Reflection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using FluentValidation;

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

builder.Services.AddScoped<IPatientService, PatientService>();
builder.Services.AddScoped<IPatientRepository, PatientRepository>();

builder.Services.AddScoped<IDoctorService, DoctorService>();
builder.Services.AddScoped<IDoctorRepository, DoctorRepository>();

builder.Services.AddScoped<ICalendarService, CalendarService>();
builder.Services.AddScoped<ICalendarRepository, CalendarRepository>();

builder.Services.AddScoped<IDoctorVisitRepository, DoctorVisitRepository>();
builder.Services.AddScoped<IDoctorVisitService, DoctorVisitService>();

builder.Services.AddScoped<IDiseaseRepository, DiseaseRepository>();
builder.Services.AddScoped<IMedicalSpecializationRepository, MedicalSpecializationRepository>();
builder.Services.AddScoped<IServiceRepository, ServiceRepository>();
builder.Services.AddScoped<IDoctorServiceRepository, DoctorServiceRepository>();
builder.Services.AddScoped<IStatusRepository, StatusRepository>();

builder.Services.AddScoped<IPasswordHashService, PasswordHashService>();
builder.Services.AddScoped<ILoginService, LoginService>();

//builder.Host.UseSerilog((ctx, lc)
//    => lc.ReadFrom.Configuration(ctx.Configuration));

builder.Configuration.AddUserSecrets<Program>(true);

string signingKey = builder.Configuration.GetSection("Token")["ServiceApiKey"]!;
string audience = builder.Configuration.GetSection("Token")["Audience"]!;
string issuer = builder.Configuration.GetSection("Token")["Issuer"]!;

if (signingKey != null && audience != null && issuer != null)
{
    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(
        options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey)),
                ValidIssuer = issuer,
                ValidAudience = audience,
                ValidateIssuer = true,
                ValidateLifetime = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true
            };
        });
}
else
{
    throw new Exception("Startup of the program has failed - there's a missing Authentication builder, please check json's configuration");
}

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