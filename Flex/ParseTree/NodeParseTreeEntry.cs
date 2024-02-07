using System.Diagnostics.CodeAnalysis;

namespace Flex.ParseTree;

class NodeParseTreeEntry(Type nodeType, ParseTreeEntry[] elements) : ParseTreeEntry
{
    public override Type NodeType { get; } = nodeType;
    public ParseTreeEntry[] Elements { get; } = elements;

    public override bool TryParse(ref TokenReader tokens, ErrorHandler? errorHandler, [NotNullWhen(true)] out object? result)
    {
        object[] results = new object[Elements.Length];

        for (int i = 0; i < Elements.Length; i++)
        {
            var element = Elements[i];
            if (element.TryParse(ref tokens, errorHandler, out object? elementResult))
            {
                results[i] = elementResult;
            }
            else
            {
                result = null;
                return false;
            }
        }

        result = Activator.CreateInstance(NodeType, results)!;
        return true;
    }
}

/*
class ParseTreeEntry
{
    public Type NodeType;

    public bool IsOptional { get; init; }
    public bool IsArray { get; init; }
    public ParseTreeEntry? ArrayDelimiter { get; init; }

    public ParseTreeEntry(Type nodeType)
    {
        NodeType = nodeType;
    }

    public TokenAttribute[] GetTokenAttributes()
    {
        throw new NotImplementedException();
    }

    public ParseAttribute[] GetParseAttributes()
    {
        throw new NotImplementedException();
    }

    public object Parse(TokenStream tokens, ErrorHandler errorHandler)
    {
        
    }
}
*/