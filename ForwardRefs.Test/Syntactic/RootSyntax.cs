namespace ForwardRefs.Test.Syntactic
{
    public class RootSyntax
    {
        public ProcedureSyntax[] Procedures { get; }

        public RootSyntax(ProcedureSyntax[] procedures)
        {
            Procedures = procedures;
        }
    }
}