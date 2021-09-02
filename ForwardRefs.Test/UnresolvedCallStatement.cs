namespace ForwardRefs.Test
{
    internal class UnresolvedCallStatement : BoundStatement
    {
        public string ProcedureName { get; }

        public UnresolvedCallStatement(string procedureName)
        {
            ProcedureName = procedureName;
        }
    }
}