namespace ForwardRefs.Test.Syntactic
{
    public class ProcedureSyntax
    {
        public string Name { get; }
        public StatementSyntax[] Statements { get; }

        public ProcedureSyntax(string name, StatementSyntax[] statements)
        {
            Name = name;
            Statements = statements;
        }
    }
}