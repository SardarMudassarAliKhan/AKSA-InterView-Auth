using JWTToken_Auth_DAL.Data;
using JWTToken_Auth_DAL.IRepository;
using JWTToken_Auth_DAL.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JWTToken_Auth_DAL.Repository
{
    public class DepartmentRepository : IDepartmentRepository
    {

        private readonly ApplicationDbContext _appDBContext;

        public DepartmentRepository(ApplicationDbContext context)
        {
            _appDBContext = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<Department>> GetDepartment()
        {
            return await _appDBContext.Departments.ToListAsync();
        }

        public async Task<Department> GetDepartmentByID(int ID)
        {
            return await _appDBContext.Departments.FindAsync(ID);
        }
        public async Task<Department> GetDepartmentByLoggedInUserId(string ID)
        {
            return await _appDBContext.Departments.Where(x=>x.LogInId== ID).FirstOrDefaultAsync();
        }
        public async Task<Department> InsertDepartment(Department objDepartment)
        {
            _appDBContext.Departments.Add(objDepartment);
            await _appDBContext.SaveChangesAsync();
            return objDepartment;

        }

        public async Task<Department> UpdateDepartment(Department objDepartment)
        {
            _appDBContext.Entry(objDepartment).State = EntityState.Modified;
            await _appDBContext.SaveChangesAsync();
            return objDepartment;

        }

        public bool DeleteDepartment(int ID)
        {
            bool result = false;
            var department = _appDBContext.Departments.Find(ID);
            if (department != null)
            {
                _appDBContext.Entry(department).State = EntityState.Deleted;
                _appDBContext.SaveChanges();
                result = true;
            }
            else
            {
                result = false;
            }
            return result;
        }
    }
}