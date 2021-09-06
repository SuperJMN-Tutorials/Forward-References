using System;
using System.Collections.Generic;
using System.Linq;
using ForwardRefs.Test.Syntactic;

namespace ForwardRefs.Test.Semantic
{
    public class Binder
    {
        private readonly IDictionary<string, BoundProcedure> cache = new Dictionary<string, BoundProcedure>();

        public BoundRoot Bind(RootSyntax rootSyntax)
        {
            var table = CreateProceduresTable(rootSyntax);
            var resolved = ResolveProcedures(table);
            return new BoundRoot(resolved.ToList());
        }

        private Dictionary<string, BoundProcedure> CreateProceduresTable(RootSyntax rootSyntax)
        {
            var table = rootSyntax.Procedures
                .Select(p => new { Procedure = BindProcedure(p), p.Name })
                .ToDictionary(x => x.Name, x => x.Procedure);

            return table;
        }

        private BoundStatement BindStatement(StatementSyntax statement)
        {
            cache.Clear();

            switch (statement)
            {
                case CallStatementSyntax callStatementSyntax:
                    return new UnresolvedCallStatement(callStatementSyntax.ProcedureName);
                default:
                    throw new ArgumentOutOfRangeException(nameof(statement));
            }
        }

        private BoundProcedure BindProcedure(ProcedureSyntax procedure)
        {
            var statements = procedure.Statements.Select(BindStatement);
            return new BoundProcedure(statements.ToList());
        }

        private IEnumerable<BoundProcedure> ResolveProcedures(Dictionary<string, BoundProcedure> boundProcedures)
        {
            return boundProcedures.Select(entry => ResolveProcedure(entry.Key, entry.Value, boundProcedures));
        }

        private BoundProcedure ResolveProcedure(string name, BoundProcedure procedure, Dictionary<string, BoundProcedure> boundProcedures)
        {
            if (cache.TryGetValue(name, out var cached))
            {
                return cached;
            }

            var statements = procedure.Statements.Select(st => ResolveStatement(st, boundProcedures));
            var boundProcedure = new BoundProcedure(statements.ToList());
            cache.Add(name, boundProcedure);
            return boundProcedure;
        }

        private BoundStatement ResolveStatement(BoundStatement statement, Dictionary<string, BoundProcedure> boundProcedures)
        {
            switch (statement)
            {
                case UnresolvedCallStatement unresolvedCall:
                    var proc = boundProcedures[unresolvedCall.ProcedureName];
                    return new BoundCallStatement(ResolveProcedure(unresolvedCall.ProcedureName, proc, boundProcedures));

                default:
                    throw new ArgumentOutOfRangeException(nameof(statement));
            }
        }
    }
}