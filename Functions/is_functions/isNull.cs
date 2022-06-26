using System;

namespace Easy14_Programming_Language
{
    class isNull
    {
        public bool Interperate(string? null_item)
        {
            if (null_item is null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
