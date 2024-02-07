using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Flex;
public static class Lexer
{
    public static TokenStream Lex(string source)
    {
        return Lex(source, LexerConfig.Default);
    }

    public static TokenStream Lex(string source, LexerConfig config)
    {
        return Lex(source, config, new ExceptionErrorHandler());
    }

    public static TokenStream Lex(string source, LexerConfig config, ErrorHandler errorHandler)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(config);
        ArgumentNullException.ThrowIfNull(errorHandler);

        List<Token> tokens = [];
        
        int tokenStartPosition = 0;
        SourceReader reader = new(source);

        while (!reader.IsAtEnd)
        {
            bool foundToken = false;
            foreach (var kind in config.TokenKinds)
            {
                var readerCopy = reader;
                if (kind.ReadToken(ref readerCopy))
                {
                    reader = readerCopy;
                    if (!kind.ShouldDiscard)
                    {
                        Token t = new(source, tokenStartPosition..reader.Position, kind);
                        tokens.Add(t);
                    }
                    tokenStartPosition = reader.Position;
                    foundToken = true;
                    break;
                }
            }
            if (!foundToken)
            {
                errorHandler.ReportError(null!, $"unknown token '{(int)reader.Next():x4}'");
            }
        }

        return new([..tokens]);
    }
}

public class LexerConfig
{
    public static LexerConfig Default => new LexerConfig()
        .With(TokenKind.Identifier(), TokenKind.WhiteSpace().Ignore());

    public List<TokenKind> TokenKinds { get; } = new();

    public LexerConfig()
    {

    }

    public LexerConfig With(TokenKind kind)
    {
        TokenKinds.Add(kind);
        return this;
    }

    public LexerConfig With(params TokenKind[] kinds)
    {
        TokenKinds.AddRange(kinds);
        return this;
    }
}

public abstract class TokenKind
{
    public bool ShouldDiscard { get; private set; }
    public bool ShouldIgnore { get; private set; }
    public Predicate<char>? WordBoundaryPredicate { get; private set; }

    public static TokenKind Enum<TEnum>() 
        where TEnum : struct, Enum
    {
        return new EnumTokenKind<TEnum>();
    }

    /// <summary>
    /// Scans an identifier which can contain letters or digits as a token.
    /// </summary>
    public static TokenKind Identifier()
    {
        return new IdentifierTokenKind(char.IsLetterOrDigit);
    }

    /// <summary>
    /// Scans an identifier as a token.
    /// </summary>
    public static TokenKind Identifier(Predicate<char> first, Predicate<char> rest)
    {
        return new IdentifierTokenKind(first, rest);
    }

    /// <summary>
    /// Scans blocks of white-space characters as tokens.
    /// </summary>
    public static TokenKind WhiteSpace()
    {
        return new IdentifierTokenKind(char.IsWhiteSpace);
    }

    /// <summary>
    /// Scans individual symbols as tokens.
    /// </summary>
    public static TokenKind Symbol()
    {
        return new IdentifierTokenKind(c => char.IsSymbol(c) || char.IsPunctuation(c), c => false);
    }

    public abstract bool ReadToken(ref SourceReader source);

    public TokenKind Discard()
    {
        this.ShouldDiscard = true;
        return this;
    }

    public TokenKind Ignore()
    {
        this.ShouldIgnore = true;
        return this;
    }

    public TokenKind WithWordBoundary() => WordBoundary(char.IsLetterOrDigit);
    // token must *not* be followed by a char for which the delimiter returns true
    public TokenKind WordBoundary(Predicate<char> delimiter)
    {
        this.WordBoundaryPredicate = delimiter;
        return this;
    }

}

internal class EnumTokenKind<TEnum> : TokenKind
    where TEnum : struct, Enum
{
    private static readonly string[] values = GetValues();

    public override bool ReadToken(ref SourceReader source)
    {
        foreach (var value in values)
        {
            SourceReader src = source;
            if (ConsumeString(ref src, value) && (!WordBoundaryPredicate?.Invoke(src.Peek()) ?? true))
            {
                source = src;
                return true;
            }
        }

        return false;
    }

    private bool ConsumeString(ref SourceReader reader, string value)
    {
        foreach (var c in value)
        {
            if (reader.Next() != c)
            {
                return false;
            }
        }
        return true;
    }

    private static string[] GetValues()
    {
        var fields = typeof(TEnum).GetFields(BindingFlags.Public | BindingFlags.Static);

        var values = fields.Select(val =>
        {
            var attr = val.GetCustomAttribute<LiteralAttribute>();
            if (attr is null)
            {
                return val.Name;
            }
            else
            {
                return attr.LiteralValue;
            }
        });

        // handle big strings first
        values = values.OrderByDescending(s => s.Length);

        return [..values];
    }
}

internal class IdentifierTokenKind : TokenKind
{
    public Predicate<char> First { get; }
    public Predicate<char> Rest { get; }

    public IdentifierTokenKind(Predicate<char> predicate) : this(predicate, predicate)
    {
    }

    public IdentifierTokenKind(Predicate<char> first, Predicate<char> rest)
    {
        this.First = first;
        this.Rest = rest;
    }

    public override bool ReadToken(ref SourceReader source)
    {
        if (!First(source.Peek()))
        {
            return false;
        }
        source.Next();

        while (Rest(source.Peek()))
        {
            source.Next();
        }

        return true;
    }

}