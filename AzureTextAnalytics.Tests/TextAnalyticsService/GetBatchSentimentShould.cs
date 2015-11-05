//  
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

    [TestClass]
    public class GetBatchSentimentShould
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

            AssertEx.TaskThrows<InvalidOperationException>(() => sut.GetBatchSentimentAsync(requests));
        }

        [TestMethod]
        public async Task ReturnEmptyDictionaryForEmptyInput()
        {
            var expected = new Dictionary<string, SentimentResult>();
            var sut = TextAnalyticsTestHelper.BuildSut(GetMessage());

            var result = await sut.GetBatchSentimentAsync(new Dictionary<string, string>());
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
            await sut.GetBatchSentimentAsync(this._input);

            Assert.AreEqual(Expected, request);
        }

        [TestMethod]
        public async Task DecodeResponse()
        {
            var expected = new Dictionary<string, SentimentResult>
                               {
                                   { "1", SentimentResult.Build(0.9549767M) },
                                   { "2", SentimentResult.Build(0.7767222M) },
                                   { "3", SentimentResult.Build(0.8988889M) },
                                   { "4", SentimentResult.Build(HttpStatusCode.BadRequest, "Record cannot be null/empty") }
                               };
            var sut = TextAnalyticsTestHelper.BuildSut(GetMessage());

            var result = await sut.GetBatchSentimentAsync(this._input);

            CollectionAssert.AreEquivalent(expected, result.ToList());
        }

        [TestMethod]
        public async Task ReturnAllFailureOnBadResult()
        {
            var expected = new Dictionary<string, SentimentResult>
                               {
                                   { "1", SentimentResult.Build(HttpStatusCode.BadRequest, Error) },
                                   { "2", SentimentResult.Build(HttpStatusCode.BadRequest, Error) },
                                   { "3", SentimentResult.Build(HttpStatusCode.BadRequest, Error) },
                                   { "4", SentimentResult.Build(HttpStatusCode.BadRequest, Error) }
                               };

            var sut = TextAnalyticsTestHelper.BuildSut(TextAnalyticsTestHelper.GetErrorMessage(Error));
            var result = await sut.GetBatchSentimentAsync(this._input);
            CollectionAssert.AreEqual(expected, result.ToList());
        }

        private static HttpResponseMessage GetMessage()
        {
            return new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(@"{
  ""odata.metadata"":""https://api.datamarket.azure.com/data.ashx/amla/text-analytics/v1/$metadata"", ""SentimentBatch"":
    [{""Score"":0.9549767,""Id"":""1""},
     {""Score"":0.7767222,""Id"":""2""},
     {""Score"":0.8988889,""Id"":""3""}
    ],  
    ""Errors"":[
       {""Id"": ""4"", Message:""Record cannot be null/empty""}
    ]
}") };
        }
    }
}