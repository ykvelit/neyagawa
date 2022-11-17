﻿namespace Neyagawa.Core.Strategies.ClassLevelGeneration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    using Neyagawa.Core.Frameworks;
    using Neyagawa.Core.Helpers;
    using Neyagawa.Core.Models;
    using Neyagawa.Core.Options;

    public class CanConstructNoConstructorGenerationStrategy : IGenerationStrategy<ClassModel>
    {
        private readonly IFrameworkSet _frameworkSet;

        public CanConstructNoConstructorGenerationStrategy(IFrameworkSet frameworkSet)
        {
            _frameworkSet = frameworkSet ?? throw new ArgumentNullException(nameof(frameworkSet));
        }

        public bool IsExclusive => false;

        public int Priority => 1;

        public Func<IStrategyOptions, bool> IsEnabled => x => x.ConstructorChecksAreEnabled;

        public bool CanHandle(ClassModel method, ClassModel model)
        {
            if (method is null)
            {
                throw new ArgumentNullException(nameof(method));
            }

            if (model is null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            return !model.Declaration.ChildNodes().OfType<ConstructorDeclarationSyntax>().Any() && !(model.Declaration is RecordDeclarationSyntax) && !model.IsStatic;
        }

        public IEnumerable<SectionedMethodHandler> Create(ClassModel method, ClassModel model, NamingContext namingContext)
        {
            if (method is null)
            {
                throw new ArgumentNullException(nameof(method));
            }

            if (model is null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var generatedMethod = _frameworkSet.CreateTestMethod(_frameworkSet.NamingProvider.CanConstruct, namingContext, false, false, "Checks that instance construction works.");

            generatedMethod.Act(Generate.ImplicitlyTypedVariableDeclaration("instance", Generate.ObjectCreation(model.TypeSyntax)));

            generatedMethod.Assert(_frameworkSet.AssertionFramework.AssertNotNull(SyntaxFactory.IdentifierName("instance")));

            yield return generatedMethod;
        }
    }
}