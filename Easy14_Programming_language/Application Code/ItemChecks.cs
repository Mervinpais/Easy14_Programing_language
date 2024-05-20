using System;
using System.Collections.Generic;
using System.Linq;

namespace Easy14_Programming_Language
{
    public static class ItemChecks
    {
        public static string DetectType(string data)
        {
            if (IsString(data))
                return "str";
            else if (IsInt(data))
                return "int";
            else if (IsDouble(data))
                return "double";
            else if (IsCommand(data))
                return "cmd";
            else if (IsBoolean(data))
                return "bool";
            else if (IsVariable(data, VariableCode.variables))
                return "var";

            return "unkwn";
        }

        public static bool IsString(string data)
        {
            return data.StartsWith("\"") && data.EndsWith("\"") && !data.Substring(1, data.Length - 2).Contains("\"");
        }

        public static bool IsCommand(string data)
        {
            return data.StartsWith("() =>");
        }

        public static bool IsBoolean(string data)
        {
            return bool.TryParse(data, out _);
        }

        public static bool IsVariable(string data, List<VariableCode.Variable> variables)
        {
            return variables.FirstOrDefault(item => item.Name == data) != null;
        }

        public static bool IsInt(string data)
        {
            return int.TryParse(data, out _);
        }

        public static bool IsDouble(string data)
        {
            return double.TryParse(data, out _);
        }
    }
}
