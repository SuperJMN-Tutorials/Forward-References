namespace ForwardRefs.Test
{
    internal class CallStatement : BoundStatement
    {
        public BoundProcedure Procedure { get; }

        public CallStatement(BoundProcedure procedure)
        {
            Procedure = procedure;
        }
    }
}