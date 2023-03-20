using System;
using Laputa.Localization;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class Example : MonoBehaviour
{
    public string randomStr1 = "hello";
    public string randomStr2 = "help";

    public TextMeshProUGUI textMeshProUGUI;
    
    void Start()
    {
        LocalizationObserver.onLanguageChanged += UpdateText;
        UpdateText(LocalizationManager.currentLanguageName);
    }

    private void OnDestroy()
    {
        LocalizationObserver.onLanguageChanged -= UpdateText;
    }

    void UpdateText(LanguageName languageName)
    {
        TMP_FontAsset tmpFontAsset = LocalizationController.Instance.GetLanguageData(languageName).tmpFontAsset;
        int rd = Random.Range(0, 2);
        string content = rd == 0 ? randomStr1 : randomStr2;
        textMeshProUGUI.text = LocalizationController.Instance.GetPreTranslatedText(content);
        textMeshProUGUI.font = tmpFontAsset;
    }
}
