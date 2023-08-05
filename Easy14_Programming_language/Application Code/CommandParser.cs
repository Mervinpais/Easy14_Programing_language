﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Easy14_Programming_Language.Application_Code
{
    public class CommandParser
    {
        public static (List<string> className, string methodName, List<string> paramItems) SplitCommand(string command)
        {
            var statementParts = SplitCommandPart(command);
            for (int i = 0; i < statementParts.className.Count; i++)
            {
                string item = statementParts.className[i];
                Console.WriteLine($"Class {i + 1}: {item}");
            }
            Console.WriteLine("Method Name: " + statementParts.methodName);

            if (statementParts.paramItems.Count > 0)
            {
                for (int i = 0; i < statementParts.paramItems.Count; i++)
                {
                    Console.WriteLine($"Parameter {i}: [{ItemChecks.detectType(statementParts.paramItems[i])}] {statementParts.paramItems[i]}");
                }
            }

            return (statementParts.className, statementParts.methodName, statementParts.paramItems);
        }

        private static (List<string> className, string methodName, List<string> paramItems) SplitCommandPart(string commandPart)
        {
            if (commandPart.EndsWith(");"))
            {
                int openParenIndex = commandPart.IndexOf('(');
                int closeParenIndex = commandPart.LastIndexOf(')');

                if (openParenIndex > 0 && closeParenIndex == commandPart.Length - 2)
                {
                    string classAndMethodPart = commandPart.Substring(0, openParenIndex).Trim();
                    List<string> parts = classAndMethodPart.Split('.').ToList();
                    string methodName = parts[parts.Count - 1];
                    parts.RemoveAt(parts.Count - 1);
                    string className = string.Join(".", parts);

                    string paramsPart = commandPart.Substring(openParenIndex + 1, closeParenIndex - openParenIndex - 1).Trim();
                    List<string> parameters = SplitParameters(paramsPart);

                    return (new List<string> { className }, methodName, parameters);
                }
                else
                {
                    // Invalid syntax, parenthesis not found or misplaced
                    Console.WriteLine("Invalid command syntax: " + commandPart);
                }
            }
            else
            {
                // Invalid syntax, command should end with ');'
                Console.WriteLine("Invalid command syntax: " + commandPart);
            }

            return (new List<string>() { "" }, "", new List<string>() { "" });
        }

        private static List<string> SplitParameters(string paramsPart)
        {
            List<string> parameters = new List<string>();
            int start = 0;
            int nestingLevel = 0;

            for (int i = 0; i < paramsPart.Length; i++)
            {
                char c = paramsPart[i];
                if (c == ',' && nestingLevel == 0)
                {
                    parameters.Add(paramsPart.Substring(start, i - start).Trim());
                    start = i + 1;
                }
                else if (c == '(')
                {
                    nestingLevel++;
                }
                else if (c == ')')
                {
                    nestingLevel--;
                }
            }

            // Add the last parameter
            parameters.Add(paramsPart.Substring(start).Trim());

            return parameters;
        }
    }
}
