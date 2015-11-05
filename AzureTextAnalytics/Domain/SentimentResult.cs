namespace AzureTextAnalytics.Domain
{
    using System;

    public class SentimentResult : HttpResult
    {
        private SentimentResult(bool success, string error, decimal score)
            : base(success, error)
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

            return new SentimentResult(true, null, result);
        }

        public static SentimentResult Build(string error)
        {
            return new SentimentResult(false, error, decimal.MinValue);
        }
    }
}