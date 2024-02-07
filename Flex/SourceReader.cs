using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex;
public struct SourceReader(string source)
{
    private readonly string source = source;
    private int position = 0;

    public readonly int Position => position;
    public readonly bool IsAtEnd => position >= source.Length;

    public readonly char Peek()
    {
        if (IsAtEnd)
        {
            return '\0';
        }

        return source[position];
    }

    public char Next()
    {
        if (IsAtEnd)
        {
            return '\0';
        }

        return source[position++];
    }
}
