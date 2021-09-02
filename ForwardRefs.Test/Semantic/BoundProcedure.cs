using System.Collections.Generic;

namespace ForwardRefs.Test.Semantic
{
    public class BoundProcedure
    {
        public ICollection<BoundStatement> Statements { get; }

        public BoundProcedure(ICollection<BoundStatement> statements)
        {
            Statements = statements;
        }
    }
}