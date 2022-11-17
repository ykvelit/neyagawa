namespace Neyagawa.Core.Assets
{
    using Neyagawa.Core.Options;

    public interface IAsset
    {
        string AssetFileName { get; }

        string Content(string targetNamespace, TestFrameworkTypes testFrameworkTypes);
    }
}