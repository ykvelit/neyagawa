// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global - is set by VS
namespace Neyagawa.Options
{
    using Neyagawa.Core.Options;

    internal class StrategyOptions : IStrategyOptions
    {
        public bool ConstructorChecksAreEnabled { get; set; } = true;

        public bool ConstructorParameterChecksAreEnabled { get; set; } = true;

        public bool InitializerChecksAreEnabled { get; set; } = true;

        public bool InitializerPropertyChecksAreEnabled { get; set; } = true;

        public bool MethodCallChecksAreEnabled { get; set; } = true;

        public bool MappingMethodChecksAreEnabled { get; set; } = true;

        public bool MethodParameterChecksAreEnabled { get; set; } = true;

        public bool IndexerChecksAreEnabled { get; set; } = true;

        public bool PropertyChecksAreEnabled { get; set; } = true;

        public bool InitializedPropertyChecksAreEnabled { get; set; } = true;

        public bool OperatorChecksAreEnabled { get; set; } = true;

        public bool OperatorParameterChecksAreEnabled { get; set; } = true;

        public bool InterfaceImplementationChecksAreEnabled { get; set; } = true;
    }
}