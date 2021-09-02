using System;
using System.Collections.Generic;
using System.Linq;

namespace ForwardRefs.Test
{
    public class Binder
    {
        public BoundRoot Bind(RootSyntax rootSyntax)
        {
            var table = CreateProcedureDictionary(rootSyntax);
            var otherAst = ResolveNode(table);
            return otherAst;
        }

        private Dictionary<string, BoundProcedure> CreateProcedureDictionary(RootSyntax rootSyntax)
        {
            var boundProcedures = rootSyntax.Procedures
                .Select(p => new { Procedure = Bind(p), p.Name })
                .ToDictionary(x => x.Name, x => x.Procedure);

            return boundProcedures;
        }


        private BoundRoot ResolveNode(Dictionary<string, BoundProcedure> boundProcedures)
        {
            var procedures = boundProcedures.Select(entry => ResolveNode(entry.Value, boundProcedures));
            return new BoundRoot(procedures);
        }

        private BoundProcedure ResolveNode(BoundProcedure boundRoot, Dictionary<string, BoundProcedure> boundProcedures)
        {
            var statements = boundRoot.Statements.Select(st => ResolveNode(st, boundProcedures));
            return new BoundProcedure(statements);
        }

        private BoundStatement ResolveNode(BoundStatement statement, Dictionary<string, BoundProcedure> boundProcedures)
        {
            switch (statement)
            {
                case UnresolvedCallStatement unresolvedCall:
                    var procName = unresolvedCall.ProcedureName;
                    var proc = boundProcedures[procName];
                    return new CallStatement(ResolveNode(proc, boundProcedures));
                default:
                    throw new ArgumentOutOfRangeException(nameof(statement));
            }
        }

        private BoundProcedure Bind(ProcedureSyntax procedure)
        {
            var statements = procedure.Statements.Select(Bind);
            return new BoundProcedure(statements);
        }

        private BoundStatement Bind(SyntaxStatement statement)
        {
            switch (statement)
            {
                case CallStatementSyntax callStatementSyntax:
                    return new UnresolvedCallStatement(callStatementSyntax.ProcedureName);
                default:
                    throw new ArgumentOutOfRangeException(nameof(statement));
            }
        }
    }
}