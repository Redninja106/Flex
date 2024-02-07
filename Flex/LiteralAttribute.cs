using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex;
[AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Field)]
public class LiteralAttribute(string literalValue) : TokenAttribute
{
    public string LiteralValue => literalValue;
}
