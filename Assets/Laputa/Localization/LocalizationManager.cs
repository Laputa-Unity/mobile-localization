using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using UnityEngine;

public static class LocalizationManager
{
    public static LanguageName currentLanguageName = LanguageName.English;
    private static readonly HttpClient Client = new HttpClient();

    public static void OnChangeLanguage(LanguageName languageName)
    {
        currentLanguageName = languageName;
        LocalizationObserver.onLanguageChanged?.Invoke(languageName);
    }

    public static async Task<string> TranslateAsync(string text, string targetLanguage, string sourceLanguage = "en")
    {
        string url = $"https://translate.googleapis.com/translate_a/single?client=gtx&sl={sourceLanguage}&tl={targetLanguage}&dt=t&q={Uri.EscapeDataString(text)}";

        HttpResponseMessage response = await Client.GetAsync(url);
        response.EnsureSuccessStatusCode();

        Debug.Log(response.EnsureSuccessStatusCode());
        
        string responseBody = await response.Content.ReadAsStringAsync();
        JArray responseJson = JArray.Parse(responseBody);
        

        return (string) responseJson[0][0]?[0];
    }
}

public static class LocalizationObserver
{
    public static Action<LanguageName> onLanguageChanged;
}