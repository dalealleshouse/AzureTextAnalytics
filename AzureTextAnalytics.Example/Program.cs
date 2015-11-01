namespace AzureTextAnalytics.Example
{
    using System;

    internal class Program
    {
        private static void Main(string[] args)
        {
            var service = ServiceFactory.Build();

            var sentiment = service.GetSentimentAsync("This is some awesome text that needs sentiment analysis;").Result;
            Console.WriteLine(
                sentiment.Success
                    ? $"The sentiment score is {sentiment.Score}"
                    : $"Error: Http Status: {sentiment.StatusCode}, Contents: {sentiment.Error}");

            var phrases = service.GetKeyPhrasesAsync("This is some awesome text that needs the key phrases extracted from.").Result;
            Console.WriteLine(
                sentiment.Success
                    ? $"The key phrases are: {string.Join(",", phrases.Phrases)}"
                    : $"Error: Http Status: {phrases.StatusCode}, Contents: {phrases.Error}");
        }
    }
}