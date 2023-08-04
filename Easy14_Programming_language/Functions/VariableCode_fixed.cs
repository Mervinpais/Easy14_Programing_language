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
                variableList.Add(name, value);
                return null;
            }
            else
            {
                return variableList.TryGetValue(name, out _);
            }
            return null;
        }
    }
}