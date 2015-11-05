namespace AzureTextAnalytics
{
    using System.Net;

    public interface IErrorMessageGenerator
    {
        string GenerateError(HttpStatusCode code, string error);
    }
}