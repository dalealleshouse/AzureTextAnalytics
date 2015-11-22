namespace AzureTextAnalytics
{
    using System;
    using System.Configuration;

    using AzureTextAnalytics.Domain;

    public class Settings : ISettings
    {
        public string GetApiKey()
        {
            var key = ConfigurationManager.AppSettings[Constants.AccountConfigKey];

            if (key == null)
            {
                throw new InvalidOperationException(
                    $@"You must specify an account key ({Constants.AccountConfigKey}) app setting. "
                    + $"If you do not have a key, please obtain one from: https://datamarket.azure.com/dataset/amla/text-analytics");
            }

            return key;
        }

        public Uri GetServiceBaseUri()
        {
            var uri = ConfigurationManager.AppSettings[Constants.ServiceBaseUriConfigKey] ?? Constants.DefaultServiceBaseUri;

            try
            {
                return new Uri(uri);
            }
            catch (UriFormatException)
            {
                throw new UriFormatException($"Unable to parse the {Constants.ServiceBaseUriConfigKey} setting into a valid URI");
            }
        }

        public int GetBatchLimit()
        {
            var configValue = ConfigurationManager.AppSettings[Constants.BatchLimitConfigKey];

            if (configValue == null)
            {
                return Constants.DefaultBatchLimit;
            }

            int limit;
            if (!int.TryParse(configValue, out limit))
            {
                throw new InvalidCastException($"Invalid value for {Constants.BatchLimitConfigKey} configraution setting");
            }

            return limit;
        }
    }
}