using System.Diagnostics.CodeAnalysis;

namespace Flex.ParseTree;

class OptionalParseTreeEntry(ParseTreeEntry element) : ParseTreeEntry
{
    public ParseTreeEntry Element { get; init; } = element;
    public override Type NodeType => Element.NodeType;

    public override bool TryParse(ref TokenReader tokens, ErrorHandler? errorHandler, [NotNullWhen(true)] out object? result)
    {
        TokenReader tempTokens = tokens;
        if (Element.TryParse(ref tempTokens, null, out result))
        {
            tokens = tempTokens;
            return true;
        }

        return false;
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