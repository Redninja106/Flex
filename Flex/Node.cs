using Flex.ParseTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Flex;
public abstract record Node
{
    public string VisualizeTree()
    {
        var writer = new TreeWriter();
        writer.AppendNode(this);
        return writer.ToString();
    }

    public sealed override string ToString()
    {
        if (this is Token token)
            return token.Value.ToString();

        return base.ToString()!;
    }
}

