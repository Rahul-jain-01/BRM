using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChainMarketing.Domain.Entities;
using ChainMarketing.Domain.Interfaces;
using ChainMarketing.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using ChainMarketing.Domain.Enums;


namespace ChainMarketing.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _db;
        public UserRepository(AppDbContext appDbContext)
        {
            _db = appDbContext;
            
        }
        public async Task AddAsync(User user)
        {
            _db.Users.Add(user);
            await _db.SaveChangesAsync();
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _db.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _db.Users.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<List<User>> GetAllUserAync()
        {
            return await _db.Users.Where(u=>u.Role!=UserRole.Admin).ToListAsync();
        }
        public async Task<User?> GetByReferralCodeAsync(string referralCode)
        {
            return await _db.Users.FirstOrDefaultAsync(u => u.ReferralCode == referralCode);
        }

        public async Task<List<User>> GetDirectReferralsAsync(int userId)
        {
            return await _db.Users
        .Where(u => u.ReferredById == userId)
        .ToListAsync();
        }

        public async Task UpdateAsync(User user)
        {
            _db.Users.Update(user);  // Mark the user for update
            await _db.SaveChangesAsync();  // Save changes to DB
        }

        public async Task AssignCoApplicantAsync(int userId, int? selectedReferralId = null)
        {
            var user = await _db.Users
                .Include(u => u.DirectReferrals.OrderBy(r => r.CreatedAt)) // or .OrderBy(r => r.Id)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null || !user.DirectReferrals.Any())
                return;

            if (selectedReferralId.HasValue)
            {
                var allowedReferrals = user.DirectReferrals.Take(2);
                if (allowedReferrals.Any(r => r.Id == selectedReferralId.Value))
                {
                    user.CoApplicantId = selectedReferralId.Value;
                }
            }
            else if (user.CoApplicantId == null && user.DirectReferrals.Count >= 3)
            {
                user.CoApplicantId = user.DirectReferrals[2].Id;
            }

            await _db.SaveChangesAsync();
        }

    }
}
