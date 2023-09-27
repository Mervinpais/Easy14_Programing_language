using System;
using System.Collections.Generic;

namespace Easy14_Programming_Language
{
    public static class MethodCode
    {
        public static Dictionary<object, object> methodList = new Dictionary<object, object>();

        public static object Interperate(object name, object value = null, bool setVariable = false)
        {
            if (setVariable)
            {
                if (value != null)
                {
                    if (value.ToString().StartsWith("() =>"))
                    {
                        value = value.ToString().Substring("() =>".Length).Trim();
                        if (!value.ToString().EndsWith(";")) value = value.ToString() + ";";
                        methodList[name] = Program.CompileCode(new string[] { value.ToString() }); ;
                    }
                    else
                    {
                        if (ItemChecks.DetectType(value.ToString()) != "var")
                        {
                            methodList[name] = value;
                        }
                        else
                        {
                            methodList[name] = methodList[value];
                        }
                    }
                }
                else
                {
                    methodList[name] = "";
                }
                return null;
            }
            else
            {
                if (methodList.TryGetValue(name, out var storedValue))
                {
                    return Program.CompileCode(storedValue.ToString().Split(Environment.NewLine));
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