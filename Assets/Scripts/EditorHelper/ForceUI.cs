#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CustomUIComponent), true)]
public class ForceUI : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        CustomUIComponent script = (CustomUIComponent)target;

        if (GUILayout.Button("Refresh UI"))
        {
            script.Init();
            EditorUtility.SetDirty(script);
        }
    }
}
#endif