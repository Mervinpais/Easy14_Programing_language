using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Easy14_Programming_Language;

namespace Easy14_Programming_Language
{
    public class Lexer
    {
        public enum TokenType
        {
            Number,
            Identifier,

            //Key words
            Var,
            Const,

            // Groups n Operators
            Equals,
            Comma,
            Colon,
            OpenBrace, // {
            CloseBrace, // }
            SemiColon,
            OpenParen, // (
            CloseParen, // )
            BinaryOperator,
            EOF, //End of file
        }

        public class Token
        {
            public string Value { get; set; }
            public TokenType Type { get; set; }

            public Token(string value, TokenType tag)
            {
                Value = value;
                Type = tag;
            }
        }

        static List<string> SplitByComma(string input)
        {
            // Use regex to match commas outside quotes
            string pattern = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";
            string[] result = Regex.Split(input, pattern);

            // Trim spaces from each element
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = result[i].Trim();
            }

            // Convert the string array to an object array
            List<string> objectArray = result.ToList();

            return objectArray;
        }

        public class Tokenizer
        {
            string identifierPattern = @"[a-zA-Z_]\w*";
            string methodsPattern = @"(.*?\ )([A-Za-z]+)\ \{(.*?)\};";
            string numberPattern = @"\d+";
            string operatorPattern = @"\+|-|\*|/";

            public Dictionary<string, TokenType> keywords = new()
            {
                { "var", TokenType.Var },
                { "const", TokenType.Const },
            };

            public bool isAlpha(string src)
            {
                return src.ToUpper() != src.ToLower();
            }

            public bool isInt(string src)
            {
                char c = src[0];
                int unicode_c = (int)c;
                int[] bounds = [(int)'0', (int)'9'];
                return (c >= bounds[0] && c <= bounds[1]);
            }

            public bool isSkippable(string src)
            {
                return (src == " " || src == "\n" || src == "\t" || src=="\r");
            }

            public List<Token> Tokenize(string sourceCode)
            {
                List<Token> tokens = new();

                List<char> src = sourceCode.ToCharArray().ToList();

                while (src.Count > 0)
                {
                    if (src[0] == '(')
                    {
                        tokens.Add(new Token(src[0].ToString(), TokenType.OpenParen));
                        src.RemoveAt(0);
                    }
                    else if (src[0] == ')')
                    {
                        tokens.Add(new Token(src[0].ToString(), TokenType.CloseParen));
                        src.RemoveAt(0);
                    }
                    else if (src[0] == '{')
                    {
                        tokens.Add(new Token(src[0].ToString(), TokenType.OpenBrace));
                        src.RemoveAt(0);
                    }
                    else if (src[0] == '}')
                    {
                        tokens.Add(new Token(src[0].ToString(), TokenType.CloseBrace));
                        src.RemoveAt(0);
                    }
                    else if (src[0] == '+' || src[0] == '-' || src[0] == '*' || src[0] == '/' || src[0] == '%')
                    {
                        tokens.Add(new Token(src[0].ToString(), TokenType.BinaryOperator));
                        src.RemoveAt(0);
                    }
                    else if (src[0] == '=')
                    {
                        tokens.Add(new Token(src[0].ToString(), TokenType.Equals));
                        src.RemoveAt(0);
                    }
                    else if (src[0] == ';')
                    {
                        tokens.Add(new Token(src[0].ToString(), TokenType.SemiColon));
                        src.RemoveAt(0);
                    }
                    else if (src[0] == ':')
                    {
                        tokens.Add(new Token(src[0].ToString(), TokenType.Colon));
                        src.RemoveAt(0);
                    }
                    else if (src[0] == ',')
                    {
                        tokens.Add(new Token(src[0].ToString(), TokenType.Comma));
                        src.RemoveAt(0);
                    }
                    else
                    {
                        if (isInt(src[0].ToString()))
                        {
                            var num = "";
                            while (src.Count > 0 && isInt(src[0].ToString()))
                            {
                                num += src[0];
                                src.RemoveAt(0);
                            }

                            tokens.Add(new Token(num, TokenType.Number));
                        }
                        else if (isAlpha(src[0].ToString()))
                        {
                            var ident = ""; //foo let
                            while (src.Count > 0 && isAlpha(src[0].ToString()))
                            {
                                ident += src[0];
                                src.RemoveAt(0);
                            }

                            //check keyword
                            TokenType reserved;
                            keywords.TryGetValue(ident, out reserved);
                            if (keywords.TryGetValue(ident, out reserved))
                            {
                                tokens.Add(new Token(ident, reserved));
                            }
                            else
                            {
                                tokens.Add(new Token(ident, TokenType.Identifier));
                            }
                        }
                        else if (isSkippable(src[0].ToString()))
                        {
                            src.RemoveAt(0);
                        }
                        else
                        {
                            Debug.WriteLine($"Unrecognised Char found \'{src[0]}\'");
                            Environment.Exit(0);
                        }
                    }
                }

                tokens.Add(new Token("EndOfFile", TokenType.EOF));

                return tokens;
            }

            private TokenType DetermineTag(string value)
            {
                if (Regex.IsMatch(value, identifierPattern))
                    return TokenType.Identifier;

                else if (Regex.IsMatch(value, numberPattern))
                    return TokenType.Number;

                else
                    return TokenType.Identifier;
            }
        }
    }
}
