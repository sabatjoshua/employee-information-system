using EmployeeInformationSystem.Application.Common.Interfaces.Security;
using EmployeeInformationSystem.Domain.Entities;
using EmployeeInformationSystem.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace EmployeeInformationSystem.Persistence.Seeding;

public sealed class ApplicationDbSeeder
{
    private readonly ApplicationDbContext _context;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IConfiguration _configuration;

    private static readonly Guid SystemUserId =
    Guid.Parse("01bc3847-81dd-41c6-8fa9-c3c672f374a6");

    private static readonly Guid AdministrationDepartmentId =
        Guid.Parse("c33b65f1-c85e-4e3a-b275-09e965aa8823");

    private static readonly Guid SystemAdministratorPositionId =
        Guid.Parse("db596a9a-6f82-4836-af9f-d90ee41870b7");

    private static readonly Guid SystemAdministratorEmployeeId =
        Guid.Parse("b14dc5e8-3e1f-4dba-b3d0-88a2e3963867");
    private static readonly Guid HrAdministratorRoleId =
    Guid.Parse("94c044bb-e08d-49de-bf17-39d86ec5db07");

    private static readonly Guid EmployeeCreateFunctionId =
        Guid.Parse("0f0b098d-7d64-4301-a513-050a447d2342");

    private static readonly Guid EmployeeUpdateFunctionId =
        Guid.Parse("db65ed4a-4c22-4ab1-a094-605152822a59");

    private static readonly Guid EmployeeViewFunctionId =
        Guid.Parse("aed2f0ec-d03b-462e-ac9f-64aa9114e74c");

    private static readonly Guid EmployeeDeleteFunctionId =
        Guid.Parse("336d3cd5-6882-4513-b7e6-ac0b17b99111");

    // Fixed IDs for USER permissions
    private static readonly Guid UserViewFunctionId =
        Guid.Parse("ad40d5e6-b8c8-4d48-a001-000000000001");

    private static readonly Guid UserCreateFunctionId =
        Guid.Parse("ad40d5e6-b8c8-4d48-a001-000000000002");

    private static readonly Guid UserUpdateFunctionId =
        Guid.Parse("ad40d5e6-b8c8-4d48-a001-000000000003");

    private static readonly Guid UserDeleteFunctionId =
        Guid.Parse("ad40d5e6-b8c8-4d48-a001-000000000004"); 
    
    private static readonly Guid EmployeeViewRoleFunctionId =
    Guid.Parse("f87da4d2-da01-4789-9ef8-40cc5d4f6c5d");

    private static readonly Guid EmployeeDeleteRoleFunctionId =
        Guid.Parse("33da365c-b939-4363-992d-81fbaf6c3b99");

    private static readonly Guid EmployeeUpdateRoleFunctionId =
        Guid.Parse("76593474-b4dd-4173-b953-a5c61a1e48d7");

    private static readonly Guid EmployeeCreateRoleFunctionId =
        Guid.Parse("298c1c18-74f1-4688-9691-fce2d0996425");

    private static readonly Guid UserViewRoleFunctionId =
        Guid.Parse("bf50e6f7-c9d9-4e59-b002-000000000001");

    private static readonly Guid UserCreateRoleFunctionId =
        Guid.Parse("bf50e6f7-c9d9-4e59-b002-000000000002");

    private static readonly Guid UserUpdateRoleFunctionId =
        Guid.Parse("bf50e6f7-c9d9-4e59-b002-000000000003");

    private static readonly Guid UserDeleteRoleFunctionId =
        Guid.Parse("bf50e6f7-c9d9-4e59-b002-000000000004"); 
    
    private static readonly Guid SystemAdministratorEmployeeRoleId =
    Guid.Parse("5fcde590-4bad-4e30-9c8d-25c854139098");

    public ApplicationDbSeeder(
    ApplicationDbContext context,
    IPasswordHasher passwordHasher,
    IConfiguration configuration)
    {
        _context = context;
        _passwordHasher = passwordHasher;
        _configuration = configuration;
    }

