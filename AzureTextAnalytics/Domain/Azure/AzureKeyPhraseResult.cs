namespace AzureTextAnalytics.Domain.Azure
{
    using System.Collections.Generic;

    public class AzureKeyPhraseResult
    {
        public string Id { get; set; }

        public IEnumerable<string> KeyPhrases { get; set; }
    }
}