namespace Easy14_Programming_Language
{
    public static class isInt
    {
        public static bool Interperate(object integer)
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
