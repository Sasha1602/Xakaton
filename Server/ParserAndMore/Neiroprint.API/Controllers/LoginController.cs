using System.Reflection.Metadata;
using Domain;
using DAL;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        public MyDbContext dbContext = new MyDbContext();
        /*private List<UserEntity> _persons = new List<UserEntity>
        {
            new UserEntity {Login = "admin", Password = "admin", Id = "1"},
            new UserEntity { Login = "123", Password = "123", Id = "2"},
        };*/

        [HttpGet]
        public async Task GetAllUsers()
        {
            var collection = dbContext.Users.ToList();
            await Response.WriteAsJsonAsync(collection);
        }
        
        
        [HttpPost]
        public async Task CreatePerson([FromBody] UserEntity person)
        {
            try
            {
                //Check user is not already exist
                var user = dbContext.Users.ToList().FirstOrDefault(user => user.Login == person.Login);
                
                // check request params
                if (user == null)
                {
                    // set new user's Id
                    person.Id = Guid.NewGuid().ToString();
                    // add user to database
                    await dbContext.Users.AddAsync(person);
                    await dbContext.SaveChangesAsync();
                    // send entity back
                    await Response.WriteAsJsonAsync(person);
                }
                else
                {
                    await Response.WriteAsJsonAsync(new { message = "User already exist" });
                }
            }
            catch (Exception)
            {
                Response.StatusCode = 400;
                await Response.WriteAsJsonAsync(new { message = "Not correct data"});
            }
        }
        
        [HttpDelete]
        public async Task DeleteUser([FromBody] UserEntity? user)
        {
            try
            {
                //check request body not null
                if (user != null)
                {
                    // Check current Id is exist in database
                    var person = dbContext.Users.FirstOrDefault(u => u.Id == user.Id);
                    
                    if (person != null)
                    {
                        //delete user
                        dbContext.Users.Remove(person);
                        await dbContext.SaveChangesAsync();
                        //return deleted user back
                        await Response.WriteAsJsonAsync(person);
                    }
                    else
                    {
                        // user is not exist in database
                        Response.StatusCode = 404;
                        await Response.WriteAsJsonAsync(new { message = "Not Found" });

                    }
                }
                else
                {
                    
                    Response.StatusCode = 404;
                    await Response.WriteAsJsonAsync(new { message = "Not Found" });

                }
            }
            catch(Exception)
            {
                await Response.WriteAsJsonAsync(new { message = "Not correct data" });
            }
        }
    }
}
