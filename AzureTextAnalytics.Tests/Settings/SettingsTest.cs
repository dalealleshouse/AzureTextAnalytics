namespace AzureTextAnalytics.Tests.Settings
{
    using DotNetTestHelper;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Settings = AzureTextAnalytics.Settings;

    [TestClass]

    public abstract class SettingsTest
    {
        protected Settings Sut;

        [TestInitialize]
        public void Init()
        {
            this.Sut = new SutBuilder<Settings>().Build();
        }
    }
}