namespace Easy14_Programming_Language
{
    public static class isString
    {
        public static bool Interperate(string str)
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
