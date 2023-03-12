using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Laputa.Localization.Components
{
    public class LocalizedDropdown : MonoBehaviour
    {
        private Dropdown Dropdown => GetComponent<Dropdown>();
        private TMP_Dropdown TmpDropDown => GetComponent<TMP_Dropdown>();
        
        private void OnDrawGizmos()
        {
            if (Application.isPlaying) return;
            UpdateCurrentLanguage(LocalizationManager.currentLanguageName);
        }

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

            LocalizationObserver.onLanguageChanged += UpdateCurrentLanguage;
        }

        private void OnDestroy()
        {
            if (TmpDropDown)
            {
                TmpDropDown.onValueChanged.RemoveListener(delegate
                {
                    OnChangeValue();
                });
            }
            else if (Dropdown)
            {
                Dropdown.onValueChanged.RemoveListener(delegate
                {
                    OnChangeValue();
                });
            }
            
            LocalizationObserver.onLanguageChanged -= UpdateCurrentLanguage;
        }

        public void UpdateCurrentLanguage(LanguageName languageName)
        {
            if (TmpDropDown)
            {
                var index = TmpDropDown.options.FindIndex(item => item.text == languageName.ToString());
                TmpDropDown.value = index;
            }
        }

        void OnChangeValue()
        {
            if (TmpDropDown)
            {
                int selectedOptionIndex = TmpDropDown.value;
                string selectedOptionText = TmpDropDown.options[selectedOptionIndex].text;
                LocalizationManager.OnChangeLanguage(Enum.Parse<LanguageName>(selectedOptionText));
            }
            else if (Dropdown)
            {
                int selectedOptionIndex = Dropdown.value;
                string selectedOptionText = Dropdown.options[selectedOptionIndex].text;
                LocalizationManager.OnChangeLanguage(Enum.Parse<LanguageName>(selectedOptionText));
            }
        }

        public void UpdateOptions()
        {
            LocalizationConfig localizationConfig = Resources.Load<LocalizationConfig>("LocalizationConfig");

            if (TmpDropDown)
            {
                TmpDropDown.options.Clear();
                foreach (LanguageData data in localizationConfig.languageDataList)
                {
                    TmpDropDown.options.Add(new TMP_Dropdown.OptionData(data.languageName.ToString(),data.sprite));
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
}
