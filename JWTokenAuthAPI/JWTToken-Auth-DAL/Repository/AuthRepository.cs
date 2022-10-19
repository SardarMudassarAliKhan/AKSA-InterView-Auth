using JWTToken_Auth_DAL.Data;
using JWTToken_Auth_DAL.Dto;
using JWTToken_Auth_DAL.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JWTToken_Auth_DAL.Repository
{
    public class AuthRepository : IRepository<RegisterModel>
    {
        private readonly ApplicationDbContext _dbContext;
        public AuthRepository(ApplicationDbContext applicationDbContext)
        {
            _dbContext = applicationDbContext;
        }
        public async Task<RegisterModel> Create(RegisterModel _object)
        {
            var obj = await _dbContext.RegisterModel.AddAsync(_object);
            _dbContext.SaveChanges();
            return obj.Entity;
        }

        public void Delete(RegisterModel _object)
        {
            _dbContext.Remove(_object);
            _dbContext.SaveChanges();
        }

        public IEnumerable<RegisterModel> GetAll()
        {
            try
            {
                return _dbContext.RegisterModel.ToList();
            }
            catch (Exception ee)
            {
                throw;
            }
        }

        public RegisterModel GetById(int Id)
        {
            return _dbContext.RegisterModel.FirstOrDefault();
        }

        public void Update(RegisterModel _object)
        {
            _dbContext.RegisterModel.Update(_object);
            _dbContext.SaveChanges();
        }
    }
}