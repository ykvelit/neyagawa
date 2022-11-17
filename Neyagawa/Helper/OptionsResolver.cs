﻿namespace Neyagawa.Helper
{
    using System.Linq;

    using EnvDTE;

    using Microsoft.VisualStudio.Shell;

    using Neyagawa.Core.Helpers;
    using Neyagawa.Core.Models;
    using Neyagawa.Core.Options;

    using VSLangProj;

    using VSLangProj80;

    internal class OptionsResolver
    {
        public static IGenerationOptions DetectFrameworks(Project targetProject, IGenerationOptions baseOptions)
        {
            if (!baseOptions.AutoDetectFrameworkTypes || targetProject == null)
            {
                return baseOptions;
            }

            ThreadHelper.ThrowIfNotOnUIThread();

            var vsLangProj = targetProject.Object as VSProject;
            if (vsLangProj == null)
            {
                return baseOptions;
            }

            return FrameworkDetection.ResolveTargetFrameworks(vsLangProj.References.OfType<Reference3>().Select(x => new ReferencedAssembly(x.Name, x.MajorVersion)), baseOptions);
        }
    }
}