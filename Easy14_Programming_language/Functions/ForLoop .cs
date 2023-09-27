using Easy14_Programming_Language.Application_Code;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Easy14_Programming_Language
{
    public static class ForLoop
    {
        public static string[] Interperate(int currentLine, List<string> lines)
        {
            List<string> ifBlock = new List<string>(lines);

            int indention = 0;
            foreach (char c in ifBlock[currentLine])
            {
                if (c == ' ')
                {
                    indention = indention + 1;
                }
                else
                {
                    break;
                }
            }

            string indent = "";
            for (int i = 0; i < indention; i++)
            {
                indent += " ";
            }

            ifBlock.RemoveRange(0, currentLine);

            int endBlock = 0;

            for (int i = 0; i < ifBlock.Count; i++)
            {
                if (ifBlock[i] == indent + "end")
                {
                    endBlock = i;
                    break;
                }
            }

            ifBlock.RemoveRange(endBlock, ifBlock.Count - endBlock);
            List<string> ifBlockUntrimmed = new List<string>(ifBlock);
            ifBlock.Clear();

            foreach (string line in ifBlockUntrimmed)
            {
                ifBlock.Add(line.Trim());
            }

            string ifLine = ifBlock[0].Substring(3);

            ifBlock.RemoveAt(0);

            for (int i = 0; i < Convert.ToInt32(ifLine); i++)
            {
                Program.CompileCode(ifBlock.ToArray());
            }

            List<string> lines_ = new List<string>(lines);
            lines_.RemoveRange(0, currentLine);
            lines_.RemoveRange(0, ifBlock.Count + 2);
            lines = lines_;
            return lines.ToArray();
        }
    }
}