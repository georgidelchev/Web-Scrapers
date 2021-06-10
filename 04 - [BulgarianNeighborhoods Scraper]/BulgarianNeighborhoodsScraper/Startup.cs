using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AngleSharp.Html.Parser;

namespace BulgarianNeighborhoodsScraper
{
    public class Startup
    {
        public static async Task Main(string[] args)
        {
            var dict = new Dictionary<string, string>();

            var getCitiesUrl = "https://bazar.bg/obiavi/prodazhba-imoti";

            var parser = new HtmlParser();
            var httpClient = new HttpClient();

            var html = await httpClient.GetStringAsync(getCitiesUrl);

            var document = parser.ParseDocument(html);

            var cities = document
                .QuerySelectorAll("#autocompleteLocations a")
                .Select(cu => cu.InnerHtml)
                .ToList();

            var index = 0;

            foreach (var city in cities)
            {
                if (index++ % 2 == 0) {continue;}

                var editedCity = city
                    .Trim(new[] { 'n', 'b', 's', 'p', ';', 'г', 'р', '.', ' ', '&' });

                Console.WriteLine(editedCity);

                if (!dict.ContainsKey(editedCity))
                {
                    dict[editedCity] = ConvertCyrillicToLatinLetters(editedCity);
                }
            }

            foreach (var neighborhoodUrl in dict
                .Select(kvp => getCitiesUrl + $"/{kvp.Value}"))
            {
                Console.WriteLine(neighborhoodUrl);

                html = await httpClient.GetStringAsync(neighborhoodUrl);

                document = parser.ParseDocument(html);

                var cityNeighborhoods = document
                    .QuerySelectorAll(".wrapper .control .item-name")
                    .Select(cn => cn.InnerHtml);

                foreach (var neighborhood in cityNeighborhoods)
                {
                    Console.WriteLine(neighborhood);
                }
            }
        }

        private static string ConvertCyrillicToLatinLetters(string input)
        {
            switch (input)
            {
                case "Смолян":
                    return "smolian";
                case "Ямбол":
                    return "iambol";
            }

            input = input.ToLower();

            var bulgarianLetters = new[]
            {
                "а", "б", "в", "г", "д", "е", "ж", "з", "и", "й", "к", "л", "м", "н", "о", "п",
                "р", "с", "т", "у", "ф", "х", "ц", "ч", "ш", "щ", "ъ", "ь", "ю", "я",
            };

            var latinRepresentationsOfBulgarianLetters = new[]
            {
                "a", "b", "v", "g", "d", "e", "zh", "z", "i", "y", "k",
                "l", "m", "n", "o", "p", "r", "s", "t", "u", "f", "h",
                "ts", "ch", "sh", "sht", "a", "i", "yu", "a",
            };

            for (var i = 0; i < bulgarianLetters.Length; i++)
            {
                input = input.Replace(bulgarianLetters[i], latinRepresentationsOfBulgarianLetters[i]);
            }

            return input.Replace(' ', '-');
        }
    }
}
