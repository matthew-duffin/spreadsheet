using SpreadsheetUtilities;
/// <summary>
/// Author:    Matthew Duffin
/// Partner:   None
/// Date:      February 5, 2023
/// Course:    CS 3500, University of Utah, School of Computing
/// Copyright: CS 3500 and Matthew Duffin - This work may not 
///            be copied for use in Academic Coursework.
///
/// I, Matthew Duffin, certify that I wrote this code from scratch and
/// did not copy it in part or whole from another source.  All 
/// references used in the completion of the assignments are cited 
/// in my README file.
///
/// File Contents
///
///   This class tests our formula class 
namespace FormulaTests
{
    [TestClass]
    public class FormulaTests
    {
        /// <summary>
        /// Tests our first constructor where just a formula is passed in. 
        /// </summary>
        [TestMethod]
        public void Constructor1Test()
        {
            Formula f = new Formula("2+2");
            Assert.IsTrue(f.ToString().Equals("2+2"));

        }
        /// <summary>
        /// Tests our second constructor where just a formula is passed in. 
        /// </summary>
        [TestMethod]
        public void Constructor2Test()
        {
            Formula f = new Formula("2+2", normalizer, validator);
            Assert.IsTrue(f.ToString().Equals("2+2"));
        }
        /// <summary>
        /// Tests our second constructor where just a formula is passed in. 
        /// </summary>
        [TestMethod]
        public void Constructor2TestSingleLetter()
        {
            Formula f = new Formula("2+a", normalizer, validator);
            Assert.IsTrue(f.ToString().Equals("2+A"));
        }
        /// <summary>
        /// Tests our second constructor where just a formula is passed in. 
        /// </summary>
        [TestMethod]
        public void Constructor2TestSingleUnderscore()
        {
            Formula f = new Formula("2+_", normalizer, validator);
            Assert.IsTrue(f.ToString().Equals("2+_"));
        }
        /// <summary>
        /// Tests our second constructor where just a formula is passed in. 
        /// </summary>
        [TestMethod]
        public void Constructor2TestUnderscore()
        {
            Formula f = new Formula("2+_a", normalizer, validator);
            Assert.IsTrue(f.ToString().Equals("2+_a"));
        }
        /// <summary>
        /// Tests our second constructor where just a formula is passed in. 
        /// tests the first condition where a variable does not pass the validity of the default check
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Constructor2ErrorTest()
        {
            Formula f = new Formula("A 1", normalizer, validator);
        }
        /// <summary>
        /// Tests our second constructor where just a formula is passed in. 
        /// tests an invalid variable name
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void invalidVariableTest()
        {
            Formula f = new Formula("4 + a1234%6", normalizer, validator);
        }
        /// <summary>
        /// Tests our second constructor where just a formula is passed in. 
        /// tests the a long variable name
        /// </summary>
        [TestMethod]
        public void longVariableNameTest()
        {
            Formula f = new Formula("4 + a1234_6_b_c", normalizer, validator);
            Assert.AreEqual(16.0, f.Evaluate(variableValues));
        }
        /// <summary>
        /// Tests our second constructor where just a formula is passed in. 
        /// tests the first condition where a variable does not pass the validity of the default check
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Constructor2ErrorTest2()
        {
            Formula f = new Formula("2 + a3", normalizer, testingValidator);
        }
        /// <summary>
        /// Tests our second constructor where just a formula is passed in. 
        /// tests the first condition where a variable does not pass the validity of the default check
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Constructor2ValidatorErrorTest()
        {
            Formula f = new Formula("2+a%", normalizer, validator);
        }
        /// <summary>
        /// Tests the one token rule
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void OneTokenRule()
        {
            Formula f = new Formula("", normalizer, validator);
        }
        /// <summary>
        /// Tests the starter token rule
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void StarterTokenRule1()
        {
            Formula f = new Formula(")", normalizer, validator);
        }
        /// <summary>
        /// Tests the starter token rule
        /// </summary>
        [TestMethod]
        public void StarterTokenRule2()
        {
            Formula f = new Formula("2+3", normalizer, validator);
        }
        /// <summary>
        /// Tests the starter token rule
        /// </summary>
        [TestMethod]
        public void StarterTokenRule3()
        {
            Formula f = new Formula("A1+3", normalizer, validator);
        }
        /// <summary>
        /// Tests the starter token rule
        /// </summary>
        [TestMethod]
        public void StarterTokenRule4()
        {
            Formula f = new Formula("(A1+3)", normalizer, validator);
        }
        /// <summary>
        /// Tests the ending token rule
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void endingTokenRule()
        {
            Formula f = new Formula("2+", normalizer, validator);
        }
        /// <summary>
        /// Tests the left Parenthesis Rule
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void leftParenthesisRule()
        {
            Formula f = new Formula("(2+1", normalizer, validator);
        }
        /// <summary>
        /// Tests the right Parenthesis Rule
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void rightParenthesisRule()
        {
            Formula f = new Formula("(2+1))", normalizer, validator);
        }
        /// <summary>
        /// Tests the operator / Parenthesis Rule
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Operator_ParenthesisRule1()
        {
            Formula f = new Formula("(2+#)", normalizer, validator);
        }
        /// <summary>
        /// Tests the operator / Parenthesis Rule
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Operator_ParenthesisRule2()
        {
            Formula f = new Formula("(&)", normalizer, validator);
        }
        /// <summary>
        /// Tests the operator / Parenthesis Rule
        /// </summary>
        [TestMethod]
        public void Operator_ParenthesisRule3()
        {
            Formula f = new Formula("((2+ A1))", normalizer, validator);
            Assert.IsTrue(f.ToString().Equals("((2+A1))"));
        }
        /// <summary>
        /// Tests the operator / Parenthesis Rule
        /// </summary>
        [TestMethod]
        public void Operator_ParenthesisRule4()
        {
            Formula f = new Formula("((2+ _1))", normalizer, validator);
            Assert.IsTrue(f.ToString().Equals("((2+_1))"));
        }
        /// <summary>
        /// Tests the operator / Parenthesis Rule
        /// </summary>
        [TestMethod]
        public void Operator_ParenthesisRule5()
        {
            Formula f = new Formula("((2+ _))", normalizer, validator);
            Assert.IsTrue(f.ToString().Equals("((2+_))"));
        }
        /// <summary>
        /// Tests the extra following Rule
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void ExtraFollowingRule1()
        {
            Formula f = new Formula("3+a1%", normalizer, validator);
        }
        /// <summary>
        /// Tests the extra following Rule
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void ExtraFollowingRule2()
        {
            Formula f = new Formula("3^", normalizer, validator);
        }
        /// <summary>
        /// Tests the extra following Rule
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void ExtraFollowingRule3()
        {
            Formula f = new Formula("(3+3)2", normalizer, validator);
        }
        /// <summary>
        /// Tests the extra following Rule
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void ExtraFollowingRule4()
        {
            Formula f = new Formula("(3+A1&)", normalizer, validator);
        }
        /// <summary>
        /// Tests the extra following Rule
        /// </summary>
        [TestMethod]
        public void ExtraFollowingRule5()
        {
            Formula f = new Formula("(3+A1* A1)", normalizer, validator);
            Assert.IsTrue(f.ToString().Equals("(3+A1*A1)"));
        }
        /// <summary>
        /// Tests the extra following Rule
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void validTokenRule3()
        {
            Formula f = new Formula("$+2", normalizer, validator);
        }
        /// <summary>
        /// Tests our second constructor where just a formula is passed in. 
        /// </summary>
        [TestMethod]
        public void evaluatorTest()
        {
            Formula f = new Formula("2+2");
            Formula g = new Formula("2+2", normalizer, validator);

            Assert.AreEqual(f.Evaluate(variableValues), 4.0);
            Assert.AreEqual(g.Evaluate(variableValues), 4.0);
        }
        /// <summary>
        /// tests that it correctly enumerates the variables in our formula
        /// </summary>
        [TestMethod]
        public void getVariabblesTest()
        {
            Formula f = new Formula("a1+2");
            Formula g = new Formula("a1+2", normalizer, validator);

            IEnumerator<string> e1 = f.GetVariables().GetEnumerator();
            Assert.IsTrue(e1.MoveNext());
            Assert.AreEqual("a1", e1.Current);

            e1 = g.GetVariables().GetEnumerator();// checks that it works for our other constructor
            Assert.IsTrue(e1.MoveNext());
            Assert.AreEqual("A1", e1.Current);
        }
        /// <summary>
        /// tests that it correctly enumerates the variables in our formula
        /// </summary>
        [TestMethod]
        public void getVariabblesTest2()
        {
            Formula g = new Formula("a1+ a1234_6_B_C + x2", normalizer, validator);

            IEnumerator<string> e1 = g.GetVariables().GetEnumerator();// checks that it works for our other constructor
            Assert.IsTrue(e1.MoveNext());
            Assert.AreEqual("A1", e1.Current);
            Assert.IsTrue(e1.MoveNext());
            Assert.AreEqual("A1234_6_B_C", e1.Current);
            Assert.IsTrue(e1.MoveNext());
            Assert.AreEqual("X2", e1.Current);

        }
        /// <summary>
        /// tests that it correctly enumerates the variables in our formula
        /// </summary>
        [TestMethod]
        public void getVariabblesTest3()
        {
            Formula f = new Formula("a1+_");
            Formula g = new Formula("a1+_", normalizer, validator);

            IEnumerator<string> e1 = f.GetVariables().GetEnumerator();
            Assert.IsTrue(e1.MoveNext());
            Assert.AreEqual("a1", e1.Current);
            Assert.IsTrue(e1.MoveNext());
            Assert.AreEqual("_", e1.Current);

            e1 = g.GetVariables().GetEnumerator();// checks that it works for our other constructor
            Assert.IsTrue(e1.MoveNext());
            Assert.AreEqual("A1", e1.Current);
            Assert.IsTrue(e1.MoveNext());
            Assert.AreEqual("_", e1.Current);
        }

