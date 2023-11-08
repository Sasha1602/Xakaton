using MongoDB.Bson;

namespace API.Models;

public class PicEntity
{
    public ObjectId Id { get; set; }

    public string Name { get; set; }

    public byte[] ImageCode { get; set; }
}