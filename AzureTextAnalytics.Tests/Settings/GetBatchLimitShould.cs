namespace AzureTextAnalytics.Tests.Settings
{
    using System;
    using System.Configuration;

    using AssertExLib;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class GetBatchLimitShould : SettingsTest
    {
        [TestMethod]
        public void ReturnDefaultWhenNotSet()
        {
            ConfigurationManager.AppSettings[Constants.BatchLimitConfigKey] = null;
            var result = this.Sut.GetBatchLimit();
            Assert.AreEqual(Constants.DefaultBatchLimit, result);
        }

        [TestMethod]
        public void ThrowInvalidCastExceptionWithCustomMessageWhenInvalidInConfiguration()
        {
            ConfigurationManager.AppSettings[Constants.BatchLimitConfigKey] = "foo";

            AssertEx.Throws<InvalidCastException>(() => this.Sut.GetBatchLimit());
        }

        [TestMethod]
        public void ReturnValueFromConfiguration()
        {
            const int Expected = 500;
            ConfigurationManager.AppSettings[Constants.BatchLimitConfigKey] = 500.ToString();

            var result = this.Sut.GetBatchLimit();
            Assert.AreEqual(Expected, result);
        }
    }
}