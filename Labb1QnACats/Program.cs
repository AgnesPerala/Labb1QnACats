using Azure;
using Azure.AI.Language.QuestionAnswering;
using System;

namespace Labb1QnACats;

internal class Program
{
    static async Task Main(string[] args)
    {
        // Byt ut dessa värden med dina egna
        string endpoint = "https://agnesqnacats.cognitiveservices.azure.com/";
        string apiKey = "3318a56e503840edb1feda30fab0f79e"; // din nyckel från Azure-portalen
        string projectName = "AgnesQnA"; // namnet på ditt projekt i Language Studio
        string deploymentName = "production"; // Deployment name, ofta "production" som standard

        // Skapa en credential för att autentisera mot Azure
        var credential = new AzureKeyCredential(apiKey);

        // Skapa en klient för Custom Question Answering
        var client = new QuestionAnsweringClient(new Uri(endpoint), credential);

        bool continueAsking = true;

        while (continueAsking)
        {
            // Användaren skriver in en fråga
            Console.WriteLine("Ask a question about cats:");
            string question = Console.ReadLine();

            try
            {
                // Skapa ett QuestionAnsweringProject-objekt för att referera till ditt projekt och deployment
                var qnaProject = new QuestionAnsweringProject(projectName, deploymentName);

                // Anropa tjänsten för att få svar från den publicerade kunskapsbasen
                var response = await client.GetAnswersAsync(question, qnaProject);

                // Visa svaret på ett snyggt sätt
                Console.WriteLine("\nAnswer:");
                foreach (var answer in response.Value.Answers)
                {
                    Console.WriteLine($"- {answer.Answer}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

            // Fråga användaren om de vill ställa en ny fråga
            Console.WriteLine("\nWould you like to ask another question? (yes/no)");
            string userResponse = Console.ReadLine().ToLower();
            if (userResponse != "yes" && userResponse != "y")
            {
                continueAsking = false;
            }
        }

        // Avsluta programmet
        Console.WriteLine("Thank you for using the Q&A service. Goodbye!");
    }
}