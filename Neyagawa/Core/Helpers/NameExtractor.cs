namespace Neyagawa.Core.Helpers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    public static class NameExtractor
    {
        public static string GetClassName(this TypeDeclarationSyntax declaration)
        {
            if (declaration == null)
            {
                throw new ArgumentNullException(nameof(declaration));
            }

            var classIdentifierToken = declaration.ChildTokens().FirstOrDefault(n => n.Kind() == SyntaxKind.IdentifierToken);
            if (classIdentifierToken == default(SyntaxToken))
            {
                throw new InvalidOperationException("Could not find type identifier.");
            }

            return classIdentifierToken.Text;
        }

        public static async Task<string> GetNamespace(this SemanticModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var root = await model.SyntaxTree.GetRootAsync().ConfigureAwait(true);
            var namespaceDeclaration = root.DescendantNodes().FirstOrDefault(node => node.IsKind(SyntaxKind.NamespaceDeclaration));
            if (namespaceDeclaration != null)
            {
                return ((NamespaceDeclarationSyntax)namespaceDeclaration)?.Name.ToString();
            }

            var fileScopedNamespaceDeclaration = root.DescendantNodes().FirstOrDefault(node => node.IsKind(SyntaxKind.FileScopedNamespaceDeclaration));
            if (fileScopedNamespaceDeclaration != null)
            {
                return ((FileScopedNamespaceDeclarationSyntax)fileScopedNamespaceDeclaration)?.Name.ToString();
            }

            return null;
        }
    }
}