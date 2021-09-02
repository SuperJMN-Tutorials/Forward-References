using System;
using System.Collections.Generic;
using System.Linq;
using ForwardRefs.Test.Semantic;

namespace ForwardRefs.Test
{
    public static class BoundTreeExtensions
    {
        public static string AsString(this BoundRoot actualAst)
        {
            return string.Concat(actualAst.Procedures.Select((procedure, i) => AsString(procedure, i, actualAst.Procedures.ToList())));
        }

        private static string AsString(BoundProcedure procedure, int i, IList<BoundProcedure> actualAstProcedures)
        {
            return $"#{i + 1}{{" + string.Concat(procedure.Statements.Select(statement => AsString(statement, actualAstProcedures))) + "}";
        }

        private static string AsString(BoundStatement statement, IList<BoundProcedure> actualAstProcedures)
        {
            switch (statement)
            {
                case BoundCallStatement boundCallStatement:

                    var id = actualAstProcedures.IndexOf(boundCallStatement.Procedure);
                    return $"Call(#{id + 1});";
                default:
                    throw new ArgumentOutOfRangeException(nameof(statement));
            }
        }
    }
}