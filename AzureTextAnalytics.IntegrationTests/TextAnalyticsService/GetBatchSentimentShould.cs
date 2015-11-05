namespace AzureTextAnalytics.IntegrationTests.TextAnalyticsService
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;

    using AzureTextAnalytics.Domain;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class GetBatchSentimentShould
    {
        [TestMethod]
        public async Task GetResultFromAzure()
        {
            var input = new Dictionary<string, string>
                            {
                                { "1", "This is very positive text because I love this service" },
                                { "2", "Test is very bad because I hate this service" },
                                { "3", "The service was OK, nothing special, I've had better" },
                                { "4", "" }
                            };

            var expected = new Dictionary<string, SentimentResult>
                               {
                                   { "1", SentimentResult.Build(0.9742637M) },
                                   { "2", SentimentResult.Build(0.03089833M) },
                                   { "3", SentimentResult.Build(0.4267564M) },
                                   { "4", SentimentResult.Build(HttpStatusCode.BadRequest, "Record cannot be null/empty") }
                               };

            var sut = ServiceFactory.Build();
            var result = await sut.GetBatchSentimentAsync(input);
            CollectionAssert.AreEquivalent(expected, result.ToList());
        }
    }
}