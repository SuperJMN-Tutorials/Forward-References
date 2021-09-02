using System;

namespace ForwardRefs.Test.Syntactic
{
    public class ProcedureSyntax
    {
        public string Name { get; }
        public StatementSyntax[] Statements { get; }

        public ProcedureSyntax(string name) : this(name, Array.Empty<StatementSyntax>())
        {
        }

        public ProcedureSyntax(string name, StatementSyntax[] statements)
        {
            Name = name;
            Statements = statements;
        }
    }
}