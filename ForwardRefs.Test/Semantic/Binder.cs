using System;
using System.Collections.Generic;
using System.Linq;
using ForwardRefs.Test.Syntactic;

namespace ForwardRefs.Test.Semantic
{
    public class Binder
    {
        public BoundRoot Bind(RootSyntax rootSyntax)
        {
            var table = CreateProceduresTable(rootSyntax);
            var resolved = Resolve(table);
            return new BoundRoot(resolved);
        }

        private Dictionary<string, BoundProcedure> CreateProceduresTable(RootSyntax rootSyntax)
        {
            var table = rootSyntax.Procedures
                .Select(p => new { Procedure = Bind(p), p.Name })
                .ToDictionary(x => x.Name, x => x.Procedure);

            return table;
        }

        private BoundStatement Bind(StatementSyntax statement)
        {
            switch (statement)
            {
                case CallStatementSyntax callStatementSyntax:
                    return new UnresolvedCallStatement(callStatementSyntax.ProcedureName);
                default:
                    throw new ArgumentOutOfRangeException(nameof(statement));
            }
        }

        private BoundProcedure Bind(ProcedureSyntax procedure)
        {
            var statements = procedure.Statements.Select(Bind);
            return new BoundProcedure(statements);
        }

        private IEnumerable<BoundProcedure> Resolve(Dictionary<string, BoundProcedure> boundProcedures)
        {
            return boundProcedures.Select(entry => Resolve(entry.Value, boundProcedures));
        }

        private BoundProcedure Resolve(BoundProcedure procedure, Dictionary<string, BoundProcedure> boundProcedures)
        {
            var statements = procedure.Statements.Select(st => Resolve(st, boundProcedures));
            return new BoundProcedure(statements);
        }

        private BoundStatement Resolve(BoundStatement statement, Dictionary<string, BoundProcedure> boundProcedures)
        {
            switch (statement)
            {
                case UnresolvedCallStatement unresolvedCall:
                    var procName = unresolvedCall.ProcedureName;
                    var proc = boundProcedures[procName];
                    return new BoundCallStatement(Resolve(proc, boundProcedures));
                default:
                    throw new ArgumentOutOfRangeException(nameof(statement));
            }
        }
    }
}