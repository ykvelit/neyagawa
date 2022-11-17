namespace Neyagawa.Core.Strategies.MethodGeneration
{
    using System;
    using System.Collections.Generic;

    using Neyagawa.Core.Frameworks;
    using Neyagawa.Core.Models;

    public class MethodGenerationStrategyFactory : ItemGenerationStrategyFactory<IMethodModel>
    {
        private readonly IFrameworkSet _frameworkSet;

        public MethodGenerationStrategyFactory(IFrameworkSet frameworkSet)
        {
            _frameworkSet = frameworkSet ?? throw new ArgumentNullException(nameof(frameworkSet));
        }

        protected override IEnumerable<IGenerationStrategy<IMethodModel>> Strategies =>
            new IGenerationStrategy<IMethodModel>[]
            {
                new CanCallMethodGenerationStrategy(_frameworkSet),
                new NullParameterCheckMethodGenerationStrategy(_frameworkSet),
                new StringParameterCheckMethodGenerationStrategy(_frameworkSet),
                new MappingMethodGenerationStrategy(_frameworkSet),
            };
    }
}