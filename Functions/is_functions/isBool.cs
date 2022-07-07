namespace Easy14_Programming_Language
{
    public static class isBool
    {
        public static bool Interperate(bool? boolean)
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
