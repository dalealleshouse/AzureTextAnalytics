namespace AzureTextAnalytics.Domain
{
    using System;
    using System.Net;

    public class SentimentResult : HttpResult
    {
        private SentimentResult(HttpStatusCode code, string error, decimal score)
            : base(code, error)
        {
            this.Score = score;
        }

        public decimal Score { get; }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType()) return false;

            var s = (SentimentResult)obj;
            return base.Equals(obj) && this.Score == s.Score;
        }

        public override int GetHashCode() => base.GetHashCode() ^ Score.GetHashCode();

        public static SentimentResult Build(decimal result)
        {
            if (result < 0 || result > 1)
            {
                throw new ArgumentOutOfRangeException(nameof(result), Constants.ScoreOutOfRangeError);
            }
            return new SentimentResult(HttpStatusCode.OK, null, result);
        }

        public static SentimentResult Build(HttpStatusCode code, string error)
        {
            if (code == HttpStatusCode.OK)
            {
                throw new InvalidOperationException(Constants.ErrorWithOkResultErrorText);
            }

            return new SentimentResult(code, error, decimal.MinValue);
        }

        public static SentimentResult Build(HttpStatusCode code, string error, decimal result)
        {
            return code == HttpStatusCode.OK ? Build(result) : Build(code, error);
        }
    }
}