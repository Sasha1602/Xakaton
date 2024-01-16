using System;
using System.Net;
using HtmlAgilityPack;
using MongoDB.Driver;
using MongoDB.Bson;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Text;
using DAL;
using Domain;

namespace MyParser
{
    public class ParserTest
    {
        public static int maxPages = 1;

        public static string[] links =
            {
            "https://www.lamoda.ru/c/2474/clothes-tolstovki-olimpiyki/",
            "https://www.lamoda.ru/c/2512/clothes-muzhskie-futbolki/", 
            /*"https://www.lamoda.ru/c/1590/clothes-dlia-devochek/",
            "https://www.lamoda.ru/c/5378/default-malchikam/"*/
        };

        static string ExtractValueFromJS(string html, string key, string regexPattern)
        {
            var regex = new Regex(regexPattern);
            var match = regex.Match(html);

            if (match.Success)
            {
                var value = match.Groups[1].Value;
                return value;
            }

            Console.WriteLine("Значение для ключа '" + key + "' не найдено.");
            return null;
        }

        public static void Parse()
        {
            using MyDbContext dbContext = new MyDbContext();

            for (int i = 0; i < links.Length; i++)
            {
                var link = links[i];

                // бегаем по страницам
                for (int page = 1; page < maxPages; page++)
                {
                    var workLink = link + "?page=" + page;
                    var folder = "myphotodb" + i;

                    // создание клиента для парсинга
                    WebClient client = new WebClient();
                    var html = client.DownloadString(workLink);

                    var htmlDocument = new HtmlDocument();
                    htmlDocument.LoadHtml(html);

                    // выбираем по селектору class все классы, хранящие изображение на данной странице
                    var productNodes = htmlDocument.DocumentNode.SelectNodes("//div[@class='x-product-card__card']/a");

                    // Перебор всех полученных классов и создание коллекции ссылок на сами картинки
                    foreach (var productNode in productNodes)
                    {
                        var imageNode = productNode.SelectSingleNode("//img[@class='x-product-card__pic-img']");
                        var imageSrc = imageNode.GetAttributeValue("src", null);
                        var slink = productNode.GetAttributeValue("href", null);
                        var productHtml = client.DownloadString("https://lamoda.ru" + slink);
                        Encoding utf8 = Encoding.GetEncoding("UTF-8");
                        Encoding win1251 = Encoding.GetEncoding("Windows-1251");
                        byte[] utf8Bytes = win1251.GetBytes(productHtml);
                        byte[] win1251Bytes = Encoding.Convert(utf8, win1251, utf8Bytes);
                        productHtml = win1251.GetString(win1251Bytes);

                        var color = ExtractValueFromJS(productHtml, "color", @"""key"":""color_family"",""title"":""[^""]+"",""value"":""([^""]+)""");
                        var tone = ExtractValueFromJS(productHtml, "tone", @"""key"":""print"",""title"":""[^""]+"",""value"":""([^""]+)""");
                        var name = ExtractValueFromJS(productHtml, "name", @"""@type"": ""Product""[^]]+""name"": ""([^""]+)""").Replace("&quot;", "");

                        string fileName = $"image_{Guid.NewGuid()}.jpg";
                        if (!Directory.Exists(folder))
                            Directory.CreateDirectory(folder);

                        client.DownloadFile("https:" + imageSrc, folder + "/" + fileName);
                        if (!String.IsNullOrEmpty(imageSrc))
                        {
                            var image = new ImageEntity()
                            {
                                ClotheType = name,
                                Color = color,
                                Tone = tone,
                                ImagePath = folder + "/" + fileName,
                            };

                           dbContext.Images.Add(image);
                           dbContext.SaveChanges();
                        }
                        Console.WriteLine($"Downlad file \"{fileName}\".");
                    }
                }
            }

            Console.WriteLine("Parsing end.");
        }

    }
}