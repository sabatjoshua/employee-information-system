using EmployeeInformationSystem.Application.Common.Interfaces;
using EmployeeInformationSystem.Application.Common.Interfaces.Repositories;
using EmployeeInformationSystem.Application.Common.Interfaces.Security;
using EmployeeInformationSystem.Infrastructure.Security;
using EmployeeInformationSystem.Persistence.Contexts;
using EmployeeInformationSystem.Persistence.Repositories;
using EmployeeInformationSystem.Persistence.Security;
using EmployeeInformationSystem.Persistence.Seeding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EmployeeInformationSystem.Persistence.DependencyInjection
{
    public static class PersistenceServiceRegistration
    {
        public static IServiceCollection AddPersistence(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<ApplicationDbSeeder>();

            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<IEmployeeHistoryRepository, EmployeeHistoryRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserHistoryRepository, UserHistoryRepository>();
            services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            services.AddScoped<IDepartmentHistoryRepository,DepartmentHistoryRepository>();
            services.AddScoped<IPositionRepository, PositionRepository>();
            services.AddScoped<IPositionHistoryRepository, PositionHistoryRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IRoleHistoryRepository, RoleHistoryRepository>();
            services.AddScoped<IFunctionKeyRepository, FunctionKeyRepository>();
            services.AddScoped<IFunctionKeyHistoryRepository, FunctionKeyHistoryRepository>();
            services.AddScoped<IRoleFunctionRepository, RoleFunctionRepository>();
            services.AddScoped<IRoleFunctionHistoryRepository, RoleFunctionHistoryRepository>();
            services.AddScoped<IEmployeeRoleRepository, EmployeeRoleRepository>();
            services.AddScoped<IEmployeeRoleHistoryRepository, EmployeeRoleHistoryRepository>();
            services.AddScoped<IEmployeeFileRepository, EmployeeFileRepository>();
            services.AddScoped<IEmployeeFileHistoryRepository,EmployeeFileHistoryRepository>();

            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddScoped<IUnitOfWork>(
                provider => provider.GetRequiredService<ApplicationDbContext>());
            services.AddScoped<IPermissionService, PermissionService>();


            return services;
        }
    }
}
