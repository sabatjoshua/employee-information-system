using EmployeeInformationSystem.Application.Features.Departments;
using EmployeeInformationSystem.Application.Features.Employees;
using EmployeeInformationSystem.Application.Features.Positions;
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
            services.AddScoped<CreateDepartmentHandler>();
            services.AddScoped<GetDepartmentByIdHandler>(); 
            services.AddScoped<UpdateDepartmentHandler>();
            services.AddScoped<DeleteDepartmentHandler>();
            services.AddScoped<CreatePositionHandler>();
            services.AddScoped<GetEmployeeByIdHandler>();

            return services;
        }
    }
}
