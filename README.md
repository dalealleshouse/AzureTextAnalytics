# Azure Text Analytics
Extract key phrases and do sentiment analysis on text.

C# library for accessing Azure Machine Learning Text Analitics : [https://azure.microsoft.com/en-us/documentation/articles/machine-learning-apps-text-analytics/]
(https://azure.microsoft.com/en-us/documentation/articles/machine-learning-apps-text-analytics/)


How does it work?
===================
First, you need to install the package via NuGet:

```
PM> Install-Package AzureTextAnalytics
```

Next, create a configration setting with your Account Key. You can obtain an Account Key here: [https://datamarket.azure.com/dataset/amla/text-analytics](https://datamarket.azure.com/dataset/amla/text-analytics)

```xml
<appSettings>
    <add key="AccountKey" value="{YOUR ACCOUNT KEY HERE}" />
</appSettings>
```

Extract Sentiment Score
===================
The service returns a score betwen 0 and 1 where 0 is negative and 1 is positive.

```csharp
var service = ServiceFactory.Build();

var sentiment = await service.GetSentimentAsync("This is some awesome text that needs sentiment analysis;");
Console.WriteLine(
    sentiment.Success 
        ? $"The sentiment score is {sentiment.Score}" 
        : sentiment.Error);
```

Batch request

```csharp
var service = ServiceFactory.Build();

var request = new Dictionary<string, string>
                    {
                        { "1", "This is very positive text because I love this service" },
                        { "2", "Test is very bad because I hate this service" },
                        { "3", "The service was OK, nothing special, I've had better" },
                        { "4", "" }
                    };

var batchResult = await service.GetBatchSentimentAsync(request);

foreach (var result in batchResult)
{
    Console.WriteLine(
        result.Value.Success 
            ? $"Id: {result.Key} = {result.Value.Score}" 
            : $"Id: {result.Key} = Error: {result.Value.Error}");
}
```

Extract Key Phrases
===================
The service returns a a collection of key phrases.

```csharp
var service = ServiceFactory.Build();

var phrases = await service.GetKeyPhrasesAsync("This is some awesome text that needs the key phrases extracted from.");
Console.WriteLine(
    phrases.Success 
        ? $"The key phrases are: {string.Join(",", phrases.Phrases)}" 
        : phrases.Error);
```

Batch request

```csharp
var service = ServiceFactory.Build();

var request = new Dictionary<string, string>
                    {
                        { "1", "This is very positive text because I love this service" },
                        { "2", "Test is very bad because I hate this service" },
                        { "3", "The service was OK, nothing special, I've had better" },
                        { "4", "" }
                    };

var batchResult = await service.GetBatchKeyPhrasesAsync(request);

foreach (var result in batchResult)
{
    Console.WriteLine(
        result.Value.Success 
            ? $"Id: {result.Key} = {string.Join(",", result.Value.Phrases)}" 
            : $"Id: {result.Key} = Error: {result.Value.Error}");
}
```

I'm happy to accept any pull requests (as long as they build).