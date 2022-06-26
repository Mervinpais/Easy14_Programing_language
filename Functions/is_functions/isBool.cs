using System;

namespace Easy14_Programming_Language
{
    class isBool
    {
        public bool Interperate(bool? boolean)
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
