using System;
using System.Threading.Tasks;

using Microsoft.VisualStudio.LanguageServices;
using Microsoft.VisualStudio.Threading;

using Neyagawa.Core.Options;

namespace Neyagawa
{
    public interface INeyagawaPackage : IServiceProvider
    {
        JoinableTaskFactory JoinableTaskFactory { get; }

        VisualStudioWorkspace Workspace { get; }

        IUnitTestGeneratorOptions Options { get; }

        Task<object> GetServiceAsync(Type serviceType);
    }
}