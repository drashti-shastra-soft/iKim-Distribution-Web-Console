using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IKimWebConsole.Infrastructure.Helper
{
    public static class EnumsHelper
    {
        public static List<string> GetEnumDisplayNames(Enum enm)
        {
            var type = enm.GetType();
            var displayNames = new List<string>();
            var names = Enum.GetNames(type);

            foreach (var name in names)
            {
                var field = type.GetField(name);
                var objArray = field.GetCustomAttributes(typeof(DisplayAttribute), true);
                if (objArray.Length == 0)
                    displayNames.Add(name);

                foreach (DisplayAttribute fd in objArray)
                    displayNames.Add(fd.Name);
            }
            return displayNames;
        }

        public static string GetDisplayValue(this Enum en)
        {
            string displayName = string.Empty;
            displayName = en.GetType()
                .GetMember(en.ToString())
                .FirstOrDefault()
                .GetCustomAttribute<DisplayAttribute>()?
                .GetName();
            if (displayName.IsNullOrEmpty())
            {
                displayName = en.ToString();
            }
            return displayName;
        }

    }
}
