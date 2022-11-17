namespace Neyagawa.Core.Frameworks
{
    using System.Collections.Generic;

    using Microsoft.CodeAnalysis.CSharp.Syntax;

    using Neyagawa.Core.Helpers;
    using Neyagawa.Core.Options;

    public interface ITestMethodFactory
    {
        SectionedMethodHandler CreateSetupMethod(string targetTypeName, string className);

        SectionedMethodHandler CreateTestCaseMethod(NameResolver nameResolver, NamingContext namingContext, bool isAsync, bool isStatic, TypeSyntax valueType, IEnumerable<object> testValues, string description);

        SectionedMethodHandler CreateTestMethod(NameResolver nameResolver, NamingContext namingContext, bool isAsync, bool isStatic, string description);
    }
}