using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKimWebConsole.Infrastructure
{
    public static class ExtensionMethods
    {
        public static bool IsNotNullOrEmpty(this string inputtedString)
        {
            bool hasValue = false;
            if (!string.IsNullOrEmpty(inputtedString) && !string.IsNullOrWhiteSpace(inputtedString))
                hasValue = true;
            return hasValue;
        }

        public static bool IsNullOrEmpty(this string inputtedString)
        {
            bool hasValue = false;
            if (string.IsNullOrEmpty(inputtedString) || string.IsNullOrWhiteSpace(inputtedString))
                hasValue = true;
            return hasValue;
        }

        public static string StrToUpper(this string inputtedString)
        {
            if (!string.IsNullOrEmpty(inputtedString) && !string.IsNullOrWhiteSpace(inputtedString))
                inputtedString = inputtedString.Trim().ToUpper();
            return inputtedString;
        }

        public static string StrToLower(this string inputtedString)
        {
            if (!string.IsNullOrEmpty(inputtedString) && !string.IsNullOrWhiteSpace(inputtedString))
                inputtedString = inputtedString.Trim().ToLower();
            return inputtedString;
        }

        public static string ToEnumString(this Enum enumValue)
        {
            string value = string.Empty;
            if (enumValue != null && enumValue is Enum)
            {
                value = Convert.ToString(enumValue).Replace("_", " ");
            }
            return value;
        }

        public static string ToTrim(this string inputtedString)
        {
            if (inputtedString.Contains("\r") || inputtedString.Contains("\t") || inputtedString.Contains("\n"))
            {
                inputtedString = inputtedString.Replace("\n", "").Replace("\r", "").Replace("\t", "");
            }
            if (!string.IsNullOrEmpty(inputtedString) && !string.IsNullOrWhiteSpace(inputtedString))
            {
                inputtedString = inputtedString.Trim();
            }
            return inputtedString;
        }

        public static string ToDateString(this string inputtedValue)
        {
            string outputValue = string.Empty;
            if (inputtedValue.IsNotNullOrEmpty())
            {
                DateTime result = DateTime.MinValue;
                if (DateTime.TryParseExact(inputtedValue, "dd/MM/yyyy", new CultureInfo("en-IN"), DateTimeStyles.None, out result))
                {
                    outputValue = result.ToShortDateString();
                }
                else
                {
                    outputValue = inputtedValue;
                }
            }
            return outputValue;
        }

        public static decimal ToDecimal(this string input, decimal defaultValue = 0m)
        {
            if (string.IsNullOrWhiteSpace(input))
                return defaultValue;

            return decimal.TryParse(input, out var result) ? result : defaultValue;
        }

        public static int ToInt(this string input, int defaultValue = 0)
        {
            if (string.IsNullOrWhiteSpace(input))
                return defaultValue;

            return int.TryParse(input, out var result) ? result : defaultValue;
        }

    }
}
