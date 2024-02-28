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
        public async Task GetUserImages(Guid? userId)
        {
            try
            {
                var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
                if (user != null)
                {
                    await Response.WriteAsJsonAsync(dbContext.UserImages.Where(x => x.UserId == userId).ToListAsync());
                }
                else
                {
                    Response.StatusCode = 404;
                    await Response.WriteAsJsonAsync(new { message = "User not found" });
                }
            }
            catch (Exception exception)
            {
                Response.StatusCode = 500;
            }
        }

        [HttpPut]
        [Route("AddImage")]
        public async Task AddImage(Guid? imageId, Guid? userId)
        {
            try
            {
                if (dbContext.Users.FirstOrDefault(u => u.Id == userId) != null)
                {
                    if (dbContext.Images.FirstOrDefault(img => img.Id == imageId) != null)
                    {
                        var uImage = new UserEntityImages() { UserId = userId, ImageId = imageId };
                        await dbContext.UserImages.AddAsync(uImage);
                        await dbContext.SaveChangesAsync();

                        await Response.WriteAsJsonAsync(uImage);
                    }
                    else
                    {
                        Response.StatusCode = 404;
                        await Response.WriteAsJsonAsync(new { message = "Image not found." });
                    }
                }
                Response.StatusCode = 404;
                await Response.WriteAsJsonAsync(new { message = "User not found." });
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
