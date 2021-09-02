using System.Collections.Generic;

namespace ForwardRefs.Test.Semantic
{
    public class BoundRoot
    {
        public IEnumerable<BoundProcedure> Procedures { get; }

        public BoundRoot(IEnumerable<BoundProcedure> procedures)
        {
            Procedures = procedures;
        }
    }
}