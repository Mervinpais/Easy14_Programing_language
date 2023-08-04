using System;
using System.Collections.Generic;
using System.Linq;

namespace Easy14_Programming_Language.Application_Code
{
    public class CommandParser
    {

        public static (List<string> className, string methodName, List<string> paramItems) SplitCommand(string command)
        {
            Tuple<List<string>, string, List<string>> statementParts = null;
            statementParts = SplitCommandPart(command).ToTuple();
            for (int i = 0; i < statementParts.Item1.Count; i++)
            {
                string item = statementParts.Item1[i];
                Console.WriteLine($"Class {i + 1}: {item}");
            }
            Console.WriteLine("Method Name: " + statementParts.Item2);

            if (statementParts.Item3.Count > 0)
            {
                for (int i = 0; i < statementParts.Item3.Count; i++)
                {
                    Console.WriteLine($"Parameter {i}: [{ItemChecks.detectType(statementParts.Item3[i])}] {statementParts.Item3[i]}");
                }
            }

            return (statementParts.Item1, statementParts.Item2, statementParts.Item3);
        }

        private static (List<string> className, string methodName, List<string> params_) SplitCommandPart(string commandPart)
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
                    List<string> parameters = new List<string> { paramsPart };

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

    }
}
