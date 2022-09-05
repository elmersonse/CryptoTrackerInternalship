using CryptoTracker.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoTracker.DAL.Interfaces
{
    public interface IBaseRepository<T>
    {
        Task<bool> Create(T entity);
        Task<bool> Delete(T entity);
        IQueryable<T> GetAll();
        Task<T> Update(T entity);
    }
}
