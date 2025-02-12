using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

public class Translator
{
    public static async Task<string> TraduireTexte(string texte, string sourceLang, string targetLang)
    {
        string url = $"https://translate.googleapis.com/translate_a/single?client=gtx&sl={sourceLang}&tl={targetLang}&dt=t&q={Uri.EscapeDataString(texte)}";

        using HttpClient client = new HttpClient();
        string response = await client.GetStringAsync(url);
        using JsonDocument json = JsonDocument.Parse(response);
        string translatedText = json.RootElement[0][0][0].GetString();

        return translatedText;
    }
}
