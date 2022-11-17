namespace Neyagawa.Core.Options
{
    using System.Collections.Generic;

    public interface IUnitTestGeneratorOptions
    {
        IGenerationOptions GenerationOptions { get; }

        INamingOptions NamingOptions { get; }

        IStrategyOptions StrategyOptions { get; }

        IEnumerable<KeyValuePair<string, int>> SourceCounts { get; }
    }
}