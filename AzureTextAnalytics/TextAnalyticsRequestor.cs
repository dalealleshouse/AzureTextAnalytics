namespace AzureTextAnalytics
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;

    using AzureTextAnalytics.Domain;

    public class TextAnalyticsRequestor : ITextAnalyticsRequestor
    {
        private readonly IRequestHeaderFactory _headerFactory;

        private readonly ISettings _settings;

        public TextAnalyticsRequestor(IRequestHeaderFactory headerFactory, ISettings settings)
        {
            if (headerFactory == null)
            {
                throw new ArgumentNullException(nameof(headerFactory));
            }

            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            this._headerFactory = headerFactory;
            this._settings = settings;
        }

        public async Task<HttpResponseMessage> GetAsync(string request)
        {
            using (var client = this.BuildClient())
            {
                return await client.GetAsync(request);
            }
        }

        public async Task<HttpResponseMessage> PostAsync(string request, string body)
        {
            using (var client = this.BuildClient())
            {
                using (var content = new StringContent(body))
                {
                    return await client.PostAsync(request, content);
                }
            }
        }

        private HttpClient BuildClient()
        {
            var client = new HttpClient { BaseAddress = this._settings.GetServiceBaseUri() };
            this._headerFactory.SetHeaders(client.DefaultRequestHeaders);

            return client;
        }
    }
}