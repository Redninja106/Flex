namespace Flex;

/// <summary>
/// Indicates a token should be a specific type of enum.
/// </summary>
public class EnumAttribute<TEnum> : TokenAttribute where TEnum : struct, Enum
{
    public EnumAttribute(TEnum value)
    {

    }

    public EnumAttribute()
    {

    }

}
