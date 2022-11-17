namespace Neyagawa.Core.Strategies.ClassGeneration
{
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    using Neyagawa.Core.Models;

    public interface IClassGenerationStrategy
    {
        int Priority { get; }

        bool CanHandle(ClassModel model);

        ClassDeclarationSyntax Create(ClassModel model);
    }
}