using NUnit.Framework;
using System.Collections.Generic;

namespace CalculatorApp.Tests
{
    public class CalculatorTests
    {
        // --------------------------
        // ТЕСТЫ Tokenize
        // --------------------------

        [Test]
        public void TestTokenizeBasic()
        {
            var tokens = Calculator.Tokenize("2 + 3");
            CollectionAssert.AreEqual(new List<string> { "2", "+", "3" }, tokens);
        }

        [Test]
        public void TestTokenizeWithParentheses()
        {
            var tokens = Calculator.Tokenize("( 10 - 2 ) * ( 3 + 1 )");
            CollectionAssert.AreEqual(new List<string> { "(", "10", "-", "2", ")", "*", "(", "3", "+", "1", ")" }, tokens);
        }

        [Test]
        public void TestTokenizeMultipleSpaces()
        {
            var tokens = Calculator.Tokenize("  4   *   (  2 + 1 )  ");
            CollectionAssert.AreEqual(new List<string> { "4", "*", "(", "2", "+", "1", ")" }, tokens);
        }

        // --------------------------
        // ТЕСТЫ ToPostfix
        // --------------------------

        [Test]
        public void TestPostfixSimpleAddition()
        {
            var postfix = Calculator.ToPostfix(new List<string> { "2", "+", "3" });
            CollectionAssert.AreEqual(new List<string> { "2", "3", "+" }, postfix);
        }

        [Test]
        public void TestPostfixOperatorPrecedence()
        {
            var postfix = Calculator.ToPostfix(new List<string> { "2", "+", "3", "*", "4" });
            CollectionAssert.AreEqual(new List<string> { "2", "3", "4", "*", "+" }, postfix);
        }

        [Test]
        public void TestPostfixWithParentheses()
        {
            var postfix = Calculator.ToPostfix(new List<string> { "(", "2", "+", "3", ")", "*", "4" });
            CollectionAssert.AreEqual(new List<string> { "2", "3", "+", "4", "*" }, postfix);
        }

        [Test]
        public void TestPostfixNestedParentheses()
        {
            var tokens = new List<string> { "2", "*", "(", "3", "+", "(", "4", "-", "1", ")", ")" };
            var postfix = Calculator.ToPostfix(tokens);
            CollectionAssert.AreEqual(new List<string> { "2", "3", "4", "1", "-", "+", "*" }, postfix);
        }

        [Test]
        public void TestPostfixUnbalancedParentheses()
        {
            Assert.Throws<System.Exception>(() =>
                Calculator.ToPostfix(new List<string> { "(", "2", "+", "3" })
            );
        }

        // --------------------------
        // ТЕСТЫ EvalPostfix
        // --------------------------

        [Test]
        public void TestEvalPostfixAddition()
        {
            Assert.AreEqual(5, Calculator.EvalPostfix(new List<string> { "2", "3", "+" }));
        }

        [Test]
        public void TestEvalPostfixMixedOperations()
        {
            Assert.AreEqual(17, Calculator.EvalPostfix(new List<string> { "2", "3", "5", "*", "+" }));
            Assert.AreEqual(11, Calculator.EvalPostfix(new List<string> { "10", "2", "-", "3", "+" }));
        }

        [Test]
        public void TestEvalPostfixDivision()
        {
            Assert.AreEqual(4, Calculator.EvalPostfix(new List<string> { "8", "2", "/" }));
            Assert.AreEqual(3.5, Calculator.EvalPostfix(new List<string> { "7", "2", "/" }));
        }

        [Test]
        public void TestEvalPostfixChained()
        {
            var postfix = new List<string> { "2", "3", "+", "4", "1", "-", "*" };
            Assert.AreEqual(15, Calculator.EvalPostfix(postfix));
        }

        [Test]
        public void TestEvalPostfixErrorNotEnoughOperands()
        {
            Assert.Throws<System.Exception>(() =>
                Calculator.EvalPostfix(new List<string> { "2", "+" })
            );
        }

        [Test]
        public void TestEvalPostfixErrorUnknownToken()
        {
            Assert.Throws<System.Exception>(() =>
                Calculator.EvalPostfix(new List<string> { "2", "X", "+" })
            );
        }

        // --------------------------
        // ТЕСТЫ Calculate (интеграционные)
        // --------------------------

        [Test]
        public void TestCalculateSimple()
        {
            Assert.AreEqual(5, Calculator.Calculate("2 + 3"));
        }

        [Test]
        public void TestCalculateWithParentheses()
        {
            Assert.AreEqual(-18, Calculator.Calculate("2 + 5 * ( 3 - 7 )"));
        }

        [Test]
        public void TestCalculateMultiple()
        {
            Assert.AreEqual(32, Calculator.Calculate("( 10 - 2 ) * ( 3 + 1 )"));
        }

        [Test]
        public void TestCalculateDivision()
        {
            Assert.AreEqual(4, Calculator.Calculate("8 / 2"));
            Assert.AreEqual(2, Calculator.Calculate("7 - 10 / 2"));
            Assert.AreEqual(2, Calculator.Calculate("10 / 2 - 3"));
        }

        [Test]
        public void TestCalculateNestedParentheses()
        {
            Assert.AreEqual(38, Calculator.Calculate("2 * ( 3 + ( 4 * ( 5 - 1 ) ) )"));
        }

        [Test]
        public void TestCalculateNegativeResult()
        {
            Assert.AreEqual(-7, Calculator.Calculate("3 - 10"));
        }

        [Test]
        public void TestCalculateMultipleOperations()
        {
            Assert.AreEqual(14, Calculator.Calculate("10 - 2 + 3 * 2"));
        }

        [Test]
        public void TestCalculateLargeExpression()
        {
            var expr = "1 + 2 + 3 + 4 * 5 + ( 6 - 2 ) * 3";
            Assert.AreEqual(38, Calculator.Calculate(expr));
        }

        [Test]
        public void TestCalculateLargeExpression2()
        {
            var expr = "1 + 2 * 3 + 4 * 5 + ( 6 - 2 ) * 3";
            Assert.AreEqual(39, Calculator.Calculate(expr));
        }

        // --------------------------
        // ТЕСТЫ на исключения
        // --------------------------

        [Test]
        public void TestErrorUnbalancedParentheses()
        {
            Assert.Throws<System.Exception>(() =>
                Calculator.Calculate("( 2 + 3")
            );
        }

        [Test]
        public void TestErrorUnknownToken()
        {
            Assert.Throws<System.Exception>(() =>
                Calculator.ToPostfix(new List<string> { "2", "?", "3" })
            );
        }

        [Test]
        public void TestErrorNotEnoughOperands()
        {
            Assert.Throws<System.Exception>(() =>
                Calculator.EvalPostfix(new List<string> { "2", "+" })
            );
        }
    }
}
