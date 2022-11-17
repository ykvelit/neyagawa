namespace Neyagawa.Core.Helpers
{
    using System;

    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    using Neyagawa.Core.Frameworks;

    public static class FrameworkSetHelper
    {
        public static ExpressionSyntax QualifyFieldReference(this IFrameworkSet frameworkSet, SimpleNameSyntax nameSyntax)
        {
            if (frameworkSet is null)
            {
                throw new ArgumentNullException(nameof(frameworkSet));
            }

            if (nameSyntax is null)
            {
                throw new ArgumentNullException(nameof(nameSyntax));
            }

            return nameSyntax;
        }
    }
}