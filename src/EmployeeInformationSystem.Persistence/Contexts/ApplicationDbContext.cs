using EmployeeInformationSystem.Application.Common.Interfaces;
using EmployeeInformationSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeInformationSystem.Persistence.Contexts
{
    public class ApplicationDbContext : DbContext, IUnitOfWork
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Employee> Employees => Set<Employee>();
        public DbSet<EmployeeHistory> EmployeeHistories => Set<EmployeeHistory>();

        public DbSet<User> Users => Set<User>();
        public DbSet<UserHistory> UserHistories => Set<UserHistory>();

        public DbSet<EmployeeFiles> EmployeeFiles => Set<EmployeeFiles>();
        public DbSet<EmployeeFilesHistory> EmployeeFilesHistories => Set<EmployeeFilesHistory>();

        public DbSet<Department> Departments => Set<Department>();
        public DbSet<DepartmentHistory> DepartmentHistories => Set<DepartmentHistory>();

        public DbSet<Position> Positions => Set<Position>();
        public DbSet<PositionHistory> PositionHistories => Set<PositionHistory>();

        public DbSet<Role> Roles => Set<Role>();
        public DbSet<RoleHistory> RoleHistories => Set<RoleHistory>();

        public DbSet<FunctionKey> FunctionKeys => Set<FunctionKey>();
        public DbSet<FunctionKeyHistory> FunctionKeyHistories => Set<FunctionKeyHistory>();

        public DbSet<EmployeeRole> EmployeeRoles => Set<EmployeeRole>();
        public DbSet<EmployeeRoleHistory> EmployeeRoleHistories => Set<EmployeeRoleHistory>();

        public DbSet<RoleFunction> RoleFunctions => Set<RoleFunction>();
        public DbSet<RoleFunctionHistory> RoleFunctionHistories => Set<RoleFunctionHistory>();

        public DbSet<Lookup> Lookups => Set<Lookup>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }
    }
}
