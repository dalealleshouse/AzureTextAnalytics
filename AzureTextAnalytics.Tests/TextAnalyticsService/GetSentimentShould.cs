namespace AzureTextAnalytics.Tests.TextAnalyticsService
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web;

    using AzureTextAnalytics.Domain;

    using DotNetTestHelper;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    using TextAnalyticsService = AzureTextAnalytics.TextAnalyticsService;

    [TestClass]
    public class GetSentimentShould
    {
        private const string Input = "some crazy text +0- with stuff &*";

        private const string Error = "This is an error";

        [TestMethod]
        public async Task ReturnBadRequestIfEmptyInput()
        {
            var expected = SentimentResult.Build(HttpStatusCode.BadRequest, Constants.NullInputErrorText);
            var requestor = MockRequestor(s => { }, GetMessage());
            var sut = new SutBuilder<TextAnalyticsService>().AddDependency(requestor.Object).Build();

            var result = await sut.GetSentimentAsync(null);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public async Task UrlEncodeInput()
        {
            var expected = $"{Constants.SentimentRequest}{HttpUtility.UrlEncode(Input)}";
            var request = "";

            var requestor = MockRequestor(s => request = s, GetMessage());
            var sut = BuildSut(requestor.Object);
            await sut.GetSentimentAsync(Input);

            Assert.AreEqual(expected, request);
        }

        [TestMethod]
        public async Task DecodeResponse()
        {
            var expected = SentimentResult.Build(1M);
            var requestor = MockRequestor(s => { }, GetMessage());

            var sut = new SutBuilder<TextAnalyticsService>().AddDependency(requestor.Object).Build();
            var result = await sut.GetSentimentAsync(Input);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public async Task ReturnFailureOnBadResult()
        {
            var expected = SentimentResult.Build(HttpStatusCode.BadRequest, Error);
            var requestor = MockRequestor(s => { }, GetErrorMessage());

            var sut = new SutBuilder<TextAnalyticsService>().AddDependency(requestor.Object).Build();
            var result = await sut.GetSentimentAsync(Input);
            Assert.AreEqual(expected, result);
        }

        private static TextAnalyticsService BuildSut(ITextAnalyticsRequestor requestor)
        {
            var sut = new SutBuilder<TextAnalyticsService>().AddDependency(requestor).Build();
            return sut;
        }

        private static Mock<ITextAnalyticsRequestor> MockRequestor(Action<string> callback, HttpResponseMessage message)
        {
            var requestor = new Mock<ITextAnalyticsRequestor>();
            requestor.Setup(r => r.GetAsync(It.IsAny<string>())).ReturnsAsync(message).Callback(callback);
            return requestor;
        }

        public static HttpResponseMessage GetMessage()
        {
            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content =
                               new StringContent(
                               @"{""odata.metadata"":""https://api.datamarket.azure.com/data.ashx/amla/text-analytics/v1/$metadata"",""Score"":1.0}")
            };
        }

        public static HttpResponseMessage GetErrorMessage()
        {
            return new HttpResponseMessage(HttpStatusCode.BadRequest) { Content = new StringContent(Error) };
        }
    }
}