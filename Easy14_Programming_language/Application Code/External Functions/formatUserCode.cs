using System.Collections.Generic;

namespace Easy14_Programming_Language
{
    public static class formatUserCode
    {
        public static string[] format(string[] lines)
        {
            List<string> lines_list = new List<string>(lines);
            List<string> lines_list_mod = new List<string>();
            int linesListMod_LineCounter = 0;
            foreach (string item_ in lines_list)
            {
                string itemToBeAdded = item_.TrimStart();
                if (itemToBeAdded.Contains(";"))
                {
                    string[] itemsTobeAdded = itemToBeAdded.Split(";");
                    foreach (string item in itemsTobeAdded)
                    {
                        if (item.TrimStart() + ";" != ";") lines_list_mod.Add(item.TrimStart() + ";");
                    }
                    continue;
                }
                if (itemToBeAdded.Contains("//")) itemToBeAdded = itemToBeAdded.Substring(0, itemToBeAdded.IndexOf("//"));
                lines_list_mod.Add(itemToBeAdded);
                linesListMod_LineCounter++;
            }
            return lines_list_mod.ToArray();
        }
    }
}
