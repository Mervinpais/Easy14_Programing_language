/*
 * Long Story Short; This was originally from my other project "MentalCrash" a test to see if i can make a programming language that runs in one line, because im lazy to rewrite it, and so i used the MentalCrash "ItemChecks.cs" and am now using it here
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Easy14_Programming_Language //Mental Crash
{
    public static class ItemChecks
    {
        public static string detectType(string data)
        {
            if (IsString(data) == true)
            {
                return "str";
            }
            else if (IsInt(data) == true)
            {
                return "int";
            }
            else if (IsDouble(data) == true)
            {
                return "double";
            }
            else if (IsCommand(data) == true)
            {
                return "cmd";
            }
            else if (IsBoolean(data) == true)
            {
                return "bool";
            }
            else if (IsVariable(data, VariableCode.variableList.Keys.ToList())) {
                return "var";
            }
            //IsVariable(data);
            return "";
        }
        public static bool IsString(object data)
        {
            try
            {
                string firstElement = data.ToString();
                bool startsWithQuote = firstElement.StartsWith("\"");
                bool endsWithQuote = firstElement.EndsWith("\"");
                bool containsEscapedQuotes = false;
                if (firstElement.Length > 2)
                {
                    string substring = firstElement.Substring(1, firstElement.Length - 2);
                    containsEscapedQuotes = substring.Contains("\"");
                }

                if (startsWithQuote && endsWithQuote && !containsEscapedQuotes)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }
        }
        public static bool IsCommand(object data)
        {
            return data.ToString().StartsWith(":") && !(data.ToString().StartsWith(": "));
        }
        public static bool IsBoolean(object data)
        {
            return bool.TryParse((string?)data, out _);
        }
        public static bool IsVariable(object data, List<object> variables)
        {
            var foundItem = variables.FirstOrDefault(item => item.ToString().StartsWith(data.ToString()));
            if (foundItem != null)
            {
                return true;
            }
            return false;
        }
        public static bool IsInt(object data)
        {
            try
            {
                if (int.TryParse(data.ToString(), out _) == true)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
        public static bool IsDouble(string data)
        {
            try
            {
                if (double.TryParse(data, out _) == true)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
