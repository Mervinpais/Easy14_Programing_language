using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Easy14_Programming_Language;
using static Easy14_Programming_Language.AST;
using static Easy14_Programming_Language.Program;
using static Easy14_Programming_Language.Lexer;
using Easy14_Programming_Language.Application_Code;
using System.Security.Cryptography.Pkcs;


namespace Easy14_Programming_Language
{
    public class Parser
    {
        private List<Token> tokens = [];

        private bool not_eof()
        {
            return this.tokens[0].Type != TokenType.EOF;
        }

        private Token at()
        {
            return this.tokens[0] as Token;
        }

        private Token eat()
        {
            Token prev = at();
            tokens.RemoveAt(0);
            return prev;
        }

        private Token expect(TokenType type, object err)
        {
            Token prev = at();
            tokens.RemoveAt(0);
            if (prev == null || prev.Type != type)
            {
                Debugger.Error($"Parser Error:\n{err}\n Expected \'{type}\'");
                Environment.Exit(-1);
            }
            return prev;
        }

        public Program_ produceAST(string sourceCode)
        {
            Tokenizer tk = new Tokenizer();
            tokens = tk.Tokenize(sourceCode);
            Program_ program = new Program_
            {
                Kind = NodeType.Program,
                Body = new List<Stmt> { }
            };

            //Parse till EOF
            while (this.not_eof())
            {
                program.Body.Add(parse_stmt());
            }

            return program;
        }

        private Stmt parse_stmt()
        {
            switch (this.at().Type)
            {
                case TokenType.Var or TokenType.Const:
                    return this.parse_var_declaration();
                default:
                    return this.parse_expr();
            }
        }

        private Stmt parse_var_declaration()
        {
            bool isConstant = eat().Type == TokenType.Const;
            var identifier = this.expect(TokenType.Identifier, "Expected identifer name following var | const keywords").Value;
            
            if (this.at().Type == TokenType.SemiColon)
            {
                this.eat(); //expect semicolon
                if (isConstant)
                {
                    throw new Exception("Must assign value to constant expression. No value provided.");
                }

                return new VarDeclaration
                {
                    Kind = NodeType.VarDeclaration,
                    identifier = identifier,
                    constant = isConstant,
                    value = null
                };
            }

            this.expect(TokenType.Equals, "Expected equals token following identifier in var declaration");
            var declaration = new VarDeclaration
            {
                Kind = NodeType.VarDeclaration,
                value = this.parse_expr(),
                constant = isConstant,
                identifier = identifier,
            };

            this.expect(TokenType.SemiColon, "Variable Declaration statment must end with semicolon");
            return declaration;
        }

        private Expr parse_expr()
        {
            return parse_assignment_expr();
        }

        private Expr parse_assignment_expr()
        {
            var left = this.parse_object_expr(); //switch out with objectExpr

            if (this.at().Type == TokenType.Equals)
            {
                this.eat(); //advance past equal token
                var value = this.parse_assignment_expr();
                return new AssignmentExpr
                {
                    value = value,
                    assigne = left,
                    Kind = NodeType.AssignmentExpr
                };
            }

            return left;
        }

        private Expr parse_object_expr()
        {
            if (this.at().Type != TokenType.OpenBrace)
            {
                return this.parse_additive_expr();
            }

            this.eat();
            List<Property> properties = new List<Property>();
            
            while (this.not_eof() && this.at().Type != TokenType.CloseBrace)
            {
                var key = this.expect(TokenType.Identifier, "Object literal key expected").Value;

                // 
                if (this.at().Type == TokenType.Comma)
                {
                    this.eat();
                    properties.Add(new Property { key = key, value = null });
                    continue;
                }
                else if (this.at().Type == TokenType.CloseBrace)
                {
                    properties.Add(new Property { key = key, value = null });
                    continue;
                }

                this.expect(TokenType.Colon, "Missing colon following identifier in ObjectExpr");
                var value = this.parse_expr();

                properties.Add(new Property { key = key, value = value });
                if (this.at().Type == TokenType.CloseBrace)
                {
                    this.expect(TokenType.Comma, "Missing comma or closing bracket following property");
                }
            }

            this.expect(TokenType.CloseBrace, "Object literal missing closing brace");
            return new ObjectLiteral { properties = properties.ToArray() };

        }

        private Expr parse_additive_expr()
        {
            var left = this.parse_multiplicative_expr();

            while (this.at().Value == "+" || this.at().Value == "-")
            {
                var @operator = this.eat().Value;
                var right = this.parse_multiplicative_expr();
                left = new BinaryExpr
                {
                    Kind = NodeType.BinaryExpr,
                    left = left,
                    right = right,
                    @operator = @operator
                };
            }


            return left;
        }
        
        private Expr parse_multiplicative_expr()
        {
            var left = this.parse_primary_expr();

            while (this.at().Value == "/" || this.at().Value == "*" || this.at().Value == "%")
            {
                var @operator = this.eat().Value;
                var right = this.parse_primary_expr();
                left = new BinaryExpr
                {
                    Kind = NodeType.BinaryExpr,
                    left = left,
                    right = right,
                    @operator = @operator
                };
            }


            return left;
        }

        private Expr parse_primary_expr()
        {
            TokenType currentToken = this.at().Type;

            if (currentToken == TokenType.SemiColon && tokens[tokens.Count - 1].Type == TokenType.EOF)
            {
                this.eat();
                return null;
            }

            switch (currentToken)
            {
                case TokenType.Identifier:
                    return new Identifier
                    {
                        Kind = NodeType.Identifier,
                        symbol = eat().Value
                    };
                case TokenType.Number:
                    return new NumericLiteral
                    {
                        Kind = NodeType.Identifier,
                        value = int.Parse(eat().Value)
                    };
                case TokenType.OpenParen:
                    this.eat(); // eatin dat open tag
                    var value = this.parse_expr();
                    this.expect(TokenType.CloseParen, "Unexpected token inside parenthesised expression. Expected closing parenthesis"); // closing paren tag
                    return value;
                default:
                    // Muhehehe trick the compiler
                    Debugger.Error($"Unexpected Token during parsing \'{this.at().Value}\'");
                    Environment.Exit(-1);
                    return new Expr();
            }

        }
    }
}
