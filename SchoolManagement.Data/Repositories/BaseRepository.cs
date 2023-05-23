using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SchoolManagement.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Data.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private readonly ApplicationDbContext _dbContext;

        public BaseRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        //Expression<Func<T, bool>> expression là điều kiện
        public async Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>> expression = null)
        {
            //Nếu expression = null thì sẽ trả về dữ liệu ko có điều kiện
            if (expression == null)
            {
                return await _dbContext.Set<T>().ToListAsync();
            }
            //Nếu expression khác null thì tức là đang get data theo điều kiện
            return await _dbContext.Set<T>().Where(expression).ToListAsync();
        }

        public async Task<T> GetSingleByConditionAsync(Expression<Func<T, bool>> expression = null)
        {
            return await _dbContext.Set<T>().Where(expression).FirstOrDefaultAsync();
        }

        public async Task Create(T entity)
        {
            await _dbContext.AddAsync(entity);
            SaveChanges();
        }

        public async Task Update(T entity)
        {
            await _dbContext.SaveChangesAsync();
        }

        public void Delete(int id)
        {
            var query = _dbContext.Set<T>().Find(id);
            if (query != null)
            {
                _dbContext.Set<T>().Remove(query);
                SaveChanges();
            }
        }

        public void SaveChanges()
        {
            _dbContext.SaveChanges();
        }

        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }
            }
            this.disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
