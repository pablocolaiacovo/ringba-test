using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ringba
{
    class Program
    {
        private static HttpClient Client = new HttpClient();
        const string FILE_URL = "https://ringba-test-html.s3-us-west-1.amazonaws.com/TestQuestions/output.txt";
        static async Task Main(string[] args)
        {
            var text = await Client.GetStringAsync(FILE_URL);
            if (!string.IsNullOrWhiteSpace(text))
            {
                CountLetters(text);
                CountCapitalLetters(text);
                CountWords(text);
                CountPrefixes(text);
            }
        }

        static void CountLetters(string text)
        {
            WriteHeader("Counting Letters");
            var groups = text.ToUpperInvariant().GroupBy(t => t).OrderBy(t => t.Key).ToDictionary(t => t.Key, t => t.Count());
            foreach (var g in groups)
            {
                System.Console.WriteLine($"{g.Key}: {g.Value}");
            }
        }

        static void CountCapitalLetters(string text)
        {
            WriteHeader("Counting Capital Letters");
            System.Console.WriteLine($"{text.Count(c => char.IsUpper(c))} capital letters.");
        }

        static void CountWords(string text)
        {
            WriteHeader("Counting Words");
            var splittedText = Regex.Split(text, @"(?<!^)(?=[A-Z])");
            var groups = splittedText.GroupToDictionary().OrderByDescending(t => t.Value);
            System.Console.WriteLine($"The most common word is {groups.FirstOrDefault().Key} and it appears {groups.FirstOrDefault().Value}");
        }

        static void CountPrefixes(string text)
        {
            WriteHeader("Counting Prefixes");
            var splittedText = Regex.Split(text, @"(?<!^)(?=[A-Z])");
            var words = splittedText.Where(t => t.Length > 2).Select(t => t.Substring(0, 2)).ToArray();
            var groups = words.GroupToDictionary().OrderByDescending(t => t.Value);
            System.Console.WriteLine($"The most common prefix is {groups.FirstOrDefault().Key} and it appears {groups.FirstOrDefault().Value}");
        }

        static void WriteHeader(string header)
        {
            System.Console.WriteLine("-----------------------------");
            System.Console.WriteLine(header);
            System.Console.WriteLine("-----------------------------");
        }
    }

    public static class ArrayExtentions
    {
        public static Dictionary<string, int> GroupToDictionary(this string[] array)
        {
            return array.GroupBy(t => t).ToDictionary(t => t.Key, t => t.Count());
        }
    }
}
