using System.Collections.Generic;

namespace Easy14_Programming_Language
{
    public static class Method_Code_old
    {
        public static void Interperate_old(int currentLine, List<string> lines, bool newMethod = false)
        {
            List<string> methodBlock = new List<string>(lines);

            int indention = 0;
            foreach (char c in methodBlock[currentLine])
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

            methodBlock.RemoveRange(0, currentLine);

            int endBlock = 0;

            for (int i = 0; i < methodBlock.Count; i++)
            {
                if (methodBlock[i] == indent + "end")
                {
                    endBlock = i;
                    break;
                }
            }

            methodBlock.RemoveRange(endBlock, methodBlock.Count - endBlock);
            List<string> methodBlockUntrimmed = new List<string>(methodBlock);
            methodBlock.Clear();

            foreach (string line in methodBlockUntrimmed)
            {
                methodBlock.Add(line.Trim());
            }

            string methodHeaderLine = methodBlock[0].Substring(2);

            methodBlock.RemoveAt(0);

            // Process the method block here

            List<string> lines_ = new List<string>(lines);
            lines_.RemoveRange(0, currentLine);
            lines_.RemoveRange(0, methodBlock.Count + 2);
            lines = lines_;
        }
    }
}