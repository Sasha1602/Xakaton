using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        List<UserEntity> persons = new List<UserEntity>
        {
            new UserEntity {Login = "admin", Password = "admin"},
            new UserEntity { Login = "123", Password = "123"},
        };

        [HttpGet]
        [Route("api/GetAllUsers")]
        public async Task GetAllUsers()
        {   
            await Response.WriteAsJsonAsync(persons);
        }

        [HttpPost]
        [Route("api/CreateUser")]
        public async Task CreatePerson([FromBody] UserEntity person)
        {
            try
            {
                // check request params
                if (person != null)
                {
                    // set new user's Id
                    person.Id = Guid.NewGuid().ToString();
                    // add user to database
                    persons.Add(person);
                    await Response.WriteAsJsonAsync(person);
                }
                else
                {
                    throw new Exception("Not correct data");
                }
            }
            catch (Exception)
            {
                Response.StatusCode = 400;
                await Response.WriteAsJsonAsync(new { message = "Not correct data" });
            }
        }
    }
}
