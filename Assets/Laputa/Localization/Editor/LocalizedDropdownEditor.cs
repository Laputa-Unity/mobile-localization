using System.Linq;
using Laputa.Localization.Components;
using UnityEditor;
using UnityEngine;

namespace Laputa.Localization.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(LocalizedDropdown))]
    public class LocalizedDropdownEditor : UnityEditor.Editor
    {
        private LocalizedDropdown[] _localizedDropdowns;

        private void OnEnable()
        {
            _localizedDropdowns = targets.Cast<LocalizedDropdown>().ToArray();
        }
        
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if(GUILayout.Button("Update Data", GUILayout.Height(40)))
            {
                foreach (var localizedDropdown in _localizedDropdowns)
                {
                    localizedDropdown.UpdateOptions();
                }
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
