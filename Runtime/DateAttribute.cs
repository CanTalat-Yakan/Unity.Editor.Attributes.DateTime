using UnityEngine;

namespace UnityEssentials
{
    /// <summary>
    /// Attribute for marking a property as a date value, with editable year, month, and day fields. 
    /// Apply to properties representing dates to provide a more user-friendly editing experience in custom editors.
    /// Draws a custom GUI for a <see cref="SerializedProperty"/> of type <see
    /// cref="SerializedPropertyType.Vector3Int"/> that represents a date using day, month, and year components.
    /// </summary>
    /// <remarks>This method renders a custom editor field for a <see cref="Vector3Int"/> property,
    /// interpreting its components as a date: <list type="bullet"> <item><description><c>x</c>: Represents the
    /// day.</description></item> <item><description><c>y</c>: Represents the month.</description></item>
    /// <item><description><c>z</c>: Represents the year.</description></item> </list> If the property is not of
    /// type <see cref="SerializedPropertyType.Vector3Int"/>, an error message is displayed instead.</remarks>
    /// <param name="position">The rectangle on the screen to use for the property GUI.</param>
    /// <param name="property">The serialized property to make the custom GUI for. Must be of type <see
    /// cref="SerializedPropertyType.Vector3Int"/>.</param>
    /// <param name="label">The label to display next to the property field in the Inspector.</param>
    public class DateAttribute : PropertyAttribute { }
}
