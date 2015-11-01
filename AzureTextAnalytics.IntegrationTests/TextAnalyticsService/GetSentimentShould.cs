namespace AzureTextAnalytics.IntegrationTests.TextAnalyticsService
{
    using System.Threading.Tasks;

    using AzureTextAnalytics.Domain;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class GetSentimentShould
    {
        [TestMethod]
        public async Task GetResultFromAzure()
        {
            var expected = SentimentResult.Build(0.9742637M);
            const string Input = "This is very positive text because I love this service";

            var sut = ServiceFactory.Build();
            var result = await sut.GetSentimentAsync(Input);
            Assert.AreEqual(expected, result);
        }
    }
}