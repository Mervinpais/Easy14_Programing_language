namespace Easy14_Programming_Language
{
    public static class isNull
    {
        public static bool Interperate(string? null_item)
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
