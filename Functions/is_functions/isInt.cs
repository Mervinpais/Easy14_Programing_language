using System;

namespace Easy14_Programming_Language
{
    class isInt
    {
        public bool Interperate(int? integer)
        {
            if (integer is int)
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
