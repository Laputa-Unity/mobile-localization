using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LocalizedDropdown))]
public class LocalizedDropdownEditor : Editor
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
