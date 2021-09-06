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
            var resolved = ResolveProcedures(table, new HashSet<string>());
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

        private IEnumerable<BoundProcedure> ResolveProcedures(Dictionary<string, BoundProcedure> boundProcedures, ISet<string> resolving)
        {
            return boundProcedures.Select(entry => ResolveProcedure(entry.Key, entry.Value, boundProcedures, resolving));
        }

        private BoundProcedure ResolveProcedure(string name, BoundProcedure procedure, Dictionary<string, BoundProcedure> boundProcedures,
            ISet<string> resolving)
        {
            if (resolving.Contains(name))
            {
                throw new InvalidOperationException("Circular reference detected");
            }

            resolving.Add(name);
            if (cache.TryGetValue(name, out var cached))
            {
                return cached;
            }

            var statements = procedure.Statements.Select(st => ResolveStatement(st, boundProcedures, resolving));
            var boundProcedure = new BoundProcedure(statements.ToList());
            cache.Add(name, boundProcedure);
            return boundProcedure;
        }

        private BoundStatement ResolveStatement(BoundStatement statement, Dictionary<string, BoundProcedure> boundProcedures, ISet<string> resolving)
        {
            switch (statement)
            {
                case UnresolvedCallStatement unresolvedCall:
                    var procName = unresolvedCall.ProcedureName;
                    var proc = boundProcedures[procName];
                    var resolvedProc = ResolveProcedure(procName, proc, boundProcedures, resolving);
                    resolving.Remove(procName);
                    return new BoundCallStatement(resolvedProc);

                default:
                    throw new ArgumentOutOfRangeException(nameof(statement));
            }
        }
    }
}