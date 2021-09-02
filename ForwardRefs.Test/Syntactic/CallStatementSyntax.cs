namespace ForwardRefs.Test.Syntactic
{
    public class CallStatementSyntax : StatementSyntax
    {
        public string ProcedureName { get; }

        public CallStatementSyntax(string procedureName)
        {
            ProcedureName = procedureName;
        }
    }
}