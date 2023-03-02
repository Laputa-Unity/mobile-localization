using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "LocalizationConfig", menuName = "ScriptableObject/LocalizationConfig")]
public class LocalizationConfig : ScriptableObject
{
    public List<LanguageData> languageDataList;

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
}

[Serializable]
public struct LanguageData
{
    public LanguageName languageName;
    public string encode;
    public Font font;
    public TMP_FontAsset tmpFontAsset;

}