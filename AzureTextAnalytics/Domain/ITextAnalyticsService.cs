namespace AzureTextAnalytics.Domain
{
    using System.Threading.Tasks;

    public interface ITextAnalyticsService
    {
        Task<SentimentResult> GetSentimentAsync(string text);

        Task<KeyPhraseResult> GetKeyPhrases(string text);
    }
}