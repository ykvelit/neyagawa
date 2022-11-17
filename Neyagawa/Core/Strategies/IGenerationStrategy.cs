namespace Neyagawa.Core.Strategies
{
    using System;
    using System.Collections.Generic;

    using Neyagawa.Core.Helpers;
    using Neyagawa.Core.Models;
    using Neyagawa.Core.Options;

    public interface IGenerationStrategy<in T>
    {
        bool IsExclusive { get; }

        int Priority { get; }

        Func<IStrategyOptions, bool> IsEnabled { get; }

        bool CanHandle(T member, ClassModel model);

        IEnumerable<SectionedMethodHandler> Create(T member, ClassModel model, NamingContext namingContext);
    }
}