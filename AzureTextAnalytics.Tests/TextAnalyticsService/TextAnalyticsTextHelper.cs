//  
namespace AzureTextAnalytics.Tests.TextAnalyticsService
{
    using System;
    using System.Net;
    using System.Net.Http;

    using AzureTextAnalytics.Domain;

    using DotNetTestHelper;

    using Moq;

    using Settings = AzureTextAnalytics.Settings;
    using TextAnalyticsService = AzureTextAnalytics.TextAnalyticsService;

    internal static class TextAnalyticsTestHelper
    {
        public static TextAnalyticsService BuildSut(HttpResponseMessage message)
        {
            var requestor = BuildMockRequestor(message);
            return BuildSut(requestor.Object);
        }

        public static TextAnalyticsService BuildSut(ITextAnalyticsRequestor requestor)
        {
            var sut = new SutBuilder<TextAnalyticsService>().AddDependency(requestor).AddDependency(new Settings()).Build();
            return sut;
        }

        public static Mock<ITextAnalyticsRequestor> BuildMockRequestor(HttpResponseMessage message)
        {
            var requestor = new Mock<ITextAnalyticsRequestor>();
            requestor.Setup(r => r.GetAsync(It.IsAny<string>())).ReturnsAsync(message);
            requestor.Setup(r => r.PostAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(message);
            return requestor;
        }

        public static Mock<ITextAnalyticsRequestor> BuildMockRequestorForGet(Action<string> callback, HttpResponseMessage message)
        {
            var requestor = new Mock<ITextAnalyticsRequestor>();
            requestor.Setup(r => r.GetAsync(It.IsAny<string>())).ReturnsAsync(message).Callback(callback);
            return requestor;
        }

        public static Mock<ITextAnalyticsRequestor> BuildMockRequestorForPost(Action<string, string> callback, HttpResponseMessage message)
        {
            var requestor = new Mock<ITextAnalyticsRequestor>();
            requestor.Setup(r => r.PostAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(message).Callback(callback);
            return requestor;
        }

        public static HttpResponseMessage GetErrorMessage(string error)
        {
            return new HttpResponseMessage(HttpStatusCode.BadRequest) { Content = new StringContent(error) };
        }
    }
}