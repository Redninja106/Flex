using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.ParseTree;
internal class MultiParseTreeEntry(Type nodeType, ParseTreeEntry[] elements) : ParseTreeEntry
{
    public override Type NodeType { get; } = nodeType;
    public ParseTreeEntry[] Elements { get; } = elements;

    public override bool TryParse(ref TokenReader tokens, ErrorHandler? errorHandler, [NotNullWhen(true)] out object? result)
    {
        foreach (var element in Elements)
        {
            TokenReader tempTokens = tokens;
            if (element.TryParse(ref tempTokens, null, out var resultElement))
            {
                tokens = tempTokens;
                result = resultElement;
                return true;
            }
        }

        result = null;
        return false;
    }
}
