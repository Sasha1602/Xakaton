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
            new UserEntity("admin", "admin"),
            new UserEntity("123", "123"),
        };

        [HttpGet]
        [Route("api/GetAllUsers")]
        public async Task GetAllUsers()
        {   
            await Response.WriteAsJsonAsync(persons);
        }

        [HttpPost]
        [Route("api/CreateUser")]
        public async Task CreateUser( HttpRequest request)
        {
            try
            {
                var userData = await request.ReadFromJsonAsync<UserEntity>();

                if (userData != null)
                {
                    var user = persons.FirstOrDefault(u => u.Id == userData.Id);

                    if (user != null)
                    {
                        await Response.WriteAsJsonAsync(user);
                    }
                    else
                    {
                        user = new UserEntity(userData.Login, userData.Password);
                        persons.Add(user);
                        await Response.WriteAsJsonAsync(user);
                    }
                }
                else
                {
                    Response.StatusCode = 400;
                    await Response.WriteAsJsonAsync(new { message = "Not correct data" });
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }
    }
}
