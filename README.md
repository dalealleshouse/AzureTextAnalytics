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

var sentiment = service.GetSentimentAsync("This is some awesome text that needs sentiment analysis;").Result;
Console.WriteLine(
    sentiment.Success
        ? $"The sentiment score is {sentiment.Score}"
        : $"Error: Http Status: {sentiment.StatusCode}, Contents: {sentiment.Error}");
```

Extract Key Phrases
===================
The service returns a a collection of key phrases.

```csharp
var service = ServiceFactory.Build();

var phrases = service.GetKeyPhrasesAsync("This is some awesome text that needs the key phrases extracted from.").Result;
Console.WriteLine(
    sentiment.Success
        ? $"The key phrases are: {string.Join(",", phrases.Phrases)}"
        : $"Error: Http Status: {phrases.StatusCode}, Contents: {phrases.Error}");
```


I'm happy to accept any pull requests (as long as they build).