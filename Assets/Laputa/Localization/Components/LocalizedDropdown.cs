using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LocalizedDropdown : MonoBehaviour
{
    private Dropdown Dropdown => GetComponent<Dropdown>();
    private TMP_Dropdown TmpDropDown => GetComponent<TMP_Dropdown>();

    private void Awake()
    {
        if (TmpDropDown)
        {
            TmpDropDown.onValueChanged.AddListener(delegate
            {
                OnChangeValue();
            });
        }
        else if (Dropdown)
        {
            Dropdown.onValueChanged.AddListener(delegate
            {
                OnChangeValue();
            });
        }
    }

    void OnChangeValue()
    {
        if (TmpDropDown)
        {
            LocalizationManager.OnChangeLanguage((LanguageName) TmpDropDown.value);
        }
        else if (Dropdown)
        {
            LocalizationManager.OnChangeLanguage((LanguageName) Dropdown.value);
        }
    }

    void OnDrawGizmos()
    {
        if (TmpDropDown == null && Dropdown == null)
        {
            gameObject.AddComponent<TMP_Dropdown>();
        }
    }

    public void UpdateOptions()
    {
        LocalizationConfig localizationConfig= Resources.Load<LocalizationConfig>("LocalizationConfig");

        if (TmpDropDown)
        {
            TmpDropDown.options.Clear();
            foreach (LanguageData data in localizationConfig.languageDataList)
            {
                TmpDropDown.options.Add(new TMP_Dropdown.OptionData(data.languageName.ToString()));
            }
        }
        else if (Dropdown)
        {
            Dropdown.options.Clear();
            List<string> listData = new List<string>();
            foreach (LanguageData data in localizationConfig.languageDataList)
            {
                listData.Add(data.languageName.ToString());
            }
            Dropdown.AddOptions(listData);
        }
    }
}
