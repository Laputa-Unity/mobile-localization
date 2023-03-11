using System.Linq;
using Laputa.Localization.Components;
using UnityEditor;
using UnityEngine;

namespace Laputa.Localization.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(LocalizedText))]
    public class LocalizedTextEditor : UnityEditor.Editor
    {
        private LocalizedText[] _localizedTexts;

        private void OnEnable()
        {
            _localizedTexts = targets.Cast<LocalizedText>().ToArray();
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Auto generate (require internet)", GUILayout.Height(40)))
            {
                foreach (var localizedText in _localizedTexts)
                {
                    localizedText.AutoGenerate();
                }
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
