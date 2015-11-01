namespace AzureTextAnalytics.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;

    public class KeyPhraseResult : HttpResult
    {
        private KeyPhraseResult(HttpStatusCode code, string error, IEnumerable<string> phrases)
            : base(code, error)
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

        public static KeyPhraseResult Build(IEnumerable<string> phrases)
        {
            return new KeyPhraseResult(HttpStatusCode.OK, null, phrases);
        }

        public static KeyPhraseResult Build(HttpStatusCode code, string error)
        {
            if (code == HttpStatusCode.OK)
            {
                throw new InvalidOperationException(Constants.ErrorWithOkResultErrorText);
            }

            return new KeyPhraseResult(code, error, null);
        }

        public static KeyPhraseResult Build(HttpStatusCode code, string error, IEnumerable<string> phrases)
        {
            return code == HttpStatusCode.OK ? Build(phrases) : Build(code, error);
        }
    }
}