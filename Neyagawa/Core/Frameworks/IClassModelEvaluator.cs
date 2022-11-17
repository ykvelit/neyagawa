namespace Neyagawa.Core.Frameworks
{
    using Neyagawa.Core.Models;

    public interface IClassModelEvaluator
    {
        void EvaluateTargetModel(ClassModel classModel);
    }
}