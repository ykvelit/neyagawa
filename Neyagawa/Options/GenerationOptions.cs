namespace Neyagawa.Options
{
    using Neyagawa.Core.Options;

    public class GenerationOptions : IGenerationOptions
    {
        public bool AutoDetectFrameworkTypes { get; set; } = true;

        public TestFrameworkTypes FrameworkType { get; set; } = TestFrameworkTypes.XUnit;

        public MockingFrameworkType MockingFrameworkType { get; set; } = MockingFrameworkType.Moq;

        public string TestProjectNaming { get; set; } = "{0}.UnitTest";

        public string TestTypeNaming { get; set; } = "{0}Tests";

        public bool PartialGenerationAllowed { get; set; } = true;

        public bool EmitTestsForInternals { get; set; } = false;

        public bool EmitSubclassForProtectedMethods { get; set; } = true;

        public string ArrangeComment { get; set; } = "Arrange";

        public string ActComment { get; set; } = "Act";

        public string AssertComment { get; set; } = "Assert";

        public UserInterfaceModes UserInterfaceMode { get; set; } = UserInterfaceModes.WhenTargetNotFound;

        public bool RememberManuallySelectedTargetProjectByDefault { get; set; } = true;

        public FallbackTargetFindingMethod FallbackTargetFinding { get; set; } = FallbackTargetFindingMethod.TypeInCorrectNamespace;

        public string TestFileNaming { get; set; } = "{0}Tests";
    }
}