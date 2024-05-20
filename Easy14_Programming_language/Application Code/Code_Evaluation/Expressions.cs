using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Easy14_Programming_Language.AST;
using static Easy14_Programming_Language.Values;
using static Easy14_Programming_Language.Interpreter;
using System.Windows.Forms;

namespace Easy14_Programming_Language
{
    public class Expressions
    {
        public static NumberVal evaluate_numeric_binary_expr(NumberVal lhs, NumberVal rhs, string @operator)
        {
            var result = 0;
            if (@operator == "+")
            {
                result = lhs.Value + rhs.Value;
            }
            else if (@operator == "-")
            {
                result = (int)lhs.Value - (int)rhs.Value;
            }
            else if (@operator == "*")
            {
                result = (int)lhs.Value * (int)rhs.Value;
            }
            else if (@operator == "/")
            {
                result = (int)lhs.Value / (int)rhs.Value; //error by 0 check needed
            }
            else if (@operator == "%")
            {
                result = (int)lhs.Value % (int)rhs.Value;
            }

            return new NumberVal(result);
        }

        public static RuntimeVal evaluate_binary_expr(BinaryExpr binop, LanguageEnvironment env)
        {
            var lhs = evaluate(binop.left, env);
            var rhs = evaluate(binop.right, env);

            if (lhs.Type == Values.ValueType.Number && rhs.Type == Values.ValueType.Number)
            {
                return evaluate_numeric_binary_expr((NumberVal)lhs, (NumberVal)rhs, binop.@operator);
            }

            return MK_NULL();

        }

        public static RuntimeVal eval_identifier(Identifier ident, LanguageEnvironment env)
        {
            var val = env.lookupVar(ident.symbol);
            return val;
        }

        public static RuntimeVal eval_assignment (AssignmentExpr node, LanguageEnvironment env)
        {
            if (node.assigne.Kind != NodeType.Identifier)
            {
                throw new Exception($"Invalid LHS inaide assignment expr {node.assigne}");
            }

            var varname = (node.assigne as Identifier).symbol;

            return env.assignVar(varname, evaluate(node.value, env));
        }

        public static RuntimeVal eval_object_expr (ObjectLiteral obj, LanguageEnvironment env)
        {
            var @object = new ObjectVal { properties = new Dictionary<string, RuntimeVal>() };

            foreach (var property in obj.properties)
            {
                var key = property.key;
                var value = property.value;

                var runtimeVal = (value == null)
                    ? env.lookupVar(key)
                    : evaluate(value, env);

                // Check if the key already exists in the dictionary
                if (!@object.properties.ContainsKey(key))
                {
                    // Add the key-value pair if it doesn't exist
                    @object.properties.Add(key, runtimeVal);
                }
                else
                {
                    // Update the value if the key already exists
                    @object.properties[key] = runtimeVal;
                }
            }

            return @object;
        }
    }
}
