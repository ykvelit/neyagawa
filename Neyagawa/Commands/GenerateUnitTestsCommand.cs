using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.IO;
using System.Linq;

using EnvDTE;

using EnvDTE80;

using Microsoft.CodeAnalysis;
using Microsoft.VisualStudio.Shell;

using Neyagawa.Helper;

using Task = System.Threading.Tasks.Task;

namespace Neyagawa.Commands
{
    /// <summary>
    /// Command handler
    /// </summary>
    internal sealed class GenerateUnitTestsCommand
    {
        /// <summary>
        /// Command ID.
        /// </summary>
        public const int CommandId = 0x0100;

        /// <summary>
        /// Command menu group (command set GUID).
        /// </summary>
        public static readonly Guid CommandSet = new Guid("47fe2566-6e9d-4d25-a35d-289319a9aedb");

        private static DTE2 _dte;

        /// <summary>
        /// VS Package that provides this command, not null.
        /// </summary>
        private readonly INeyagawaPackage _package;

        /// <summary>
        /// Initializes a new instance of the <see cref="GenerateUnitTestsCommand"/> class.
        /// Adds our command handlers for menu (commands must exist in the command table file)
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        /// <param name="commandService">Command service to add command to, not null.</param>
        private GenerateUnitTestsCommand(INeyagawaPackage package, OleMenuCommandService commandService)
        {
            _package = package ?? throw new ArgumentNullException(nameof(package));
            commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));

            var menuCommandID = new CommandID(CommandSet, CommandId);
            var menuItem = new OleMenuCommand(this.Execute, menuCommandID);
            commandService.AddCommand(menuItem);
        }

        /// <summary>
        /// Gets the instance of the command.
        /// </summary>
        public static GenerateUnitTestsCommand Instance
        {
            get;
            private set;
        }

        /// <summary>
        /// Initializes the singleton instance of the command.
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        public static async Task InitializeAsync(INeyagawaPackage package)
        {
            _dte = await package.GetServiceAsync(typeof(DTE)) as DTE2;

            OleMenuCommandService commandService = await package.GetServiceAsync(typeof(IMenuCommandService)) as OleMenuCommandService;
            Instance = new GenerateUnitTestsCommand(package, commandService);
        }

        /// <summary>
        /// This function is the callback used to execute the command when the menu item is clicked.
        /// See the constructor to see how the menu item is associated with this function using
        /// OleMenuCommandService service and MenuCommand class.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event args.</param>
        private void Execute(object sender, EventArgs e)
        {
            var timer = new Stopwatch();
            timer.Start();
            var generationItems = new List<GenerationItem>();

            var messageLogger = new AggregateLogger();
            messageLogger.Initialize();

            bool isSingleCreation = false;

            Attempt.Action(
                () =>
                {
                    ThreadHelper.ThrowIfNotOnUIThread();

                    var sources = SolutionUtilities.GetSupportedFiles(_dte, true).ToList();
                    if (sources.Count == 0)
                    {
                        throw new InvalidOperationException("Cannot generate unit tests for this item because no supported files were found");
                    }

                    var baseOptions = _package.Options;
                    isSingleCreation = sources.Count == 1;

                    var sourceProjects = sources.Select(x => x.Project).Distinct().ToList();
                    if (sourceProjects.Count > 1)
                    {
                        throw new InvalidOperationException("Cannot generate unit tests for multiple projects at the same time, please select a single project");
                    }

                    var mapping = ProjectMappingFactory.CreateMappingFor(sourceProjects.Single(), baseOptions, true, false, messageLogger);
                    if (mapping == null)
                    {
                        return;
                    }

                    if (mapping.TargetProject == null)
                    {
                        throw new InvalidOperationException("Cannot create tests for '" + Path.GetFileName(sources.First().FilePath) + "' because there is no project '" + mapping.TargetProjectName + "'");
                    }

                    foreach (var source in sources)
                    {
                        var projectItem = source.Item;

                        var targetItemStatus = TargetFinder.FindExistingTargetItem(null, source, mapping, _package, messageLogger, out var foundTarget, out var wasRedirection);
                        if (!false && !mapping.Options.GenerationOptions.PartialGenerationAllowed && targetItemStatus == FindTargetStatus.Found)
                        {
                            if (isSingleCreation)
                            {
                                throw new InvalidOperationException("Cannot create tests for '" + Path.GetFileName(source.FilePath) + "' because tests already exist. If you want to re-generate tests, hold down the left Shift key while opening the context menu and select the 'Regenerate tests' option. If you want to add new tests for any new code, enable the 'Partial Generation' option.");
                            }
                            else
                            {
                                messageLogger.LogMessage("Cannot create tests for '" + Path.GetFileName(source.FilePath) + "' because tests already exist.");
                            }
                            continue;
                        }

                        var item = new GenerationItem(source, mapping);
                        if (foundTarget != null && wasRedirection)
                        {
                            item.OverrideTargetFileName = Path.GetFileName(foundTarget.Name);
                        }

                        generationItems.Add(item);
                    }

                    if (generationItems.Any())
                    {
                        _ = _package.JoinableTaskFactory.RunAsync(() => Attempt.ActionAsync(() => CodeGenerator.GenerateCodeAsync(generationItems, false, _package, messageLogger), _package));
                    }
                    else
                    {
                        var message = "No tests could be created because tests already exist for all selected source items. If you want to re-generate tests, hold down the left Shift key while opening the context menu and select the 'Regenerate tests' option. If you want to add new tests for any new code, enable the 'Partial Generation' option.";

                        if (!isSingleCreation)
                        {
                            VsMessageBox.Show(message, true, _package);
                        }

                        messageLogger.LogMessage(message);
                    }
                }, _package);

            timer.Stop();
        }
    }
}