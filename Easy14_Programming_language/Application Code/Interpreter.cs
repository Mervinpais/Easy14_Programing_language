using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Easy14_Programming_Language.Values;
using static Easy14_Programming_Language.AST;
using static Easy14_Programming_Language.LanguageEnvironment;
using static Easy14_Programming_Language.Expressions;
using static Easy14_Programming_Language.Statements;
using Easy14_Programming_Language.Application_Code;
namespace Easy14_Programming_Language
{
    public class Interpreter
    {
        public static RuntimeVal evaluate(Stmt astNode, LanguageEnvironment env)
        {
            if (astNode == null)
            {
                return null;
            }
            switch (astNode.Kind)
            {
                case NodeType.NumericLiteral:
                    return new NumberVal(((NumericLiteral)astNode).value);
                case NodeType.Identifier:
                    return eval_identifier((Identifier)astNode, env);
                case NodeType.ObjectLiteral:
                    return eval_object_expr((ObjectLiteral)astNode, env);
                case NodeType.AssignmentExpr:
                    return eval_assignment((AssignmentExpr)astNode, env);
                case NodeType.BinaryExpr:
                    return evaluate_binary_expr((BinaryExpr)astNode, env);
                case NodeType.Program:
                    return eval_program((Program_)astNode, env);

                // Handle Statments
                case NodeType.VarDeclaration:
                    return eval_var_declaration((VarDeclaration)astNode, env);
                // Handle unimplemented ast types as error.
                default:
                    Console.WriteLine($"This AST Node has not yet been set up for interpretation. \'{astNode}\'");
                    Environment.Exit(0);
                    return null; // This return statement is just to satisfy C# compiler, but it won't be reached.
            }
        }
    }
}
