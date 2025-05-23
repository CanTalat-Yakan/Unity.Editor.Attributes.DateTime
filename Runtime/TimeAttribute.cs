using UnityEngine;

namespace UnityEssentials
{
    /// <summary>
    /// Attribute for marking a property as a time value with editable hours, minutes, and seconds fields. 
    /// Apply to float or Vector3Int properties representing time in seconds to provide a more user-friendly editing experience.
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
    public class TimeAttribute : PropertyAttribute { }
}
