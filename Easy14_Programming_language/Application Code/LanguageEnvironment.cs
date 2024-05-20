using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Easy14_Programming_Language;
using static Easy14_Programming_Language.Values;

namespace Easy14_Programming_Language
{
    public class LanguageEnvironment
    {
        public static LanguageEnvironment setupGlobalEnv()
        {
            var env = new LanguageEnvironment();
            env.declareVal("true", MK_BOOL(true), true);
            env.declareVal("false", MK_BOOL(false), true);
            env.declareVal("null", MK_NULL(), true);

            return env;
        }

        private LanguageEnvironment? parent;
        private Dictionary<string, RuntimeVal> variables;
        private List<string> constants;

        public LanguageEnvironment(LanguageEnvironment? parentENV = null) {
            bool global = parentENV != null ? true : false;
            this.parent = parentENV;
            this.variables = new Dictionary<string, RuntimeVal>();
            this.constants = new List<string>();

            if (global)
            {
                setupGlobalEnv();
            }
        }

        public RuntimeVal declareVal (string varname, RuntimeVal value, bool constant = false)
        {
            if (this.variables.ContainsKey(varname))
            {
                throw new Exception($"Can not declare variable {varname} as it is already defined");
            }

            this.variables.Add(varname, value);

            if (constant)
            {
                this.constants.Add(varname);
            }

            return value;
        }

        public RuntimeVal assignVar (string varname, RuntimeVal value)
        {
            var env = this.resolve(varname);

            //Cannot assign to constant
            if (env.constants.Contains(varname))
            {
                throw new Exception($"Cannot reassign to variable \'{varname}\' as it was declared constant");
            }
            if (env.variables.ContainsKey(varname))
            {
                env.variables.Remove(varname);
            }
            env.variables.Add(varname, value);

            return value;
        }

        public RuntimeVal lookupVar (string varname)
        {
            var env = this.resolve(varname);
            return env.variables[varname];
        }

        public LanguageEnvironment resolve (string varname)
        {
            try
            {
                if (this.variables[varname] != null)
                {
                    return this;
                }
            }
            catch
            {

            }
            if (this.parent == null)
            {
                throw new Exception($"Cannot resolve \'{varname}\' as it does not exist");
            }

            return this.parent.resolve(varname);
        }
    }
}
