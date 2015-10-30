namespace AzureTextAnalytics.Domain.Azure
{
    using System.Collections.Generic;

    public class KeyPhrasesBatchResult
    {
        public IEnumerable<AzureKeyPhraseResult> KeyPhrases { get; set; }

        public IEnumerable<ErrorRecord> Errors { get; set; }
    }
}