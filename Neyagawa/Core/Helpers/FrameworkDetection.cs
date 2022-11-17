namespace Neyagawa.Core.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Neyagawa.Core.Models;
    using Neyagawa.Core.Options;

    public static class FrameworkDetection
    {
        private class Matcher<T>
        {
            public Matcher(string assemblyName, Func<int, bool> majorVersionMatch, T result)
            {
                AssemblyName = assemblyName;
                MajorVersionMatch = majorVersionMatch ?? (_ => true);
                Result = result;
            }

            public string AssemblyName { get; }

            public Func<int, bool> MajorVersionMatch { get; }

            public T Result { get; }

            public bool IsMatch(string referenceName, int referenceMajorVersion)
            {
                return string.Equals(AssemblyName, referenceName, StringComparison.OrdinalIgnoreCase) && MajorVersionMatch(referenceMajorVersion);
            }
        }

        private static readonly IList<Matcher<bool>> FluentAssertionsMatchers = new[] { new Matcher<bool>("FluentAssertions", null, true) };

        private static readonly IList<Matcher<bool>> AutoFixtureMatchers = new[] { new Matcher<bool>("AutoFixture", null, true) };

        private static readonly IList<Matcher<bool>> AutoFixtureMockingMatchers = new[]
        {
            new Matcher<bool>("AutoFixture.AutoMoq", null, true),
            new Matcher<bool>("AutoFixture.AutoNSubstitute", null, true),
            new Matcher<bool>("AutoFixture.AutoFakeItEasy", null, true),
        };

        private static readonly IList<Matcher<TestFrameworkTypes>> TestFrameworkMatchers = new[]
        {
            new Matcher<TestFrameworkTypes>("xunit.assert", null, TestFrameworkTypes.XUnit),
        };

        private static readonly IList<Matcher<MockingFrameworkType>> MockingFrameworkMatchers = new[]
        {
            new Matcher<MockingFrameworkType>("Moq", null, MockingFrameworkType.Moq),
        };

        private static void Resolve<T>(ref T? value, IList<Matcher<T>> matchers, string referenceName, int referenceMajorVersion)
            where T : struct
        {
            if (!value.HasValue)
            {
                var match = matchers.FirstOrDefault(x => x.IsMatch(referenceName, referenceMajorVersion));
                if (match != null)
                {
                    value = match.Result;
                }
            }
        }

        public static IGenerationOptions ResolveTargetFrameworks(IEnumerable<ReferencedAssembly> referencedAssemblies, IGenerationOptions baseOptions)
        {
            if (referencedAssemblies is null)
            {
                throw new ArgumentNullException(nameof(referencedAssemblies));
            }

            if (baseOptions is null)
            {
                throw new ArgumentNullException(nameof(baseOptions));
            }

            if (!baseOptions.AutoDetectFrameworkTypes)
            {
                return baseOptions;
            }

            bool? fluentAssertionsPresent = null;
            bool? autoFixturePresent = null;
            bool? autoFixtureMockingPresent = null;
            TestFrameworkTypes? detectedTestFramework = null;
            MockingFrameworkType? detectedMockingFramework = null;

            foreach (var reference in referencedAssemblies)
            {
                Resolve(ref fluentAssertionsPresent, FluentAssertionsMatchers, reference.AssemblyName, reference.MajorVersion);
                Resolve(ref autoFixturePresent, AutoFixtureMatchers, reference.AssemblyName, reference.MajorVersion);
                Resolve(ref autoFixtureMockingPresent, AutoFixtureMockingMatchers, reference.AssemblyName, reference.MajorVersion);
                Resolve(ref detectedTestFramework, TestFrameworkMatchers, reference.AssemblyName, reference.MajorVersion);
                Resolve(ref detectedMockingFramework, MockingFrameworkMatchers, reference.AssemblyName, reference.MajorVersion);

                if (fluentAssertionsPresent.HasValue && detectedTestFramework.HasValue && detectedMockingFramework.HasValue && autoFixturePresent.HasValue)
                {
                    break;
                }
            }

            return new DetectedGenerationOptions(baseOptions, detectedTestFramework, detectedMockingFramework);
        }
    }
}