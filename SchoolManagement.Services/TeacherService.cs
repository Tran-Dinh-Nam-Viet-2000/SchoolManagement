using Microsoft.EntityFrameworkCore;
using SchoolManagement.Contracts.Dtos;
using SchoolManagement.Data.Context;
using SchoolManagement.Data.Entity;
using SchoolManagement.Data.Repositories;
using SchoolManagement.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Services
{
    public class TeacherService : ITeacherService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IBaseRepository<Teachers> _repository;

        public TeacherService(ApplicationDbContext dbContext, IBaseRepository<Teachers> repository)
        {
            _dbContext = dbContext;
            _repository = repository;
        }
        public async Task<Teachers> Create(TeacherDto teacher)
        {
            var create = new Teachers()
            {
                Name = teacher.Name,
                Age = teacher.Age,
                Phone = teacher.Phone,
                ClassId = teacher.ClassId,
            };
            await _repository.Create(create);
            return create;
        }

        public void Delete(int id)
        {
            _repository.Delete(id);
        }

        public async Task<IEnumerable<Teachers>> GetAll()
        {
            return await _repository.GetAll();
        }

        public async Task<IEnumerable<Teachers>> GetDataByCondition(Expression<Func<Teachers, bool>> expression = null)
        {
            return await _repository.GetAll(n => n.Id == 9);
        }

        public async Task<IEnumerable<Teachers>> Search(string keyword)
        {
            var search = await _dbContext.Teachers.Where(n => n.Name.Contains(keyword) ||
                                                        n.Age.ToString().Contains(keyword) ||
                                                        n.Phone.ToString().Contains(keyword))
                .Select(x => new Teachers
                {
                    Id = x.Id,
                    Name = x.Name,
                    Age = x.Age,
                    Phone = x.Phone
                }).ToListAsync();
            return search;
        }

        public async Task<Teachers> Update(TeacherDto teacher, int id)
        {
            var query = _dbContext.Teachers.FirstOrDefault(n => n.Id == id);
            if (query == null) return null;
            
            query.Age = teacher.Age;
            query.Phone = teacher.Phone;
            query.Name = teacher.Name;
            query.ClassId = teacher.ClassId;
            await _repository.Update(query);
            
            return query;
        }
    }
}
