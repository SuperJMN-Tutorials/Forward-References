using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using ForwardRefs.Test.Semantic;
using ForwardRefs.Test.Syntactic;
using Xunit;

namespace ForwardRefs.Test
{
    public class BinderTests
    {
        [Fact]
        public void Test_forward_reference()
        {
            var sut = new Binder();

            var actualAst = sut.Bind(new RootSyntax(new[]
            {
                new ProcedureSyntax("Main", new StatementSyntax[]
                {
                    new CallStatementSyntax("Proc1"),
                    new CallStatementSyntax("Proc1")
                }),
                new ProcedureSyntax("Proc1")
            }));

            DumpNodes(actualAst).Should().Be("#1{Call(#2);Call(#2);}#2{}");
        }

        private string DumpNodes(BoundRoot actualAst)
        {
            return string.Concat(actualAst.Procedures.Select((procedure, i) => DumpNodes(procedure, i, actualAst.Procedures.ToList())));
        }

        private string DumpNodes(BoundProcedure procedure, int i, IList<BoundProcedure> actualAstProcedures)
        {
            return $"#{i + 1}{{" + string.Concat(procedure.Statements.Select(statement => DumpNodes(statement, actualAstProcedures))) + "}";
        }

        private string DumpNodes(BoundStatement statement, IList<BoundProcedure> actualAstProcedures)
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
