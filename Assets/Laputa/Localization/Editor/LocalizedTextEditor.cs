using Laputa.Localization.Components;
using UnityEditor;
using UnityEngine;

namespace Laputa.Localization.Editor
{
    [CustomEditor(typeof(LocalizedText))]
    public class LocalizedTextEditor : UnityEditor.Editor
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

            serializedObject.ApplyModifiedProperties();
        }
    }
}
