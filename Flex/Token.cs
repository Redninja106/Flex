using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex;
public sealed record Token : Node
{
    private readonly string source;
    private readonly Range range;
    private readonly TokenKind kind;

    public Range Range => range;
    public ReadOnlySpan<char> Value => source.AsSpan(range);
    public TokenKind Kind => kind;

    public Token(string source, Range range, TokenKind kind)
    {
        this.source = source;
        this.range = range;
        this.kind = kind;
    }

    internal Token(string value, TokenKind kind) : this(value, 0..value.Length, kind)
    {
        this.kind = kind;
    }
}
