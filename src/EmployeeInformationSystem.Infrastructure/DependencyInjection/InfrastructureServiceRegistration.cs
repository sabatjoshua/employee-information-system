using EmployeeInformationSystem.Application.Common.Interfaces.Security;
using EmployeeInformationSystem.Infrastructure.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EmployeeInformationSystem.Infrastructure.DependencyInjection
{
    public static class InfrastructureServiceRegistration
    {

        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.Configure<JwtOptions>(
                configuration.GetSection("Jwt"));


            services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

            services.AddHttpContextAccessor();

            services.AddScoped<ICurrentUserService, CurrentUserService>();
            return services;
        }
    }
}
