using MongoDB.Bson;

namespace API.Models;

public class ResponseEntity
{
    public ObjectId Id { get; set; }
    
    public string Body { get; set; }

    public string PickId { get; set; }
    
}