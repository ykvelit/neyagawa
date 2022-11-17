namespace Neyagawa.Core.Helpers
{
    using System;
    using System.Collections.Generic;

    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    using Neyagawa.Core.Models;

    public class InvocationExtractor : CSharpSyntaxWalker
    {
        private InvocationExtractor(SemanticModel semanticModel, IEnumerable<string> targetFields)
        {
            _semanticModel = semanticModel;
            _targetFields = new HashSet<string>(targetFields);
        }

        public static DependencyAccessMap ExtractFrom(CSharpSyntaxNode node, SemanticModel semanticModel, IEnumerable<string> targetFields)
        {
            if (node is null)
            {
                throw new ArgumentNullException(nameof(node));
            }

            if (semanticModel is null)
            {
                throw new ArgumentNullException(nameof(semanticModel));
            }

            if (targetFields is null)
            {
                throw new ArgumentNullException(nameof(targetFields));
            }

            var extractor = new InvocationExtractor(semanticModel, targetFields);
            node.Accept(extractor);

            return new DependencyAccessMap(extractor._methodCalls, extractor._propertyCalls, extractor._invocationCount, extractor._memberAccessCount);
        }

        private readonly List<Tuple<IMethodSymbol, string>> _methodCalls = new List<Tuple<IMethodSymbol, string>>();

        private readonly List<Tuple<IPropertySymbol, string>> _propertyCalls = new List<Tuple<IPropertySymbol, string>>();

        private readonly SemanticModel _semanticModel;

        private readonly HashSet<string> _targetFields;

        private int _invocationCount;

        private int _memberAccessCount;
    }
}