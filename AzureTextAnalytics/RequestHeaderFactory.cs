namespace AzureTextAnalytics
{
    using System;
    using System.Net.Http.Headers;
    using System.Text;

    using AzureTextAnalytics.Domain;

    public class RequestHeaderFactory : IRequestHeaderFactory
    {
        private readonly ISettings _settings;

        public RequestHeaderFactory(ISettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            this._settings = settings;
        }

        public string AuthorizationHeader()
        {
            var cred = "AccountKey:" + this._settings.GetApiKey();
            return "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes(cred));
        }

        public MediaTypeWithQualityHeaderValue AcceptHeader()
        {
            return new MediaTypeWithQualityHeaderValue("application/json");
        }
    }
}