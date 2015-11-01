namespace AzureTextAnalytics.Example
{
    using System;
    using System.Threading.Tasks;

    internal class Program
    {
        private static void Main(string[] args)
        {
            Sentiment().Wait();
            KeyPhrases().Wait();
        }

        private static async Task Sentiment()
        {
            var service = ServiceFactory.Build();

            var sentiment = await service.GetSentimentAsync("This is some awesome text that needs sentiment analysis;");
            Console.WriteLine(
                sentiment.Success
                    ? $"The sentiment score is {sentiment.Score}"
                    : $"Error: Http Status: {sentiment.StatusCode}, Contents: {sentiment.Error}");
        }

        private static async Task KeyPhrases()
        {
            var service = ServiceFactory.Build();

            var phrases = await service.GetKeyPhrasesAsync("This is some awesome text that needs the key phrases extracted from.");
            Console.WriteLine(
                phrases.Success
                    ? $"The key phrases are: {string.Join(",", phrases.Phrases)}"
                    : $"Error: Http Status: {phrases.StatusCode}, Contents: {phrases.Error}");
        }
    }
}