using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
 
var client = new MongoClient("mongodb://192.168.14.228");
var db = client.GetDatabase("XakaDB");
 
var builder = WebApplication.CreateBuilder();
var app = builder.Build();
// по адресу "/" отправляем список с ссылками на файлы
app.MapGet("/", async(context) =>
{
    IGridFSBucket gridFS = new GridFSBucket(db);
    // получаем список файлов
    var files = await gridFS.FindAsync("{}");
    string fileListHtml = "";
    // формируем из списка файлов список ul с ссылками на файлы 
    foreach(var file in files.ToList())
    {
        fileListHtml = @$"{fileListHtml}<li>
                            <p><a href='file/{file.Id}'>{file.Filename}</a> </p>
                            <form action='/delete/{file.Id}' method='post'><input type='submit' value='Удалить' /></form>
                           </li>";
    }
    // отправляем сформированный код html
    context.Response.ContentType = "text/html";
    await context.Response.WriteAsync(@$"<!DOCTYPE html><html>
                                <head>
                                    <meta charset='utf-8'/>
                                    <title>METANIT.COM</title>
                                </head>
                                <body>
                                    <h2>Список файлов</h2>
                                    <ul>{fileListHtml}</ul>
                                </body></html>");
});
// по адресу "/file/{id}" отправляем файл по id
app.MapGet("/file/{id}", async (HttpContext context, string id) =>
{
    IGridFSBucket gridFS = new GridFSBucket(db);
    await gridFS.DownloadToStreamAsync(new ObjectId(id), context.Response.Body);
});

app.Run();