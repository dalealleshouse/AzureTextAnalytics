namespace AzureTextAnalytics.Tests.Settings
{
    using System;
    using System.Configuration;

    using AssertExLib;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class GetServiceBaseUriShould : SettingsTest
    {
        [TestMethod]
        public void ReturnDefaultWhenNotSet()
        {
            ConfigurationManager.AppSettings[Constants.ServiceBaseUriConfigKey] = null;
            var result = this.Sut.GetServiceBaseUri();
            Assert.AreEqual(new Uri(Constants.DefaultServiceBaseUri), result);
        }

        [TestMethod]
        public void ThrowUriFormatExceptionWithCustomMessageWhenInvalidInConfiguration()
        {
            ConfigurationManager.AppSettings[Constants.ServiceBaseUriConfigKey] = "foo";

            AssertEx.Throws<UriFormatException>(() => this.Sut.GetServiceBaseUri());
        }

        [TestMethod]
        public void ReturnUriFromConfiguration()
        {
            var expected = new Uri(@"http:\\www.foo.com");
            ConfigurationManager.AppSettings[Constants.ServiceBaseUriConfigKey] = expected.ToString();

            var result = this.Sut.GetServiceBaseUri();
            Assert.AreEqual(expected, result);
        }
    }
}