using System;
using System.Collections.Generic;
using System.Linq;
using Laputa.Localization;
using Laputa.Localization.Components;
using UnityEditor;
using UnityEditor.SceneManagement;
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
        
        
        //GUILayout.Label ("Use when you want to translate all text in scene using LocalizedText components", EditorStyles.boldLabel);
        if(GUILayout.Button("Translate All In Scene"))
        {
            var gameObjects = FindObjectsOfType<GameObject>(true).ToList();
            
            // Loop through all the GameObjects and add their names to the list
            foreach (GameObject gameObject in gameObjects)
            {
                var localizedText = gameObject.GetComponent<LocalizedText>();
                if (localizedText)
                {
                    localizedText.AutoGenerate();
                }
            }
            
            EditorSceneManager.SaveScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene());
        }
        
        
        if(GUILayout.Button("Translate All In Prefab Mode"))
        {
            var prefabStage = PrefabStageUtility.GetCurrentPrefabStage();

            if (prefabStage)
            {
                var prefabRoot = prefabStage.prefabContentsRoot;
                List<LocalizedText> localizedTexts = prefabRoot.GetComponentsInChildren<LocalizedText>(true).ToList();
                foreach (var localizedText in localizedTexts)
                {       
                    localizedText.AutoGenerate();
                }

                PrefabUtility.SaveAsPrefabAsset(prefabRoot, prefabStage.assetPath);
            }
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