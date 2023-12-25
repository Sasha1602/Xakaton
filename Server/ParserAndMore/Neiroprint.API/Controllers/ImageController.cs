using DAL;
using Domain;
using Microsoft.AspNetCore.Mvc;

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
            if (dbContext.Users.FirstOrDefault(u => u.Id == userId) != null)
            {
                var user = await dbContext.Users.FindAsync(userId);
                var imageIdList = user!.Images;
                var images = new List<ImageEntity>();

                foreach (var imageId in imageIdList)
                {
                    images.Add(dbContext.Images.FirstOrDefault(img => img.Id == imageId));
                }

                await Response.WriteAsJsonAsync(images.Where(img => img != null));
            }
            else
            {
                Response.StatusCode = 404;
                await Response.WriteAsJsonAsync(new { message = "User not Found" });
            }
            
        }

        [HttpPut]
        [Route("AddImage")]
        public async Task AddImage(string? imageId, string? userId)
        {
            try
            {
                var user = dbContext.Users.FirstOrDefault(u => u.Id == userId);

                if (user != null)
                {
                    if (dbContext.Images.FirstOrDefault(img => img.Id == imageId) != null)
                    {
                        user.Images.Add(imageId);
                        await Response.WriteAsJsonAsync(user);
                    }
                    else
                    {
                        Response.StatusCode = 404;
                        await Response.WriteAsJsonAsync(new { message = "Image not found." });
                    }
                }
                else
                {
                    Response.StatusCode = 404;
                    await Response.WriteAsJsonAsync(new { message = "User not found."});
                }

                await Response.WriteAsJsonAsync(user);
            }
            catch(Exception)
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
