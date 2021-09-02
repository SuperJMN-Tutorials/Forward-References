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
            var resolved = Resolve(table);
            return new BoundRoot(resolved.ToList());
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
            cache.Clear();

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
            return new BoundProcedure(statements.ToList());
        }

        private IEnumerable<BoundProcedure> Resolve(Dictionary<string, BoundProcedure> boundProcedures)
        {
            return boundProcedures.Select(entry => Resolve(entry.Value, boundProcedures, entry.Key));
        }

        private BoundProcedure Resolve(BoundProcedure procedure, Dictionary<string, BoundProcedure> boundProcedures, string name)
        {
            if (cache.TryGetValue(name, out var b))
            {
                return b;
            }

            var statements = procedure.Statements.Select(st => Resolve(st, boundProcedures));
            return new BoundProcedure(statements.ToList());
        }

        private BoundStatement Resolve(BoundStatement statement, Dictionary<string, BoundProcedure> boundProcedures)
        {
            switch (statement)
            {
                case UnresolvedCallStatement unresolvedCall:
                    var boundProcedure = FromName(unresolvedCall.ProcedureName, boundProcedures);
                    return new BoundCallStatement(boundProcedure);
                default:
                    throw new ArgumentOutOfRangeException(nameof(statement));
            }
        }

        private BoundProcedure FromName(string procName, Dictionary<string, BoundProcedure> boundProcedures)
        {
            if (cache.TryGetValue(procName, out var r))
            {
                return r;
            }

            var proc = boundProcedures[procName];
            var boundProcedure = Resolve(proc, boundProcedures, procName);
            cache.Add(procName, boundProcedure);
            return boundProcedure;
        }
    }
}