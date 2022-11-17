namespace Neyagawa.Core.Models
{
    using System.Collections.Generic;

    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    using Neyagawa.Core.Frameworks;

    public interface IMethodModel : ITestableModel<MethodDeclarationSyntax>
    {
        bool IsAsync { get; }

        bool IsVoid { get; }

        IMethodSymbol Symbol { get; }

        IList<ParameterModel> Parameters { get; }

        ExpressionSyntax Invoke(ClassModel owner, bool suppressAwait, IFrameworkSet frameworkSet, params CSharpSyntaxNode[] arguments);
    }
}