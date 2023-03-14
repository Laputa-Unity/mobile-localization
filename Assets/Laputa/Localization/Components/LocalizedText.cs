using System;
using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Laputa.Localization.Components
{
    public class LocalizedText : MonoBehaviour
    {
        [SerializeField] [ReadOnly] private string currentText;
        [SerializeField] private List<LanguageLocalizedData> languageDataList = new List<LanguageLocalizedData>();
        [SerializeField] private List<LocalizedDataText> localizedDataTextList = new List<LocalizedDataText>();
        private TextMeshProUGUI TextMeshProUGUI => GetComponent<TextMeshProUGUI>();
        private Text Text => GetComponent<Text>();

        private void OnDrawGizmos()
        {
            if (Application.isPlaying) return;
            UpdateCurrentLanguage(LocalizationManager.currentLanguageName);
        }

        private void Awake()
        {
            LocalizationObserver.onLanguageChanged += UpdateCurrentLanguage;
        }

        private void OnDestroy()
        {
            LocalizationObserver.onLanguageChanged -= UpdateCurrentLanguage;
        }

        private LanguageLocalizedData GetLanguageLocalizedData()
        {
            return languageDataList.Find(item => item.languageData.languageName == LocalizationManager.currentLanguageName);
        }

        private void UpdateLocalizedDataText()
        {
            if (TextMeshProUGUI)
            {
                var tmpText = TextMeshProUGUI.text;
                foreach (var text in localizedDataTextList)
                {
                    tmpText = tmpText.Replace("{" +$"{text.wordReplace}" + "}", text.value);
                }
            }
            else
            {
                var textLegacy = Text.text;
                foreach (var text in localizedDataTextList)
                {
                    textLegacy = textLegacy.Replace($"{text.wordReplace}", text.value);
                }
            }
        }

        public void UpdateCurrentLanguage(LanguageName languageName)
        {
            var data = GetLanguageLocalizedData();

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
            UpdateLocalizedDataText();
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

    [Serializable]
    public class LocalizedDataText
    {
        public string wordReplace;
        public string value;
    }
}