using System;
using Antlr4.Runtime;

namespace TinyLanguage
{
    class Program
    {
        static void Main(string[] args)
        {
            var lexer = new TinyLanguageLexer(new AntlrFileStream("code.txt"));
            var parser = new TinyLanguageParser(new CommonTokenStream(lexer));
            var tree = parser.program();
            var visitor = new TinyLanguageVisitor();
            visitor.Visit(tree);
        }
    }
}
