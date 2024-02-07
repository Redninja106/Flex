using System.Diagnostics.CodeAnalysis;

namespace Flex.ParseTree;

class ArrayParseTreeEntry(ParseTreeEntry element, ParseTreeEntry? separator, ParseTreeEntry? terminator) : ParseTreeEntry
{
    public override Type NodeType => Element.NodeType.MakeArrayType();
    public ParseTreeEntry Element { get; init; } = element; 
    public ParseTreeEntry? Separator { get; init; } = separator; // parsed in between elements
    public ParseTreeEntry? Terminator { get; init; } = terminator; // used to detect the end of the array. null means end of array is when an element can't be parsed

    public override bool TryParse(ref TokenReader tokens, ErrorHandler? errorHandler, [NotNullWhen(true)] out object? result)
    {
        // note: array parsing always succeeds, and instead of failing will return an empty array

        List<object> elements = new();

        TokenReader tempTokens = tokens;
        while (Element.TryParse(ref tempTokens, null, out result))
        {
            tokens = tempTokens;
            elements.Add(result);

            // try to parse a delimiter. error will be reported if it fails, although we keep parsing
            if (Separator?.TryParse(ref tempTokens, errorHandler, out _) ?? false)
            {
                continue;
            }
            else
            {
                // parsing failed, don't consume any tokens
                tempTokens = tokens;
            }

            // if we can parse a terminator, that indicates the end of the array
            if (Terminator?.TryParse(ref tempTokens, null, out _) ?? false)
            {
                break;
            }
        }

        Array resultArray = Array.CreateInstance(Element.NodeType, elements.Count);
        elements.ToArray().CopyTo(resultArray, 0);
        result = resultArray;
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