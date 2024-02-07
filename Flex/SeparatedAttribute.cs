using Flex.ParseTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Flex;
[AttributeUsage(AttributeTargets.Parameter)]
public class SeparatedAttribute : NodeAttribute
{
    public string? LiteralValue { get; }

    public SeparatedAttribute(string literalValue)
    {
        this.LiteralValue = literalValue;
    }

    internal SeparatedAttribute()
    {

    }

    internal virtual ParseTreeEntry CreateParseTreeEntry()
    {
        if (LiteralValue is not null)
        {
            return new TokenParseTreeEntry(LiteralValue);
        }

        throw new Exception();
    }
}

[AttributeUsage(AttributeTargets.Parameter)]
public class SeparatedAttribute<TNode> : SeparatedAttribute where TNode : Node
{
    internal override ParseTreeEntry CreateParseTreeEntry()
    {
        return ParseTreeEntry.Get(typeof(TNode), []);
    }
}
