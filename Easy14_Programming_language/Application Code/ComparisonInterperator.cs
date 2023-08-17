using System.Linq;

namespace Easy14_Programming_Language
{
    public static class ComparisonInterperator
    {
        public static bool IsTrueCompare(string line)
        {
            line = line.Trim();
            object LHS = line.Split("==")[0].Trim();
            object RHS = line.Split("==")[1].Trim();

            if (ItemChecks.detectType(LHS.ToString()) != ItemChecks.detectType(RHS.ToString())) return false;

            if (ItemChecks.detectType(LHS.ToString()) == "var")
            {
                if (VariableCode.variableList.Any(varName => varName.Equals(LHS)))
                {
                    LHS = VariableCode.variableList.TryGetValue(LHS, out LHS);
                }
            }
            if (ItemChecks.detectType(RHS.ToString()) == "var")
            {
                if (VariableCode.variableList.Any(varName => varName.Equals(RHS)))
                {
                    RHS = VariableCode.variableList.TryGetValue(RHS, out RHS);
                }
            }
            if (LHS.Equals(RHS))
            {
                return true;
            }
            return false;
        }

        public static bool IsFalseCompare(string line)
        {
            line = line.Trim();
            object LHS = line.Split("!=")[0].Trim();
            object RHS = line.Split("!=")[1].Trim();

            if (ItemChecks.detectType(LHS.ToString()) != ItemChecks.detectType(RHS.ToString())) return false;

            if (ItemChecks.detectType(LHS.ToString()) == "var")
            {
                if (VariableCode.variableList.Any(varName => varName.Equals(LHS)))
                {
                    LHS = VariableCode.variableList.TryGetValue(LHS, out LHS);
                }
            }
            if (ItemChecks.detectType(RHS.ToString()) == "var")
            {
                if (VariableCode.variableList.Any(varName => varName.Equals(RHS)))
                {
                    RHS = VariableCode.variableList.TryGetValue(RHS, out RHS);
                }
            }

            if (!LHS.Equals(RHS))
            {
                return true;
            }
            return false;
        }
    }
}
