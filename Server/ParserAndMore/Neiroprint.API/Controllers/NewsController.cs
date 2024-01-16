using DAL;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Controllers
{
    [ApiController]
    public class NewsController : ControllerBase
    {
        private static readonly MyDbContext DbContext = new MyDbContext();
        
        [HttpGet]
        [Route("News")]
        public async Task GetAllNews()
        {
            await Response.WriteAsJsonAsync(DbContext.News.ToList());
        }

        [HttpPost]
        [Route("NewPost")]
        public async Task CreatePost([FromBody] NewInfo post)
        {
            try
            {
                if (!await DbContext.News.AnyAsync(x => x.Title == post.Title))
                {
                    post.Id = Guid.NewGuid().ToString();
                    await DbContext.News.AddAsync(post);
                    await DbContext.SaveChangesAsync();
                    await Response.WriteAsJsonAsync(post);
                }
                else
                {
                    await Response.WriteAsJsonAsync(new { message = "This post already exist" });
                }
            }
            catch (Exception)
            {
                Response.StatusCode = 400;
                await Response.WriteAsJsonAsync(new { message = "Not correct data"});
            }
            
        }

        [HttpDelete]
        [Route("DeletePost")]
        public async Task DeletePost([FromBody] NewInfo post)
        {
            try
            {
                if (await DbContext.News.AnyAsync(x => x.Id == post.Id))
                {
                    DbContext.News.Remove(post);
                    await Response.WriteAsJsonAsync(post);
                }
                else
                {
                    await Response.WriteAsJsonAsync(new { message = "Not found" });
                }
            }
            catch (Exception)
            {
                Response.StatusCode = 404;
                await Response.WriteAsJsonAsync(new { message = "Not correct data" });
            }
        }
        
        [HttpPost]
        [Route("Refactor")]
        public async Task ChangePost([FromBody] NewInfo post)
        {
            try
            {
                if (await DbContext.News.AnyAsync(x => x.Id == post.Id))
                {
                    var postToUpdate = await DbContext.News.FindAsync(post.Id);
                    postToUpdate!.Body = post.Body;
                    postToUpdate.Title = post.Title;
                    
                    await DbContext.SaveChangesAsync();
                    await Response.WriteAsJsonAsync(postToUpdate);
                }
                else
                {
                    await Response.WriteAsJsonAsync(new { message = "Not found." });
                }
            }
            catch (Exception)
            {
                Response.StatusCode = 404;
                await Response.WriteAsJsonAsync(new { message = "Not correct data." });
            }
        }

        [HttpGet]
        [Route("findPost")]
        public async Task FindPostById([FromQuery] string id)
        {
            try
            {
                if (await DbContext.News.AnyAsync(x => x.Id == id))
                {
                    await Response.WriteAsJsonAsync(await DbContext.News.FirstOrDefaultAsync(x => x.Id == id));
                }
                else
                {
                    await Response.WriteAsJsonAsync(new { message = "Not correct data." });
                }
            }
            catch (Exception)
            {
                Response.StatusCode = 404;
                await Response.WriteAsJsonAsync(new { message = "Not found" });
            }
        }
    }
}
