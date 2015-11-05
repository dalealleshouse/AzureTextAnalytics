namespace AzureTextAnalytics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web;

    using AzureTextAnalytics.Domain;
    using AzureTextAnalytics.Domain.Azure;

    using Newtonsoft.Json;

    public class TextAnalyticsService : ITextAnalyticsService
    {
        private readonly ITextAnalyticsRequestor _requestor;

        private readonly IErrorMessageGenerator _errorMessageGenerator;

        private readonly ISettings _settings;

        public TextAnalyticsService(ITextAnalyticsRequestor requestor, IErrorMessageGenerator errorMessageGenerator, ISettings settings)
        {
            if (requestor == null)
            {
                throw new ArgumentNullException(nameof(requestor));
            }

            if (errorMessageGenerator == null)
            {
                throw new ArgumentNullException(nameof(errorMessageGenerator));
            }

            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            this._requestor = requestor;
            this._errorMessageGenerator = errorMessageGenerator;
            this._settings = settings;
        }

        public async Task<SentimentResult> GetSentimentAsync(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return SentimentResult.Build(Constants.SentimentNullInputErrorText);
            }

            var request = $"{Constants.SentimentRequest}{HttpUtility.UrlEncode(text)}";
            string content;
            using (var response = await this._requestor.GetAsync(request))
            {
                content = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    return SentimentResult.Build(this._errorMessageGenerator.GenerateError(response.StatusCode, content));
                }
            }

            var result = JsonConvert.DeserializeObject<AzureSentimentResult>(content);
            return SentimentResult.Build(result.Score);
        }

        public async Task<IDictionary<string, SentimentResult>> GetBatchSentimentAsync(Dictionary<string, string> textBatch)
        {
            this.ValidateBatchRequest(textBatch);

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
                    return textBatch.ToDictionary(r => r.Key, r => SentimentResult.Build(this._errorMessageGenerator.GenerateError(response.StatusCode, content)));
                }
            }

            var result = JsonConvert.DeserializeObject<AzureSentimentBatchResult>(content);

            var parsedResults = result.SentimentBatch.ToDictionary(sr => sr.Id, sr => SentimentResult.Build(sr.Score));

            foreach (var error in result.Errors)
            {
                parsedResults.Add(error.Id, SentimentResult.Build(error.Message));
            }

            return parsedResults;
        }

        public async Task<KeyPhraseResult> GetKeyPhrasesAsync(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return KeyPhraseResult.Build(Constants.KeyPhraseNullInputErrorText);
            }

            var request = $"{Constants.KeyPhraseRequest}{HttpUtility.UrlEncode(text)}";

            string content;
            using (var response = await this._requestor.GetAsync(request))
            {
                content = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return KeyPhraseResult.Build(this._errorMessageGenerator.GenerateError(response.StatusCode, content));
                }
            }

            var result = JsonConvert.DeserializeObject<AzureKeyPhraseResult>(content);
            return KeyPhraseResult.Build(result.KeyPhrases);
        }

        public async Task<IDictionary<string, KeyPhraseResult>> GetBatchKeyPhrasesAsync(Dictionary<string, string> textBatch)
        {
            this.ValidateBatchRequest(textBatch);

            if (!textBatch.Any())
            {
                return new Dictionary<string, KeyPhraseResult>();
            }

            string content;
            using (var response = await this._requestor.PostAsync(Constants.KeyPhraseBatchRequest, BuildInputString(textBatch)))
            {
                content = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    return textBatch.ToDictionary(r => r.Key, r => KeyPhraseResult.Build(this._errorMessageGenerator.GenerateError(response.StatusCode, content)));
                }
            }

            var result = JsonConvert.DeserializeObject<AzureKeyPhrasesBatchResult>(content);

            var parsedResults = result.KeyPhrasesBatch.ToDictionary(sr => sr.Id, sr => KeyPhraseResult.Build(sr.KeyPhrases));

            foreach (var error in result.Errors)
            {
                parsedResults.Add(error.Id, KeyPhraseResult.Build(error.Message));
            }

            return parsedResults;
        }

        private void ValidateBatchRequest(Dictionary<string, string> textBatch)
        {
            if (textBatch == null)
            {
                throw new ArgumentNullException(nameof(textBatch));
            }

            if (textBatch.Count > this._settings.GetBatchLimit())
            {
                throw new InvalidOperationException(Constants.BatchLimitExceededErrorText);
            }
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