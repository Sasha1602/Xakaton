using System.Reflection.Metadata;
using Domain;
using DAL;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    [ApiController]
    public class LoginController : ControllerBase
    {
        public MyDbContext dbContext = new MyDbContext();
        
        [HttpGet]
        [Route("getAllUsers")]
        public async Task GetAllUsers()
        {
            await Response.WriteAsJsonAsync(dbContext.Users.ToList());
        }
        
        
        [HttpPost]
        [Route("createAccount")]
        public async Task CreatePerson(string? login, string? password)
        {
            try
            {
                //Check user is not already exist
                var user = dbContext.Users.ToList().FirstOrDefault(user => user.Login == login);
                
                // check request params
                if (user == null)
                {
                    // set new user's Id
                    user = new UserEntity();
                    // add user to database
                    user.Login = login;
                    user.SetPassword(password);
                    await dbContext.Users.AddAsync(user);
                    await dbContext.SaveChangesAsync();
                    // send entity back
                    await Response.WriteAsJsonAsync(user);
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
            // ToDo: add data to the UserImages table
        }
        
        [HttpDelete]
        [Route("deleteAccount")]
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
                    // request body is null
                    Response.StatusCode = 404;
                    await Response.WriteAsJsonAsync(new { message = "Not Found" });

                }
            }
            catch(Exception)
            {
                await Response.WriteAsJsonAsync(new { message = "Not correct data" });
            }
        }

        [HttpGet]
        [Route("Login")]
        public async Task VerifyUser(string? login, string? password)
        {
            try
            {
                if (login != null && password != null)
                {
                    var person = dbContext.Users.FirstOrDefault(u => u.Login == login);

                    if (person != null && person.VerifyPassword(password))
                    {
                        await Response.WriteAsJsonAsync(person);
                    }
                }
                else
                {
                    Response.StatusCode = 404;
                    await Response.WriteAsJsonAsync(new { message = "Not Fount" });
                }
            }
            catch (Exception)
            {
                await Response.WriteAsJsonAsync(new { message = "Not correct data" });
            }
        }
    }
}
