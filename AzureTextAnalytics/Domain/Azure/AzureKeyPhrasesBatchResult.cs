namespace AzureTextAnalytics.Domain.Azure
{
    using System.Collections.Generic;

    public class AzureKeyPhrasesBatchResult
    {
        public IEnumerable<AzureKeyPhraseResult> KeyPhrasesBatch { get; set; }

        public IEnumerable<AzureErrorRecord> Errors { get; set; }
    }
}