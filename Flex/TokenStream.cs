using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex;
public class TokenStream : IEnumerable<Token>
{
    private readonly Token[] tokens;

    public TokenStream(Token[] tokens)
    {
        this.tokens = tokens;
    }

    internal TokenReader ToReader()
    {
        return new(tokens);
    }

    public Token[] ToArray()
    {
        return [..tokens];
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    public IEnumerator<Token> GetEnumerator()
    {
        return (tokens as IEnumerable<Token>).GetEnumerator();
    }

}
