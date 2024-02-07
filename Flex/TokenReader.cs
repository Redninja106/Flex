using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex;
internal struct TokenReader
{
    private readonly Token[] tokens;
    private int nextToken;

    public readonly bool IsAtEnd => nextToken >= tokens.Length;

    public TokenReader(Token[] tokens)
    {
        this.tokens = tokens;
    }

    public readonly Token? Previous()
    {
        if (nextToken == 0)
            return null;

        return tokens[nextToken - 1];
    }

    public readonly Token? Peek()
    {
        if (IsAtEnd)
        {
            return null;
        }

        return tokens[nextToken];
    }

    public Token? Next()
    {
        if (IsAtEnd)
        {
            return null;
        }

        return tokens[nextToken++];
    }
}
