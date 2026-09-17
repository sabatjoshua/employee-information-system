using EmployeeInformationSystem.Domain.Common;
using System;

namespace EmployeeInformationSystem.Domain.Entities
{
    public class User : AuditableEntity
    {
        public User()
        {
        }

        public User(Guid id)
        {
            Id = id;
        }

        public required Guid EmployeeId { get; set; }
        public required string UserName { get; set; }
        public required string PasswordHash { get; set; }
        public DateTimeOffset? LastLogin { get; set; }
        public required int FailedLoginAttempt { get; set; }
        public DateTimeOffset? PasswordChangedDate { get; set; }
        public required bool MustChangePassword { get; set; }
        public required bool IsLocked { get; set; }
    }
}