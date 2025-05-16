using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChainMarketing.Domain.Enums;

namespace ChainMarketing.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }

        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;

        public UserRole Role { get; set; } = UserRole.User; // Default to User

        public string ReferralCode { get; set; } = null!;
        public int? ReferredById { get; set; }

        public User? ReferredBy { get; set; }
        public List<User> DirectReferrals { get; set; } = new();

        public bool HasCoApplicant { get; set; }
        public bool IsPaid { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
