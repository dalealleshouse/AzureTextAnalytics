namespace AzureTextAnalytics.Domain
{
    using System.Collections.Generic;
    using System.Linq;

    public class KeyPhraseResult : HttpResult
    {
        private KeyPhraseResult(bool success, string error, IEnumerable<string> phrases)
            : base(success, error)
        {
            this.Phrases = phrases ?? new string[0];
        }

        public IEnumerable<string> Phrases { get; }

        public override bool Equals(object obj)
        {
            if (!base.Equals(obj)) return false;

            var k = (KeyPhraseResult)obj;
            return Phrases.SequenceEqual(k.Phrases);
        }

        public override int GetHashCode() => base.GetHashCode() ^ this.Phrases.GetHashCode();

        public static KeyPhraseResult Build(IEnumerable<string> phrases) => new KeyPhraseResult(true, null, phrases);

        public static KeyPhraseResult Build(string error) => new KeyPhraseResult(false, error, null);
    }
}