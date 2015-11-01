namespace AzureTextAnalytics.Domain
{
    using System.Net;

    public abstract class HttpResult
    {
        private HttpResult()
        {
        }

        protected HttpResult(HttpStatusCode code, string error)
        {
            this.StatusCode = code;
            this.Error = error;
        }

        public HttpStatusCode StatusCode { get; }

        public string Error { get; }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType()) return false;

            var sr = (HttpResult)obj;
            return StatusCode == sr.StatusCode && Error == sr.Error;
        }

        public override int GetHashCode() => StatusCode.GetHashCode() ^ Error?.GetHashCode() ?? 0;

        public bool Success => this.StatusCode == HttpStatusCode.OK;
    }
}