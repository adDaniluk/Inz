using Inz.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Inz.Services
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddCustomServices(this IServiceCollection services)
        {
            services.AddScoped<IPatientService, PatientService>();
            services.AddScoped<IPatientRepository, PatientRepository>();

            services.AddScoped<IDoctorService, DoctorService>();
            services.AddScoped<IDoctorRepository, DoctorRepository>();

            services.AddScoped<ICalendarService, CalendarService>();
            services.AddScoped<ICalendarRepository, CalendarRepository>();

            services.AddScoped<IDoctorVisitRepository, DoctorVisitRepository>();
            services.AddScoped<IDoctorVisitService, DoctorVisitService>();

            services.AddScoped<IDiseaseRepository, DiseaseRepository>();
            services.AddScoped<IMedicalSpecializationRepository, MedicalSpecializationRepository>();
            services.AddScoped<IServiceRepository, ServiceRepository>();
            services.AddScoped<IDoctorServiceRepository, DoctorServiceRepository>();
            services.AddScoped<IStatusRepository, StatusRepository>();

            services.AddScoped<ILoginService, LoginService>();

            return services;
        }

        public static IServiceCollection AddAuthService(this IServiceCollection services, IConfiguration configuration)
        {
            string signingKey = configuration.GetSection("Token")["ServiceApiKey"]!;
            string audience = configuration.GetSection("Token")["Audience"]!;
            string issuer = configuration.GetSection("Token")["Issuer"]!;

            if (signingKey != null && audience != null && issuer != null)
            {
                services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(
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

            return services;
        }
    }
}
