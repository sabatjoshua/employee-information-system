using Microsoft.Extensions.DependencyInjection;

namespace EmployeeInformationSystem.Application.DependencyInjection
{
    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection AddApplication(
            this IServiceCollection services)
        {
            services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(
                typeof(ApplicationServiceRegistration).Assembly));

            return services;
        }
    }
}
