using JWTToken_Auth_DAL.Dto;
using JWTToken_Auth_DAL.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JWTToken_Auth_BAL.Service
{
    public class PersonService
    {
        private readonly IRepository<RegisterModel> _registermodel;

    public PersonService(IRepository<RegisterModel> perosn)
    {
            _registermodel = perosn;
    }
    //Get Person Details By Person Id
    public IEnumerable<RegisterModel> GetPersonByUserId(string UserId)
    {
        return _registermodel.GetAll().ToList();
    }
    //GET All Perso Details 
    public IEnumerable<RegisterModel> GetAllPersons()
    {
        try
        {
            return _registermodel.GetAll().ToList();
        }
        catch (Exception)
        {
            throw;
        }
    }
    //Get Person by Person Name
    public RegisterModel GetPersonByUserName(string UserName)
    {
        return _registermodel.GetAll().FirstOrDefault();
    }
    //Add Person
    public async Task<RegisterModel> AddPerson(RegisterModel Person)
    {
        return await _registermodel.Create(Person);
    }
    //Delete Person 
    public bool DeletePerson(string UserEmail)
    {

        try
        {
            var DataList = _registermodel.GetAll().ToList();
            foreach (var item in DataList)
            {
                    _registermodel.Delete(item);
            }
            return true;
        }
        catch (Exception)
        {
            return true;
        }

    }
    //Update Person Details
    public bool UpdatePerson(RegisterModel person)
    {
        try
        {
            var DataList = _registermodel.GetAll().ToList();
            foreach (var item in DataList)
            {
                    _registermodel.Update(item);
            }
            return true;
        }
        catch (Exception)
        {
            return true;
        }
    }
}
}