namespace ForwardRefs.Test
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