namespace ShopParser;

using HtmlAgilityPack;
using System.Net;
using System.Text.RegularExpressions;
using MongoDB.Driver;

public class ParserModis

{
    private static int _imagesCount = 1;

    public static string[] links =
    {
        "https://modis.ru/woman/",
        "https://modis.ru/men/",
        "https://modis.ru/children/",
    };

    public static string? ExtractValueFromJs(string pageCode, string key, string pattern)
    {
        var regex = new Regex(pattern);
        var match = regex.Match(pageCode);

        if (match.Success)
        {
            var value = match.Groups[1].Value;
            return value;
        }
        return null;
    }
    
    public static void ParseModis()
    {
        var mongoClient = new MongoClient("mongodb://localhost");
        var mongoDb = mongoClient.GetDatabase("XakaDB");
        for (int i = 0; i < links.Length; i++)
        {
            var link = links[i];

            var webClient = new WebClient();
            var pageCode = webClient.DownloadString(link);

            var normalPageCode = new HtmlDocument();
            normalPageCode.LoadHtml(pageCode);
            
            var products =
                normalPageCode.DocumentNode.SelectNodes("//a[@class='incard']");

            foreach (var node in products)
            {
                var href = node.GetAttributeValue("href", "");

                if (href != null)
                {
                    var hrefPage = webClient.DownloadString("https://modis.ru"+href);
                    var normalHrefCode = new HtmlDocument();
                    normalHrefCode.LoadHtml(hrefPage);
                    var srcCode =
                        normalHrefCode.DocumentNode.SelectSingleNode(
                            "//img[@class='header-card-img']");
                    var srcFind = srcCode.GetAttributeValue("src", "");
                    Console.WriteLine(srcFind);
                    
                    /*string fileName = $"image_{Guid.NewGuid()}.jpg";
                    webClient.DownloadFile("https:" + srcFind, //folder + "/" + fileName);
                    if (!String.IsNullOrEmpty(srcFind))
                    {
                        var doc = new BsonDocument
                        {
                            { "imageSrc", fileName },
                            { "color", color },
                            { "tone", tone },
                            { "name", name }
                        };

                        collection.InsertOne(doc);
                    }
                    Console.WriteLine($"Downlad file \"{fileName}\".");*/
                }
                
            }
        }
    }
}