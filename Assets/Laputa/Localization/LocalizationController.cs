using System;
using UnityEngine;

namespace Laputa.Localization
{
    public class LocalizationController : MonoBehaviour
    {
        public static LocalizationController Instance;
        public LocalizationConfig localizationConfig;
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
            
            LanguageName currentLanguageName = Enum.Parse<LanguageName>(PlayerPrefs.GetString("localization","English"));
            LocalizationManager.OnChangeLanguage(currentLanguageName);

            if (localizationConfig==null) localizationConfig = Resources.Load<LocalizationConfig>("LocalizationConfig");
        }

        private void Start()
        {
            DontDestroyOnLoad(this);
        }

        protected void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }

        public string GetPreTranslatedText(string content)
        {
            return localizationConfig.GetPreTranslated(content, LocalizationManager.currentLanguageName);
        }
    }
}