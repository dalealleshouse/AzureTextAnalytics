namespace AzureTextAnalytics.Domain
{
    using System.Net.Http;
    using System.Threading.Tasks;

    public interface ITextAnalyticsRequestor
    {
        Task<HttpResponseMessage> GetAsync(string request);
    }
}