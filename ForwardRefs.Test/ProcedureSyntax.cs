namespace ForwardRefs.Test
{
    public class ProcedureSyntax
    {
        public string Name { get; }
        public SyntaxStatement[] Statements { get; }

        public ProcedureSyntax(string name, SyntaxStatement[] statements)
        {
            Name = name;
            Statements = statements;
        }
    }
}