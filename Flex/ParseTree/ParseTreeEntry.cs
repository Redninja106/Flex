using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Flex.ParseTree;

abstract class ParseTreeEntry
{
    public abstract Type NodeType { get; }

    private static readonly Dictionary<Type, ParseTreeEntry> entryCache = [];

    public static ParseTreeEntry Get(Type type, NodeAttribute[] attributes)
    {
        if (attributes is [] && entryCache.TryGetValue(type, out ParseTreeEntry? entry))
        {
            return entry;
        }

        if (type.IsArray)
        {
            var elementEntry = Get(type.GetElementType()!, []);
            var separatorAttribute = attributes.OfType<SeparatedAttribute>().FirstOrDefault();
            var separatorEntry = separatorAttribute?.CreateParseTreeEntry();
            // TODO: separator & terminator support

            return new ArrayParseTreeEntry(elementEntry, separatorEntry, null);
        }

        if (attributes.OfType<OptionalAttribute>().Any())
        {
            return new OptionalParseTreeEntry(Get(type, attributes.Where(a => a is not OptionalAttribute).ToArray()));
        }

        if (type == typeof(Token))
        {
            string? literalValue = attributes.OfType<LiteralAttribute>().FirstOrDefault()?.LiteralValue;

            return new TokenParseTreeEntry(literalValue);
        }

        Type[] subclasses = FindSubclasses(type);
        if (subclasses.Length > 0)
        {
            var multiEntry = new MultiParseTreeEntry(type, subclasses.Select(s => Get(s, [])).ToArray());
            if (attributes is [])
            {
                entryCache.Add(type, multiEntry);
            }
            return multiEntry;
        }

        var primaryConstructor = type.GetConstructors().Single();
        var layouts = primaryConstructor
            .GetParameters()
            .Select(p => Get(p.ParameterType, p.GetCustomAttributes<NodeAttribute>().ToArray()))
            .ToArray();

        var nodeEntry = new NodeParseTreeEntry(type, layouts);
        if (attributes is [])
        {
            entryCache.Add(type, nodeEntry);
        }
        return nodeEntry;
    }

    private static Type[] FindSubclasses(Type nodeType)
    {
        List<Type> result = [];
        var assembly = nodeType.Assembly;
        foreach (var type in assembly.GetTypes())
        {
            if (type.IsSubclassOf(nodeType))
            {
                result.Add(type);
            }
        }
        return [..result];
    }

    public abstract bool TryParse(ref TokenReader tokens, ErrorHandler? errorHandler, [NotNullWhen(true)] out object? result);
}