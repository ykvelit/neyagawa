namespace Neyagawa.Core.Options
{
    using System;
    using System.ComponentModel;

    [Flags]
    public enum TestFrameworkTypes
    {
        [Description("xUnit")]
        XUnit = 8,
    }
}