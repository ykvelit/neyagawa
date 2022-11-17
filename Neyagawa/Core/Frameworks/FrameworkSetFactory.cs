namespace Neyagawa.Core.Frameworks
{
    using System;

    using Neyagawa.Core.Frameworks.Assertion;
    using Neyagawa.Core.Frameworks.Mocking;
    using Neyagawa.Core.Frameworks.Test;
    using Neyagawa.Core.Helpers;
    using Neyagawa.Core.Options;

    public static class FrameworkSetFactory
    {
        public static IFrameworkSet Create(IUnitTestGeneratorOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            var context = new GenerationContext(options.GenerationOptions);

            // test
            var testFramework = CreateTestFramework(options);

            // assertion
            IAssertionFramework assertionFramework = testFramework;
            assertionFramework = new FluentAssertionFramework(assertionFramework);

            // mocking
            var mockingFramework = Create(options.GenerationOptions.MockingFrameworkType, context);

            // naming
            var namingProvider = new NamingProvider(options.NamingOptions);

            return new FrameworkSet(testFramework, mockingFramework, assertionFramework, namingProvider, context, options);
        }

        private static IMockingFramework Create(MockingFrameworkType mockingFrameworkType, IGenerationContext context)
        {
            return new MoqMockingFramework(context);
        }

        private static IExtendedTestFramework CreateTestFramework(IUnitTestGeneratorOptions options)
        {
            return new XUnitTestFramework(options);
        }
    }
}