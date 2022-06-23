using System;

namespace Easy14_Programming_Language
{
    class isBool
    {
        public bool interperate(bool? boolean)
        {
            if (boolean is bool)
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
