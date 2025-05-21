#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace UnityEssentials
{
    [CustomPropertyDrawer(typeof(DateAttribute))]
    public class DateDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType != SerializedPropertyType.Vector3)
            {
                EditorGUI.HelpBox(position, "DateAttribute attribute only supports Vector3 fields.", MessageType.Error);
                return;
            }

            EditorGUI.BeginProperty(position, label, property);

            // Get current values from Vector3
            var vectorValue = property.vector3Value;
            var dateContainer = new DateContainer
            {
                Day = (Day)(vectorValue.x == 0 ? 1 : vectorValue.x),
                Month = (Month)(vectorValue.y == 0 ? 1 : vectorValue.y),
                Year = (Year)(vectorValue.z == 0 ? 1 : vectorValue.z)
            };

            var contentPosition = EditorGUI.PrefixLabel(position, label);
            int indentLevel = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            float spacing = 16;
            float labelWidth = EditorGUIUtility.labelWidth;
            float valueAreaWidth = position.width - EditorGUIUtility.labelWidth - spacing * 3 - 3 * 3 - 1;
            float fieldWidth = valueAreaWidth / 3f;
            float lineHeight = EditorGUIUtility.singleLineHeight;
            float fieldXPosition = 20 + labelWidth;

            // Day field (D)
            EditorGUI.LabelField(new Rect(fieldXPosition, position.y, fieldWidth, lineHeight), "D");
            var dayPosition = new Rect(fieldXPosition + spacing, position.y, fieldWidth, lineHeight);
            dateContainer.Day = (Day)EditorGUI.EnumPopup(dayPosition, dateContainer.Day);
            fieldXPosition += fieldWidth + 20;

            // Month field (M)
            EditorGUI.LabelField(new Rect(fieldXPosition, position.y, fieldWidth, lineHeight), "M");
            var monthPosition = new Rect(fieldXPosition + spacing, position.y, fieldWidth, lineHeight);
            dateContainer.Month = (Month)EditorGUI.EnumPopup(monthPosition, dateContainer.Month);
            fieldXPosition += fieldWidth + 20;

            // Year field (Y)
            EditorGUI.LabelField(new Rect(fieldXPosition, position.y, fieldWidth, lineHeight), "Y");
            var yearPosition = new Rect(fieldXPosition + spacing, position.y, fieldWidth, lineHeight);
            dateContainer.Year = (Year)EditorGUI.EnumPopup(yearPosition, dateContainer.Year);

            // Validate day based on month and year
            int yearInt = GetYearFromEnum(dateContainer.Year);
            int maxDay = GetMaxDay(dateContainer.Month, yearInt);
            int currentDay = (int)dateContainer.Day;

            if (currentDay > maxDay)
                dateContainer.Day = (Day)maxDay;

            property.vector3Value = new Vector3(
                (float)dateContainer.Day,
                (float)dateContainer.Month,
                (float)dateContainer.Year);

            EditorGUI.indentLevel = indentLevel;
            EditorGUI.EndProperty();
        }

        private int GetYearFromEnum(Year yearEnum)
        {
            string yearString = yearEnum.ToString().Substring(1);
            return int.Parse(yearString);
        }

        private int GetMaxDay(Month month, int year) =>
            month switch
            {
                Month.Jan or Month.Mar or Month.May or Month.Jul or Month.Aug or Month.Oct or Month.Dec => 31,
                Month.Apr or Month.Jun or Month.Sep or Month.Nov => 30,
                Month.Feb => IsLeapYear(year) ? 29 : 28,
                _ => 31,
            };

        private bool IsLeapYear(int year)
        {
            if (year % 4 != 0)
                return false;
            else if (year % 100 != 0)
                return true;
            else return year % 400 == 0;
        }
    }

    [System.Serializable]
    public struct DateContainer
    {
        public Month Month;
        public Day Day;
        public Year Year;
    }

    public enum Month { Jan = 1, Feb, Mar, Apr, May, Jun, Jul, Aug, Sep, Oct, Nov, Dec }
    public enum Day { D1 = 1, D2, D3, D4, D5, D6, D7, D8, D9, D10, D11, D12, D13, D14, D15, D16, D17, D18, D19, D20, D21, D22, D23, D24, D25, D26, D27, D28, D29, D30, D31 }
    public enum Year
    {
        Y2100, Y2099, Y2098, Y2097, Y2096, Y2095, Y2094, Y2093, Y2092, Y2091,
        Y2090, Y2089, Y2088, Y2087, Y2086, Y2085, Y2084, Y2083, Y2082, Y2081,
        Y2080, Y2079, Y2078, Y2077, Y2076, Y2075, Y2074, Y2073, Y2072, Y2071,
        Y2070, Y2069, Y2068, Y2067, Y2066, Y2065, Y2064, Y2063, Y2062, Y2061,
        Y2060, Y2059, Y2058, Y2057, Y2056, Y2055, Y2054, Y2053, Y2052, Y2051,
        Y2050, Y2049, Y2048, Y2047, Y2046, Y2045, Y2044, Y2043, Y2042, Y2041,
        Y2040, Y2039, Y2038, Y2037, Y2036, Y2035, Y2034, Y2033, Y2032, Y2031,
        Y2030, Y2029, Y2028, Y2027, Y2026, Y2025, Y2024, Y2023, Y2022, Y2021,
        Y2020, Y2019, Y2018, Y2017, Y2016, Y2015, Y2014, Y2013, Y2012, Y2011,
        Y2010, Y2009, Y2008, Y2007, Y2006, Y2005, Y2004, Y2003, Y2002, Y2001,
        Y2000, Y1999, Y1998, Y1997, Y1996, Y1995, Y1994, Y1993, Y1992, Y1991,
        Y1990, Y1989, Y1988, Y1987, Y1986, Y1985, Y1984, Y1983, Y1982, Y1981,
        Y1980, Y1979, Y1978, Y1977, Y1976, Y1975, Y1974, Y1973, Y1972, Y1971,
        Y1970, Y1969, Y1968, Y1967, Y1966, Y1965, Y1964, Y1963, Y1962, Y1961,
        Y1960, Y1959, Y1958, Y1957, Y1956, Y1955, Y1954, Y1953, Y1952, Y1951,
        Y1950, Y1949, Y1948, Y1947, Y1946, Y1945, Y1944, Y1943, Y1942, Y1941,
        Y1940, Y1939, Y1938, Y1937, Y1936, Y1935, Y1934, Y1933, Y1932, Y1931,
        Y1930, Y1929, Y1928, Y1927, Y1926, Y1925, Y1924, Y1923, Y1922, Y1921,
        Y1920, Y1919, Y1918, Y1917, Y1916, Y1915, Y1914, Y1913, Y1912, Y1911,
        Y1910, Y1909, Y1908, Y1907, Y1906, Y1905, Y1904, Y1903, Y1902, Y1901,
        Y1900
    }
}
#endif