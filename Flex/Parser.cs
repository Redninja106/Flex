using Flex.ParseTree;

namespace Flex;

public static class Parser
{
    public static TNode Parse<TNode>(TokenStream tokens) where TNode : Node
    {
        return Parse<TNode>(tokens, new ExceptionErrorHandler())!; // ExceptionErrorHandler means we'll throw instead of returning null
    }

    public static TNode? Parse<TNode>(TokenStream tokens, ErrorHandler errorHandler) where TNode : Node
    {
        ParseTreeEntry rootEntry = ParseTreeEntry.Get(typeof(TNode), []);

        var tokenReader = tokens.ToReader();

        if (rootEntry.TryParse(ref tokenReader, errorHandler, out object? result))
        {
            return (TNode)result;
        }
        else
        {
            return null;
        }

        // return (TNode)Parse(NodeLayout.Get(typeof(TNode)), tokens, errorHandler);
    }
    /*
    private static object Parse(NodeLayout layout, TokenStream tokens, ErrorHandler errorHandler)
    {
        object[] nodes = new object[layout.Entries.Length];

        for (int i = 0; i < layout.Entries.Length; i++)
        {
            var entry = layout.Entries[i];
            nodes[i] = ParseEntry(entry, tokens, errorHandler);
        }

        return Activator.CreateInstance(layout.NodeType, nodes)!;
    }

    private static object ParseEntry(LayoutEntry entry, TokenStream tokens, ErrorHandler errorHandler)
    {
        if (entry.NodeType == typeof(Token))
        {
            return tokens.Next()!;
        }

        if (entry.IsArray)
        {
            var first = ParseEntry(entry, tokens, errorHandler);

            while (true)
            {
                Node? delimiter = null;
                if (entry.ArrayDelimiter != null)
                {
                    ParseEntry(entry.ArrayDelimiter, tokens, errorHandler);
                }

                var rest = ParseEntry(entry, tokens, errorHandler);
            }

            if (entry.NodeType == typeof(Token))
            {
                return ParseToken(entry, tokens, errorHandler);
            }

            return Parse(NodeLayout.Get(entry.NodeType), tokens, errorHandler);
        }

        return ParseEntryNoModifiers(entry, tokens, errorHandler);
    }

    private static Node ParseEntryNoModifiers(LayoutEntry entry, TokenStream tokens, ErrorHandler errorHandler)
    {
        if (entry.NodeType == typeof(Token))
        {
            return ParseToken(entry, tokens, errorHandler);
        }

        return Parse(NodeLayout.Get(entry.NodeType), tokens, errorHandler);
    }

    private static Node ParseToken(LayoutEntry entry, TokenStream tokens, ErrorHandler errorHandler)
    {


        return tokens.Next();
    }
    */
}
