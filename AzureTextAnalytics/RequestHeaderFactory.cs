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

        // I would have prefered to return a list of headers instead of relying on a side effect,
        // but I was having problems getting the accept header to work properly. If anyone has any
        // refactoring ideas, I'm listening...
        public void SetHeaders(HttpRequestHeaders headers)
        {
            headers.Add(Constants.AuthorizationHeaderName, this.AuthorizationHeader());
            headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        private string AuthorizationHeader()
        {
            var cred = "AccountKey:" + this._settings.GetApiKey();
            return "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes(cred));
        }
    }
}