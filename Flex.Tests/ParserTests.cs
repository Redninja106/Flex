namespace Flex.Tests;

[TestClass]
public class ParserTests
{
    [TestMethod]
    public void Optional()
    {
        Token[] tokens = [
            new Token("int", TokenKind.Identifier()),
        ];

        Parser.Parse<OptionalElement>(new(tokens));
    }

    private record OptionalElement([Identifier, Optional] Token Token) : Node;
}