namespace AzureTextAnalytics.Example
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    internal class Program
    {
        private static void Main(string[] args)
        {
            Sentiment().Wait();
            KeyPhrases().Wait();
            SentimentBatch().Wait();
        }

        private static async Task Sentiment()
        {
            var service = ServiceFactory.Build();

            var sentiment = await service.GetSentimentAsync("This is some awesome text that needs sentiment analysis;");
            Console.WriteLine(
                sentiment.Success ? $"The sentiment score is {sentiment.Score}" : $"Error: Http Status: {sentiment.StatusCode}, Contents: {sentiment.Error}");
        }

        private static async Task SentimentBatch()
        {
            var service = ServiceFactory.Build();

            var request = new Dictionary<string, string>
                              {
                                  { "1", "This is very positive text because I love this service" },
                                  { "2", "Test is very bad because I hate this service" },
                                  { "3", "The service was OK, nothing special, I've had better" },
                                  { "4", "" }
                              };

            var batchResult = await service.GetBatchSentimentAsync(request);

            foreach (var result in batchResult)
            {
                Console.WriteLine(result.Value.Success ? $"Id: {result.Key} = {result.Value.Score}" : $"Error: Id: {result.Key}, Contents: {result.Value.Error}");
            }
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