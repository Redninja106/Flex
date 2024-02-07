using System.Text;

namespace Flex;

class TreeWriter
{
    private TextWriter writer = new StringWriter();
    private int indentationLevel = -1;
    private HashSet<int> barLocations = new();

    public void AppendNode(object node)
    {
        writer.WriteLine(FormatNode(node, false, null));
        WriteChildren(node);
    }

    private void WriteChildren(object node)
    {
        indentationLevel++;
        var children = GetChildren(node).ToArray();
        barLocations.Add(indentationLevel);
        for (int i = 0; i < children.Length; i++)
        {
            var child = children[i];
            bool lastNode = i + 1 == children.Length;
            if (lastNode)
                barLocations.Remove(indentationLevel);
            writer.WriteLine(FormatNode(child, lastNode, node is Array ? i : null));

            WriteChildren(child);
        }
        indentationLevel--;
    }

    private string FormatNode(object node, bool isLastChild, int? arrayIndex)
    {
        StringBuilder sb = new();
        for (int i = 0; i < indentationLevel; i++)
        {
            sb.Append(barLocations.Contains(i) ? "│ " : "  ");
        }
        if (indentationLevel >= 0)
        {
            if (node is Token)
            {
                sb.Append(isLastChild ? "└ " : "├ ");
            }
            else
            {
                sb.Append(isLastChild ? "╘ " : "╞ ");
            }
        }
        if (arrayIndex is int index)
        {
            sb.Append('[');
            sb.Append(index);
            sb.Append(']');
        }
        else if (node is Array arr)
        {
            sb.Append(node.GetType().GetElementType()!.Name);
            sb.Append('[');
            sb.Append(arr.Length);
            sb.Append(']');
        }
        else
        {
            sb.Append(node);
        }
        return sb.ToString();
    }

    private IEnumerable<object> GetChildren(object node)
    {
        if (node is Array array)
        {
            foreach (var obj in array)
            {
                yield return obj;
            }
        }

        var type = node.GetType();
        var props = type.GetProperties();

        foreach (var prop in props)
        {
            if (prop.PropertyType.IsSubclassOf(typeof(Node)) || (prop.PropertyType.IsArray && prop.PropertyType.GetElementType()!.IsSubclassOf(typeof(Node))))
            {
                yield return prop.GetValue(node);
            }
        }
    }

    public override string ToString()
    {
        return writer.ToString();
    }
}

