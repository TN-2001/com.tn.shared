#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.UI;

namespace Library.UI
{
    [CustomEditor(typeof(OptionSelector))]
    public class OptionSelectorEditor : SelectableEditor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.Space(10);

            serializedObject.Update();

            EditorGUILayout.PropertyField(serializedObject.FindProperty("optionText"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("leftButton"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("rightButton"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("options"), true);
            EditorGUILayout.Space(10);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("OnChanged"));

            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif
