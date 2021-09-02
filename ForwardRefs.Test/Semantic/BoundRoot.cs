using System.Collections.Generic;

namespace ForwardRefs.Test.Semantic
{
    public class BoundRoot
    {
        public IEnumerable<BoundProcedure> Procedures { get; }

        public BoundRoot(ICollection<BoundProcedure> procedures)
        {
            Procedures = procedures;
        }
    }
}