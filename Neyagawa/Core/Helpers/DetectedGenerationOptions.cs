namespace Neyagawa.Core.Helpers
{
    using Neyagawa.Core.Options;

    public class DetectedGenerationOptions : IGenerationOptions
    {
        private readonly IGenerationOptions _baseOptions;

        private readonly TestFrameworkTypes? _testFramework;

        private readonly MockingFrameworkType? _mockingFramework;

        public DetectedGenerationOptions(IGenerationOptions baseOptions, TestFrameworkTypes? testFramework, MockingFrameworkType? mockingFramework)
        {
            _baseOptions = baseOptions ?? throw new System.ArgumentNullException(nameof(baseOptions));
            _testFramework = testFramework;
            _mockingFramework = mockingFramework;
        }

        public TestFrameworkTypes FrameworkType => _testFramework ?? _baseOptions.FrameworkType;

        public MockingFrameworkType MockingFrameworkType => _mockingFramework ?? _baseOptions.MockingFrameworkType;

        public bool AutoDetectFrameworkTypes => _baseOptions.AutoDetectFrameworkTypes;

        public string TestProjectNaming => _baseOptions.TestProjectNaming;

        public string TestFileNaming => _baseOptions.TestFileNaming;

        public string TestTypeNaming => _baseOptions.TestTypeNaming;

        public bool PartialGenerationAllowed => _baseOptions.PartialGenerationAllowed;

        public bool EmitTestsForInternals => _baseOptions.EmitTestsForInternals;

        public bool EmitSubclassForProtectedMethods => _baseOptions.EmitSubclassForProtectedMethods;

        public string ArrangeComment => _baseOptions.ArrangeComment;

        public string ActComment => _baseOptions.ActComment;

        public string AssertComment => _baseOptions.AssertComment;

        public UserInterfaceModes UserInterfaceMode => _baseOptions.UserInterfaceMode;

        public bool RememberManuallySelectedTargetProjectByDefault => _baseOptions.RememberManuallySelectedTargetProjectByDefault;

        public FallbackTargetFindingMethod FallbackTargetFinding => _baseOptions.FallbackTargetFinding;
    }
}