using Laputa.Localization.Components;
using UnityEditor;
using UnityEngine;

namespace Laputa.Localization.Editor
{
    [CustomEditor(typeof(LocalizedDropdown))]
    public class LocalizedDropdownEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var localizedDropdown = (LocalizedDropdown) target;
 
            if(GUILayout.Button("Update Data", GUILayout.Height(40)))
            {
                localizedDropdown.UpdateOptions();
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
