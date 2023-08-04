using Easy14_Programming_Language.Application_Code;
using System;
using System.Collections.Generic;

namespace Easy14_Programming_Language
{
    public static class If_Loop
    {
        public static void Interperate(int currentLine, string[] lines)
        {
            currentLine = currentLine - 1;
            if (lines == null || lines == new string[] { })
            {
                return;
            }
            int endingBracketPos = 0;
            for (int counter = 0; counter < lines.Length; counter++)
            {
                if (currentLine > counter)
                {
                    continue;
                }
                string line = lines[counter];
                if (line == "}")
                {
                    endingBracketPos = counter;
                    break;
                }
            }
            endingBracketPos = endingBracketPos + 1;
            List<string> ifStatementCode = new(lines[currentLine..endingBracketPos]);
            List<string> ifTrueCode = new(lines[(currentLine + 1)..(endingBracketPos - 1)]);
            string ifStatement = ifStatementCode[0];
            string mainComparison = ifStatement.Substring(2, ifStatement.Length - 3).Trim(); // if and '{' are removed

            if (!(mainComparison.StartsWith("(") && mainComparison.EndsWith(")"))) return;

            ifStatement = ifStatement.Substring(1, ifStatement.Length - 2).Trim(); // remove brackets

            bool isTrue = false;

            if (ifStatement.IndexOf("==") == -1) isTrue = ComparisonInterperator.IsTrueCompare(ifStatement);
            if (ifStatement.IndexOf("!=") == -1) isTrue = ComparisonInterperator.IsFalseCompare(ifStatement);

            if (isTrue)
            {
                Program prog = new Program();
                prog.ExternalComplieCode(textArray: new string[] { string.Join(Environment.NewLine, ifTrueCode.ToArray()) });
            }
            //Console.WriteLine(string.Join(Environment.NewLine, ifStatementCode.ToArray()));
        }
    }
}