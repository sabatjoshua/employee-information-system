using EmployeeInformationSystem.Application.Features.Departments;
using EmployeeInformationSystem.Application.Features.Employees;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeInformationSystem.Application.DependencyInjection
{
    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection AddApplication(
            this IServiceCollection services)
        {
            services.AddScoped<GetEmployeeByIdHandler>();
            services.AddScoped<CreateDepartmentHandler>();

            return services;
        }
    }
}
