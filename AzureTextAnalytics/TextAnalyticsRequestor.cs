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
            using (var client = new HttpClient())
            {
                client.BaseAddress = this._settings.GetServiceBaseUri();
                client.DefaultRequestHeaders.Add(Constants.AuthorizationHeaderName, this._headerFactory.AuthorizationHeader());
                client.DefaultRequestHeaders.Accept.Add(this._headerFactory.AcceptHeader());

                return await client.GetAsync(request);
            }
        }
    }
}