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
            KeyPhrasesBatch().Wait();
        }

        private static async Task Sentiment()
        {
            var service = ServiceFactory.Build();

            var sentiment = await service.GetSentimentAsync("This is some awesome text that needs sentiment analysis;");
            Console.WriteLine(sentiment.Success ? $"The sentiment score is {sentiment.Score}" : sentiment.Error);
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
                Console.WriteLine(result.Value.Success ? $"Id: {result.Key} = {result.Value.Score}" : $"Id: {result.Key} = Error: {result.Value.Error}");
            }
        }

        private static async Task KeyPhrases()
        {
            var service = ServiceFactory.Build();

            var phrases = await service.GetKeyPhrasesAsync("This is some awesome text that needs the key phrases extracted from.");
            Console.WriteLine(phrases.Success ? $"The key phrases are: {string.Join(",", phrases.Phrases)}" : phrases.Error);
        }

        private static async Task KeyPhrasesBatch()
        {
            var service = ServiceFactory.Build();

            var request = new Dictionary<string, string>
                              {
                                  { "1", "This is very positive text because I love this service" },
                                  { "2", "Test is very bad because I hate this service" },
                                  { "3", "The service was OK, nothing special, I've had better" },
                                  { "4", "" }
                              };

            var batchResult = await service.GetBatchKeyPhrasesAsync(request);

            foreach (var result in batchResult)
            {
                Console.WriteLine(
                    result.Value.Success ? $"Id: {result.Key} = {string.Join(",", result.Value.Phrases)}" : $"Id: {result.Key} = Error: {result.Value.Error}");
            }
        }
    }
}