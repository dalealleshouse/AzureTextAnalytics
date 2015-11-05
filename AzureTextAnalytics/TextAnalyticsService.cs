namespace AzureTextAnalytics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web;

    using AzureTextAnalytics.Domain;
    using AzureTextAnalytics.Domain.Azure;

    using Newtonsoft.Json;

    public class TextAnalyticsService : ITextAnalyticsService
    {
        private readonly ITextAnalyticsRequestor _requestor;

        private readonly ISettings _settings;

        public TextAnalyticsService(ITextAnalyticsRequestor requestor, ISettings settings)
        {
            if (requestor == null)
            {
                throw new ArgumentNullException(nameof(requestor));
            }

            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            this._requestor = requestor;
            this._settings = settings;
        }

        public async Task<SentimentResult> GetSentimentAsync(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return SentimentResult.Build(HttpStatusCode.BadRequest, Constants.SentimentNullInputErrorText);
            }

            var request = $"{Constants.SentimentRequest}{HttpUtility.UrlEncode(text)}";
            string content;
            using (var response = await this._requestor.GetAsync(request))
            {
                content = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    return SentimentResult.Build(response.StatusCode, content);
                }
            }

            var result = JsonConvert.DeserializeObject<AzureSentimentResult>(content);
            return SentimentResult.Build(result.Score);
        }

        public async Task<IDictionary<string, SentimentResult>> GetBatchSentimentAsync(Dictionary<string, string> textBatch)
        {
            if (textBatch == null)
            {
                throw new ArgumentNullException(nameof(textBatch));
            }

            if (textBatch.Count > this._settings.GetBatchLimit())
            {
                throw new InvalidOperationException(Constants.BatchLimitExceededErrorText);
            }

            if (!textBatch.Any())
            {
                return new Dictionary<string, SentimentResult>();
            }

            string content;
            using (var response = await this._requestor.PostAsync(Constants.SentimentBatchRequest, BuildInputString(textBatch)))
            {
                content = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    var status = response.StatusCode;
                    return textBatch.ToDictionary(r => r.Key, r => SentimentResult.Build(status, content));
                }
            }

            var result = JsonConvert.DeserializeObject<AzureSentimentBatchResult>(content);

            var parsedResults = result.SentimentBatch.ToDictionary(sr => sr.Id, sr => SentimentResult.Build(sr.Score));

            foreach (var error in result.Errors)
            {
                parsedResults.Add(error.Id, SentimentResult.Build(HttpStatusCode.BadRequest, error.Message));
            }

            return parsedResults;
        }

        public async Task<KeyPhraseResult> GetKeyPhrasesAsync(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return KeyPhraseResult.Build(HttpStatusCode.BadRequest, Constants.KeyPhraseNullInputErrorText);
            }

            var request = $"{Constants.KeyPhraseRequest}{HttpUtility.UrlEncode(text)}";

            string content;
            using (var response = await this._requestor.GetAsync(request))
            {
                content = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return KeyPhraseResult.Build(response.StatusCode, content);
                }
            }

            var result = JsonConvert.DeserializeObject<AzureKeyPhraseResult>(content);
            return KeyPhraseResult.Build(result.KeyPhrases);
        }

        private static string BuildInputString(IDictionary<string, string> textBatch)
        {
            var i = new StringBuilder();
            i.Append("{\"Inputs\":[");

            foreach (var req in textBatch)
            {
                i.AppendFormat("{{\"Id\":\"{0}\",\"Text\":\"{1}\"}},", req.Key, req.Value);
            }

            i.Append("]}");

            return i.ToString();
        }
    }
}