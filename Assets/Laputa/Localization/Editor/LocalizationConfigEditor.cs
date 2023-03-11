using System;
using UnityEditor;
using UnityEngine;

namespace Laputa.Localization.Editor
{
    [CustomEditor(typeof(LocalizationConfig))]
    public class LocalizationConfigEditor : UnityEditor.Editor
    {
        private LocalizationConfig _localizationConfig;

        void OnEnable()
        {
            _localizationConfig = (LocalizationConfig) target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if(GUILayout.Button("Update Language Data", GUILayout.Height(40)))
            {
                _localizationConfig.UpdateLanguageData();
            }
            
            if(GUILayout.Button("Translate All Predata", GUILayout.Height(40)))
            {
                _localizationConfig.TranslateAllPredata();
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
