using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChainMarketing.Domain.Entities
{
    public class UserReferralPath
    {
        public int Id { get; set; }

        public int RootUserId { get; set; }       // The user whose tree this belongs to
        public int ReferredUserId { get; set; }   // The user being referred (indirect or direct)
        public int Level { get; set; }            // 1, 2, or 3

        // Navigation (optional but helpful)
        public User RootUser { get; set; } = null!;
        public User ReferredUser { get; set; } = null!;
    }
}
