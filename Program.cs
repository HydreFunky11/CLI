using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Diagnostics;

class Program
{
    static async Task Main(string[] args)
    {
        string apiKey = "API_key";
        string apiUrl = "https://api.openai.com/v1/engines/text-davinci-002/completions";

        using (HttpClient client = new HttpClient())
        {
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

            bool continueListening = true;

            while (continueListening)
            {
                Console.Write("Entrez une commande (-c pour corriger, -t pour traduire, -create pour créer une app react , -q pour quitter ) : ");
                string command = Console.ReadLine();

                if (command == "-q")
                {
                    continueListening = false;
                    Console.WriteLine("Fin du programme.");
                    break;
                }
                else if (command == "-c")
                {
                    Console.Write("Entrez le texte à corriger : ");
                    string input = Console.ReadLine();
                    JObject requestBody = new JObject
                    {
                        { "prompt", $"Corrigez l'orthographe du texte suivant : {input}" },
                        { "temperature", 0.2 },
                        { "max_tokens", 50 }
                    };
                    StringContent content = new StringContent(requestBody.ToString(), Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync(apiUrl, content);
                    string responseContent = await response.Content.ReadAsStringAsync();
                    JObject responseData = JObject.Parse(responseContent);
                    string correctedText = responseData["choices"][0]["text"].ToString().Trim();

                    Console.WriteLine("Texte corrigé :");
                    Console.WriteLine(correctedText);
                }
                else if (command == "-t")
                {
                    Console.Write("Entrez le texte à traduire : ");
                    string input = Console.ReadLine();
                    JObject requestBody = new JObject
                    {
                        { "prompt", $"traduit le texte suivant en anglais: {input}" },
                        { "temperature", 0.2 },
                        { "max_tokens", 50 }
                    };
                    StringContent content = new StringContent(requestBody.ToString(), Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync(apiUrl, content);
                    string responseContent = await response.Content.ReadAsStringAsync();
                    JObject responseData = JObject.Parse(responseContent);
                    string correctedText = responseData["choices"][0]["text"].ToString().Trim();

                    Console.WriteLine("Texte traduit :");
                    Console.WriteLine(correctedText);
                }
                 else if (command == "-create")
                {
                    Console.Write("Nom du projet React : ");
                    string projectName = Console.ReadLine();

                    // Exécutez la commande create-react-app pour créer le projet React
                    ProcessStartInfo startInfo = new ProcessStartInfo
                    {
                        FileName = "cmd.exe",
                        Arguments = $"/c npx create-react-app {projectName}",
                        WorkingDirectory = @""//le dossier dans lequel vous le voulez exp : C:\Users\user\Desktop\...
                    };
                    Process.Start(startInfo);

                    Console.WriteLine("Application React créée avec succès.");
                }
                else
                {
                    Console.WriteLine("Commande non reconnue. Utilisez -c pour corriger ou -q pour quitter.");
                }

                Console.Write("Continuer ? (y/yes/n/no) : ");
                string continueInput = Console.ReadLine();
                continueListening = (continueInput == "y" || continueInput == "yes");
            }
        }
    }
}