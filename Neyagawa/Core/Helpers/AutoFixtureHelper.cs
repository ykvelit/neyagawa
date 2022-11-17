namespace Neyagawa.Core.Helpers
{
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    using Neyagawa.Core.Options;

    internal static class AutoFixtureHelper
    {
        internal static StatementSyntax VariableDeclaration(IGenerationOptions options)
        {
            var creationExpression = CreationExpression;

            return Generate.VariableDeclarator("fixture", creationExpression).AsLocal(SyntaxFactory.IdentifierName("var"));
        }

        internal static TypeSyntax TypeSyntax => SyntaxFactory.IdentifierName("Fixture");

        internal static ExpressionSyntax CreationExpression => Generate.ObjectCreation(TypeSyntax);
    }
}