#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;

namespace UnityEssentials
{
    /// <summary>
    /// Provides a custom property drawer for fields marked with the <see cref="TimeAttribute"/> attribute.
    /// </summary>
    /// <remarks>This drawer supports fields of type <see cref="SerializedPropertyType.Vector3Int"/> and  <see
    /// cref="SerializedPropertyType.Float"/>. It displays the time as separate hour, minute, and second fields in the
    /// Unity Inspector, allowing users to edit time values in a structured format.</remarks>
    [CustomPropertyDrawer(typeof(TimeAttribute))]
    public class TimeDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            // Two lines: enums + slider, plus spacing
            return EditorGUIUtility.singleLineHeight * 2 + EditorGUIUtility.standardVerticalSpacing;
        }
        /// <summary>
        /// Draws a custom GUI for properties with a <see cref="TimeAttribute"/> applied, supporting fields of type <see
        /// cref="SerializedPropertyType.Vector3Int"/> or <see cref="SerializedPropertyType.Float"/>.
        /// </summary>
        /// <remarks>This method renders a time picker interface, allowing users to edit hours, minutes,
        /// and seconds.  For <see cref="SerializedPropertyType.Vector3Int"/>, the X, Y, and Z components represent
        /// hours, minutes, and seconds, respectively. For <see cref="SerializedPropertyType.Float"/>, the value is
        /// interpreted as a fraction of a day, where 23.999722f represents 24 hours.  If the property type is unsupported, an
        /// error message is displayed instead of the custom GUI.</remarks>
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType != SerializedPropertyType.Float)
            {
                EditorGUI.HelpBox(position, "TimeAttribute only supports float fields.", MessageType.Error);
                return;
            }

            EditorGUI.BeginProperty(position, label, property);

            var labelWidth = EditorGUIUtility.labelWidth;
            var valueAreaWidth = position.width - labelWidth - 1;
            var fieldWidth = valueAreaWidth / 3f;
            var lineHeight = EditorGUIUtility.singleLineHeight;
            var spacing = EditorGUIUtility.standardVerticalSpacing;

            var enumRect = new Rect(position.x, position.y, position.width, lineHeight);
            var fieldXPosition = 18 + labelWidth;
            var hourPosition = new Rect(fieldXPosition, enumRect.y, fieldWidth, lineHeight);
            var minutePosition = new Rect(hourPosition.x + fieldWidth, enumRect.y, fieldWidth, lineHeight);
            var secondPosition = new Rect(minutePosition.x + fieldWidth, enumRect.y, fieldWidth, lineHeight);

            var floatValue = property.floatValue;
            var totalSeconds = Mathf.Clamp(Mathf.FloorToInt(floatValue * 3600), 0, 86399);

            EditorGUI.PrefixLabel(position, label);

            TimeContainer timeContainer = new TimeContainer
            {
                Hour = (Hour)(totalSeconds / 3600),
                Minute = (Minute)((totalSeconds % 3600) / 60),
                Second = (Second)(totalSeconds % 60)
            };

            void UpdatePropertyFloat(SerializedProperty property, TimeContainer timeContainer)
            {
                float newValue = ((int)timeContainer.Hour) + ((int)timeContainer.Minute / 60f) + ((int)timeContainer.Second / 3600f);

                property.floatValue = Mathf.Clamp(newValue, 0f, 24f);
                property.serializedObject.ApplyModifiedProperties();
            }
            EnumDrawer.EnumPopup<Hour>(hourPosition, timeContainer.Hour, (newHour) => UpdatePropertyFloat(property, timeContainer.UpdateHour(newHour)));
            EnumDrawer.EnumPopup<Minute>(minutePosition, timeContainer.Minute, (newMinute) => UpdatePropertyFloat(property, timeContainer.UpdateMinute(newMinute)));
            EnumDrawer.EnumPopup<Second>(secondPosition, timeContainer.Second, (newSecond) => UpdatePropertyFloat(property, timeContainer.UpdateSecond(newSecond)));

            var sliderRect = new Rect(position.x + labelWidth, position.y + lineHeight + spacing, position.width - labelWidth - 3, lineHeight);
            property.floatValue = EditorGUI.Slider(sliderRect, GUIContent.none, property.floatValue, 0f, 24f);
            property.serializedObject.ApplyModifiedProperties();

            EditorGUI.EndProperty();
        }
    }

    [Serializable]
    public struct TimeContainer
    {
        public Hour Hour;
        public Minute Minute;
        public Second Second;

        public TimeContainer UpdateHour(Hour newHour)
        {
            Hour = newHour;
            return this;
        }

        public TimeContainer UpdateMinute(Minute newMinute)
        {
            Minute = newMinute;
            return this;
        }

        public TimeContainer UpdateSecond(Second newSecond)
        {
            Second = newSecond;
            return this;
        }

        public override string ToString() =>
            $"{Hour:D2}:{Minute:D2}:{Second:D2}";
    }

    public enum Hour
    {
        _0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12,
        _13, _14, _15, _16, _17, _18, _19, _20, _21, _22, _23
    }

    public enum Minute
    {
        _0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15,
        _16, _17, _18, _19, _20, _21, _22, _23, _24, _25, _26, _27, _28, _29,
        _30, _31, _32, _33, _34, _35, _36, _37, _38, _39, _40, _41, _42, _43,
        _44, _45, _46, _47, _48, _49, _50, _51, _52, _53, _54, _55, _56, _57,
        _58, _59
    }

    public enum Second
    {
        _0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15,
        _16, _17, _18, _19, _20, _21, _22, _23, _24, _25, _26, _27, _28, _29,
        _30, _31, _32, _33, _34, _35, _36, _37, _38, _39, _40, _41, _42, _43,
        _44, _45, _46, _47, _48, _49, _50, _51, _52, _53, _54, _55, _56, _57,
        _58, _59
    }
}
#endif
