namespace Easy14_Programming_Language.Application_Code
{
    public static class ComparisonInterperator
    {
        public static bool IsTrueCompare(string line)
        {
            line = line.Trim();
            string LHS = line.Split("==")[0].Trim();
            string RHS = line.Split("==")[1].Trim();
            if (LHS.Equals(RHS))
            {
                return true;
            }
            return false;
        }

        public static bool IsFalseCompare(string line)
        {
            line = line.Trim();
            string LHS = line.Split("==")[0].Trim();
            string RHS = line.Split("==")[1].Trim();
            if (!LHS.Equals(RHS))
            {
                return true;
            }
            return false;
        }
    }
}
