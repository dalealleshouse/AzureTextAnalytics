namespace AzureTextAnalytics.Domain
{
    using System;
    using System.Net;

    public class SentimentResult
    {
        private SentimentResult(HttpStatusCode code, string error, decimal result)
        {
            this.StatusCode = code;
            this.Error = error;
            this.Result = result;
        }

        public HttpStatusCode StatusCode { get; }

        public decimal Result { get; }

        public string Error { get; }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType()) return false;

            var sr = (SentimentResult)obj;
            return StatusCode == sr.StatusCode && Result == sr.Result && Error == sr.Error;
        }

        public override int GetHashCode() => StatusCode.GetHashCode() ^ Result.GetHashCode() ^ Error.GetHashCode();

        public bool Success => this.StatusCode == HttpStatusCode.OK;

        public static SentimentResult Build(decimal result) => new SentimentResult(HttpStatusCode.OK, null, result);

        public static SentimentResult Build(HttpStatusCode code, string error)
        {
            if (code == HttpStatusCode.OK)
            {
                throw new InvalidOperationException("Cannot construct a SentimentResult with an error and a status code of HttpStatusCode.OK");
            }

            return new SentimentResult(code, error, decimal.MinValue);
        }

        public static SentimentResult Build(HttpStatusCode code, string error, decimal result)
        {
            return code == HttpStatusCode.OK ? Build(result) : Build(code, error);
        }
    }
}