using Easy14_Programming_Language.Application_Code;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Easy14_Programming_Language
{
    public static class IfLoop
    {
        public static string[] Interperate(int currentLine, List<string> lines)
        {
            List<string> mainCodeBlock = new List<string>();
            lines.RemoveAt(0);
            lines.RemoveRange(0, currentLine - 1);

            for (int i = 0; i < lines.Count; i++)
            {
                if (i == 0)
                {
                    mainCodeBlock.Add(lines[i].Substring(2).TrimStart());
                    continue;
                }
                mainCodeBlock.Add(lines[i].TrimStart());
                if (lines[i] == "end")
                {
                    break;
                }
            }

            if (mainCodeBlock[0].Contains("=="))
            {
                if (ComparisonInterperator.IsTrueCompare(mainCodeBlock[0]))
                    Program.CompileCode(mainCodeBlock[1..].ToArray());
            }
            else if (mainCodeBlock[0].Contains("!="))
            {
                if (ComparisonInterperator.IsFalseCompare(mainCodeBlock[0]))
                    Program.CompileCode(mainCodeBlock[1..].ToArray());
            }
            lines.RemoveRange(0, mainCodeBlock.Count);
            
            return lines.ToArray();
        }
    }
}