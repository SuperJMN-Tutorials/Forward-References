namespace ForwardRefs.Test
{
    public class CallStatementSyntax : SyntaxStatement
    {
        public string ProcedureName { get; }

        public CallStatementSyntax(string procedureName)
        {
            ProcedureName = procedureName;
        }
    }
}