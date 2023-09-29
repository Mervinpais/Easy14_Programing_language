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

            if (ItemChecks.DetectType(LHS.ToString()) == "var")
            {
                if (VariableCode.variables.Keys.Any(varName => varName.Equals(LHS)))
                {
                    VariableCode.variables.TryGetValue(LHS.ToString(), out var LHS_Var_value);
                    LHS = LHS_Var_value;
                }
            }
            if (ItemChecks.DetectType(RHS.ToString()) == "var")
            {
                if (VariableCode.variables.Keys.Any(varName => varName.Equals(RHS)))
                {
                    VariableCode.variables.TryGetValue(RHS.ToString(), out var RHS_Var_value);
                    RHS = RHS_Var_value;
                }
            }

            if (ItemChecks.DetectType(LHS.ToString()) == "string")
            {
                LHS = LHS.ToString().Substring(1, LHS.ToString().Length - 2);
            }
            if (ItemChecks.DetectType(RHS.ToString()) == "string")
            {
                RHS = RHS.ToString().Substring(1, RHS.ToString().Length - 2);
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

            if (ItemChecks.DetectType(LHS.ToString()) == "var")
            {
                if (VariableCode.variables.Keys.Any(varName => varName.Equals(LHS)))
                {
                    VariableCode.variables.TryGetValue(LHS.ToString(), out var LHS_Var_value);
                    LHS = LHS_Var_value;
                }
            }
            if (ItemChecks.DetectType(RHS.ToString()) == "var")
            {
                if (VariableCode.variables.Keys.Any(varName => varName.Equals(RHS)))
                {
                    VariableCode.variables.TryGetValue(RHS.ToString(), out var RHS_Var_value);
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
