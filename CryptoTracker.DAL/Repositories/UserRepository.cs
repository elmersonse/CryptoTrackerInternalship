using System.Linq;
using System.Threading.Tasks;
using CryptoTracker.DAL.Interfaces;
using CryptoTracker.Domain.Entity;

namespace CryptoTracker.DAL.Repositories
{
    public class UserRepository : IBaseRepository<User>
    {
        private readonly ApplicationDbContext _db;

        public UserRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<bool> Create(User entity)
        {
            _db.Users.Add(entity);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Delete(User entity)
        {
            _db.Users.Remove(entity);
            await _db.SaveChangesAsync();
            return true;
        }

        public IQueryable<User> GetAll()
        {
            return _db.Users;
        }

        public async Task<User> Update(User entity)
        {
            _db.Users.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
    }
}