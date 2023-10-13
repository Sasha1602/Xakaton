namespace API;

using MongoDB.Driver;
using MongoDB.Bson;

public class MyDbContext
{
    public MongoClient Client { get; set; }

    public MyDbContext()
    {
        Client = new MongoClient("mongodb://192.168.14.228");
    }
    
}