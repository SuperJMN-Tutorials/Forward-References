namespace ForwardRefs.Test.Semantic
{
    internal class BoundCallStatement : BoundStatement
    {
        public BoundProcedure Procedure { get; }

        public BoundCallStatement(BoundProcedure procedure)
        {
            Procedure = procedure;
        }
    }
}