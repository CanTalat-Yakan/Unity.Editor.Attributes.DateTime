#if UNITY_EDITOR
using UnityEditor;using UnityEngine;

namespace UnityEssentials
{
    [CustomPropertyDrawer(typeof(TimeAttribute))]
    public class TimeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            Rect contentPosition = EditorGUI.PrefixLabel(position, label);
            int indentLevel = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            float spacing = 16;
            float labelWidth = EditorGUIUtility.labelWidth;
            float fieldWidth = labelWidth / 2.35f;
            float lineHeight = EditorGUIUtility.singleLineHeight;

            float fieldXPosition = 20 + labelWidth;

            EditorGUI.LabelField(new Rect(fieldXPosition, position.y, fieldWidth, lineHeight), "h");
            EditorGUI.PropertyField(new Rect(fieldXPosition + spacing, position.y, fieldWidth, lineHeight), property.FindPropertyRelative("x"), GUIContent.none);

            fieldXPosition += fieldWidth + 20;

            EditorGUI.LabelField(new Rect(fieldXPosition, position.y, fieldWidth, lineHeight), "m");
            EditorGUI.PropertyField(new Rect(fieldXPosition + spacing, position.y, fieldWidth, lineHeight), property.FindPropertyRelative("y"), GUIContent.none);

            EditorGUI.indentLevel = indentLevel;
            EditorGUI.EndProperty();
        }
    }
}
#endif