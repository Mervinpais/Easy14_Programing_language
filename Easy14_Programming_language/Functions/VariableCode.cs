using System.Collections.Generic;

namespace Easy14_Programming_Language
{
    public static class VariableCode
    {
        public static Dictionary<object, object> variableList = new Dictionary<object, object>();

        public static object Interperate(object name, object value = null, bool setVariable = false)
        {
            if (setVariable)
            {
                if (value.ToString().StartsWith("() =>"))
                {
                    value = value.ToString().Substring("() =>".Length).Trim();
                    if (!value.ToString().EndsWith(";")) value = value.ToString() + ";";
                    variableList[name] = Program.CompileCode(new string[] { value.ToString() }); ;
                }
                else
                {
                    variableList[name] = value;
                }
                return null;
            }
            else
            {
                if (variableList.TryGetValue(name, out var storedValue))
                {
                    return storedValue;
                }
                else
                {
                    ErrorReportor.ConsoleLineReporter.Error($"Variable \'{name}\' does not exist!");
                    return null;
                }
            }
        }
    }
}
