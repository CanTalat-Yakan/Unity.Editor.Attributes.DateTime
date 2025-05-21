#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;

namespace UnityEssentials
{
    [CustomPropertyDrawer(typeof(DateAttribute))]
    public class DateDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType != SerializedPropertyType.Vector3Int)
            {
                EditorGUI.HelpBox(position, "DateAttribute attribute only supports Vector3Int fields.", MessageType.Error);
                return;
            }

            EditorGUI.BeginProperty(position, label, property);

            // Get current values from Vector3Int
            var vectorValue = property.vector3IntValue;
            var dateContainer = new DateContainer
            {
                Day = (Day)(vectorValue.x == 0 ? 1 : vectorValue.x),
                Month = (Month)(vectorValue.y == 0 ? 1 : vectorValue.y),
                Year = (Year)(vectorValue.z == 0 ? 1 : vectorValue.z)
            };

            var contentPosition = EditorGUI.PrefixLabel(position, label);
            int indentLevel = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            float labelWidth = EditorGUIUtility.labelWidth;
            float valueAreaWidth = position.width - EditorGUIUtility.labelWidth - 1;
            float fieldWidth = valueAreaWidth / 3f;
            float lineHeight = EditorGUIUtility.singleLineHeight;
            float fieldXPosition = 18 + labelWidth;

            var dayPosition = new Rect(fieldXPosition, position.y, fieldWidth, lineHeight);
            var monthPosition = new Rect(dayPosition.x + fieldWidth, position.y, fieldWidth, lineHeight);
            var yearPositon = new Rect(monthPosition.x + fieldWidth, position.y, fieldWidth, lineHeight);

            EnumDrawer.EnumPopup<Day>(dayPosition, dateContainer.Day, (newDay) => UpdateProperty(property, dateContainer.UpdateDay(newDay)));
            EnumDrawer.EnumPopup<Month>(monthPosition, dateContainer.Month, (newMonth) => UpdateProperty(property, dateContainer.UpdateMonth(newMonth)));
            EnumDrawer.EnumPopup<Year>(yearPositon, dateContainer.Year, (newYear) => UpdateProperty(property, dateContainer.UpdateYear(newYear)));

            EditorGUI.indentLevel = indentLevel;
            EditorGUI.EndProperty();
        }

        private void UpdateProperty(SerializedProperty property, DateContainer dateContainer)
        {
            int yearInt = GetYearFromEnum(dateContainer.Year);
            int maxDay = GetMaxDay(dateContainer.Month, yearInt);
            if ((int)dateContainer.Day > maxDay)
                dateContainer.Day = (Day)maxDay;

            property.vector3IntValue = new Vector3Int(
                (int)dateContainer.Day,
                (int)dateContainer.Month,
                (int)dateContainer.Year);

            Debug.Log(dateContainer);
        }

        private int GetYearFromEnum(Year YearEnum)
        {
            string yearString = YearEnum.ToString().Substring(1);
            return int.Parse(yearString);
        }

        private int GetMaxDay(Month month, int year) =>
            month switch
            {
                Month.January or Month.March or Month.May or Month.July or Month.August or Month.October or Month.December => 31,
                Month.April or Month.June or Month.September or Month.November => 30,
                Month.Febuary => IsLeapYear(year) ? 29 : 28,
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

    [Serializable]
    public struct DateContainer
    {
        public Month Month;
        public Day Day;
        public Year Year;

        public DateContainer UpdateDay(Day newDay)
        {
            Day = newDay;
            return this;
        }

        public DateContainer UpdateMonth(Month newMonth)
        {
            Month = newMonth;
            return this;
        }

        public DateContainer UpdateYear(Year newYear)
        {
            Year = newYear;
            return this;
        }

        public override string ToString()
        {
            return $"{Day} {Month} {Year}";
        }
    }

    public enum Month { January = 1, Febuary, March, April, May, June, July, August, September, October, November, December }
    public enum Day { _1 = 1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, _16, _17, _18, _19, _20, _21, _22, _23, _24, _25, _26, _27, _28, _29, _30, _31 }
    public enum Year
    {
        _2100, _2099, _2098, _2097, _2096, _2095, _2094, _2093, _2092, _2091,
        _2090, _2089, _2088, _2087, _2086, _2085, _2084, _2083, _2082, _2081,
        _2080, _2079, _2078, _2077, _2076, _2075, _2074, _2073, _2072, _2071,
        _2070, _2069, _2068, _2067, _2066, _2065, _2064, _2063, _2062, _2061,
        _2060, _2059, _2058, _2057, _2056, _2055, _2054, _2053, _2052, _2051,
        _2050, _2049, _2048, _2047, _2046, _2045, _2044, _2043, _2042, _2041,
        _2040, _2039, _2038, _2037, _2036, _2035, _2034, _2033, _2032, _2031,
        _2030, _2029, _2028, _2027, _2026, _2025, _2024, _2023, _2022, _2021,
        _2020, _2019, _2018, _2017, _2016, _2015, _2014, _2013, _2012, _2011,
        _2010, _2009, _2008, _2007, _2006, _2005, _2004, _2003, _2002, _2001,
        _2000, _1999, _1998, _1997, _1996, _1995, _1994, _1993, _1992, _1991,
        _1990, _1989, _1988, _1987, _1986, _1985, _1984, _1983, _1982, _1981,
        _1980, _1979, _1978, _1977, _1976, _1975, _1974, _1973, _1972, _1971,
        _1970, _1969, _1968, _1967, _1966, _1965, _1964, _1963, _1962, _1961,
        _1960, _1959, _1958, _1957, _1956, _1955, _1954, _1953, _1952, _1951,
        _1950, _1949, _1948, _1947, _1946, _1945, _1944, _1943, _1942, _1941,
        _1940, _1939, _1938, _1937, _1936, _1935, _1934, _1933, _1932, _1931,
        _1930, _1929, _1928, _1927, _1926, _1925, _1924, _1923, _1922, _1921,
        _1920, _1919, _1918, _1917, _1916, _1915, _1914, _1913, _1912, _1911,
        _1910, _1909, _1908, _1907, _1906, _1905, _1904, _1903, _1902, _1901,
        _1900
    }
}
#endif