using UnityEditor;
using UnityEngine;

namespace Laputa.Localization.Editor
{
    [CustomEditor(typeof(LocalizationConfig))]
    public class LocalizationConfigEditor : UnityEditor.Editor
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
}
