using API.Models;
using Microsoft.Extensions.FileProviders;
using MongoDB.Driver;

/*var client = new MongoClient("mongodb://192.168.14.228");
var db = client.GetDatabase("XakaDB");*/
 
var builder = WebApplication.CreateBuilder();
builder.Services.AddSingleton(new MongoClient("mongodb://192.168.14.228"));

builder.Services.AddControllers();

var app = builder.Build();

app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.UseFileServer(new FileServerOptions
{
    EnableDirectoryBrowsing = true,
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot")),
    RequestPath = new PathString("/pages"),
    EnableDefaultFiles = false
});

app.Run();