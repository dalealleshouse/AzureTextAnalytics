namespace AzureTextAnalytics
{
    public static class Constants
    {
        public const string AccountConfigKey = "AccountKey";

        public const string ServiceBaseUriConfigKey = "ServiceBaseUri";

        public const string DefaultServiceBaseUri = "https://api.datamarket.azure.com/";

        public const string AuthorizationHeaderName = "Authorization";

        public const string SentimentNullInputErrorText = "Unable to extract a sentiment from null or empty text";

        public const string KeyPhraseNullInputErrorText = "Unable to extract key phrases from null or empty text";

        public const string ErrorWithOkResultErrorText = "Cannot construct a SentimentResult with an error and a status code of HttpStatusCode.OK";

        public const string ScoreOutOfRangeError = "Score must be a number between 0 and 1";

        public const string SentimentRequest = "data.ashx/amla/text-analytics/v1/GetSentiment?Text=";

        public const string KeyPhraseRequest = "data.ashx/amla/text-analytics/v1/GetKeyPhrases?Text=";
    }
}