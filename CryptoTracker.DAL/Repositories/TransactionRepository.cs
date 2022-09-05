using System;
using System.Linq;
using System.Threading.Tasks;
using CryptoTracker.DAL.Interfaces;
using CryptoTracker.Domain.Entity;

namespace CryptoTracker.DAL.Repositories
{
    public class TransactionRepository : IBaseRepository<Transaction>
    {
        private readonly ApplicationDbContext _db;

        public TransactionRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        
        public async Task<bool> Create(Transaction entity)
        {
            _db.Transactions.Add(entity);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Delete(Transaction entity)
        {
            _db.Transactions.Remove(entity);
            await _db.SaveChangesAsync();
            return true;
        }

        public IQueryable<Transaction> GetAll()
        {
            return _db.Transactions;
        }

        public async Task<Transaction> Update(Transaction entity)
        {
            _db.Transactions.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
    }
}