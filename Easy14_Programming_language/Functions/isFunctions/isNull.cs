namespace Easy14_Programming_Language
{
    public static class isNull
    {
        public static bool Interperate(object null_item)
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
