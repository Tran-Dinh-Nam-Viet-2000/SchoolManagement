using Microsoft.EntityFrameworkCore;
using SchoolManagement.Contracts.Dtos;
using SchoolManagement.Data.Context;
using SchoolManagement.Data.Entity;
using SchoolManagement.Data.Repositories;
using SchoolManagement.Services.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Services.Services
{
    public class TeacherService : ITeacherService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IBaseRepository<Teacher> _repository;

        public TeacherService(ApplicationDbContext dbContext, IBaseRepository<Teacher> repository)
        {
            _dbContext = dbContext;
            _repository = repository;
        }
        public async Task<Teacher> Create(TeacherDto teacher)
        {
            var create = new Teacher() {
                Name = teacher.Name,
                Age = teacher.Age,
                Phone = teacher.Phone
            };
            await _repository.Create(create);
            return create;
        }

        public IEnumerable<Teacher> GetAll()
        {
            return _repository.GetAll();
        }

        public async Task<IEnumerable<Teacher>> Search(string keyword)
        {
            var search = await _dbContext.Teachers.Where(n => n.Name.Contains(keyword) ||
                                                        n.Age.ToString().Contains(keyword) ||
                                                        n.Phone.ToString().Contains(keyword))
                .Select(x => new Teacher
                {
                    Id= x.Id,
                    Name = x.Name,
                    Age = x.Age,
                    Phone = x.Phone
                }).ToListAsync();
            return search;
        }
    }
}
