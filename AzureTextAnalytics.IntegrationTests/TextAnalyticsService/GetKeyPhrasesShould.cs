﻿namespace AzureTextAnalytics.IntegrationTests.TextAnalyticsService
{
    using System.Threading.Tasks;

    using AzureTextAnalytics.Domain;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TextAnalyticsService = AzureTextAnalytics.TextAnalyticsService;

    [TestClass]
    public class GetKeyPhrasesShould
    {
        [TestMethod]
        public async Task GetResultFromAzure()
        {
            var expected = KeyPhraseResult.Build(new[] { "bunch of phrases", "wonderful hotel", "great service", "text" });
            const string Input = "I need some text that can extract a bunch of phrases from. This was a wonderful hotel with great service but really overpriced.";

            var settings = new Settings();
            var sut = new TextAnalyticsService(new TextAnalyticsRequestor(new RequestHeaderFactory(settings), settings));
            var result = await sut.GetKeyPhrases(Input);
            Assert.AreEqual(expected, result, string.Join(",", result.Phrases));
        }
    }
}