using API.Models;
using Microsoft.AspNetCore.Mvc;

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


    
}