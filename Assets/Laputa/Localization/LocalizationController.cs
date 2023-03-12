using System;
using Laputa.Localization;
using UnityEngine;

public class LocalizationController : MonoBehaviour
{
    private void Awake()
    {
        LanguageName currentLanguageName = Enum.Parse<LanguageName>(PlayerPrefs.GetString("localization","English"));
        LocalizationManager.OnChangeLanguage(currentLanguageName);
    }

    private void Update()
    {
        //Debug.Log(LocalizationManager.currentLanguageName);
    }
}