    public async Task SeedAsync(
        CancellationToken cancellationToken = default)
    {
        await SeedLookupsAsync(cancellationToken);
        await SeedDepartmentAsync(cancellationToken);
        await SeedPositionAsync(cancellationToken);
        await SeedEmployeeAsync(cancellationToken);

        await SeedFunctionKeysAsync(cancellationToken);
        await SeedRoleAsync(cancellationToken);
        await SeedRoleFunctionsAsync(cancellationToken);
        await SeedEmployeeRoleAsync(cancellationToken);

        await SeedUserAsync(cancellationToken);
    }

    private async Task SeedLookupsAsync(
     CancellationToken cancellationToken)
    {
        var lookups = new List<Lookup>
    {
        // Action Type
        new()
        {
            LookupId = Guid.Parse("B7B36F02-D6DD-45E8-917B-6629705999D7"),
            Type = "ActionType",
            Code = "INS",
            Name = "Insert",
            Sort = 1
        },
        new()
        {
            LookupId = Guid.Parse("160D4D84-BB14-4372-AD15-4319C14B1782"),
            Type = "ActionType",
            Code = "UPD",
            Name = "Update",
            Sort = 2
        },
        new()
        {
            LookupId = Guid.Parse("E4924E8F-E438-4121-8746-03FCF1EF6BE0"),
            Type = "ActionType",
            Code = "DEL",
            Name = "Delete",
            Sort = 3
        },

        // Document Status
        new()
        {
            LookupId = Guid.Parse("1A744940-E9E0-4761-BD0C-A11C705C1AA3"),
            Type = "DocStatus",
            Code = "PND",
            Name = "Pending",
            Sort = 1
        },
        new()
        {
            LookupId = Guid.Parse("3DE5A81B-F11A-4E79-A689-205833C69C69"),
            Type = "DocStatus",
            Code = "APR",
            Name = "Approved",
            Sort = 2
        },
        new()
        {
            LookupId = Guid.Parse("C13CCB31-B229-4EC0-B370-5B91CCEEBFC6"),
            Type = "DocStatus",
            Code = "REJ",
            Name = "Rejected",
            Sort = 3
        },

        // Employee Status
        new()
        {
            LookupId = Guid.Parse("ECE2E760-FFA8-41CC-A34E-24BD3F3449AA"),
            Type = "EmpStatus",
            Code = "PRO",
            Name = "Probation",
            Sort = 1
        },
        new()
        {
            LookupId = Guid.Parse("ED8B77A7-F710-40F5-B8BF-96B7FBF9C5E9"),
            Type = "EmpStatus",
            Code = "CON",
            Name = "Confirmed",
            Sort = 2
        },
        new()
        {
            LookupId = Guid.Parse("E5FE4A32-A954-4E42-BCC1-F13EAB475ADC"),
            Type = "EmpStatus",
            Code = "RES",
            Name = "Resigned",
            Sort = 3
        },
        new()
        {
            LookupId = Guid.Parse("1AE78B33-1C9B-49CE-AE13-AFBA849D9A04"),
            Type = "EmpStatus",
            Code = "TER",
            Name = "Terminated",
            Sort = 4
        },
        new()
        {
            LookupId = Guid.Parse("07B3FEF9-5883-426C-96C9-7C8181784C90"),
            Type = "EmpStatus",
            Code = "RET",
            Name = "Retired",
            Sort = 5
        },

        // File Status
        new()
        {
            LookupId = Guid.Parse("11FB266C-D104-4979-8457-DFCC6B9D727B"),
            Type = "FileStatus",
            Code = "OPN",
            Name = "Open",
            Sort = 1
        },
        new()
        {
            LookupId = Guid.Parse("738CC8D5-C7E0-4251-9866-BAAFCADE3091"),
            Type = "FileStatus",
            Code = "CLS",
            Name = "Close",
            Sort = 2
        },
        new()
        {
            LookupId = Guid.Parse("78839010-7E29-4D03-BD80-B9A7511B522A"),
            Type = "FileStatus",
            Code = "DEL",
            Name = "Deleted",
            Sort = 3
        },

        // File Type
        new()
        {
            LookupId = Guid.Parse("744DAC04-CE98-4B24-A152-A29C527264DD"),
            Type = "FileType",
            Code = "IMG",
            Name = "Profile Image",
            Sort = 1
        },
        new()
        {
            LookupId = Guid.Parse("1FC8403C-C2CB-478D-B75B-62410419114A"),
            Type = "FileType",
            Code = "RES",
            Name = "Resume",
            Sort = 2
        },
        new()
        {
            LookupId = Guid.Parse("8AB5C460-C7D4-4E23-BDA1-B15DC0A1BF4E"),
            Type = "FileType",
            Code = "CER",
            Name = "Certificate",
            Sort = 3
        },
        new()
        {
            LookupId = Guid.Parse("6DBE54F4-D74C-4027-9F96-7558B1A973F2"),
            Type = "FileType",
            Code = "ID",
            Name = "Identification",
            Sort = 4
        },
        new()
        {
            LookupId = Guid.Parse("D83A447B-8EC1-4D19-86D4-3949C31C2F28"),
            Type = "FileType",
            Code = "OTH",
            Name = "Other",
            Sort = 5
        },

        // Gender
        new()
        {
            LookupId = Guid.Parse("26307EA5-2C27-4B74-AA82-6E794929E0B7"),
            Type = "Gender",
            Code = "M",
            Name = "Male",
            Sort = 1
        },
        new()
        {
            LookupId = Guid.Parse("18B76CBA-0355-474C-A7BA-7FD599FA0D24"),
            Type = "Gender",
            Code = "F",
            Name = "Female",
            Sort = 2
        },

        // Leave Type
        new()
        {
            LookupId = Guid.Parse("C5CEA0DF-3868-43F0-8DDF-6BDBE28B316A"),
            Type = "LeaveType",
            Code = "AL",
            Name = "Annual Leave",
            Sort = 1
        },
        new()
        {
            LookupId = Guid.Parse("FA674EE6-6EB5-4DC4-AFB4-AE864227BCAE"),
            Type = "LeaveType",
            Code = "MC",
            Name = "Medical Leave",
            Sort = 2
        },
        new()
        {
            LookupId = Guid.Parse("6126C94F-1E55-44E9-B91D-276C56F09F42"),
            Type = "LeaveType",
            Code = "UL",
            Name = "Unpaid Leave",
            Sort = 3
        },

        // Marital Status
        new()
        {
            LookupId = Guid.Parse("B54C2AF6-1579-4AE0-A326-D6EB24C075F4"),
            Type = "Marital",
            Code = "SIN",
            Name = "Single",
            Sort = 1
        },
        new()
        {
            LookupId = Guid.Parse("61650D04-04E5-42A3-9020-7D0B4DDFB6E6"),
            Type = "Marital",
            Code = "MAR",
            Name = "Married",
            Sort = 2
        },
        new()
        {
            LookupId = Guid.Parse("D2A2C7B9-9D5F-4E80-9D49-4DD413848B66"),
            Type = "Marital",
            Code = "DIV",
            Name = "Divorced",
            Sort = 3
        },
        new()
        {
            LookupId = Guid.Parse("2FD7990C-7211-4FE6-8AA0-FF346683645E"),
            Type = "Marital",
            Code = "WID",
            Name = "Widowed",
            Sort = 4
        },

        // General Status
        new()
        {
            LookupId = Guid.Parse("832346DB-9AFE-41E5-875C-5AE21BEFD18A"),
            Type = "Status",
            Code = "ACT",
            Name = "Active",
            Sort = 1
        },
        new()
        {
            LookupId = Guid.Parse("E241B70A-9490-47FC-8857-58E6FACEFA75"),
            Type = "Status",
            Code = "INA",
            Name = "Inactive",
            Sort = 2
        },

        // User Status
        new()
        {
            LookupId = Guid.Parse("2389A136-F014-49F9-9D8E-5834BC3F9605"),
            Type = "UserStatus",
            Code = "ACT",
            Name = "Active",
            Sort = 1
        },
        new()
        {
            LookupId = Guid.Parse("DEAD4D8A-643B-4562-A65A-E9B71414421C"),
            Type = "UserStatus",
            Code = "LCK",
            Name = "Locked",
            Sort = 2
        },
        new()
        {
            LookupId = Guid.Parse("4177F3A9-8A7D-4F3C-8F07-DB2556999E71"),
            Type = "UserStatus",
            Code = "DIS",
            Name = "Disabled",
            Sort = 3
        }
    };

        var existingLookupIds = await _context.Lookups
            .Select(x => x.LookupId)
            .ToListAsync(cancellationToken);

        var existingIds = existingLookupIds.ToHashSet();

        var missingLookups = lookups
            .Where(x => !existingIds.Contains(x.LookupId))
            .ToList();

        if (missingLookups.Count == 0)
            return;

        await _context.Lookups.AddRangeAsync(
            missingLookups,
            cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);
    }

