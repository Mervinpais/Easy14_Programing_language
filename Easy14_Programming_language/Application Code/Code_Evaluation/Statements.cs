using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Easy14_Programming_Language.AST;
using static Easy14_Programming_Language.Values;
using static Easy14_Programming_Language.Interpreter;
using static Easy14_Programming_Language.LanguageEnvironment;

namespace Easy14_Programming_Language
{
    public class Statements
    {
        public static RuntimeVal eval_program(Program_ program, LanguageEnvironment env)
        {
            RuntimeVal lastEvaluated = MK_NULL();

            foreach (var statement in program.Body)
            {
                lastEvaluated = evaluate(statement, env);
            }

            return lastEvaluated;
        }

        public static RuntimeVal eval_var_declaration(VarDeclaration declaration, LanguageEnvironment env)
        {
            var value = declaration.value != null ? evaluate(declaration.value, env) : MK_NULL();
            
            return env.declareVal(declaration.identifier, value, declaration.constant);
        }
    }
}
