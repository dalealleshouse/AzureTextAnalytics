namespace AzureTextAnalytics
{
    public static class Constants
    {
        public const string AccountConfigKey = "AccountKey";

        public const string ServiceBaseUriConfigKey = "ServiceBaseUri";

        public const string DefaultServiceBaseUri = "https://api.datamarket.azure.com/";

        public const string AuthorizationHeaderName = "Authorization";

        public const string NullInputErrorText = "Unable to extract a sentiment from null or empty text";

        public const string SentimentRequest = "data.ashx/amla/text-analytics/v1/GetSentiment?Text=";
    }
}