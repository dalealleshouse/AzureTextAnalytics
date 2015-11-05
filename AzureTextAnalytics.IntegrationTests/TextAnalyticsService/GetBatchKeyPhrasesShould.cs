namespace AzureTextAnalytics.IntegrationTests.TextAnalyticsService
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AzureTextAnalytics.Domain;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class GetBatchKeyPhrasesShould
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

            var expected = new Dictionary<string, KeyPhraseResult>
                               {
                                   { "1", KeyPhraseResult.Build(new[] {"service", "positive text"}) },
                                   { "2", KeyPhraseResult.Build(new[] {"Test", "service"}) },
                                   { "3", KeyPhraseResult.Build(new[] {"service"}) },
                                   { "4", KeyPhraseResult.Build("Record cannot be null/empty") }
                               };

            var sut = ServiceFactory.Build();
            var result = await sut.GetBatchKeyPhrasesAsync(input);
            CollectionAssert.AreEquivalent(expected, result.ToList());
        }
    }
}