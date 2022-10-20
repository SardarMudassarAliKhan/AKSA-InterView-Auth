using JWTToken_Auth_DAL.IRepository;
using JWTToken_Auth_DAL.Model;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;
using System;

namespace JWTToken_Auth_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;
        private readonly IEmployeeRepository _employee;
        private readonly IDepartmentRepository _department;
        public EmployeeController(IWebHostEnvironment env,
            IEmployeeRepository employee,
            IDepartmentRepository department)
        {
            _env = env;
            _employee = employee ?? throw new ArgumentNullException(nameof(employee));
            _department = department ?? throw new ArgumentNullException(nameof(department));
        }

        [HttpGet]
        [Route("GetEmployee")]
        public async Task<IActionResult> Get()
        {
            return Ok(await _employee.GetEmployees());
        }



        [HttpPost]
        [Route("AddEmployee")]
        public async Task<IActionResult> Post(Employee emp)
        {

            var result = await _employee.InsertEmployee(emp);
            if (result == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Something Went Wrong");
            }
            return Ok("Added Successfully");
        }


        [HttpPut]
        [Route("UpdateEmployee")]
        public async Task<IActionResult> Put(Employee emp)
        {
            var result = await _employee.UpdateEmployee(emp);
            if (result == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Something Went Wrong");
            }
            return Ok("Updated Successfully");
        }
        [HttpPost]
        [Route("GetEmployeeById")]
        public async Task<IActionResult> GetEmployeeById(String Id)
        {

            var result = await _employee.GetEmployeedByLogedInId(Id);
            if (result == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Something Went Wrong");
            }
            return Ok("Record Fetched Successfully");
        }

        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            var result = _employee.DeleteEmployee(id);
            return new JsonResult("Deleted Successfully");
        }
        [Route("GetAllDepartmentNames")]
        [HttpGet]
        public async Task<IActionResult> GetAllDepartmentNames()
        {
            return Ok(await _department.GetDepartment());
        }
    }
}
