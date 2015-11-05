//  
namespace AzureTextAnalytics
{
    using AzureTextAnalytics.Domain;

    public static class ServiceFactory
    {
        public static ITextAnalyticsService Build()
        {
            var settings = new Settings();
            return new TextAnalyticsService(new TextAnalyticsRequestor(new RequestHeaderFactory(settings), settings), new ErrorMessageGenerator(), settings);
        }
    }
}