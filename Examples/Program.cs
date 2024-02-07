using Flex;

string source = @"int abs(int x, , int y) 
int not(int a)
struct Point;";

LexerConfig lexerConfig = new LexerConfig().With(
        TokenKind.Enum<Keyword>().WithWordBoundary(),
        TokenKind.Enum<Symbol>(),
        TokenKind.Identifier(),
        TokenKind.WhiteSpace().Discard()
        );

TokenStream tokens = Lexer.Lex(source, lexerConfig);

CompilationUnit function = Parser.Parse<CompilationUnit>(tokens);
Console.WriteLine(function.VisualizeTree());

record CompilationUnit(Declaration[] Declarations) : Node;
abstract record Declaration() : Node;

record Function([Literal("int")] Token ReturnType, [Identifier] Token Name, ParameterList ParamterList) : Declaration;
record ParameterList([Literal("(")] Token OpenParen, [Separated(",")] Parameter[] Parameters, [Literal(")")] Token CloseParen) : Node;
record Parameter([Literal("int")] Token Type, [Identifier] Token Name) : Node;

record Struct([Literal("struct")] Token StructKeyword, [Identifier] Token Name, [Literal(";")] Token Semicolon) : Declaration;

enum Symbol
{
    [Literal("(")] OpenParenthesis,
    [Literal(")")] CloseParenthesis,
    [Literal(",")] Comma,
    [Literal("+")] Plus,
    [Literal("{")] OpenBracket,
    [Literal("}")] CloseBracket,
    [Literal(";")] Semicolon,
}

enum Keyword
{
    [Literal("int")] Int,
    [Literal("return")] Return,
}


//record BinaryExpression([Recursive] Expression Left, Token Operator, Expression Right) : Node;
//record ParenthesisExpression([Literal("(")] Token OpenParen, Expression Expression, [Literal(")")] Token CloseParen) : Node;
//record NumberExpression(Token Number) : Node;