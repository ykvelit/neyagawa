namespace Neyagawa.Helper
{
    using System;
    using System.Globalization;

    using Task = System.Threading.Tasks.Task;

    public static class Attempt
    {
        public static void Action(Action action, INeyagawaPackage package)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            try
            {
                action();
            }
            catch (InvalidOperationException ex)
            {
                VsMessageBox.Show(ex.Message, false, package);
            }
            catch (Exception ex)
            {
                VsMessageBox.Show(string.Format(CultureInfo.CurrentCulture, "Exception raised\n{0}", ex), true, package);
            }
        }

        public static async Task ActionAsync(Func<Task> action, INeyagawaPackage package)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            try
            {
                await action().ConfigureAwait(true);
            }
            catch (InvalidOperationException ex)
            {
                await package.JoinableTaskFactory.SwitchToMainThreadAsync();

                VsMessageBox.Show(ex.Message, false, package);
            }
            catch (Exception ex)
            {
                await package.JoinableTaskFactory.SwitchToMainThreadAsync();

                VsMessageBox.Show(string.Format(CultureInfo.CurrentCulture, "Exception raised\n{0}", ex), true, package);
            }
        }
    }
}