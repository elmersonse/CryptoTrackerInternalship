using CryptoTracker.DAL.Interfaces;
using CryptoTracker.Domain.Entity;
using CryptoTracker.Domain.Enum;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoTracker.DAL.Repositories
{
    public class DealRepository : IBaseRepository<Deal>
    {
        private readonly ApplicationDbContext _db;

        public DealRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<bool> Create(Deal entity)
        {
            await _db.Deals.AddAsync(entity);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Delete(Deal entity)
        {
            _db.Deals.Remove(entity);
            await _db.SaveChangesAsync();
            return true;
        }

        public IQueryable<Deal> GetAll()
        {
            return _db.Deals;
        }

        public async Task<Deal> Update(Deal entity)
        {
            _db.Deals.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
    }
}
