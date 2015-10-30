namespace AzureTextAnalytics.Domain.Azure
{
    using System.Collections.Generic;

    public class SentimentBatchResult
    {
        public IEnumerable<AzureSentimentResult> SentimentBatch { get; set; }

        public IEnumerable<ErrorRecord> Errors { get; set; }
    }
}