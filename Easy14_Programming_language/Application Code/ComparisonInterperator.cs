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

            //if (ItemChecks.detectType(LHS.ToString()) != ItemChecks.detectType(RHS.ToString())) return false;

            if (ItemChecks.detectType(LHS.ToString()) == "var")
            {
                if (VariableCode.variableList.Keys.Any(varName => varName.Equals(LHS)))
                {
                    VariableCode.variableList.TryGetValue(LHS, out var LHS_Var_value);
                    LHS = LHS_Var_value;
                }
            }
            if (ItemChecks.detectType(RHS.ToString()) == "var")
            {
                if (VariableCode.variableList.Keys.Any(varName => varName.Equals(RHS)))
                {
                    VariableCode.variableList.TryGetValue(RHS, out var RHS_Var_value);
                    RHS = RHS_Var_value;
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

            //if (ItemChecks.detectType(LHS.ToString()) != ItemChecks.detectType(RHS.ToString())) return false;

            if (ItemChecks.detectType(LHS.ToString()) == "var")
            {
                if (VariableCode.variableList.Keys.Any(varName => varName.Equals(LHS)))
                {
                    VariableCode.variableList.TryGetValue(LHS, out var LHS_Var_value);
                    LHS = LHS_Var_value;
                }
            }
            if (ItemChecks.detectType(RHS.ToString()) == "var")
            {
                if (VariableCode.variableList.Keys.Any(varName => varName.Equals(RHS)))
                {
                    VariableCode.variableList.TryGetValue(RHS, out var RHS_Var_value);
                    RHS = RHS_Var_value;
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
