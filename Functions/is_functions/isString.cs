﻿using System;

namespace Easy14_Programming_Language
{
    class isString
    {
        public bool interperate(string str)
        {
            if (str is string)
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