    private async Task SeedDepartmentAsync(
        CancellationToken cancellationToken)
    {
        if (await _context.Departments
            .AnyAsync(x => x.Id == AdministrationDepartmentId, cancellationToken))
            return;

        var department = new Department(AdministrationDepartmentId)
        {
            Name = "Administration",
            StatusCode = "ACT",
            CreatedBy = SystemUserId,
            CreatedAt = DateTimeOffset.UtcNow
        };

        await _context.Departments.AddAsync(department, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    private async Task SeedPositionAsync(
        CancellationToken cancellationToken)
    {
        if (await _context.Positions
            .AnyAsync(x => x.Id == SystemAdministratorPositionId, cancellationToken))
            return;

        var position = new Position(SystemAdministratorPositionId)
        {
            Name = "System Administrator",
            DepartmentId = AdministrationDepartmentId,
            StatusCode = "ACT",
            CreatedBy = SystemUserId,
            CreatedAt = DateTimeOffset.UtcNow
        };

        await _context.Positions.AddAsync(position, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    private async Task SeedEmployeeAsync(
        CancellationToken cancellationToken)
    {
        if (await _context.Employees
            .AnyAsync(x => x.Id == SystemAdministratorEmployeeId, cancellationToken))
            return;

        var employee = new Employee(SystemAdministratorEmployeeId)
        {
            EmployeeNo = "100007",
            FirstName = "System",
            MiddleName = null,
            LastName = "Administrator",
            GenderCode = "M",

            // Bootstrap account only.
            // This is not intended to represent a real employee.
            BirthDate = new DateTimeOffset(
                2000, 1, 1, 0, 0, 0, TimeSpan.Zero),

            Email = null,
            MobileNo = null,

            HireDate = DateTimeOffset.UtcNow,

            DepartmentId = AdministrationDepartmentId,
            PositionId = SystemAdministratorPositionId,

            StatusCode = "ACT",
            CreatedBy = SystemUserId,
            CreatedAt = DateTimeOffset.UtcNow
        };

        await _context.Employees.AddAsync(employee, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    private async Task SeedFunctionKeysAsync(
    CancellationToken cancellationToken)
    {
        var functionKeys = new List<FunctionKey>
    {
        new(EmployeeViewFunctionId)
        {
            FunctionCode = "EMPLOYEE_VIEW",
            DisplayName = "View Employees",
            Remarks = "View employee information",
            StatusCode = "ACT",
            CreatedBy = SystemUserId,
            CreatedAt = DateTimeOffset.UtcNow
        },
        new(EmployeeCreateFunctionId)
        {
            FunctionCode = "EMPLOYEE_CREATE",
            DisplayName = "Create Employees",
            Remarks = "Create new employee records",
            StatusCode = "ACT",
            CreatedBy = SystemUserId,
            CreatedAt = DateTimeOffset.UtcNow
        },
        new(EmployeeUpdateFunctionId)
        {
            FunctionCode = "EMPLOYEE_UPDATE",
            DisplayName = "Update Employees",
            Remarks = "Update existing employee records",
            StatusCode = "ACT",
            CreatedBy = SystemUserId,
            CreatedAt = DateTimeOffset.UtcNow
        },
        new(EmployeeDeleteFunctionId)
        {
            FunctionCode = "EMPLOYEE_DELETE",
            DisplayName = "Delete Employees",
            Remarks = "Deactivate employee records",
            StatusCode = "ACT",
            CreatedBy = SystemUserId,
            CreatedAt = DateTimeOffset.UtcNow
        },

        new(UserViewFunctionId)
        {
            FunctionCode = "USER_VIEW",
            DisplayName = "View Users",
            Remarks = "View user accounts",
            StatusCode = "ACT",
            CreatedBy = SystemUserId,
            CreatedAt = DateTimeOffset.UtcNow
        },
        new(UserCreateFunctionId)
        {
            FunctionCode = "USER_CREATE",
            DisplayName = "Create Users",
            Remarks = "Create new user accounts",
            StatusCode = "ACT",
            CreatedBy = SystemUserId,
            CreatedAt = DateTimeOffset.UtcNow
        },
        new(UserUpdateFunctionId)
        {
            FunctionCode = "USER_UPDATE",
            DisplayName = "Update Users",
            Remarks = "Update existing user accounts",
            StatusCode = "ACT",
            CreatedBy = SystemUserId,
            CreatedAt = DateTimeOffset.UtcNow
        },
        new(UserDeleteFunctionId)
        {
            FunctionCode = "USER_DELETE",
            DisplayName = "Delete Users",
            Remarks = "Deactivate user accounts",
            StatusCode = "ACT",
            CreatedBy = SystemUserId,
            CreatedAt = DateTimeOffset.UtcNow
        }
    };

        var existingIds = await _context.FunctionKeys
            .Select(x => x.Id)
            .ToListAsync(cancellationToken);

        var existingIdSet = existingIds.ToHashSet();

        var missingFunctionKeys = functionKeys
            .Where(x => !existingIdSet.Contains(x.Id))
            .ToList();

        if (missingFunctionKeys.Count == 0)
            return;

        await _context.FunctionKeys.AddRangeAsync(
            missingFunctionKeys,
            cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);
    }

    private async Task SeedRoleAsync(
        CancellationToken cancellationToken)
    {
        if (await _context.Roles
            .AnyAsync(x => x.Id == HrAdministratorRoleId, cancellationToken))
            return;

        var role = new Role(HrAdministratorRoleId)
        {
            Name = "HR Administrator",
            Description = "Manages employees, user accounts, roles, and permissions",
            StatusCode = "ACT",
            CreatedBy = SystemUserId,
            CreatedAt = DateTimeOffset.UtcNow
        };

        await _context.Roles.AddAsync(role, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
    private async Task SeedRoleFunctionsAsync(
    CancellationToken cancellationToken)
    {
        var roleFunctions = new List<RoleFunction>
    {
        new(EmployeeViewRoleFunctionId)
        {
            RoleId = HrAdministratorRoleId,
            FunctionKeyId = EmployeeViewFunctionId,
            StatusCode = "ACT",
            CreatedBy = SystemUserId,
            CreatedAt = DateTimeOffset.UtcNow
        },
        new(EmployeeCreateRoleFunctionId)
        {
            RoleId = HrAdministratorRoleId,
            FunctionKeyId = EmployeeCreateFunctionId,
            StatusCode = "ACT",
            CreatedBy = SystemUserId,
            CreatedAt = DateTimeOffset.UtcNow
        },
        new(EmployeeUpdateRoleFunctionId)
        {
            RoleId = HrAdministratorRoleId,
            FunctionKeyId = EmployeeUpdateFunctionId,
            StatusCode = "ACT",
            CreatedBy = SystemUserId,
            CreatedAt = DateTimeOffset.UtcNow
        },
        new(EmployeeDeleteRoleFunctionId)
        {
            RoleId = HrAdministratorRoleId,
            FunctionKeyId = EmployeeDeleteFunctionId,
            StatusCode = "ACT",
            CreatedBy = SystemUserId,
            CreatedAt = DateTimeOffset.UtcNow
        },

        new(UserViewRoleFunctionId)
        {
            RoleId = HrAdministratorRoleId,
            FunctionKeyId = UserViewFunctionId,
            StatusCode = "ACT",
            CreatedBy = SystemUserId,
            CreatedAt = DateTimeOffset.UtcNow
        },
        new(UserCreateRoleFunctionId)
        {
            RoleId = HrAdministratorRoleId,
            FunctionKeyId = UserCreateFunctionId,
            StatusCode = "ACT",
            CreatedBy = SystemUserId,
            CreatedAt = DateTimeOffset.UtcNow
        },
        new(UserUpdateRoleFunctionId)
        {
            RoleId = HrAdministratorRoleId,
            FunctionKeyId = UserUpdateFunctionId,
            StatusCode = "ACT",
            CreatedBy = SystemUserId,
            CreatedAt = DateTimeOffset.UtcNow
        },
        new(UserDeleteRoleFunctionId)
        {
            RoleId = HrAdministratorRoleId,
            FunctionKeyId = UserDeleteFunctionId,
            StatusCode = "ACT",
            CreatedBy = SystemUserId,
            CreatedAt = DateTimeOffset.UtcNow
        }
    };

        var existingIds = await _context.RoleFunctions
            .Select(x => x.Id)
            .ToListAsync(cancellationToken);

        var existingIdSet = existingIds.ToHashSet();

        var missingRoleFunctions = roleFunctions
            .Where(x => !existingIdSet.Contains(x.Id))
            .ToList();

        if (missingRoleFunctions.Count == 0)
            return;

        await _context.RoleFunctions.AddRangeAsync(
            missingRoleFunctions,
            cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);
    }
    private async Task SeedEmployeeRoleAsync(
    CancellationToken cancellationToken)
    {
        if (await _context.EmployeeRoles
            .AnyAsync(
                x => x.Id == SystemAdministratorEmployeeRoleId,
                cancellationToken))
            return;

        var employeeRole = new EmployeeRole(SystemAdministratorEmployeeRoleId)
        {
            EmployeeId = SystemAdministratorEmployeeId,
            RoleId = HrAdministratorRoleId,
            StatusCode = "ACT",
            CreatedBy = SystemUserId,
            CreatedAt = DateTimeOffset.UtcNow
        };

        await _context.EmployeeRoles.AddAsync(
            employeeRole,
            cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);
    }
    private async Task SeedUserAsync(
    CancellationToken cancellationToken)
    {
        if (await _context.Users
            .AnyAsync(x => x.Id == SystemUserId, cancellationToken))
            return;

        var superPassword = _configuration["Seed:SuperPassword"];

        if (string.IsNullOrWhiteSpace(superPassword))
        {
            throw new InvalidOperationException(
                "Seed:SuperPassword configuration is required to create the bootstrap user.");
        }

        var user = new User(SystemUserId)
        {
            EmployeeId = SystemAdministratorEmployeeId,
            UserName = "super",
            PasswordHash = _passwordHasher.Hash(superPassword),

            LastLogin = null,
            FailedLoginAttempt = 0,
            PasswordChangedDate = DateTimeOffset.UtcNow,
            MustChangePassword = true,
            IsLocked = false,

            StatusCode = "ACT",
            CreatedBy = SystemUserId,
            CreatedAt = DateTimeOffset.UtcNow
        };

        await _context.Users.AddAsync(user, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
}