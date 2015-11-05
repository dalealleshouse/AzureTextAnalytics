namespace AzureTextAnalytics
{
    using System.Net;

    public class ErrorMessageGenerator : IErrorMessageGenerator
    {
        public string GenerateError(HttpStatusCode code, string error)
        {
            var intCode = (int)code;
            return $"{intCode} {code}: {error}";
        }
    }
}