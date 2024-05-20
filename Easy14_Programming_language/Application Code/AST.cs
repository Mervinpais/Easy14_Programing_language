using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Easy14_Programming_Language
{
    public class AST
    {
        public enum NodeType
        {
            //Statements
            Program,
            VarDeclaration,

            //Expressions
            AssignmentExpr,

            //Literals
            Property,
            ObjectLiteral,
            NumericLiteral,
            Identifier,
            BinaryExpr,
        }


        public class Stmt
        {
            public virtual NodeType Kind { get; set; }
        }

        public class Program_ : Stmt
        {
            public override NodeType Kind => NodeType.Program;
            public List<Stmt> Body { get; set; }
        }


        public class VarDeclaration : Stmt
        {
            public override NodeType Kind => NodeType.VarDeclaration;
            public bool constant { get; set; }
            public string identifier { get; set; }
            public Expr? value { get; set; }
        }

        public class Expr : Stmt { }

        public class AssignmentExpr : Expr
        {
            public override NodeType Kind => NodeType.AssignmentExpr;
            public Expr assigne { get; set; }
            public Expr value { get; set; }
        }

        public class BinaryExpr : Expr
        {
            public override NodeType Kind => NodeType.BinaryExpr;
            public Expr left;
            public Expr right;
            public string @operator;
        }

        public class Identifier : Expr
        {
            public override NodeType Kind => NodeType.Identifier;
            public string symbol;
        }

        public class NumericLiteral : Expr
        {
            public override NodeType Kind => NodeType.NumericLiteral;
            public int value;
        }

        public class Property : Expr
        {
            public override NodeType Kind => NodeType.Property;
            public string key;
            public Expr value;
        }

        public class ObjectLiteral : Expr
        {
            public override NodeType Kind => NodeType.ObjectLiteral;
            public Property[] properties;
            
        }
    }
}
