namespace Neyagawa.Core.Models
{
    public interface INugetPackageReference
    {
        string Name { get; }

        string Version { get; }
    }
}