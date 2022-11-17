namespace Neyagawa.Commands
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    using EnvDTE;

    using Microsoft.CodeAnalysis;
    using Microsoft.VisualStudio.Shell;

    using Neyagawa.Core.Assets;
    using Neyagawa.Core.Helpers;
    using Neyagawa.Core.Options;
    using Neyagawa.Helper;

    public class GenerationItem
    {
        private readonly string _targetPath;

        public GenerationItem(ProjectItemModel source, ProjectMapping mapping)
        {
            Source = source ?? throw new ArgumentNullException(nameof(source));
            Mapping = mapping ?? throw new ArgumentNullException(nameof(mapping));

            ThreadHelper.ThrowIfNotOnUIThread();

            var nameParts = VsProjectHelper.GetNameParts(source.Item);

            var targetProject = mapping.TargetProject;
            TargetProjectItems = TargetFinder.FindTargetFolder(targetProject, nameParts, true, out _targetPath);

            if (TargetProjectItems == null)
            {
                // we asked to create targetProjectItems - so if it's null we effectively had a problem getting to the target project
                throw new InvalidOperationException("Cannot create tests for '" + Path.GetFileName(source.FilePath) + "' because there is no project '" + mapping.TargetProjectName + "'.");
            }

            NamespaceTransform = mapping.CreateNamespaceTransform();
        }

        public Func<string, string> NamespaceTransform { get; }

        public HashSet<TargetAsset> RequiredAssets => Mapping.TargetAssets;

        public ProjectItemModel Source { get; }

        public ProjectMapping Mapping { get; }

        public ISymbol SourceSymbol { get; set; }

        public SyntaxNode SourceNode { get; set; }

        public string TargetContent { get; set; }

        public string OverrideTargetFileName { get; set; }

        public string TargetFileName
        {
            get
            {
                var targetFileName = OverrideTargetFileName ?? Mapping.Options.GenerationOptions.GetTargetFileName(Path.GetFileNameWithoutExtension(Source.FilePath)) + Path.GetExtension(Source.FilePath);
                if (string.IsNullOrEmpty(_targetPath))
                {
                    return targetFileName;
                }

                return Path.Combine(_targetPath, targetFileName);
            }
        }

        public ProjectItems TargetProjectItems { get; }

        public IUnitTestGeneratorOptions Options => Mapping.Options;

        public bool AnyMethodsEmitted { get; set; }
    }
}