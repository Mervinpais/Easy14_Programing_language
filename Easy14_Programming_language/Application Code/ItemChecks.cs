/*
 * Long Story Short; This was originally from my other project "MentalCrash" a test to see if i can make a programming language that runs in one line, because im lazy to rewrite it, and so i used the MentalCrash "ItemChecks.cs" and am now using it here
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

namespace Easy14_Programming_Language //Mental Crash
{
    public static class ItemChecks
    {
        /// <summary>
        /// Detects the datatype of the variable; Possible return values are:
        /// <list type="">
        /// <item>- str: String</item>
        /// <item>- int: Integer type</item>
        /// <item>- double: Double type</item>
        /// <item>- cmd: Command type</item>
        /// <item>- bool: Boolean type</item>
        /// <item>- var: Variable type</item>
        /// </list>
        /// </summary>
        /// <param name="data">The input data to be analyzed.</param>
        /// <returns>A string indicating the detected data type. Possible values are "str", "int", "double", "cmd", "bool", or "var".</returns>
        public static string DetectType(string data)
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
        public static bool IsString(object? data)
        {
            try
            {
                if (data == null) return false;
            }
            catch { return false; }
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
            return data.ToString().StartsWith("() =>");
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
