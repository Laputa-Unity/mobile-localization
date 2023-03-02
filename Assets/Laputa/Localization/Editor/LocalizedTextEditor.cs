using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LocalizedText))]
public class LocalizedTextEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        var localizedText = (LocalizedText) target;
 
        if(GUILayout.Button("Auto generate (require internet)", GUILayout.Height(40)))
        {
            // ReSharper disable once Unity.PerformanceCriticalCodeInvocation
            localizedText.AutoGenerate();
        }
        
        if(GUILayout.Button("Update Current Language", GUILayout.Height(40)))
        {
            localizedText.UpdateCurrentLanguage(LocalizationManager.currentLanguageName);
        }

        serializedObject.ApplyModifiedProperties();
    }
}
