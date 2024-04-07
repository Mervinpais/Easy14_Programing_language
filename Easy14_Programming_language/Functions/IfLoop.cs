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

            //List<string> ifBlock = new List<string>();
            //foreach (string code in lines)
            //{
            //    string r = code;
            //    if (code.Contains('\t'))
            //    {
            //        r = code.Replace("\t", "   ");
            //    }
            //    ifBlock.Add(r);
            //}

            //int indention = 0;
            //foreach (char c in ifBlock[currentLine])
            //{
            //    if (c == ' ') indention = indention++;
            //    else break;
            //}

            //string indent = "";
            //for (int i = 0; i < indention; i++)
            //{
            //    indent += " ";
            //}

            //ifBlock.RemoveRange(0, currentLine);

            //int endBlock = 0;

            //for (int i = 0; i < ifBlock.Count; i++)
            //{
            //    if (ifBlock[i] == indent + "end")
            //    {
            //        endBlock = i;
            //        break;
            //    }
            //}

            //ifBlock.RemoveRange(endBlock, ifBlock.Count - endBlock);
            //List<string> ifBlockUntrimmed = new List<string>(ifBlock);
            //ifBlock.Clear();

            //foreach (string line in ifBlockUntrimmed)
            //{
            //    ifBlock.Add(line.Trim());
            //}

            //string ifLine = ifBlock[0].Substring(2);

            //ifBlock.RemoveAt(0);

            //if (ifLine.Contains("=="))
            //{
            //    if (ComparisonInterperator.IsTrueCompare(ifLine))
            //        Program.CompileCode(ifBlock.ToArray());
            //}
            //else if (ifLine.Contains("!="))
            //{
            //    if (ComparisonInterperator.IsFalseCompare(ifLine))
            //        Program.CompileCode(ifBlock.ToArray());
            //}

            //List<string> lines_ = new List<string>(lines);
            //lines_.RemoveRange(0, currentLine);
            //lines_.RemoveRange(0, ifBlock.Count + 2);
            //lines = lines_;

            return lines.ToArray();
        }
    }
}