using System;
using Laputa.Localization;
using Laputa.Localization.Components;
using UnityEditor;
using UnityEngine;

public class LocalizationWindowEditor : EditorWindow
{
    private LanguageName _selectedLanguage = LocalizationManager.currentLanguageName;
    // Add menu item named "My Window" to the Window menu
    [MenuItem("Services/Laputa/Localization")]
    public static void ShowWindow()
    {
        //Show existing window instance. If one doesn't exist, make one.
        EditorWindow.GetWindow(typeof(LocalizationWindowEditor));
    }

    private void OnEnable()
    {
        LocalizationObserver.onLanguageChanged += UpdateScripts;
    }

    private void OnDestroy()
    {
        LocalizationObserver.onLanguageChanged -= UpdateScripts;
    }

    void OnGUI()
    {
        GUILayout.Label ("Base Settings", EditorStyles.boldLabel);

        EditorGUILayout.LabelField("Current Language:");

        _selectedLanguage = (LanguageName)EditorGUILayout.EnumPopup(_selectedLanguage);
        
        if (GUILayout.Button("Update"))
        {
            LocalizationManager.OnChangeLanguage(_selectedLanguage);
        }
    }
    
    private void UpdateScripts(LanguageName language)
    {
        _selectedLanguage = language;
        foreach (var obj in FindObjectsOfType<GameObject>())
        {
            var localizedTexts = obj.GetComponent<LocalizedText>();
            var localizedDropDowns = obj.GetComponent<LocalizedDropdown>();
            if (localizedTexts)
            {
                localizedTexts.UpdateCurrentLanguage(_selectedLanguage);
            }
            if (localizedDropDowns)
            {
                localizedDropDowns.UpdateCurrentLanguage(_selectedLanguage);
            }
        }
    }
}