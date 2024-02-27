using DAL;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Controllers
{
    [ApiController]
    public class ImageController : ControllerBase
    {
        public MyDbContext dbContext = new MyDbContext();


        [HttpGet]
        [Route("GetUserImages")]
        public async Task GetUserImages(string userId)
        {
            try
            {
                var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Id.ToString() == userId);
                if (user != null)
                {
                    var uImages = new List<UserEntityImages>();
                    foreach (var elem in dbContext.UserImages)
                    {
                        if (elem.UserId == user.Id)
                        {
                            uImages.Add(elem);
                        }
                    }

                    await Response.WriteAsJsonAsync(uImages);
                }

                Response.StatusCode = 404;
                await Response.WriteAsJsonAsync(new { message = "User not found" });
            }
            catch (Exception exception)
            {
                Response.StatusCode = 500;
            }
        }

        [HttpPut]
        [Route("AddImage")]
        public async Task AddImage(string? imageId, string? userId)
        {
            try
            {
                var user = dbContext.Users.FirstOrDefault(u => u.Id.ToString() == userId);
                if (user != null)
                {
                    {
                        if (dbContext.Images.FirstOrDefault(img => img.Id == imageId) != null)
                        {
                            var uImages = dbContext.UserImages.Where(x => x.UserId == user.Id);
                            await Response.WriteAsJsonAsync(uImages);
                        }
                        else
                        {
                            Response.StatusCode = 404;
                            await Response.WriteAsJsonAsync(new { message = "Image not found." });
                        }
                        
                        await Response.WriteAsJsonAsync(user);
                    }
                        
                    Response.StatusCode = 404;
                    await Response.WriteAsJsonAsync(new { message = "User not found." });
                }
            }
            catch (Exception)
            {
                await Response.WriteAsJsonAsync(new { message = "Not correct data." });
            }
        }

        [HttpPost]
        [Route("CreateImage")]
        public async Task CreateImage(string imagePath, string? clotheType, string? color, string? tone)
        {
            var image = new ImageEntity()
            {
                Id = Guid.NewGuid().ToString(),
                ClotheType = clotheType,
                Color = color,
                ImagePath = imagePath,
                Tone = tone,
            };

            dbContext.Images.Add(image);
            await dbContext.SaveChangesAsync();
            
            await Response.WriteAsJsonAsync(image);
        }
    }
}
