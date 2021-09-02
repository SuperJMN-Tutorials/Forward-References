using Xunit;

namespace ForwardRefs.Test
{
    public class BinderTests
    {
        [Fact]
        public void Test_forward_reference()
        {
            var sut = new Binder();

            var boundAst = sut.Bind(new RootSyntax(new[]
            {
                new ProcedureSyntax("Main", new SyntaxStatement[]
                {
                    new CallStatementSyntax("Proc1")
                }),
                new ProcedureSyntax("Proc1", new SyntaxStatement[]
                {
                })
            }));

            // Some appropriate assertion should go here
        }
    }
}
