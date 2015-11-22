namespace AzureTextAnalytics.Tests.RequestHeaderFactory
{
    using System;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;

    using AzureTextAnalytics.Domain;

    using DotNetTestHelper;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    using RequestHeaderFactory = AzureTextAnalytics.RequestHeaderFactory;

    [TestClass]
    public class GetHeadersShould
    {
        private const string TestApiKey = "my api key";

        [TestMethod]
        public void CreateAcceptHeader()
        {
            var expected = new MediaTypeWithQualityHeaderValue("application/json");
            var sut = new SutBuilder<RequestHeaderFactory>().Build();

            using (var client = new HttpClient())
            {
                sut.SetHeaders(client.DefaultRequestHeaders);

                Assert.AreEqual(client.DefaultRequestHeaders.Accept.First(), expected);
            }
        }

        [TestMethod]
        public void CreateAuthorizationHeader()
        {
            const string Creds = "AccountKey:" + TestApiKey;
            var expected = $"Basic {Convert.ToBase64String(Encoding.ASCII.GetBytes(Creds))}";

            var sut = new SutBuilder<RequestHeaderFactory>().AddDependency(CreateSettings()).Build();

            using (var client = new HttpClient())
            {
                sut.SetHeaders(client.DefaultRequestHeaders);

                var authHeader = client.DefaultRequestHeaders.FirstOrDefault(h => h.Key == Constants.AuthorizationHeaderName);
                Assert.IsNotNull(authHeader);
                Assert.AreEqual(expected, authHeader.Value.First());
            }
        }

        private static ISettings CreateSettings()
        {
            var settings = new Mock<ISettings>();
            settings.Setup(s => s.GetApiKey()).Returns(TestApiKey);
            return settings.Object;
        }
    }
}