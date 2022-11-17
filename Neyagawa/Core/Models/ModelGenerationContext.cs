namespace Neyagawa.Core.Models
{
    using System;

    using Neyagawa.Core.Frameworks;
    using Neyagawa.Core.Options;

    public class ModelGenerationContext
    {
        public ModelGenerationContext(ClassModel model, IFrameworkSet frameworkSet, bool withRegeneration, bool partialGenerationAllowed, NamingContext baseNamingContext)
        {
            Model = model ?? throw new ArgumentNullException(nameof(model));
            FrameworkSet = frameworkSet ?? throw new ArgumentNullException(nameof(frameworkSet));
            WithRegeneration = withRegeneration;
            PartialGenerationAllowed = partialGenerationAllowed;
            BaseNamingContext = baseNamingContext ?? throw new ArgumentNullException(nameof(baseNamingContext));

            FrameworkSet.Context.CurrentModelIsStatic = Model.IsStatic;
        }

        public ClassModel Model { get; }

        public IFrameworkSet FrameworkSet { get; }

        public bool WithRegeneration { get; }

        public bool PartialGenerationAllowed { get; }

        public NamingContext BaseNamingContext { get; }

        public int MethodsEmitted { get; set; }
    }
}