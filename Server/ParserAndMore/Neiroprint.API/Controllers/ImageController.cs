using DAL;
using Microsoft.AspNetCore.Mvc;

namespace Neiroprint.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private static MyDbContext _dbContext = new MyDbContext();

        [HttpGet]
        public string? GetPhotoPathByRequestId(string id)
        {
            if (_dbContext.Requsets.Any(x => x.Id.ToString() == id))
            {
                var request = _dbContext.Requsets.Find(id);
                return request?.Photo?.ImagePath;
            }

            return null;
        }
        // ToDo: Add Responses and controllers for requestEntity ResponseEntity UserEntity
    }
}
