namespace AzureTextAnalytics.Domain
{
    using System.Net.Http.Headers;

    public interface IRequestHeaderFactory
    {
        void SetHeaders(HttpRequestHeaders headers);
    }
}