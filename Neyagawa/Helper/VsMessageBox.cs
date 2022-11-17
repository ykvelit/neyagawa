using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace Neyagawa.Helper
{
    public static class VsMessageBox
    {
        public static void Show(string message, bool isError, INeyagawaPackage package)
        {
            VsShellUtilities.ShowMessageBox(
                package,
                message,
                "Neyagawa",
                isError ? OLEMSGICON.OLEMSGICON_CRITICAL : OLEMSGICON.OLEMSGICON_WARNING,
                OLEMSGBUTTON.OLEMSGBUTTON_OK,
                OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);
        }
    }
}