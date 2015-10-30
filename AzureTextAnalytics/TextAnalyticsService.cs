﻿namespace AzureTextAnalytics
{
    using System;
    using System.Net;
    using System.Threading.Tasks;
    using System.Web;

    using AzureTextAnalytics.Domain;
    using AzureTextAnalytics.Domain.Azure;

    using Newtonsoft.Json;

    public class TextAnalyticsService : ITextAnalyticsService
    {
        private readonly ITextAnalyticsRequestor _requestor;

        public TextAnalyticsService(ITextAnalyticsRequestor requestor)
        {
            if (requestor == null)
            {
                throw new ArgumentNullException(nameof(requestor));
            }

            this._requestor = requestor;
        }

        public async Task<SentimentResult> GetSentimentAsync(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return SentimentResult.Build(HttpStatusCode.BadRequest, Constants.NullInputErrorText);
            }

            var request = $"{Constants.SentimentRequest}{HttpUtility.UrlEncode(text)}";

            var response = await this._requestor.GetAsync(request);
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                return SentimentResult.Build(response.StatusCode, content);
            }

            var result = JsonConvert.DeserializeObject<AzureSentimentResult>(content);
            return SentimentResult.Build(result.Score);
        }
    }
}