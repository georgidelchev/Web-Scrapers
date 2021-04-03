using System;
using System.Net;
using System.Text;
using System.Linq;
using System.Globalization;
using System.Threading.Tasks;
using System.Collections.Generic;

using HtmlAgilityPack;

namespace GotvachBgScraping
{
    public class StartUp
    {
        static readonly object LockObj = new object();
        private static int SuccessfullyReceivedRecipesCount = 0;

        public static void Main(string[] args)
        {
            HtmlWeb web = new HtmlWeb();

            HttpStatusCode statusCode = HttpStatusCode.OK;

            web.PostResponse += (request, response) =>
            {
                if (response != null)
                {
                    statusCode = response.StatusCode;
                }
            };

            Parallel.For(1, 200000 + 1, i =>
            {
                try
                {
                    GetRecipeDetails(web, i);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            });
        }

        private static void GetRecipeDetails(HtmlWeb web, int i)
        {
            var html = $"https://recepti.gotvach.bg/r-{i}";

            var htmlDoc = web.Load(html);

            // ########## CHECK STATUS CODE ##########
            if (web.StatusCode != HttpStatusCode.OK)
            {
                return;
            }

            // ########## RECIPE OWNER USERNAME ##########
            var username = GetRecipeOwnerUsername(htmlDoc);

            // ########## RECIPE OWNER AVATAR URL ##########
            var recipeOwnerAvatarUrl = GetRecipeOwnerAvatarUrl(htmlDoc);

            // ########## RECIPE OWNER PROFILE URL ##########
            var recipeOwnerProfileUrl = GetRecipeOwnerProfileUrl(htmlDoc);

            // ########## RECIPE ID ##########
            var recipeRealId = html.Split("r-", 2)[1];

            // ########## RECIPE ORIGINAL LINK ##########
            var originalUrl = html;

            // ########## RECIPE NAME ##########
            var recipeName = GetRecipeName(htmlDoc);

            // ########## RECIPE CATEGORY ##########
            var recipeCategory = GetRecipeCategory(htmlDoc);

            // ########## RECIPE DATE ##########
            var publishedOn = GetRecipeOriginalPublishDate(htmlDoc);

            // ########## RECIPE PICTURES ##########
            var recipePictures = GetRecipeAllPictures(web, htmlDoc);

            // ########## PREPARATION AND COOKING TIME ##########
            TimeSpan preparationTime = TimeSpan.MinValue;
            TimeSpan cookingTime = TimeSpan.MinValue;

            GetRecipeTimes(htmlDoc, ref preparationTime, ref cookingTime);

            // ########## PORTIONS COUNT ##########
            var portions = GetPortionsCount(htmlDoc);

            // ########## INGREDIENTS ##########
            var ingredients = GetRecipeIngredients(htmlDoc);

            // ########## RECIPE DESCRIPTION ##########
            var description = GetRecipeDescription(htmlDoc);

            // ########## SUCCESSFULLY TAKEN RECIPES ##########
            lock (LockObj)
            {
                SuccessfullyReceivedRecipesCount++;
                Console.WriteLine(SuccessfullyReceivedRecipesCount);
            }
        }

        private static string GetRecipeOwnerUsername(HtmlDocument htmlDoc)
        {
            var ownerUsernameParse = htmlDoc
                            .DocumentNode
                            .SelectNodes(@"//div[@class='autbox']/a");

            var ownerUsername = ownerUsernameParse
                .Select(op => op.InnerText)
                .FirstOrDefault();

            return ownerUsername;
        }

        private static string GetRecipeOwnerAvatarUrl(HtmlDocument htmlDoc)
        {
            var ownerProfilePictureParse = htmlDoc
                .DocumentNode
                .SelectNodes(@"//div[@class='autbox']/img");

            var ownerProfilePictureUrl = ownerProfilePictureParse
                .Select(op => op.GetAttributeValue("src", "unknown"))
                .FirstOrDefault();

            return ownerProfilePictureUrl;
        }

        private static string GetRecipeOwnerProfileUrl(HtmlDocument htmlDoc)
        {
            var ownerProfileUrlParse = htmlDoc
                .DocumentNode
                .SelectNodes(@"//div[@class='autbox']/a");

            var ownerProfileUrl = ownerProfileUrlParse
                .Select(op => op.GetAttributeValue("href", "unknown"))
                .FirstOrDefault();

            return ownerProfileUrl;
        }

        private static string GetRecipeName(HtmlDocument htmlDoc)
        {
            var recipeParse = htmlDoc
                .DocumentNode
                .SelectNodes(@"//div[@class='combocolumn mr']/h1");

            if (recipeParse != null)
            {
                return recipeParse
                    .Select(r => r.InnerText)
                    .FirstOrDefault();
            }

            return "-";
        }

        private static string GetRecipeCategory(HtmlDocument htmlDoc)
        {
            var recipeCategoryParse = htmlDoc
                .DocumentNode
                .SelectNodes(@"//div[@class='breadcrumb']");

            if (recipeCategoryParse != null)
            {
                return recipeCategoryParse
                    .Select(c => c.InnerText)
                    .FirstOrDefault()
                    ?.Split(" »")
                    .Reverse()
                    .ToList()[1];
            }

            return string.Empty;
        }

        private static string GetRecipeOriginalPublishDate(HtmlDocument htmlDoc)
        {
            var recipeDateParse = htmlDoc
                .DocumentNode
                .SelectNodes(@"//span[@class='date']");

            if (recipeDateParse != null)
            {
                return recipeDateParse
                    .Select(r => r.InnerText)
                    .FirstOrDefault();
            }

            return DateTime.Now.ToString("d", CultureInfo.InvariantCulture);
        }

        private static List<string> GetRecipeAllPictures(HtmlWeb web, HtmlDocument htmlDoc)
        {
            var pictures = new List<string>();

            var allPicturesUrlParse = htmlDoc
                .DocumentNode
                .SelectNodes(@"//a[@class='morebtn']");

            if (allPicturesUrlParse != null)
            {
                var allPicturesUrlParsed = allPicturesUrlParse
                    .FirstOrDefault()
                    ?.GetAttributeValue("href", "unknown");

                var link = web.Load(allPicturesUrlParsed);

                var allPicturesUrlsParse = link
                    .DocumentNode
                    .SelectNodes(@"//div[@class='main']/div/img");

                if (allPicturesUrlsParse != null)
                {
                    var allPicturesUrl = allPicturesUrlsParse.ToList();

                    if (allPicturesUrl[0].GetAttributeValue("src", "unknown") == "https://recepti.gotvach.bg/files/recipes/photos/")
                    {
                        allPicturesUrl.Clear();
                    }
                    else
                    {
                        pictures.AddRange(allPicturesUrl.Select(picture => picture.GetAttributeValue("src", "unknown")));
                    }
                }
            }

            return pictures;
        }

        private static void GetRecipeTimes(HtmlDocument htmlDoc, ref TimeSpan preparationTime, ref TimeSpan cookingTime)
        {
            var timesParse = htmlDoc
                .DocumentNode
                .SelectNodes(@"//div[@class='feat small']");

            if (timesParse == null)
            {
                return;
            }

            if (timesParse.Count == 2)
            {
                preparationTime = TimeSpan.FromMinutes(int.Parse(ParseTime(timesParse, 0, "Приготвяне")));

                cookingTime = TimeSpan.FromMinutes(int.Parse(ParseTime(timesParse, 1, "Готвене")));
            }
            else if (timesParse.Count == 1)
            {
                if (timesParse[0].InnerText.Contains("Приготвяне"))
                {
                    preparationTime = TimeSpan.FromMinutes(int.Parse(ParseTime(timesParse, 0, "Приготвяне")));
                }
                else
                {
                    cookingTime = TimeSpan.FromMinutes(int.Parse(ParseTime(timesParse, 0, "Готвене")));
                }
            }
        }

        private static int GetPortionsCount(HtmlDocument htmlDoc)
        {
            var portionsParse = htmlDoc
                .DocumentNode
                .SelectNodes(@"//div[@class='feat']")
                .LastOrDefault()
                ?.InnerText
                .Split(new[] { "-", "Порции ", " " }, 2, StringSplitOptions.RemoveEmptyEntries)
                .ToList();

            if (portionsParse != null &&
                portionsParse[0].Contains("Порции"))
            {
                return int.Parse(portionsParse[0].Replace("Порции", string.Empty)
                    .Replace("бр", string.Empty)
                    .Replace("бр.", string.Empty)
                    .Replace("броя", string.Empty)
                    .Replace("бройки", string.Empty));
            }

            return 0;
        }

        private static List<string> GetRecipeIngredients(HtmlDocument htmlDoc)
        {
            var ingredients = new List<string>();

            var ingredientsParse = htmlDoc
                .DocumentNode
                .SelectNodes(@"//section[@class='products new']/ul/li");

            if (ingredientsParse != null)
            {
                ingredients.AddRange(ingredientsParse
                    .Select(li => li.InnerText)
                    .ToList());
            }

            return ingredients;
        }

        private static string GetRecipeDescription(HtmlDocument htmlDoc)
        {
            var fullInstructions = new StringBuilder();

            var instructionsParse = htmlDoc
                            .DocumentNode
                            .SelectNodes(@"//p[@class='desc']");

            if (instructionsParse != null)
            {
                var description = instructionsParse
                    .Select(d => d.InnerText)
                    .ToList();


                foreach (var desc in description)
                {
                    fullInstructions.AppendLine(desc);
                }
            }

            return fullInstructions.ToString().Trim();
        }

        // ########## HELPING METHOD ##########
        private static string ParseTime(HtmlNodeCollection timesParse, int index, string timeType)
        {
            var time = timesParse[index]
                .InnerText
                .Replace(timeType, string.Empty)
                .Replace(" мин.", string.Empty);

            return time;
        }
    }
}
