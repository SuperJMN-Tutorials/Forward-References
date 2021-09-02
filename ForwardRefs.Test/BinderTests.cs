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

            actualAst.AsString()
                .Should()
                .Be("#1{Call(#2);Call(#2);}#2{}");
        }
    }
}
