namespace Neyagawa.Core.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    using Neyagawa.Core.Models;

    public class ConstructorFieldAssignmentExtractor : CSharpSyntaxWalker
    {
        private ConstructorFieldAssignmentExtractor()
        {
        }

        private readonly Dictionary<string, HashSet<ParameterModel>> _setFields = new Dictionary<string, HashSet<ParameterModel>>();

        private readonly HashSet<ParameterModel> _parameters = new HashSet<ParameterModel>(new ParameterModelComparer());

        private readonly Dictionary<string, ITypeSymbol> _fieldTypes = new Dictionary<string, ITypeSymbol>();

        public static ClassDependencyMap ExtractMapFrom(TypeDeclarationSyntax classDeclaration, SemanticModel model)
        {
            if (classDeclaration is null)
            {
                throw new ArgumentNullException(nameof(classDeclaration));
            }

            if (model is null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var extractor = new ConstructorFieldAssignmentExtractor();

            foreach (var field in classDeclaration.Members.OfType<FieldDeclarationSyntax>())
            {
                var info = model.GetTypeInfo(field.Declaration.Type);
                if (info.Type != null)
                {
                    foreach (var declaration in field.Declaration.Variables)
                    {
                        extractor._fieldTypes[declaration.Identifier.Text] = info.Type;
                    }
                }
            }

            foreach (var constructor in classDeclaration.Members.OfType<ConstructorDeclarationSyntax>())
            {
                extractor._parameters.Clear();

                foreach (var parameter in constructor.ParameterList.Parameters)
                {
                    var typeModel = model.GetDeclaredSymbol(parameter);
                    if (typeModel != null && parameter.Type != null)
                    {
                        var typeInfo = model.GetTypeInfo(parameter.Type);

                        var parameterModel = new ParameterModel(parameter.Identifier.Text, parameter, typeModel.ToDisplayString(), typeInfo);

                        extractor._parameters.Add(parameterModel);
                    }
                }

                constructor.Accept(extractor);
            }

            return new ClassDependencyMap(extractor._setFields, extractor._fieldTypes);
        }
    }
}