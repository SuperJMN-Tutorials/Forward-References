using System.Collections.Generic;

namespace ForwardRefs.Test
{
    public class BoundProcedure
    {
        public IEnumerable<BoundStatement> Statements { get; }

        public BoundProcedure(IEnumerable<BoundStatement> statements)
        {
            Statements = statements;
        }
    }
}