namespace Neyagawa.Helper
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using EnvDTE;

    using Microsoft.VisualStudio.Shell;

    using Neyagawa.Core.Helpers;
    using Neyagawa.Core.Options;
    using Neyagawa.Core.Options.Editing;

    internal class ProjectMappingFactory
    {
        public static ProjectMapping CreateMappingFor(Project sourceProject, IUnitTestGeneratorOptions baseOptions, bool interactive, bool skipManuallySelectedTargetResolution, IMessageLogger logger)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            logger = logger ?? new InertLogger();

            // resolve the options for this project
            var projectOptions = baseOptions;

            logger.LogMessage("Creating unit test project mapping for '" + sourceProject.Name + "'.");
            foreach (var source in projectOptions.SourceCounts)
            {
                logger.LogMessage(source.Value + " option(s) loaded from " + source.Key);
            }

            // resolve the target from a session selected project, if any
            string resolvedTarget = ResolveTargetProject(sourceProject, skipManuallySelectedTargetResolution, projectOptions, logger, out var targetProject);

            // now resolve the frameworks in use by the target project (if target project is not null and options allow)
            var generationOptions = OptionsResolver.DetectFrameworks(targetProject, projectOptions.GenerationOptions);

            // now create the final options, including resolved frameworks
            var finalOptions = new UnitTestGeneratorOptions(generationOptions, projectOptions.NamingOptions, projectOptions.StrategyOptions, new Dictionary<string, string>());

            // now return the mapping from source to target, along with the per-project options
            return new ProjectMapping(sourceProject, targetProject, resolvedTarget, finalOptions);
        }

        private static string ResolveTargetProject(Project sourceProject, bool skipManuallySelectedTargetResolution, IUnitTestGeneratorOptions projectOptions, IMessageLogger logger, out Project targetProject)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var sessionSelectedProject = TargetSelectionRegister.Instance.GetTargetFor(sourceProject.UniqueName);
            if (!string.IsNullOrWhiteSpace(sessionSelectedProject) && !skipManuallySelectedTargetResolution)
            {
                targetProject = VsProjectHelper.FindProject(sourceProject.DTE.Solution, sessionSelectedProject);
                logger.LogMessage("Using the target project '" + sessionSelectedProject + "' because it was previously selected in the session.");
                return sessionSelectedProject;
            }

            // derive the target project name
            var targetProjectNames = projectOptions.GenerationOptions.GetTargetProjectNames(sourceProject.Name).ToList();

            if (targetProjectNames.Count == 0)
            {
                throw new InvalidOperationException("No target project names could be derived from the souce name '" + sourceProject.Name + "'");
            }

            foreach (var targetProjectName in targetProjectNames)
            {
                var project = VsProjectHelper.FindProject(sourceProject.DTE.Solution, targetProjectName);
                if (project != null)
                {
                    logger.LogMessage("Found target project '" + targetProjectName + "'.");
                    targetProject = project;
                    return targetProjectName;
                }
                else
                {
                    logger.LogMessage("Could not find project '" + targetProjectName + "'...");
                }
            }

            logger.LogMessage("No project naming patterns matched a project. Using '" + targetProjectNames[0] + "'.");
            targetProject = null;
            return targetProjectNames[0];
        }
    }
}