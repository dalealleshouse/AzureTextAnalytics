namespace AzureTextAnalytics.Tests.RequestHeaderFactory
{
    using System;
    using System.Text;

    using AzureTextAnalytics.Domain;

    using DotNetTestHelper;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    using RequestHeaderFactory = AzureTextAnalytics.RequestHeaderFactory;

    [TestClass]
    public class AuthorizationHeaderShould
    {
        private const string TestApiKey = "my api key";

        [TestMethod]
        public void CreateHeader()
        {
            const string Creds = "AccountKey:" + TestApiKey;
            var expected = $"Basic {Convert.ToBase64String(Encoding.ASCII.GetBytes(Creds))}";
            var sut = new SutBuilder<RequestHeaderFactory>().AddDependency(CreateSettings()).Build();

            Assert.AreEqual(expected, sut.AuthorizationHeader());
        }

        private static ISettings CreateSettings()
        {
            var settings = new Mock<ISettings>();
            settings.Setup(s => s.GetApiKey()).Returns(TestApiKey);
            return settings.Object;
        }
    }
}