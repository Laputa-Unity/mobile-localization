using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LocalizationConfig))]
public class LocalizationConfigEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        var localizationConfig = (LocalizationConfig) target;
 
        if(GUILayout.Button("Update Data", GUILayout.Height(40)))
        {
            localizationConfig.UpdateLanguageData();
        }

        serializedObject.ApplyModifiedProperties();
    }
}
