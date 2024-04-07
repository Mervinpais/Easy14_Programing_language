using System.Linq;
using static Easy14_Programming_Language.VariableCode;

namespace Easy14_Programming_Language
{
    public static class ComparisonInterperator
    {
        public static (object, object) ConvertToLHS_RHS(string line)
        {
            line = line.Trim();
            object LHS = "";
            object RHS = "";
            if (line.Contains("=="))
            {
                LHS = line.Split("==")[0].Trim();
                RHS = line.Split("==")[1].Trim();
            }
            else if (line.Contains("!="))
            {
                LHS = line.Split("!=")[0].Trim();
                RHS = line.Split("!=")[1].Trim();
            }

            if (ItemChecks.DetectType(LHS.ToString()) == "var")
            {
                LHS = VariableCode.ReturnString((string)LHS);
            }
            if (ItemChecks.DetectType(RHS.ToString()) == "var")
            {
                Variable variable = null;
                foreach (var item in VariableCode.variables)
                {
                    if (item.Name == (string)RHS)
                    {
                        variable = item;
                        break;
                    }
                }

                if (variable != null)
                {
                    var RHS_Var_value = VariableCode.variables.FirstOrDefault(v => v.Name == RHS.ToString())?.Contents;
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

            return (LHS, RHS);
        }



        public static bool IsTrueCompare(string line)
        {
            object LHS = ConvertToLHS_RHS(line).Item1;
            object RHS = ConvertToLHS_RHS(line).Item2;

            if (LHS.Equals(RHS))
            {
                return true;
            }
            return false;
        }

        public static bool IsFalseCompare(string line)
        {
            object LHS = ConvertToLHS_RHS(line).Item1;
            object RHS = ConvertToLHS_RHS(line).Item2;

            if (!LHS.Equals(RHS))
            {
                return true;
            }
            return false;
        }
    }
}
