using API.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace API.Controllers;

using MongoDB.Driver;

[Route("api/data")]

public class PicEntityController : ControllerBase
{
    private readonly IMongoCollection<PicEntity> _picCollection;
    private readonly IMongoCollection<ResponseEntity> _responseCollection;

    public PicEntityController(IMongoClient client)
    {
        var database = client.GetDatabase("XakaDB");
        _picCollection = database.GetCollection<PicEntity>("generatedPics");
       _responseCollection = database.GetCollection<ResponseEntity>("responsesdb");
    }
    
    
    [HttpGet]
    public string GetPic(string responseBody)
    {
        var response = new ResponseEntity();
        response.Body = responseBody;

        var pic = _picCollection.Find(x => x.Id.ToString() == response.PicId);
        return String.Empty;

    }
    
}