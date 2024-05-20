using Easy14_Programming_Language.Application_Code;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Easy14_Programming_Language
{
    public static class CommentCode
    {
        public static string[] Interperate(int currentLine, List<string> lines)
        {
            List<string> commentCode = new List<string>(lines);
            commentCode.RemoveRange(0, currentLine);

            int endBlock = 0;

            for (int i = 0; i < commentCode.Count; i++)
            {
                if (commentCode[i].EndsWith("*/"))
                {
                    endBlock = i;
                    break;
                }
            }

            commentCode.RemoveRange(endBlock, commentCode.Count - endBlock);
            List<string> ifBlockUntrimmed = new List<string>(commentCode);
            commentCode.Clear();

            foreach (string line in ifBlockUntrimmed)
            {
                commentCode.Add(line.Trim());
            }

            commentCode.RemoveAt(0);

            List<string> lines_ = new List<string>(lines);
            lines_.RemoveRange(0, currentLine);
            lines_.RemoveRange(0, commentCode.Count + 2);
            lines = lines_;
            return lines.ToArray();
        }
    }
}