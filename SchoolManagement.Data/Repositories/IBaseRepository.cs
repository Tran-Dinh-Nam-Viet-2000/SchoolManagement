using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Data.Repositories
{
    public interface IBaseRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>> expression = null);
        Task Create(T entity);
        Task Update(T entity);
        void Delete(int id);

        //Hàm lưu vào db
        void SaveChanges();
        Task<T> GetSingleByConditionAsync(Expression<Func<T, bool>> expression = null);

        //Expression<Func<T, bool>> expression là xóa theo điều kiện
        //void Delete(Expression<Func<T, bool>> expression);
    }
}
