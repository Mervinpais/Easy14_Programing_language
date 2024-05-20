using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Easy14_Programming_Language
{
    public class Values
    {
        public enum ValueType
        {
            Null,
            Number,
            Boolean,
        }

        public class RuntimeVal
        {
            public virtual ValueType Type { get; set; }
        }

        public class NullVal : RuntimeVal
        {
            public new ValueType Type { get; set; } = ValueType.Null;
            public object Value { get; set; } = null;
        }

        public static NullVal MK_NULL()
        {
            return new NullVal { Type = ValueType.Null, Value = null };
        }

        public class BooleanVal : RuntimeVal
        {
            public new ValueType Type { get; set; } = ValueType.Boolean;
            public bool Value { get; set; }
            public BooleanVal(bool b)
            {
                this.Value = b;
            }
        }

        public static BooleanVal MK_BOOL(bool b)
        {
            return new BooleanVal(b);
        }

        public class NumberVal : RuntimeVal
        {
            public override ValueType Type => ValueType.Number;
            public int Value { get; set; }

            public NumberVal(int value)
            {
                this.Value = value;
            }
        }

        public static NumberVal MK_NUMBER(int n)
        {
            return new NumberVal(n);
        }

        public class ObjectVal : RuntimeVal
        {
            public override ValueType Type => ValueType.Number;
            public Dictionary<string, RuntimeVal> properties;
        }
    }
}
