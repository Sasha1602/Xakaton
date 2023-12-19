using Domain;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private List<UserEntity> _persons = new List<UserEntity>
        {
            new UserEntity {Login = "admin", Password = "admin", Id = "1"},
            new UserEntity { Login = "123", Password = "123", Id = "2"},
        };

        [HttpGet]
        [Route("api/GetAllUsers")]
        public async Task GetAllUsers()
        {
            var collection = _persons;
            await Response.WriteAsJsonAsync(collection);
        }

        [HttpPost]
        [Route("api/CreateUser")]
        public async Task CreatePerson([FromBody] UserEntity person)
        {
            try
            {
                var user = _persons.FirstOrDefault(user => user.Login == person.Login);
                
                // check request params
                if (user is null)
                {
                    // set new user's Id
                    person.Id = Guid.NewGuid().ToString();
                    // add user to database
                    _persons.Add(person);
                    await Response.WriteAsJsonAsync(person);
                }
                else
                {
                    await Response.WriteAsJsonAsync(new { message = "Not correct data" });
                }
            }
            catch (Exception)
            {
                Response.StatusCode = 400;
                await Response.WriteAsJsonAsync(new { message = "Not correct data" });
            }
        }
        
        [HttpDelete]
        [Route("DeleteUser")]
        public async Task DeleteUser([FromBody] string? id)
        {
            // Check current Id is exist
            var user = _persons.FirstOrDefault(u => u.Id == id);
            {
                if (user != null)
                {
                    //Delete user from db
                    _persons.Remove(user);
                    await Response.WriteAsJsonAsync(user);
                }
                else
                {
                    Response.StatusCode = 404;
                    await Response.WriteAsJsonAsync(new {mewssage = "User not found."});
                }
            }
        }
    }
}
