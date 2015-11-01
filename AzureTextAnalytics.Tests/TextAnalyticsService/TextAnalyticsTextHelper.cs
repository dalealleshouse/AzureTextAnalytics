//  
namespace AzureTextAnalytics.Tests.TextAnalyticsService
{
    using System;
    using System.Net;
    using System.Net.Http;

    using AzureTextAnalytics.Domain;

    using DotNetTestHelper;

    using Moq;

    using TextAnalyticsService = AzureTextAnalytics.TextAnalyticsService;

    internal static class TextAnalyticsTestHelper
    {
        public static TextAnalyticsService BuildSut(ITextAnalyticsRequestor requestor)
        {
            var sut = new SutBuilder<TextAnalyticsService>().AddDependency(requestor).Build();
            return sut;
        }

        public static Mock<ITextAnalyticsRequestor> BuildMockRequestor(Action<string> callback, HttpResponseMessage message)
        {
            var requestor = new Mock<ITextAnalyticsRequestor>();
            requestor.Setup(r => r.GetAsync(It.IsAny<string>())).ReturnsAsync(message).Callback(callback);
            return requestor;
        }

        public static HttpResponseMessage GetErrorMessage(string error)
        {
            return new HttpResponseMessage(HttpStatusCode.BadRequest) { Content = new StringContent(error) };
        }
    }
}