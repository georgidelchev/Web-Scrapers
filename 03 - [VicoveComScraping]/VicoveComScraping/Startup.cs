using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AngleSharp.Html.Parser;

namespace VicoveComScraping
{
    class Startup
    {
        [SuppressMessage("ReSharper.DPA", "DPA0002: Excessive memory allocations in SOH", MessageId = "type: System.Char[]")]
        static async Task Main(string[] args)
        {
            var successCount = 1;
            for (int i = 1; i <= 100000; i++)
            {
                var url = $"https://vicove.com/vic-{i}";

                try
                {
                    var parser = new HtmlParser();
                    var httpClient = new HttpClient();

                    var html = await httpClient
                        .GetStringAsync(url);

                    var document = parser.ParseDocument(html);

                    // Source Url
                    var source = url;
                    //Console.WriteLine(url);

                    // Category
                    var category = document
                        .QuerySelector(".col-12 h1 span")
                        .InnerHtml;
                    //Console.WriteLine(category);

                    // Created On
                    var createdOn = document
                        .QuerySelector(".jokes-category h2 time")
                        .InnerHtml;
                    //Console.WriteLine(createdOn);

                    // Content
                    var content = document
                        .QuerySelector(".joke_text")
                        .InnerHtml
                        .Replace("<br>", string.Empty)
                        .Replace(" - ", string.Empty)
                        .Trim();
                    //Console.WriteLine(content);

                    Console.WriteLine(successCount++);
                }
                catch
                {
                    // ignored
                }
            }

            successCount = 1;

            for (int i = 1; i <= 100000; i++)
            {
                var url = $"https://vicove.com/pic-{i}";

                try
                {
                    var parser = new HtmlParser();
                    var httpClient = new HttpClient();

                    var html = await httpClient
                        .GetStringAsync(url);

                    var document = parser.ParseDocument(html);

                    // Source Url
                    var source = url;
                    //Console.WriteLine(url);

                    // Category
                    var category = document
                        .QuerySelector(".col-12 h2 span")
                        .InnerHtml;
                    //Console.WriteLine(category);

                    // Title
                    var title = document
                        .QuerySelector(".picture h1")
                        .InnerHtml;
                    //Console.WriteLine(title);

                    // Created On
                    var createdOn = document
                        .QuerySelector(".picture h3 time")
                        .InnerHtml;
                    //Console.WriteLine(createdOn);

                    // PictureUrl
                    var pictureUrl = document
                        .QuerySelector(".center a")
                        .GetAttribute("href");
                    //Console.WriteLine("https://vicove.com"+pictureUrl);

                    Console.WriteLine(successCount++);
                }
                catch
                {
                    // ignored
                }
            }
        }
    }
}
