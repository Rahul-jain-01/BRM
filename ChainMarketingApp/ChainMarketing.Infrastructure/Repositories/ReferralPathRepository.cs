using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChainMarketing.Application.Interfaces;
using ChainMarketing.Domain.Entities;
using ChainMarketing.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ChainMarketing.Infrastructure.Repositories
{
    public class ReferralPathRepository : IReferralPathRepository
    {
        //private readonly AppDbContext _db;

        //public ReferralPathRepository(AppDbContext db)
        //{
        //    _db = db;
        //}
        //public async Task AddReferralPathAsync(int referrerId, int userId, int level)
        //{
        //    var referralPath = new ReferralPath
        //    {
        //        ReferrerId = referrerId,
        //        UserId = userId,
        //        Level = level
        //    };

        //    _db.ReferralPaths.Add(referralPath);
        //    await _db.SaveChangesAsync();
        //}

        //public async  Task<List<ReferralPath>> GetReferralPathsAsync(int userId)
        //{
        //    return await _db.ReferralPaths
        //    .Where(r => r.UserId == userId)
        //    .ToListAsync();
        //}

        private readonly AppDbContext _dbContext;

        public ReferralPathRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddReferralPathAsync(ReferralPath referralPath)
        {
            await _dbContext.ReferralPaths.AddAsync(referralPath);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<ReferralPath> GetReferralPathAsync(int userId)
        {
            return await _dbContext.ReferralPaths
                .Where(rp => rp.UserId == userId)
                .FirstOrDefaultAsync();
        }

        public async Task<List<User>> GetDirectReferralsAsync(int userId)
        {
            return await _dbContext.ReferralPaths
                .Where(rp => rp.ReferrerId == userId)
                .GroupBy(rp => rp.UserId)
                .Select(g => g.First().User)
               
                .ToListAsync();
        }


        public async Task<List<ReferralPath>> GetReferralPathsAsync(int referrerId)
        {
            return await _dbContext.ReferralPaths
             .Where(rp => rp.ReferrerId == referrerId && rp.Level == 1)
             .Include(rp => rp.User)
     
             .ToListAsync();

        }
        public async Task<List<ReferralPath>> GetReferralPathsByLevelAsync(int referrerId, int level)
        {
            return await _dbContext.ReferralPaths
                .Where(rp => rp.ReferrerId == referrerId && rp.Level == level)
                .Include(rp => rp.User)
                .ToListAsync();
        }


        public async Task<List<ReferralPath>> GetAllReferralPathsAsync()
        {
            return await _dbContext.ReferralPaths
                .Include(rp => rp.User)
                .Include(rp => rp.Referrer)
                .ToListAsync();
        }


    }
}