        /// <summary>
        /// tests that it correctly enumerates the variables in our formula
        /// </summary>
        [TestMethod]
        public void getVariablesTestUnderscore()
        {
            Formula f = new Formula("_1+2");
            Formula g = new Formula("_+2", normalizer, validator);

            IEnumerator<string> e1 = f.GetVariables().GetEnumerator();
            Assert.IsTrue(e1.MoveNext());
            Assert.AreEqual("_1", e1.Current);

            e1 = g.GetVariables().GetEnumerator();// checks that it works for our other constructor
            Assert.IsTrue(e1.MoveNext());
            Assert.AreEqual("_", e1.Current);
        }
        /// <summary>
        /// Tests our equals method
        /// </summary>
        [TestMethod]
        public void EqualsTest()
        {
            Formula f = new Formula("a1 + 2");
            Formula g = new Formula("a1 + 2", normalizer, validator);

            Assert.IsTrue(f.Equals(new Formula("a1+2")));
            Assert.IsFalse(f.Equals(new Formula("A1+2")));
            Assert.IsTrue(g.Equals(new Formula("A1+2", normalizer, validator)));
            Assert.IsFalse(g.Equals(new Formula("B2+4", normalizer, validator)));
        }
        /// <summary>
        /// Tests our toString method
        /// </summary>
        [TestMethod]
        public void toStringTest()
        {
            Formula f = new Formula("a1 + 2");
            Formula g = new Formula("a1 + 2", normalizer, validator);

            Assert.AreEqual(f.ToString(), "a1+2");
            Assert.AreEqual(g.ToString(), "A1+2");
        }
        /// <summary>
        /// Tests our toString method
        /// </summary>
        [TestMethod]
        public void toStringTest2()
        {
            Formula f = new Formula("a1 + A1234_6_B_C");
            Formula g = new Formula("a1+3*5+(3+4*8)*5+2", normalizer, validator);

            Assert.AreEqual(f.ToString(), "a1+A1234_6_B_C");
            Assert.AreEqual(g.ToString(), "A1+3*5+(3+4*8)*5+2");
        }
        /// <summary>
        /// Tests our '==' method
        /// </summary>
        [TestMethod]
        public void EqualsOperatorTest()
        {
            Formula f = new Formula("a1 + 2");
            Formula g = new Formula("a1 + 2", normalizer, validator);

            Assert.IsTrue(f == (new Formula("a1+2")));
            Assert.IsFalse(f == (new Formula("A1+2")));
            Assert.IsTrue(g == (new Formula("A1+2", normalizer, validator)));
            Assert.IsFalse(g == (new Formula("B2+4", normalizer, validator)));

        }
        /// <summary>
        /// Tests our '==' method
        /// </summary>
        [TestMethod]
        public void EqualsTestNullValue()
        {
            Formula f = new Formula("a1 + 2");
            Formula g = new Formula("a1 + 2", normalizer, validator);

            Assert.IsFalse(f == (null));
            Assert.IsFalse(g == (null));
        }
        /// <summary>
        /// Tests our equals method
        /// </summary>
        [TestMethod]
        public void EqualsTestNotFormula()
        {
            Formula f = new Formula("a1 + 2");
            Formula g = new Formula("a1 + 2", normalizer, validator);

            Assert.IsFalse(f.Equals(5));
            Assert.IsFalse(g.Equals("a1 + 2"));
            Assert.IsFalse(g.Equals("A1+2"));
        }
        /// <summary>
        /// Tests our equals method
        /// </summary>
        [TestMethod]
        public void EqualsTestPlusDiffFormula()
        {
            Formula f = new Formula("a1 + 2");
            Formula g = new Formula("a1 + 2", normalizer, validator);

            Assert.IsFalse(f.Equals(new Formula("a1*2")));
            Assert.IsFalse(g.Equals(new Formula("a1*2")));
        }
        /// <summary>
        /// Tests our equals method
        /// </summary>
        [TestMethod]
        public void EqualsTestDifferentIntegersFormula()
        {
            Formula f = new Formula("a1 + 3");
            Formula g = new Formula("a1 + 3", normalizer, validator);

            Assert.IsFalse(f.Equals(new Formula("a1+2")));
            Assert.IsFalse(g.Equals(new Formula("a1+2")));
        }
        /// <summary>
        /// Tests our equals method
        /// </summary>
        [TestMethod]
        public void EqualsTestDifferentIntegersFormula2()
        {
            Formula f = new Formula("2+3+ 3");
            Formula g = new Formula("a1 + 3", normalizer, validator);

            Assert.IsFalse(f.Equals(new Formula("a1+(2*2)")));
            Assert.IsFalse(g.Equals(new Formula("a1+2")));
        }
        /// <summary>
        /// Tests our equals method
        /// </summary>
        [TestMethod]
        public void EqualsTestDoubles()
        {
            Formula f = new Formula("2.0");
            Formula g = new Formula("5e-5", normalizer, validator);

            Assert.IsTrue(f.Equals(new Formula("2.000000")));
            Assert.IsTrue(g.Equals(new Formula("5e-5")));
        }
        /// <summary>
        /// Tests our equals method
        /// </summary>
        [TestMethod]
        public void EqualsTestWhiteSpace()
        {
            Formula f = new Formula("2.0+     4");
            Formula g = new Formula("5e-5 * 6", normalizer, validator);

            Assert.IsTrue(f.Equals(new Formula("2.000000+4")));
            Assert.IsTrue(g.Equals(new Formula("5e-5*6")));
        }
        /// <summary>
        /// Tests our equals method
        /// </summary>
        [TestMethod]
        public void EqualsTestUnderScore()
        {
            Formula f = new Formula("2.0+_");
            Formula g = new Formula("5e-5 * _1", normalizer, validator);

            Assert.IsTrue(f.Equals(new Formula("2.000000+_")));
            Assert.IsTrue(g.Equals(new Formula("5e-5*_1")));
        }
        /// <summary>
        /// Tests our '!=' method
        /// </summary>
        [TestMethod]
        public void NotEqualsOperatorTest()
        {
            Formula f = new Formula("a1 + 2");
            Formula g = new Formula("a1 + 2", normalizer, validator);

            Assert.IsTrue(f != (new Formula("A1+2")));
            Assert.IsFalse(f != (new Formula("a1+2")));
            Assert.IsTrue(g != (new Formula("B2+4", normalizer, validator)));
            Assert.IsFalse(g != (new Formula("A1+2", normalizer, validator)));
        }
        /// <summary>
        /// Tests our GetHashCode method
        /// </summary>
        [TestMethod]
        public void GetHashCodeTest()
        {
            Formula f = new Formula("a1+2");
            Formula g = new Formula("a1 + 2", normalizer, validator);
            f.GetHashCode();
            new Formula("a1+2").GetHashCode();

            Assert.IsTrue(f.Equals(new Formula("a1+2")));
            Assert.AreEqual(f.GetHashCode(), (new Formula("a1+2").GetHashCode()));
            Assert.AreNotEqual(f.GetHashCode(), (new Formula("A1+2").GetHashCode()));
            Assert.AreEqual(g.GetHashCode(), (new Formula("A1+2", normalizer, validator).GetHashCode()));
            Assert.AreNotEqual(g.GetHashCode(), (new Formula("B2+4", normalizer, validator).GetHashCode()));
        }
        /// <summary>
        /// Tests our GetHashCode method
        /// </summary>
        [TestMethod]
        public void GetHashCodeTest2()
        {
            Formula f = new Formula("2+3*5+(3+4*8)*5+2");
            Formula g = new Formula("2+3*5+(3+4*8)*5+2", normalizer, validator);
            f.GetHashCode();

            Assert.IsTrue(f.Equals(new Formula("2+3*5+(3+4*8)*5+2")));
            Assert.AreEqual(f.GetHashCode(), (new Formula("2+3*5+(3+4*8)*5+2").GetHashCode()));
            Assert.AreNotEqual(f.GetHashCode(), (new Formula("A1+2").GetHashCode()));
            Assert.AreEqual(g.GetHashCode(), (new Formula("2+3*5+(3+4*8)*5+2", normalizer, validator).GetHashCode()));
            Assert.AreNotEqual(g.GetHashCode(), (new Formula("B2+4", normalizer, validator).GetHashCode()));
        }
        /// <summary>
        /// Tests our GetHashCode method
        /// </summary>
        [TestMethod]
        public void GetHashCodeTest3()
        {
            Formula f = new Formula("2+A1*5+(3+4*8)*_1+2");
            Formula g = new Formula("2+_*_1+(3+4*8)*_+2", normalizer, validator);
            f.GetHashCode();

            Assert.IsTrue(f.Equals(new Formula("2+A1*5+(3+4*8)*_1+2")));
            Assert.AreEqual(f.GetHashCode(), (new Formula("2+A1*5+(3+4*8)*_1+2").GetHashCode()));
            Assert.AreNotEqual(f.GetHashCode(), (new Formula("A1+2").GetHashCode()));
            Assert.AreEqual(g.GetHashCode(), (new Formula("2+_*_1+(3+4*8)*_+2", normalizer, validator).GetHashCode()));
            Assert.AreNotEqual(g.GetHashCode(), (new Formula("B2+4", normalizer, validator).GetHashCode()));
        }
        /// <summary>
        /// Tests a basic formula exception when the formula is not in the correct format
        /// </summary>
        [TestMethod(), Timeout(5000)]
        [ExpectedException(typeof(FormulaFormatException))]
        public void GeneralException()
        {
            Formula g = new Formula("a1 + 2+", normalizer, validator);
        }
        /// <summary>
        /// Tests a basic formula exception when the formula is not in the correct format
        /// </summary>
        [TestMethod(), Timeout(5000)]
        public void EvaluateDivideByZeroException()
        {
            Formula g = new Formula("a1 / 0.0", normalizer, validator);
            Object a = g.Evaluate(variableValues);
            Assert.AreEqual(typeof(FormulaError), a.GetType());
        }
        /// <summary>
        /// Tests a single number
        /// </summary>
        [TestMethod(), Timeout(5000)]
        [TestCategory("1")]
        public void TestSingleNumber()
        {
            Formula g = new Formula("5.0");
            Assert.AreEqual(5.0, g.Evaluate(variableValues));
        }
        /// <summary>
        /// Tests a simple multiplication
        /// </summary>
        [TestMethod(), Timeout(5000)]
        [TestCategory("1")]
        public void TestTimesNumber()
        {
            Formula g = new Formula("5.0*2.0");
            Assert.AreEqual(10.0, g.Evaluate(variableValues));
        }
        /// <summary>
        /// tests a single variable
        /// </summary>
        [TestMethod(), Timeout(5000)]
        [TestCategory("2")]
        public void TestSingleVariable()
        {
            Formula g = new Formula("A1");
            Assert.AreEqual(4.0, g.Evaluate(variableValues));
        }
        /// <summary>
        /// test addition
        /// </summary>
        [TestMethod(), Timeout(5000)]
        [TestCategory("3")]
        public void TestAddition()
        {
            Formula g = new Formula("5+3");
            Assert.AreEqual(8.0, g.Evaluate(variableValues));
        }
        /// <summary>
        /// Test Subtraction
        /// </summary>
        [TestMethod(), Timeout(5000)]
        [TestCategory("4")]
        public void TestSubtraction()
        {
            Formula g = new Formula("18-10");
            Assert.AreEqual(8.0, g.Evaluate(variableValues));
        }
        /// <summary>
        /// tests multiplication
        /// </summary>
        [TestMethod(), Timeout(5000)]
        [TestCategory("5")]
        public void TestMultiplication()
        {
            Formula g = new Formula("2.5 * 4.2");
            Assert.AreEqual(10.5, g.Evaluate(variableValues));
        }
        /// <summary>
        /// tests division 
        /// </summary>
        [TestMethod(), Timeout(5000)]
        [TestCategory("6")]
        public void TestDivision()
        {
            Formula g = new Formula("16/2");
            Assert.AreEqual(8.0, g.Evaluate(variableValues));
        }
        /// <summary>
        /// tests arithmetic with a variable 
        /// </summary>
        [TestMethod(), Timeout(5000)]
        [TestCategory("7")]
        public void TestArithmeticWithVariable()
        {
            Formula g = new Formula("2+A1");
            Assert.AreEqual(6.0, g.Evaluate(variableValues));
        }
        /// <summary>
        /// test Unknown Variable 
        /// </summary>
        [TestMethod(), Timeout(5000)]
        [TestCategory("8")]
        public void TestUnknownVariable()
        {
            Formula g = new Formula("2 +_7");
            Object a = g.Evaluate(variableValues);
            Assert.AreEqual(typeof(FormulaError), a.GetType());
        }
        /// <summary>
        /// tests order of op from left to right
        /// </summary>
        [TestMethod(), Timeout(5000)]
        [TestCategory("9")]
        public void TestLeftToRight()
        {
            Formula g = new Formula("2*6+3");
            Assert.AreEqual(15.0, g.Evaluate(variableValues));
        }
        /// <summary>
        /// tests order of operations
        /// </summary>
        [TestMethod(), Timeout(5000)]
        [TestCategory("10")]
        public void TestOrderOperations()
        {
            Formula g = new Formula("2+6*3");
            Assert.AreEqual(20.0, g.Evaluate(variableValues));
        }
        /// <summary>
        /// test parenthesis times 
        /// </summary>
        [TestMethod(), Timeout(5000)]
        [TestCategory("11")]
        public void TestParenthesesTimes()
        {
            Formula g = new Formula("(2+6)*3");
            Assert.AreEqual(24.0, g.Evaluate(variableValues));
        }
        /// <summary>
        /// tests times parentheses
        /// </summary>
        [TestMethod(), Timeout(5000)]
        [TestCategory("12")]
        public void TestTimesParentheses()
        {
            Formula g = new Formula("2*(3+5)");
            Assert.AreEqual(16.0, g.Evaluate(variableValues));
        }
        /// <summary>
        /// tests plus parentheses
        /// </summary>
        [TestMethod(), Timeout(5000)]
        [TestCategory("13")]
        public void TestPlusParentheses()
        {
            Formula g = new Formula("2+(3+5)");
            Assert.AreEqual(10.0, g.Evaluate(variableValues));
        }
        /// <summary>
        /// test plus complex
        /// </summary>
        [TestMethod(), Timeout(5000)]
        [TestCategory("14")]
        public void TestPlusComplex()
        {
            Formula g = new Formula("2+(3+5*9)");
            Assert.AreEqual(50.0, g.Evaluate(variableValues));
        }
        /// <summary>
        /// test operators after parenthesis
        /// </summary>
        [TestMethod(), Timeout(5000)]
        [TestCategory("15")]
        public void TestOperatorAfterParens()
        {
            Formula g = new Formula("(1 * 1) - 2 / 2");
            Assert.AreEqual(0.0, g.Evaluate(variableValues));
        }
        /// <summary>
        /// tests complex times parentheses
        /// </summary>
        [TestMethod(), Timeout(5000)]
        [TestCategory("16")]
        public void TestComplexTimesParentheses()
        {
            Formula g = new Formula("2+3*(3+5)");
            Assert.AreEqual(26.0, g.Evaluate(variableValues));
        }
        /// <summary>
        /// tests complex divide parentheses
        /// </summary>
        [TestMethod(), Timeout(5000)]
        [TestCategory("16")]
        public void TestComplexDivideParentheses()
        {
            Formula g = new Formula("2+3/(5+5)");
            Assert.AreEqual(2.3, g.Evaluate(variableValues));
        }
        /// <summary>
        /// tests complex divide by 0 parentheses
        /// </summary>
        [TestMethod(), Timeout(5000)]
        [TestCategory("16")]
        public void TestComplexDivideParenthesesDivideBy0()
        {
            Formula g = new Formula("3/(0)");
            Object a = g.Evaluate(variableValues);
            Assert.AreEqual(typeof(FormulaError), a.GetType());
        }
        /// <summary>
        /// tests complex and parentheses 
        /// </summary>
        [TestMethod(), Timeout(5000)]
        [TestCategory("17")]
        public void TestComplexAndParentheses()
        {
            Formula g = new Formula("2+3*5+(3+4*8)*5+2");
            Assert.AreEqual(194.0, g.Evaluate(variableValues));
        }
        /// <summary>
        /// test divide by zero
        /// </summary>
        [TestMethod(), Timeout(5000)]
        [TestCategory("18")]
        public void TestDivideByZero()
        {
            Formula g = new Formula("5/0");
            Object a = g.Evaluate(variableValues);
            Assert.AreEqual(typeof(FormulaError), a.GetType());
        }
        /// <summary>
        /// test divide by zero
        /// </summary>
        [TestMethod(), Timeout(5000)]
        [TestCategory("18")]
        public void TestDivideByZeroVariable()
        {
            Formula g = new Formula("5/B2");
            Object a = g.Evaluate(variableValues);
            Assert.AreEqual(typeof(FormulaError), a.GetType());
        }
        /// <summary>
        /// test divide by zero
        /// </summary>
        [TestMethod(), Timeout(5000)]
        [TestCategory("18")]
        public void TestDivideInCloseParenthesesBy0()
        {
            Formula g = new Formula("(5/B2)");
            Object a = g.Evaluate(variableValues);
            Assert.AreEqual(typeof(FormulaError), a.GetType());
        }
        /// <summary>
        /// test divide by zero
        /// </summary>
        [TestMethod(), Timeout(5000)]
        [TestCategory("18")]
        public void TestDivideInCloseParentheses()
        {
            Formula g = new Formula("(5/5)");
            Assert.AreEqual(1.0, g.Evaluate(variableValues));
        }
        /// <summary>
        /// test a complex equation with multiple variables including underscores
        /// </summary>
        [TestMethod(), Timeout(900000000)]
        [TestCategory("26")]
        public void TestComplexMultiVar()
        {
            Formula g = new Formula("A1*3-8/2+4*(8-9*2)/14*_");
            Assert.AreEqual(5.142857142857142, g.Evaluate(variableValues));
        }
        /// <summary>
        /// test a complex equation with multiple variables including underscores
        /// </summary>
        [TestMethod(), Timeout(900000000)]
        [TestCategory("26")]
        public void UnderscoreEvaluateTest()
        {
            Formula g = new Formula("_1+_");
            Assert.AreEqual(2.0, g.Evaluate(variableValues));
        }
        /// <summary>
        /// test a nested parentheses from the right equation
        /// </summary>
        [TestMethod(), Timeout(5000)]
        [TestCategory("27")]
        public void TestComplexNestedParensRight()
        {
            Formula g = new Formula("x1+(x2+(x3+(x4+(x5+x6))))", normalizer, validator);
            Assert.AreEqual(6.0, g.Evaluate(variableValues));
        }
        /// <summary>
        /// test a nest parentheses from the left equation
        /// </summary>
        [TestMethod(), Timeout(5000)]
        [TestCategory("28")]
        public void TestComplexNestedParensLeft()
        {
            Formula g = new Formula("((((x1+x2)+x3)+x4)+x5)+x6", normalizer, validator);
            Assert.AreEqual(6.0, g.Evaluate(variableValues));
        }
        /// <summary>
        /// tests a repeated variable
        /// </summary>
        [TestMethod(), Timeout(5000)]
        [TestCategory("29")]
        public void TestRepeatedVar()
        {
            Formula g = new Formula("x1-x1*x1/x1", normalizer, validator);
            Assert.AreEqual(0.0, g.Evaluate(variableValues));
        }
        /// <summary>
        /// Our example normalizer that will convert the first character to uppercase values.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public string normalizer(String s)
        {
            if (!String.IsNullOrEmpty(s) && Char.IsLetter(s[0]))
                s = s.ToUpper();
            return s;
        }
        /// <summary>
        /// This validates whether or not an expression is in the valid format. This validator just makes sure the first character is a letter.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public bool validator(String s)
        {
            if (!String.IsNullOrEmpty(s) && Char.IsLetter(s[0]))
                return true;
            else if (s[0] == '_')
                return true;
            else return false;
        }
        /// <summary>
        /// This validates whether or not an expression is in the valid format. This validator just makes sure the first character is an
        /// letter followed by the number 5.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public bool testingValidator(String s)
        {
            if (!String.IsNullOrEmpty(s) && Char.IsLetter(s[0]))
            {
                if (Char.IsNumber(s[1]) && s[1] == 5)
                    return true;
            }
            return false;
        }
        /// <summary>
        /// Our delegate containing variable definitions.
        /// </summary>
        /// <param name="variable"></param> the value our library will send back to be checked. 
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>Throws this if the variable does not exist in the list
        public static double variableValues(String variable)
        {
            int result = 0;
            Dictionary<String, int> dictionary = new Dictionary<String, int>();

            dictionary.Add("A1", 4);
            dictionary.Add("B2", 0);
            dictionary.Add("C3", -5);
            dictionary.Add("D4", 2);
            dictionary.Add("_", 1);
            dictionary.Add("_1", 1);
            dictionary.Add("X1", 1);
            dictionary.Add("X2", 1);
            dictionary.Add("X3", 1);
            dictionary.Add("X4", 1);
            dictionary.Add("X5", 1);
            dictionary.Add("X6", 1);
            dictionary.Add("A1234_6_B_C", 12);
            if (dictionary.ContainsKey(variable) == true)
            {
                dictionary.TryGetValue(variable, out result);
            }
            else
                throw new ArgumentException();

            return result;
        }
    }
}