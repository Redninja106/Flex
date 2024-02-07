using System.Diagnostics.CodeAnalysis;

namespace Flex.ParseTree;

class TokenParseTreeEntry(string? literalValue) : ParseTreeEntry
{
    public override Type NodeType => typeof(Token);
    public string? LiteralValue { get; init; } = literalValue;

    public override bool TryParse(ref TokenReader tokens, ErrorHandler? errorHandler, [NotNullWhen(true)] out object? result)
    {
        var next = tokens.Next();
        if (next is null)
        {
            errorHandler?.ReportError(tokens.Previous()!, "unexpectedEndOfStream");
            result = null;
            return false;
        }

        if (LiteralValue != null && !next.Value.SequenceEqual(LiteralValue.AsSpan()))
        {
            errorHandler?.ReportError(next, "expectedLiteral: " + LiteralValue);
            result = null;
            return false;
        }
        
        // TODO: attribute verification here (ie check literal attribute values)

        result = next;
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