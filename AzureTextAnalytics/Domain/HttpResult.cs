namespace AzureTextAnalytics.Domain
{
    public abstract class HttpResult
    {
        HttpResult()
        {
        }

        protected HttpResult(bool success, string error)
        {
            this.Success = success;
            this.Error = error;
        }

        public string Error { get; }

        public bool Success { get; }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType()) return false;

            var sr = (HttpResult)obj;
            return Success == sr.Success && Error == sr.Error;
        }

        public override int GetHashCode() => Success.GetHashCode() ^ Error?.GetHashCode() ?? 0;

    }
}