namespace Neyagawa.Core.Options
{
    public interface IGenerationOptions
    {
        TestFrameworkTypes FrameworkType { get; }

        MockingFrameworkType MockingFrameworkType { get; }

        bool AutoDetectFrameworkTypes { get; }

        string TestProjectNaming { get; }

        string TestFileNaming { get; }

        string TestTypeNaming { get; }

        bool PartialGenerationAllowed { get; }

        bool EmitTestsForInternals { get; }

        bool EmitSubclassForProtectedMethods { get; }

        string ArrangeComment { get; }

        string ActComment { get; }

        string AssertComment { get; }

        UserInterfaceModes UserInterfaceMode { get; }

        bool RememberManuallySelectedTargetProjectByDefault { get; }

        FallbackTargetFindingMethod FallbackTargetFinding { get; }
    }
}