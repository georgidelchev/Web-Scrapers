using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AngleSharp.Html.Parser;

namespace BtvScraping
{
    public class Startup
    {
        public static async Task Main(string[] args)
        {
            var url = "https://btvnovinite.bg/novinite-ot-dnes/";

            var parser = new HtmlParser();
            var httpClient = new HttpClient();

            var html = await httpClient
                .GetStringAsync(url);

            var document = parser.ParseDocument(html);

            var urlElement = document
                .QuerySelector(".list .link");


            var articleUrl = "https://btvnovinite.bg" + urlElement
                .Attributes["href"]
                .Value;

            var article = await httpClient
                    .GetStringAsync(articleUrl);

            document = parser.ParseDocument(article);

            // ########## ARTICLE ORIGINAL URL ##########
            Console.WriteLine("##ORIGINAL URL");

            var originalUrl = articleUrl;

            Console.WriteLine(originalUrl + Environment.NewLine);

            // ########## ARTICLE TITLE ##########
            Console.WriteLine("##TITLE");

            var articleTitle = document
                .QuerySelector(".main-container .article-title");

            Console.WriteLine(articleTitle.InnerHtml + Environment.NewLine);

            // ########## ARTICLE SUMMARY ##########
            Console.WriteLine("##ARTICLE SUMMARY");

            var articleSummary = document
                .QuerySelector(".article-summary")
                .InnerHtml;

            Console.WriteLine(articleSummary + Environment.NewLine);

            // ########## ARTICLE PUBLISH DATE ##########
            Console.WriteLine("##PUBLISH DATE");

            var articlePublishDate = document
                .QuerySelector(".date-wrapper .published")
                .InnerHtml;

            Console.WriteLine(articlePublishDate + Environment.NewLine);

            // ########## ARTICLE BODY ##########
            Console.WriteLine("##BODY");

            var articleBody = document
                .QuerySelectorAll(".article-body p")
                .Select(b => b.InnerHtml.Trim())
                .ToList();

            foreach (var body in articleBody)
            {
                Console.WriteLine(body);
            }
            Console.WriteLine();

            // ########## ARTICLE IMAGES ##########
            Console.WriteLine("##ARTICLE IMAGES URLS");
            var articleImagesUrls = document
                .QuerySelectorAll(".article-media-wrapper .image img")
                .Select(i => i.Attributes["src"].Value.Trim())
                .ToList();

            // ########## ARTICLE IMAGE SOURCE ##########
            var articleImagesSources = document
                .QuerySelectorAll(".image-info .source")
                .Select(s => s.InnerHtml.Replace("\t", "").Replace("\n", "").Replace("Снимка:", ""))
                .ToList();

            var counter = 0;
            foreach (var imageUrl in articleImagesUrls)
            {
                Console.WriteLine(imageUrl);
                Console.WriteLine(articleImagesSources[counter]);
            }
            Console.WriteLine();

            // ########## ARTICLE TAGS ##########
            Console.WriteLine("##TAGS");

            var articleTags = document
                .QuerySelectorAll(".keywords-wrapper ul li a")
                .Select(t => t.InnerHtml.Trim())
                .ToList();

            foreach (var tag in articleTags)
            {
                Console.WriteLine(tag);
            }
        }
    }
}
