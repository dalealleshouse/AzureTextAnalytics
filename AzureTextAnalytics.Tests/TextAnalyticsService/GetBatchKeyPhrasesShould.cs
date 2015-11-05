namespace AzureTextAnalytics.Tests.TextAnalyticsService
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;

    using AssertExLib;

    using AzureTextAnalytics.Domain;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using ErrorMessageGenerator = AzureTextAnalytics.ErrorMessageGenerator;

    [TestClass]
    public class GetBatchKeyPhrasesShould
    {
        private const string Error = "Real bad error happend";

        private readonly Dictionary<string, string> _input = new Dictionary<string, string>
                                                                 {
                                                                     { "1", "This is the first request" },
                                                                     { "2", "This is the second request" },
                                                                     { "3", "This is the third request" },
                                                                     { "4", "this should cause an error" }
                                                                 };

        [TestMethod]
        public void ThrowIfOverBatchLimit()
        {
            ConfigurationManager.AppSettings[Constants.BatchLimitConfigKey] = "10";
            var sut = TextAnalyticsTestHelper.BuildSut(GetMessage());

            var requests = new Dictionary<string, string>();
            for (var i = 0; i < 11; i++)
            {
                requests.Add(i.ToString(), "this is text");
            }

            AssertEx.TaskThrows<InvalidOperationException>(() => sut.GetBatchKeyPhrasesAsync(requests));
        }

        [TestMethod]
        public async Task ReturnEmptyDictionaryForEmptyInput()
        {
            var expected = new Dictionary<string, SentimentResult>();
            var sut = TextAnalyticsTestHelper.BuildSut(GetMessage());

            var result = await sut.GetBatchKeyPhrasesAsync(new Dictionary<string, string>());
            CollectionAssert.AreEqual(expected, result.ToList());
        }

        [TestMethod]
        public async Task ConvertDictionaryToPostBody()
        {
            const string Expected =
                @"{""Inputs"":[{""Id"":""1"",""Text"":""This is the first request""},{""Id"":""2"",""Text"":""This is the second request""},{""Id"":""3"",""Text"":""This is the third request""},{""Id"":""4"",""Text"":""this should cause an error""},]}";
            var request = "";

            var requestor = TextAnalyticsTestHelper.BuildMockRequestorForPost((req, body) => request = body, GetMessage());
            var sut = TextAnalyticsTestHelper.BuildSut(requestor.Object);
            await sut.GetBatchKeyPhrasesAsync(this._input);

            Assert.AreEqual(Expected, request);
        }

        [TestMethod]
        public async Task DecodeResponse()
        {
            var expected = new Dictionary<string, KeyPhraseResult>
                               {
                                   { "1", KeyPhraseResult.Build(new[] { "unique decor", "friendly staff", "wonderful hotel" }) },
                                   { "2", KeyPhraseResult.Build(new[] { "amazing build conference", "interesting talks" }) },
                                   { "3", KeyPhraseResult.Build(new[] { "hours", "traffic", "airport" }) },
                                   { "4", KeyPhraseResult.Build("Record cannot be null/empty") }
                               };
            var sut = TextAnalyticsTestHelper.BuildSut(GetMessage());

            var result = await sut.GetBatchKeyPhrasesAsync(this._input);

            CollectionAssert.AreEquivalent(expected, result.ToList());
        }

        [TestMethod]
        public async Task ReturnAllFailureOnBadResult()
        {
            var err = new ErrorMessageGenerator().GenerateError(HttpStatusCode.BadRequest, Error);
            var expected = new Dictionary<string, KeyPhraseResult>
                               {
                                   { "1", KeyPhraseResult.Build(err) },
                                   { "2", KeyPhraseResult.Build(err) },
                                   { "3", KeyPhraseResult.Build(err) },
                                   { "4", KeyPhraseResult.Build(err) }
                               };

            var sut = TextAnalyticsTestHelper.BuildSut(TextAnalyticsTestHelper.GetErrorMessage(Error));
            var result = await sut.GetBatchKeyPhrasesAsync(this._input);
            CollectionAssert.AreEqual(expected, result.ToList());
        }

        private static HttpResponseMessage GetMessage()
        {
            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content =
                               new StringContent(@"{ ""odata.metadata"":""https://api.datamarket.azure.com/data.ashx/amla/text-analytics/v1/$metadata"",
    ""KeyPhrasesBatch"":
    [
       {""KeyPhrases"":[""unique decor"",""friendly staff"",""wonderful hotel""],""Id"":""1""},
       {""KeyPhrases"":[""amazing build conference"",""interesting talks""],""Id"":""2""},
       {""KeyPhrases"":[""hours"",""traffic"",""airport""],""Id"":""3"" }
    ],
    ""Errors"":[
       {""Id"": ""4"", Message:""Record cannot be null/empty""}
    ]
}")
            };
        }
    }
}