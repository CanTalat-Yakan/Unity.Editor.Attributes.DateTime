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
        /// <summary>
        /// Draws a custom GUI for properties with a <see cref="TimeAttribute"/> applied, supporting fields of type <see
        /// cref="SerializedPropertyType.Vector3Int"/> or <see cref="SerializedPropertyType.Float"/>.
        /// </summary>
        /// <remarks>This method renders a time picker interface, allowing users to edit hours, minutes,
        /// and seconds.  For <see cref="SerializedPropertyType.Vector3Int"/>, the X, Y, and Z components represent
        /// hours, minutes, and seconds, respectively. For <see cref="SerializedPropertyType.Float"/>, the value is
        /// interpreted as a fraction of a day, where 23.999722f represents 24 hours.  If the property type is unsupported, an
        /// error message is displayed instead of the custom GUI.</remarks>
        /// <param name="position">The rectangle on the screen to use for the property GUI.</param>
        /// <param name="property">The serialized property to make the custom GUI for. Must be of type <see
        /// cref="SerializedPropertyType.Vector3Int"/> or <see cref="SerializedPropertyType.Float"/>.</param>
        /// <param name="label">The label of the property being drawn.</param>
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType != SerializedPropertyType.Vector3Int && property.propertyType != SerializedPropertyType.Float)
            {
                EditorGUI.HelpBox(position, "TimeAttribute only supports Vector3Int or float fields.", MessageType.Error);
                return;
            }

            EditorGUI.BeginProperty(position, label, property);

            float labelWidth = EditorGUIUtility.labelWidth;
            float valueAreaWidth = position.width - labelWidth - 1;
            float fieldWidth = valueAreaWidth / 3f;
            float lineHeight = EditorGUIUtility.singleLineHeight;
            float fieldXPosition = 18 + labelWidth;

            var hourPosition = new Rect(fieldXPosition, position.y, fieldWidth, lineHeight);
            var minutePosition = new Rect(hourPosition.x + fieldWidth, position.y, fieldWidth, lineHeight);
            var secondPosition = new Rect(minutePosition.x + fieldWidth, position.y, fieldWidth, lineHeight);

            TimeContainer timeContainer;

            if (property.propertyType == SerializedPropertyType.Vector3Int)
            {
                var vectorValue = property.vector3IntValue;
                timeContainer = new TimeContainer
                {
                    Hour = (Hour)Mathf.Clamp(vectorValue.x, 0, 23),
                    Minute = (Minute)Mathf.Clamp(vectorValue.y, 0, 59),
                    Second = (Second)Mathf.Clamp(vectorValue.z, 0, 59)
                };

                EditorGUI.PrefixLabel(position, label);

                EnumDrawer.EnumPopup<Hour>(hourPosition, timeContainer.Hour, (newHour) => UpdatePropertyVector(property, timeContainer.UpdateHour(newHour)));
                EnumDrawer.EnumPopup<Minute>(minutePosition, timeContainer.Minute, (newMinute) => UpdatePropertyVector(property, timeContainer.UpdateMinute(newMinute)));
                EnumDrawer.EnumPopup<Second>(secondPosition, timeContainer.Second, (newSecond) => UpdatePropertyVector(property, timeContainer.UpdateSecond(newSecond)));
            }
            else if (property.propertyType == SerializedPropertyType.Float)
            {
                float floatValue = property.floatValue;
                int totalSeconds = Mathf.Clamp(Mathf.FloorToInt(floatValue * 3600), 0, 86399);

                int hour = totalSeconds / 3600;
                int minute = (totalSeconds % 3600) / 60;
                int second = totalSeconds % 60;

                timeContainer = new TimeContainer
                {
                    Hour = (Hour)hour,
                    Minute = (Minute)minute,
                    Second = (Second)second
                };

                EditorGUI.PrefixLabel(position, label);

                EnumDrawer.EnumPopup<Hour>(hourPosition, timeContainer.Hour, (newHour) => UpdatePropertyFloat(property, timeContainer.UpdateHour(newHour)));
                EnumDrawer.EnumPopup<Minute>(minutePosition, timeContainer.Minute, (newMinute) => UpdatePropertyFloat(property, timeContainer.UpdateMinute(newMinute)));
                EnumDrawer.EnumPopup<Second>(secondPosition, timeContainer.Second, (newSecond) => UpdatePropertyFloat(property, timeContainer.UpdateSecond(newSecond)));
            }

            EditorGUI.EndProperty();
        }

        /// <summary>
        /// Updates the specified serialized property with the time values from the provided <see
        /// cref="TimeContainer"/>.
        /// </summary>
        /// <remarks>The <paramref name="property"/> is updated with the hour, minute, and second values
        /// from the <paramref name="timeContainer"/> as a <see cref="Vector3Int"/>. The changes are applied to the
        /// serialized object after the update.</remarks>
        /// <param name="property">The serialized property to update. Must represent a <see cref="Vector3Int"/> value.</param>
        /// <param name="timeContainer">The <see cref="TimeContainer"/> containing the hour, minute, and second values to set.</param>
        private void UpdatePropertyVector(SerializedProperty property, TimeContainer timeContainer)
        {
            property.vector3IntValue = new Vector3Int((int)timeContainer.Hour, (int)timeContainer.Minute, (int)timeContainer.Second);
            property.serializedObject.ApplyModifiedProperties();
        }

        /// <summary>
        /// Updates the specified serialized property with a float value representing the time contained in the provided
        /// <see cref="TimeContainer"/>.
        /// </summary>
        /// <remarks>The float value is calculated as the sum of the hour, the minute divided by 60, and
        /// the second divided by 3600. The resulting value is clamped to ensure it remains within the valid range for a
        /// 24-hour time format.</remarks>
        /// <param name="property">The serialized property to update. The property's value will be set to a float representing the time in
        /// hours, clamped between 0 and 23.999722.</param>
        /// <param name="timeContainer">The <see cref="TimeContainer"/> instance containing the hour, minute, and second values used to calculate
        /// the property's new float value.</param>
        private void UpdatePropertyFloat(SerializedProperty property, TimeContainer timeContainer)
        {
            float newValue = ((int)timeContainer.Hour) + ((int)timeContainer.Minute / 60f) + ((int)timeContainer.Second / 3600f);

            property.floatValue = Mathf.Clamp(newValue, 0f, 23.999722f);
            property.serializedObject.ApplyModifiedProperties();
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
