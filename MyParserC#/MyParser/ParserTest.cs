using System.Net;
using HtmlAgilityPack;
using MongoDB.Driver;
using MongoDB.Bson;

namespace MyParser;

public class ParserTest
{
    public static int maxPages = 50;

    /*public static Dictionary<char, string> directiries = new Dictionary<char, string>()
    {
        { 'Ж', "https://www.lamoda.ru/c/355/clothes-zhenskaya-odezhda/" },
        { 'М', "https://www.lamoda.ru/c/477/clothes-muzhskaya-odezhda" },
        { 'д', "https://www.lamoda.ru/c/1590/clothes-dlia-devochek/" },
        { 'м', "https://www.lamoda.ru/c/5378/default-malchikam/" }
    };*/

    public static string[] links =
        {
            "https://www.lamoda.ru/c/2474/clothes-tolstovki-olimpiyki/",
            "https://www.lamoda.ru/c/2512/clothes-muzhskie-futbolki/", 
            /*"https://www.lamoda.ru/c/1590/clothes-dlia-devochek/",
            "https://www.lamoda.ru/c/5378/default-malchikam/"*/
        };

    public static void Parse()
    {
        var mongoClient = new MongoClient("mongodb://192.168.14.228");
        var mongoDb = mongoClient.GetDatabase("XakaDB");
        
        for (int i = 0; i < links.Length; i++)
        {
            mongoDb.CreateCollection("myphotodb" + i);
            var collection = mongoDb.GetCollection<BsonDocument>("myphotodb" + i);
            var link = links[i];
            
            // бегаем по страницам
            for (int page = 1; page < maxPages; page++)
            {
                var workLink= link +"?page=" + page;
                var folder = "myphotodb" + i;

                // создание клиента для парсинга
                WebClient client = new WebClient();

                // Скачал html код в строку
                string pageContent = client.DownloadString(workLink);

                // обработка pageContent для того, чтобы с ней можно было работать 
                HtmlDocument document = new HtmlDocument();
                document.LoadHtml(pageContent);

                // выбираем по селектору class все классы, хранящие изображение на данной странице
                HtmlNodeCollection images =
                    document.DocumentNode.SelectNodes("//img[@class='x-product-card__pic-img']");

                // Перебор всех полученных классов и создание коллекции ссылок на сами картинки
                foreach (var image in images)
                {
                    string imageUrl = image.GetAttributeValue("src", "");

                    if (string.IsNullOrEmpty(imageUrl))
                    {
                        string srcSet = image.GetAttributeValue("srcset", "");
                        string[] urls = srcSet.Split(',');

                        imageUrl = urls[0].Split(' ')[0];
                    }

                    string fileName = $"image_{Guid.NewGuid()}.jpg";

                    client.DownloadFile("https:" + imageUrl, folder + "/" +fileName);
                    var doc = new BsonDocument { { "imageSrc", fileName } };
                    collection.InsertOne(doc);
                    Console.WriteLine($"Downlad file \"{fileName}\".");
                }
            }

        }

        Console.WriteLine("Parsing end.");
    }
    
}