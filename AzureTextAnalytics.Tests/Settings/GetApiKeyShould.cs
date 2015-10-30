namespace AzureTextAnalytics.Tests.Settings
{
    using System;
    using System.Configuration;

    using AssertExLib;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class GetApiKeyShould : SettingsTest
    {
        [TestMethod]
        public void ThrowIfApiKeyNotSetInConfiguration()
        {
            AssertEx.Throws<InvalidOperationException>(() => this.Sut.GetApiKey());
        }

        [TestMethod]
        public void PullKeyFromConfiguration()
        {
            const string Expected = "my account key is set here";
            ConfigurationManager.AppSettings[Constants.AccountConfigKey] = Expected;

            Assert.AreEqual(Expected, this.Sut.GetApiKey());
        }
    }
}