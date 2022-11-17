namespace Neyagawa.Core.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    using Neyagawa.Core.Frameworks;
    using Neyagawa.Core.Models;
    using Neyagawa.Core.Options;

    public static class TargetNameTransform
    {
        public static IEnumerable<string> GetTargetProjectNames(this IGenerationOptions options, string sourceProjectName)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            if (string.IsNullOrWhiteSpace(sourceProjectName))
            {
                throw new ArgumentNullException(nameof(sourceProjectName));
            }

            foreach (var format in options.TestProjectNaming.Split(';'))
            {
                if (string.IsNullOrWhiteSpace(format))
                {
                    continue;
                }

                string targetProject;
                try
                {
                    targetProject = string.Format(CultureInfo.CurrentCulture, format.Trim(), sourceProjectName);
                }
                catch (FormatException)
                {
                    throw new InvalidOperationException("Cannot not derive target project name, please check the test project naming setting.");
                }

                yield return targetProject;
            }
        }

        public static string GetTargetFileName(this IGenerationOptions options, string sourceFileName)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            if (string.IsNullOrWhiteSpace(sourceFileName))
            {
                throw new ArgumentNullException(nameof(sourceFileName));
            }

            try
            {
                return string.Format(CultureInfo.CurrentCulture, options.TestFileNaming, sourceFileName);
            }
            catch (FormatException)
            {
                throw new InvalidOperationException("Cannot not derive target file name, please check the test file naming setting.");
            }
        }

        public static string GetTargetTypeName(this IFrameworkSet frameworkSet, ITypeSymbolProvider classModel)
        {
            if (frameworkSet is null)
            {
                throw new ArgumentNullException(nameof(frameworkSet));
            }

            return frameworkSet.Options.GenerationOptions.GetTargetTypeName(classModel);
        }

        public static string GetTargetTypeName(this IGenerationOptions options, ITypeSymbolProvider classModel)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            if (classModel == null)
            {
                throw new ArgumentNullException(nameof(classModel));
            }

            try
            {
                var sourceName = classModel.ClassName;
                if (classModel.TypeSymbol?.TypeParameters.Length > 0)
                {
                    sourceName += "_" + classModel.TypeSymbol.TypeParameters.Length;
                }

                return string.Format(CultureInfo.CurrentCulture, options.TestTypeNaming, sourceName);
            }
            catch (FormatException)
            {
                throw new InvalidOperationException("Cannot not derive target type name, please check the test type naming setting.");
            }
        }

        public static string GetFullyQualifiedTargetTypeName(this IGenerationOptions options, ITypeSymbolProvider classModel, Func<string, string> transform)
        {
            var symbol = options.GetTargetTypeName(classModel);

            var transformed = transform(classModel.TypeSymbol.ContainingNamespace.ToFullName());

            return transformed + "." + symbol;
        }
    }
}