using System.Linq;
using Laputa.Localization;
using Laputa.Localization.Components;
using TMPro;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

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
            
            Debug.Log($"<color=green> Change Language To {_selectedLanguage} Succeed </color>");
        }

        //GUILayout.Label ("Use when you want to translate all text in scene using LocalizedText components", EditorStyles.boldLabel);
        if(GUILayout.Button("Add LocalizedText In Scene"))
        {
            TranslateAllInScene();
        }
        
        if(GUILayout.Button("Remove LocalizedText In Scene"))
        {
            RemoveAllInScene();
        }
        
        
        if(GUILayout.Button("Add LocalizedText In Prefab Mode"))
        {
            TranslateAllInPrefab();
        }

        if(GUILayout.Button("Remove LocalizedText In Prefab Mode"))
        {
            RemoveAllInPrefab();
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

    private void TranslateAllInScene()
    {
        var gameObjects = FindObjectsOfType<GameObject>(true).ToList();
        
        foreach (GameObject gameObject in gameObjects)
        {
            var text = gameObject.GetComponent<Text>();
            var tmpText = gameObject.GetComponent<TextMeshProUGUI>();
            var localizedText = gameObject.GetComponent<LocalizedText>();
            if (localizedText)
            {
                localizedText.AutoGenerate();
            }
            else
            {
                if (tmpText || text)
                {
                    var tempLocalizedText = gameObject.gameObject.AddComponent<LocalizedText>();
                    tempLocalizedText.AutoGenerate();
                }
            }
        }
            
        EditorSceneManager.SaveScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene());
    }
    
    private void TranslateAllInPrefab()
    {
         var prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
        
        if (prefabStage)
        {
            var prefabRoot = prefabStage.prefabContentsRoot;
            var tmpTextList = prefabRoot.GetComponentsInChildren<TextMeshProUGUI>(true).ToList();
            var textList = prefabRoot.GetComponentsInChildren<Text>(true).ToList();
            
            foreach (var tempText in tmpTextList)
            {
                var localizedText = tempText.GetComponent<LocalizedText>();
                if (localizedText)
                {
                    localizedText.AutoGenerate();
                }
                else
                {
                    var tempLocalizedText = tempText.gameObject.AddComponent<LocalizedText>();
                    tempLocalizedText.AutoGenerate();
                }
            }
            
            foreach (var text in textList)
            {
                var localizedText = text.GetComponent<LocalizedText>();
                if (localizedText)
                {
                    localizedText.AutoGenerate();
                }
                else
                {
                    var tempLocalizedText = text.gameObject.AddComponent<LocalizedText>();
                    tempLocalizedText.AutoGenerate();
                }
            }

            PrefabUtility.SaveAsPrefabAsset(prefabRoot, prefabStage.assetPath);
        }
    }
    
    private void RemoveAllInScene()
    {
        var gameObjects = FindObjectsOfType<GameObject>(true).ToList();
        
        foreach (GameObject gameObject in gameObjects)
        {
            var localizedText = gameObject.GetComponent<LocalizedText>();
            if(localizedText != null)
            {
                // Remove the component from the game object
                
            }
        }
            
        EditorSceneManager.SaveScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene());
    }
    
    private void RemoveAllInPrefab()
    {
         var prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
        
        if (prefabStage)
        {
            var prefabRoot = prefabStage.prefabContentsRoot;
            var tmpTextList = prefabRoot.GetComponentsInChildren<TextMeshProUGUI>(true).ToList();
            var textList = prefabRoot.GetComponentsInChildren<Text>(true).ToList();
            
            foreach (var tempText in tmpTextList)
            {
                var localizedText = tempText.GetComponent<LocalizedText>();
                if (localizedText)
                {
                    DestroyImmediate(localizedText);
                }
            }
            
            foreach (var text in textList)
            {
                var localizedText = text.GetComponent<LocalizedText>();
                if (localizedText)
                {
                    DestroyImmediate(localizedText);
                }
            }

            PrefabUtility.SaveAsPrefabAsset(prefabRoot, prefabStage.assetPath);
        }
    }
}