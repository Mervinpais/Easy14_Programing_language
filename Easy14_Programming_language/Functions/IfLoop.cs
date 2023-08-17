using Easy14_Programming_Language.Application_Code;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Easy14_Programming_Language
{
    public static class If_Loop
    {
        public static string[] Interperate(int currentLine, string[] lines)
        {
            List<string> ifBlock = new List<string>(lines);
            ifBlock.RemoveRange(0, currentLine);
            int endBlock = 0;
            for (int i = 0; i < ifBlock.Count; i++)
            {
                if (ifBlock[i] == "end")
                {
                    endBlock = i;
                }    
            }
            ifBlock.RemoveRange(endBlock, ifBlock.Count - endBlock);
            List<string> ifBlockUntrimmed = new List<string>(ifBlock);
            ifBlock.Clear();
            foreach (string line in ifBlockUntrimmed)
            {
                ifBlock.Add(line.Trim());
            }
            string ifLine = ifBlock[0];
            ifBlock.RemoveAt(0);
            ifLine = ifLine.Substring(2).Trim();
            if (ifLine.Contains("==")) if (ComparisonInterperator.IsTrueCompare(ifLine)) Program.CompileCode(ifBlock.ToArray());
            else if (ifLine.Contains("!=")) if (ComparisonInterperator.IsFalseCompare(ifLine)) Program.CompileCode(ifBlock.ToArray());
            List<string> lines_ = new List<string>(lines);
            lines_.RemoveRange(0, ifBlock.Count + currentLine);
            lines = lines_.ToArray();
            return lines;
        }
    }
}