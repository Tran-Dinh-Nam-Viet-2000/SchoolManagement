using SchoolManagement.Contracts.Dtos;
using SchoolManagement.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Services.Interface
{
    public interface ITeacherService
    {
        Task<IEnumerable<Teachers>> Search(string keyword);
        Task<IEnumerable<Teachers>> GetAll();
        //Lấy giá trị theo điều kiện
        Task<IEnumerable<Teachers>> GetDataByCondition(Expression<Func<Teachers, bool>> expression = null);
        Task<Teachers> Create(TeacherDto teacher);
        Task<Teachers> Update(TeacherDto teacher, int id);
        void Delete(int id);
    }
}
