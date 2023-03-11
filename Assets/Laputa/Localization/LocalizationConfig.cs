using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace Laputa.Localization
{
    [CreateAssetMenu(fileName = "LocalizationConfig", menuName = "ScriptableObject/LocalizationConfig")]
    public class LocalizationConfig : ScriptableObject
    {
        public List<LanguageData> languageDataList;
        public List<LocalizedData> localizedDataList;

        public LanguageData GetLanguageData(LanguageName languageName)
        {
            return languageDataList.Find(item => item.languageName == languageName);
        }
        
        public void UpdateLanguageData()
        {
            for (int i = 0; i < Enum.GetNames(typeof(LanguageName)).Length; i++)
            {
                LanguageData data = new LanguageData {languageName = (LanguageName) i};
                if (!IsContainItem(data.languageName))
                {
                    languageDataList.Add(data);
                }
            }

            languageDataList = languageDataList.GroupBy(item => item.languageName).Select(group => group.First()).ToList();
        }

        private bool IsContainItem(LanguageName languageName)
        {
            foreach (LanguageData data in languageDataList)
            {
                if (data.languageName == languageName) return true;
            }

            return false;
        }

        public void TranslateAllPredata()
        {
            foreach (var localizedData in localizedDataList)
            {
                localizedData.Localize(this);
            }
        }

        // public void GetPretranslate(string content, LanguageName languageName)
        // {
        //     return localizedDataList.Find(item => item.GetLocalizedData(languageName))
        // }
    }

    [Serializable]
    public struct LanguageData
    {
        public LanguageName languageName;
        public string encode;
        public Sprite sprite;
        public Font font;
        public TMP_FontAsset tmpFontAsset;
    }
    
    [Serializable]
    public class LocalizedData
    {
        public string content;
        public List<PreLocalizedData> preLocalizedDataList;

        public string GetTranslatedContent(LanguageName languageName)
        {
            PreLocalizedData preLocalizedData = GetPreLocalizedData(languageName);
            return preLocalizedData.translatedContent;
        }

        public PreLocalizedData GetPreLocalizedData(LanguageName languageName)
        {
            return preLocalizedDataList.Find(item => item.languageName == languageName);
        }

        public async void Localize(LocalizationConfig localizationConfig)
        {
            try
            {
                Debug.Log("<color=green> Start translating ...</color>");
                
                for (int i = 0; i < Enum.GetNames(typeof(LanguageName)).Length; i++)
                {
                    LanguageName language = (LanguageName) i;
                    PreLocalizedData preLocalizedData = GetPreLocalizedData(language);
                    if (preLocalizedData!=null)
                    {
                        preLocalizedData.translatedContent = await LocalizationManager.TranslateAsync(content, localizationConfig.GetLanguageData((LanguageName) i).encode);
                    }
                    else
                    {
                        PreLocalizedData tempPreLocalizedData = new PreLocalizedData(language, await LocalizationManager.TranslateAsync(content, localizationConfig.GetLanguageData((LanguageName) i).encode));
                        preLocalizedDataList.Add(tempPreLocalizedData);
                    }
                }
                
                preLocalizedDataList = preLocalizedDataList.GroupBy(item => item.languageName).Select(group => group.First()).ToList();
                Debug.Log("<color=green> Update succeed </color>");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
    
    [Serializable]
    public class PreLocalizedData
    {
        public LanguageName languageName;
        public string translatedContent;

        public PreLocalizedData(LanguageName languageName, string content)
        {
            this.languageName = languageName;
            translatedContent = content;
        }
    }
}