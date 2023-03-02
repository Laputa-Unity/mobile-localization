using System;
using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LocalizedText : MonoBehaviour
{
    [SerializeField] [ReadOnly] private string currentText;
    [SerializeField] private List<LanguageLocalizedData> languageDataList = new List<LanguageLocalizedData>();
    private TextMeshProUGUI TextMeshProUGUI => GetComponent<TextMeshProUGUI>();
    private Text Text => GetComponent<Text>();
    
    public void OnDrawGizmos()
    {
        if (TextMeshProUGUI == null && Text == null)
        {
            gameObject.AddComponent<TextMeshProUGUI>();
        }
        currentText = TextMeshProUGUI ? TextMeshProUGUI.text : Text.text;
    }

    private void Awake()
    {
        LocalizationObserver.onLanguageChanged += UpdateCurrentLanguage;
    }

    private void OnDestroy()
    {
        LocalizationObserver.onLanguageChanged -= UpdateCurrentLanguage;
    }

    public void UpdateCurrentLanguage(LanguageName languageName)
    {
        LanguageLocalizedData data = languageDataList.Find(item =>
            item.languageData.languageName == LocalizationManager.currentLanguageName);

        if (TextMeshProUGUI)
        {
            TextMeshProUGUI.text = data.text;
            TextMeshProUGUI.font = data.languageData.tmpFontAsset;
        }
        else
        {
            Text.text = data.text;
            Text.font = data.languageData.font;
        }
    }

    public async void AutoGenerate()
    {
        Debug.Log("<color=green> Start generating ... </color>");
        LocalizationConfig localizationConfig= Resources.Load<LocalizationConfig>("LocalizationConfig");

        if (localizationConfig)
        {
            try
            {
                foreach (var data in localizationConfig.languageDataList)
                {
                    LanguageLocalizedData languageLocalizedData = GetLanguage(data);
                    string translatedText = await LocalizationManager.TranslateAsync(currentText, data.encode);
                
                    if (languageLocalizedData != null)
                    {
                        if (!languageLocalizedData.save)
                        {
                            languageLocalizedData.text = translatedText;
                            languageLocalizedData.languageData.font = data.font;
                            languageLocalizedData.languageData.tmpFontAsset = data.tmpFontAsset;
                        }
                    }
                    else
                    {
                        languageLocalizedData = new LanguageLocalizedData {languageData = data,text = translatedText};
                        languageDataList.Add(languageLocalizedData);
                    }
                }
                Debug.Log("<color=green> Update succeed </color>");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        else
        {
            Debug.LogWarning("Missing LocalizationConfig.asset");
        }
    }

    private LanguageLocalizedData GetLanguage(LanguageData languageData)
    {
        return languageDataList.Find(item => item.languageData.languageName == languageData.languageName);
    }
}

[Serializable]
public class LanguageLocalizedData
{
    public bool save;
    public LanguageData languageData;
    public string text;
}