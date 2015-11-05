namespace AzureTextAnalytics.Domain
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ITextAnalyticsService
    {
        Task<SentimentResult> GetSentimentAsync(string text);

        Task<IDictionary<string, SentimentResult>> GetBatchSentimentAsync(Dictionary<string, string> textBatch);

        Task<KeyPhraseResult> GetKeyPhrasesAsync(string text);
    }
}