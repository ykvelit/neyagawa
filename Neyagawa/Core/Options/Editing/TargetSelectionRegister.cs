namespace Neyagawa.Core.Options.Editing
{
    using System;
    using System.Collections.Generic;

    public class TargetSelectionRegister
    {
        private Dictionary<string, string> _selections = new Dictionary<string, string>();

        private TargetSelectionRegister()
        {
        }

        public string GetTargetFor(string sourceProjectUniqueName)
        {
            if (string.IsNullOrWhiteSpace(sourceProjectUniqueName))
            {
                throw new ArgumentNullException(nameof(sourceProjectUniqueName));
            }

            _selections.TryGetValue(sourceProjectUniqueName, out var result);
            return result;
        }

        private static TargetSelectionRegister _instance;

        public static TargetSelectionRegister Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new TargetSelectionRegister();
                }

                return _instance;
            }
        }
    }
}