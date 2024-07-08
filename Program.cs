using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

// dotnet add package OpenAI --version 1.7.0
class Program
{
    private static readonly HttpClient client = new HttpClient();

    static async Task Main(string[] args)
    {

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(" _______           _______ _________   _______  _______ _________\r\n(  ____ \\|\\     /|(  ___  )\\__   __/  (  ____ \\(  ____ )\\__   __/\r\n| (    \\/| )   ( || (   ) |   ) (     | (    \\/| (    )|   ) (   \r\n| |      | (___) || (___) |   | |     | |      | (____)|   | |   \r\n| |      |  ___  ||  ___  |   | |     | | ____ |  _____)   | |   \r\n| |      | (   ) || (   ) |   | |     | | \\_  )| (         | |   \r\n| (____/\\| )   ( || )   ( |   | |     | (___) || )         | |   \r\n(_______/|/     \\||/     \\|   )_(     (_______)|/          )_( \r\n\r\n                C O N S O L E   E D I T I O N");
        Console.WriteLine("");
        Console.WriteLine("Appuyez sur une touche pour continuer...");
        Console.ReadLine();
        Console.Clear();





        // Mettre ici la clé API
        string apiKey = "ENTER_YOUR_API_KEY_HERE";
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

        Print("Bienvenue dans votre application de chat !");
        Print("Posez-moi des questions ou discutez avec moi.");

        while (true)
        {
            Print("");
            Console.Write("Vous : ");
            var input = Console.ReadLine();

            var requestBody = new
            {
                model = "gpt-3.5-turbo",
                messages = new[]
                {
                    new { role = "user", content = input }
                },
                max_tokens = 1500 // Vous pouvez ajuster ce paramètre selon vos besoins
            };

            var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync("https://api.openai.com/v1/chat/completions", content);
            response.EnsureSuccessStatusCode();

            string responseBody = await response.Content.ReadAsStringAsync();
            var jsonDoc = JsonDocument.Parse(responseBody);
            var chatResponse = jsonDoc.RootElement.GetProperty("choices")[0].GetProperty("message").GetProperty("content").GetString();

            Print("");
            Print("ChatGPT : " + chatResponse);

        }
    }



    public static void Print(string text)
    {
        int windowWidth = Console.WindowWidth;
        string[] words = text.Split(' ');
        int currentLineLength = 0;

        foreach (string word in words)
        {
            if (currentLineLength + word.Length + 1 > windowWidth)
            {
                Console.WriteLine();
                currentLineLength = 0;
            }

            foreach (char c in word)
            {
                Console.Write(c);
                Thread.Sleep(40);
            }

            Console.Write(' ');
            currentLineLength += word.Length + 1;
        }

        Console.WriteLine();
    }
}
