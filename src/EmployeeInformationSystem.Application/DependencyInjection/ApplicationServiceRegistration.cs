using EmployeeInformationSystem.Application.Common.Behaviors;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace EmployeeInformationSystem.Application.DependencyInjection
{
    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection AddApplication(
            this IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(
                typeof(ApplicationServiceRegistration).Assembly);

            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(
                    typeof(ApplicationServiceRegistration).Assembly);

                cfg.AddOpenBehavior(
                    typeof(ValidationBehavior<,>));
            });

            return services;
        }
    }
}