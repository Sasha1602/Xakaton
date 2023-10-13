using API.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RestEntityController : ControllerBase
{

    public MyDbContext _dbContext = new MyDbContext();

    [HttpPost]
    public void PostRequest(string body)
    {
        var response = new BsonElement("response", new BsonString(body));
        var collection = _dbContext.Client.GetDatabase("XakaDB").GetCollection<BsonDocument>("generatedPics");
        collection.Add
    }
    
}


    
