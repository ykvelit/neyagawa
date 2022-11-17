namespace Neyagawa.Core.Frameworks.Test
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    using Neyagawa.Core.Helpers;
    using Neyagawa.Core.Options;

    public abstract class BaseTestFramework
    {
        protected BaseTestFramework(IUnitTestGeneratorOptions options)
        {
            Options = options;
        }

        public abstract bool SupportsStaticTestClasses { get; }

        protected abstract string TestAttributeName { get; }

        protected abstract string TestCaseMethodAttributeName { get; }

        protected abstract string TestCaseAttributeName { get; }

        protected abstract BaseMethodDeclarationSyntax CreateSetupMethodSyntax(string targetTypeName);

        public IUnitTestGeneratorOptions Options { get; }

        public SectionedMethodHandler CreateSetupMethod(string targetTypeName, string className)
        {
            if (string.IsNullOrWhiteSpace(targetTypeName))
            {
                throw new ArgumentNullException(nameof(targetTypeName));
            }

            var method = CreateSetupMethodSyntax(targetTypeName);

            return new SectionedMethodHandler(method, Options.GenerationOptions);
        }

        public SectionedMethodHandler CreateTestMethod(NameResolver nameResolver, NamingContext namingContext, bool isAsync, bool isStatic, string description)
        {
            if (nameResolver is null)
            {
                throw new ArgumentNullException(nameof(nameResolver));
            }

            if (namingContext is null)
            {
                throw new ArgumentNullException(nameof(namingContext));
            }

            var method = Generate.Method(nameResolver.Resolve(namingContext), isAsync, isStatic && SupportsStaticTestClasses)
                                 .AddAttributeLists(Generate.Attribute(TestAttributeName).AsList());

            return new SectionedMethodHandler(method, Options.GenerationOptions, Options.GenerationOptions.ArrangeComment, Options.GenerationOptions.ActComment, Options.GenerationOptions.AssertComment);
        }

        public SectionedMethodHandler CreateTestCaseMethod(NameResolver nameResolver, NamingContext namingContext, bool isAsync, bool isStatic, TypeSyntax valueType, IEnumerable<object> testValues, string description)
        {
            if (valueType == null)
            {
                throw new ArgumentNullException(nameof(valueType));
            }

            if (testValues == null)
            {
                throw new ArgumentNullException(nameof(testValues));
            }

            if (nameResolver is null)
            {
                throw new ArgumentNullException(nameof(nameResolver));
            }

            if (namingContext is null)
            {
                throw new ArgumentNullException(nameof(namingContext));
            }

            var method = Generate.Method(nameResolver.Resolve(namingContext), isAsync, isStatic && SupportsStaticTestClasses);

            method = method.AddParameterListParameters(Generate.Parameter("value").WithType(valueType));
            if (!string.IsNullOrWhiteSpace(TestCaseMethodAttributeName))
            {
                method = method.AddAttributeLists(Generate.Attribute(TestCaseMethodAttributeName).AsList());
            }

            foreach (var testValue in testValues)
            {
                method = method.AddAttributeLists(Generate.Attribute(TestCaseAttributeName, testValue).AsList());
            }

            return new SectionedMethodHandler(method, Options.GenerationOptions, Options.GenerationOptions.ArrangeComment, Options.GenerationOptions.ActComment, Options.GenerationOptions.AssertComment);
        }
    }
}