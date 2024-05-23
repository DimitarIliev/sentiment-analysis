using Azure;
using Azure.AI.TextAnalytics;
using Microsoft.Extensions.Configuration;

var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json");
var configuration = builder.Build();

var languageEndpoint = configuration["LanguageServiceUri"];
var languageKey = configuration["LanguageServiceAdminApiKey"];

var credential = new AzureKeyCredential(languageKey);
var endpoint = new Uri(languageEndpoint);

var client = new TextAnalyticsClient(endpoint, credential);

var documentsToAnalyze = new List<string>
            {
                "The apartment was not that much clean. At least it was close to the city center.",
                "The car is in a good state. But the price is pretty high and shocking!",
            };

AnalyzeSentimentResultCollection reviews = client.AnalyzeSentimentBatch(documentsToAnalyze, options: new AnalyzeSentimentOptions());

int index = 0;

foreach (AnalyzeSentimentResult review in reviews)
{
    Console.WriteLine($"Sentence: {documentsToAnalyze[index]}\n");
    Console.WriteLine($"Analyzed document sentiment: {review.DocumentSentiment.Sentiment}\n");
    Console.WriteLine($"Positive score: {review.DocumentSentiment.ConfidenceScores.Positive:0.00}");
    Console.WriteLine($"Negative score: {review.DocumentSentiment.ConfidenceScores.Negative:0.00}");
    Console.WriteLine($"Neutral score: {review.DocumentSentiment.ConfidenceScores.Neutral:0.00}\n");

    foreach (SentenceSentiment sentence in review.DocumentSentiment.Sentences)
    {
        Console.WriteLine($"Analyzed sentence: {sentence.Text}\n");
        Console.WriteLine($"Analyzed sentence sentiment: {sentence.Sentiment}\n");
        Console.WriteLine($"Analyzed sentence positive score: {sentence.ConfidenceScores.Positive:0.00}");
        Console.WriteLine($"Analyzed sentence negative score: {sentence.ConfidenceScores.Negative:0.00}");
        Console.WriteLine($"Analyzed sentence neutral score: {sentence.ConfidenceScores.Neutral:0.00}\n\n");
    }
    Console.WriteLine($"\n");
    index++;
}