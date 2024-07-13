// Skeleton written by Joe Zachary for CS 3500, September 2013
// Read the entire skeleton carefully and completely before you
// do anything else!

// Version 1.1 (9/22/13 11:45 a.m.)

// Change log:
//  (Version 1.1) Repaired mistake in GetTokens
//  (Version 1.1) Changed specification of second constructor to
//                clarify description of how validation works

// (Daniel Kopta) 
// Version 1.2 (9/10/17) 

// Change log:
//  (Version 1.2) Changed the definition of equality with regards
//                to numeric tokens


using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;

namespace SpreadsheetUtilities
{
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
    ///   This class creates formula objects that are immutable and can be used to represent formulas as objects
    ///   
    /// <summary>
    /// Represents formulas written in standard infix notation using standard precedence
    /// rules.  The allowed symbols are non-negative numbers written using double-precision 
    /// floating-point syntax (without unary preceeding '-' or '+'); 
    /// variables that consist of a letter or underscore followed by 
    /// zero or more letters, underscores, or digits; parentheses; and the four operator 
    /// symbols +, -, *, and /.  
    /// 
    /// Spaces are significant only insofar that they delimit tokens.  For example, "xy" is
    /// a single variable, "x y" consists of two variables "x" and y; "x23" is a single variable; 
    /// and "x 23" consists of a variable "x" and a number "23".
    /// 
    /// Associated with every formula are two delegates:  a normalizer and a validator.  The
    /// normalizer is used to convert variables into a canonical form, and the validator is used
    /// to add extra restrictions on the validity of a variable (beyond the standard requirement 
    /// that it consist of a letter or underscore followed by zero or more letters, underscores,
    /// or digits.)  Their use is described in detail in the constructor and method comments.
    /// </summary>
    public class Formula
    {
        private List<String> tokens = new List<String>();
        private String[] tokenArray;
        private double sum;
        private bool isFormula;
        /// <summary>
        /// Creates a Formula from a string that consists of an infix expression written as
        /// described in the class comment.  If the expression is syntactically invalid,
        /// throws a FormulaFormatException with an explanatory Message.
        /// 
        /// The associated normalizer is the identity function, and the associated validator
        /// maps every string to true.  
        /// </summary>
        public Formula(String formula) :
        this(formula, s => s, s => true)
        {
        }

        /// <summary>
        /// Creates a Formula from a string that consists of an infix expression written as
        /// described in the class comment.  If the expression is syntactically incorrect,
        /// throws a FormulaFormatException with an explanatory Message.
        /// 
        /// The associated normalizer and validator are the second and third parameters,
        /// respectively.  
        /// 
        /// If the formula contains a variable v such that normalize(v) is not a legal variable, 
        /// throws a FormulaFormatException with an explanatory message. 
        /// 
        /// If the formula contains a variable v such that isValid(normalize(v)) is false,
        /// throws a FormulaFormatException with an explanatory message.
        /// 
        /// Suppose that N is a method that converts all the letters in a string to upper case, and
        /// that V is a method that returns true only if a string consists of one letter followed
        /// by one digit.  Then:
        /// 
        /// new Formula("x2+y3", N, V) should succeed
        /// new Formula("x+y3", N, V) should throw an exception, since V(N("x")) is false
        /// new Formula("2x+y3", N, V) should throw an exception, since "2x+y3" is syntactically incorrect.
        /// </summary>
        public Formula(String formula, Func<string, string> normalize, Func<string, bool> isValid)
        {
            IEnumerator<string> tokenEnumerator = GetTokens(formula).GetEnumerator();
            while (tokenEnumerator.MoveNext())
                tokens.Add(tokenEnumerator.Current);
            tokenArray = tokens.ToArray();

            for (int i = 0; i < tokenArray.Length; i++)
            {
                if (tokenArray[i][0] == '_' || (!String.IsNullOrEmpty(tokenArray[i]) && Char.IsLetter(tokenArray[i][0])))// checks if we are looking at a letter, thus a variable
                {
                    tokenArray[i] = normalize(tokenArray[i]);
                    for(int j = 0; j < tokenArray[i].Length; j++)// loops through all of the characters in the string to check for variable validity 
                    {
                        if (!Char.IsLetter(tokenArray[i][j]) && !Char.IsNumber(tokenArray[i][j]) && tokenArray[i][j] != '_')
                            throw new FormulaFormatException("incorrect variable according to general form");
                    }
                    if (!isValid(tokenArray[i]))
                        throw new FormulaFormatException("invalid variable according to your validator");
                }
                if (double.TryParse(tokenArray[i], out double x)) 
                {
                    if (x == (int)x) //if we have a double, and it is equal to its int counterpart, add that string to the array.
                    {               // Example: 2.0 would be added as 2. 
                        int y = (int)x;
                        tokenArray[i] = y.ToString();
                    }
                }
            }
            FormatChecker();
            isFormula = true;
        }

        /// <summary>
        /// Evaluates this Formula, using the lookup delegate to determine the values of
        /// variables.  When a variable symbol v needs to be determined, it should be looked up
        /// via lookup(normalize(v)). (Here, normalize is the normalizer that was passed to 
        /// the constructor.)
        /// 
        /// For example, if L("x") is 2, L("X") is 4, and N is a method that converts all the letters 
        /// in a string to upper case:
        /// 
        /// new Formula("x+7", N, s => true).Evaluate(L) is 11
        /// new Formula("x+7").Evaluate(L) is 9
        /// 
        /// Given a variable symbol as its parameter, lookup returns the variable's value 
        /// (if it has one) or throws an ArgumentException (otherwise).
        /// 
        /// If no undefined variables or divisions by zero are encountered when evaluating 
        /// this Formula, the value is returned.  Otherwise, a FormulaError is returned.  
        /// The Reason property of the FormulaError should have a meaningful explanation.
        ///
        /// This method should never throw an exception.
        /// </summary>
        public object Evaluate(Func<string, double> lookup)
        {
            Stack<double> values = new Stack<double>();
            Stack<String> operatorStack = new Stack<String>();
            double valuesPeekValue = 0;
            double secondValuesPeekValue = 0;
            double token = 0;
            double variableValue = 0;
            String variable = "";

            for (int i = 0; i < tokenArray.Length; i++)
            {
                sum = 0;
                variable = tokenArray[i].Trim(); // removes white space that might be added to variables.

                //The first condition
                if (double.TryParse(tokenArray[i], out token)) // checks if we have an int
                {
                    if (operatorStack.Count() > 0 && operatorStack.Peek() == ("*") && values.Count() > 0) // checks if we have any operators and values, and if the top of the stack is *
                    {
                        mulitplicationEvaluator(values, operatorStack, token);
                        continue;
                    }
                    else if (operatorStack.Count() > 0 && operatorStack.Peek() == ("/") && values.Count() > 0)// checks if we have any operators and values, and if the top of the stack is /
                    {
                        try
                        {
                            divisionEvaluator(values, operatorStack, token);
                        }
                        catch (ArgumentException)
                        {
                            FormulaError f = new FormulaError("Error Division by 0");
                            return f;
                        }
                        continue;
                    }
                    else
                    {
                        values.Push(token);
                        continue;
                    }
                }
                //The second condition in the table
                if (Char.IsLetter(variable[0]) || variable[0] == '_')// checks if the first character in the string is a letter and or an _ thus signifying a variable. We already know it is valid
                {
                    try
                    {
                        variableValue = lookup(variable);
                    }
                    catch (ArgumentException)// checks for negative numbers or invalid variables 
                    {
                        FormulaError f = new FormulaError("the variable was not found in the list of variables provided");
                        return f;
                    }
                    if (operatorStack.Count() > 0 && operatorStack.Peek() == ("*") && values.Count() > 0) // checks if we have any operators and values, and if the top of the stack is *
                    {
                        mulitplicationEvaluator(values, operatorStack, variableValue);
                        continue;
                    }
                    else if (operatorStack.Count() > 0 && operatorStack.Peek() == ("/") && values.Count() > 0)// checks if we have any operators and values, and if the top of the stack is /
                    {
                        try
                        {
                            divisionEvaluator(values, operatorStack, variableValue);
                        }
                        catch(ArgumentException) 
                        {
                            FormulaError f = new FormulaError("Error Division by 0");
                            return f;
                        }
                        continue;
                    }
                    else
                    {
                        values.Push(variableValue);
                        continue;
                    }

                }
                // The third condition in the table 
                if ((tokenArray[i] == "+" || tokenArray[i] == "-")) // checks if we have + or - or if the previous item is either of those two operators
                {
                    if (operatorStack.Count > 0 && (operatorStack.Peek() == "+") && values.Count >= 2)
                    {
                        additionEvaluator(values, operatorStack);
                    }
                    else if (operatorStack.Count > 0 && (operatorStack.Peek() == "-") && values.Count >= 2)
                    {
                        subtractionEvaluator(values, operatorStack);
                    }
                    operatorStack.Push(tokenArray[i]);
                }
                // Condition 4 from the table
                if (tokenArray[i] == "*" || tokenArray[i] == "/")
                    operatorStack.Push(tokenArray[i]);
                // condition 5 from the table
                if (tokenArray[i] == "(")
                    operatorStack.Push(tokenArray[i]);
                // condition 6 from the table
                if (tokenArray[i] == ")")
                {
                    if (operatorStack.Count > 0 && (operatorStack.Peek() == "+" || operatorStack.Peek() == "-") && values.Count >= 2)
                    {
                        if (operatorStack.Peek() == "+")
                        {
                            additionEvaluator(values, operatorStack);
                        }
                        if (operatorStack.Peek() == "-" && operatorStack.Count > 0)
                        {
                            subtractionEvaluator(values, operatorStack);
                        }
                    }
                    if (operatorStack.Count > 0)
                    {
                        operatorStack.Pop(); //Operator stack should have '(' on top. Pop this
                    }
                    //The way this condition is programmed, it does multiplication slightly differently, so the helper method calls are not used. 
                    if ((operatorStack.Count > 0 && (operatorStack.Peek() == "*" || operatorStack.Peek() == "/")) && values.Count >= 2)
                    {
                        if (operatorStack.Peek() == "*" && operatorStack.Count > 0)
                        {
                            sum = values.Pop() * values.Pop();
                            operatorStack.Pop();
                            values.Push(sum);
                            continue;
                        }
                        if (operatorStack.Peek() == "/" && operatorStack.Count > 0)
                        {
                            valuesPeekValue = values.Pop();
                            secondValuesPeekValue = values.Pop();
                            if (valuesPeekValue == 0)
                            {
                                FormulaError f = new FormulaError("Error Division by 0");
                                return f;
                            }
                            sum = secondValuesPeekValue / valuesPeekValue;
                            operatorStack.Pop();
                            values.Push(sum);
                            continue;
                        }
                    }
                }
            }
            // These are the conditions for after every operator has been sorted. 
            if (operatorStack.Count == 0)
            {
                if (values.Count == 1)
                    return values.Pop();
            }
            if (operatorStack.Count == 1)
            {
                if (operatorStack.Peek() == "+" && values.Count == 2 && operatorStack.Count > 0)
                {
                    sum = values.Pop() + values.Pop();
                    operatorStack.Pop();
                }
                if (operatorStack.Count > 0 && operatorStack.Peek() == "-" && values.Count == 2)
                {
                    sum = Math.Abs((values.Pop() - values.Pop()));
                    operatorStack.Pop();
                }
            }
            return sum;
        }

        /// <summary>
        /// Enumerates the normalized versions of all of the variables that occur in this 
        /// formula.  No normalization may appear more than once in the enumeration, even 
        /// if it appears more than once in this Formula.
        /// 
        /// For example, if N is a method that converts all the letters in a string to upper case:
        /// 
        /// new Formula("x+y*z", N, s => true).GetVariables() should enumerate "X", "Y", and "Z"
        /// new Formula("x+X*z", N, s => true).GetVariables() should enumerate "X" and "Z".
        /// new Formula("x+X*z").GetVariables() should enumerate "x", "X", and "z".
        /// </summary>
        public IEnumerable<String> GetVariables()
        {
            List<String> variablesList = new List<String>();
            for (int i = 0; i < tokenArray.Length; i++)
            {
                // we dont have to check if its a valid variable, because that was already checked in the constructor
                if (tokenArray[i][0] == '_'||  Char.IsLetter(tokenArray[i][0]))
                {
                    if (!variablesList.Contains(tokenArray[i])) //checks if its in list
                        variablesList.Add(tokenArray[i]); 
                }
            }
            return variablesList;
        }

        /// <summary>
        /// Returns a string containing no spaces which, if passed to the Formula
        /// constructor, will produce a Formula f such that this.Equals(f).  All of the
        /// variables in the string should be normalized.
        /// 
        /// For example, if N is a method that converts all the letters in a string to upper case:
        /// 
        /// new Formula("x + y", N, s => true).ToString() should return "X+Y"
        /// new Formula("x + Y").ToString() should return "x+Y"
        /// </summary>
        public override string ToString()
        {
            string toString = "";

            for (int i = 0; i < tokenArray.Length; i++)
                toString += tokenArray[i];
            return toString;
        }

        /// <summary>
        ///  <change> make object nullable </change>
        ///
        /// If obj is null or obj is not a Formula, returns false.  Otherwise, reports
        /// whether or not this Formula and obj are equal.
        /// 
        /// Two Formulae are considered equal if they consist of the same tokens in the
        /// same order.  To determine token equality, all tokens are compared as strings 
        /// except for numeric tokens and variable tokens.
        /// Numeric tokens are considered equal if they are equal after being "normalized" 
        /// by C#'s standard conversion from string to double, then back to string. This 
        /// eliminates any inconsistencies due to limited floating point precision.
        /// Variable tokens are considered equal if their normalized forms are equal, as 
        /// defined by the provided normalizer.
        /// 
        /// For example, if N is a method that converts all the letters in a string to upper case:
        ///  
        /// new Formula("x1+y2", N, s => true).Equals(new Formula("X1  +  Y2")) is true
        /// new Formula("x1+y2").Equals(new Formula("X1+Y2")) is false
        /// new Formula("x1+y2").Equals(new Formula("y2+x1")) is false
        /// new Formula("2.0 + x7").Equals(new Formula("2.000 + x7")) is true
        /// </summary>
        public override bool Equals(object? obj)
        {
            double token = 0;
            double objToken = 0;
            String[] stringObj;
            if (obj == null)
                return false;
            try
            {
                stringObj = secondaryFormulaTokenGetter(obj); // turns our obj into an usable tokenArray, and will check if it is a valid formula or not
            }
            catch (FormulaFormatException)
            {
                return false;
            }
            for (int i = 0; i < tokenArray.Length; i++)
            {
                if ((tokenArray[i] == "+" || tokenArray[i] == "-" || tokenArray[i] == "*" || tokenArray[i] == "/") && stringObj[i] != tokenArray[i])// checks if we have equivalent operators 
                    return false;
                if (double.TryParse(tokenArray[i], out token))// checks if we have a number
                {
                    // performs normalization of c# doubles
                    if (!double.TryParse(stringObj[i], out objToken))
                        return false;
                    if (token.ToString() != objToken.ToString())
                        return false;
                }
                if (tokenArray[i][0] == '_'|| Char.IsLetter(tokenArray[i][0])) // checks if variables is in tokenArray
                {
                    if (stringObj[i][0] != '_' && Char.IsLetter(stringObj[i][0]) && tokenArray[i] != stringObj[i]) // checks if the variable is in the obj and they are also equal 
                        return false;
                }
            }
            return true;
        }

        /// <summary>
        ///   <change> We are now using Non-Nullable objects.  Thus neither f1 nor f2 can be null!</change>
        /// Reports whether f1 == f2, using the notion of equality from the Equals method.
        /// 
        /// </summary>
        public static bool operator ==(Formula f1, Formula f2)
        {
            return f1.Equals(f2);
        }

        /// <summary>
        ///   <change> We are now using Non-Nullable objects.  Thus neither f1 nor f2 can be null!</change>
        ///   <change> Note: != should almost always be not ==, if you get my meaning </change>
        ///   Reports whether f1 != f2, using the notion of equality from the Equals method.
        /// </summary>
        public static bool operator !=(Formula f1, Formula f2)
        {
            return !f1.Equals(f2);
        }

        /// <summary>
        /// Returns a hash code for this Formula.  If f1.Equals(f2), then it must be the
        /// case that f1.GetHashCode() == f2.GetHashCode().  Ideally, the probability that two 
        /// randomly-generated unequal Formulae have the same hash code should be extremely small.
        /// </summary>
        public override int GetHashCode()
        {
            int hash = 0;

            for (int i = 0; i < tokenArray.Length; i++)
            {
               hash += tokenArray[i].GetHashCode();
            }
            return hash;
        }

        /// <summary>
        /// Given an expression, enumerates the tokens that compose it.  Tokens are left paren;
        /// right paren; one of the four operator symbols; a string consisting of a letter or underscore
        /// followed by zero or more letters, digits, or underscores; a double literal; and anything that doesn't
        /// match one of those patterns.  There are no empty tokens, and no token contains white space.
        /// </summary>
        private static IEnumerable<string> GetTokens(String formula)
        {
            // Patterns for individual tokens
            String lpPattern = @"\(";
            String rpPattern = @"\)";
            String opPattern = @"[\+\-*/]";
            String varPattern = @"[a-zA-Z_](?: [a-zA-Z_]|\d)*";
            String doublePattern = @"(?: \d+\.\d* | \d*\.\d+ | \d+ ) (?: [eE][\+-]?\d+)?";
            String spacePattern = @"\s+";

            // Overall pattern
            String pattern = String.Format("({0}) | ({1}) | ({2}) | ({3}) | ({4}) | ({5})",
                                            lpPattern, rpPattern, opPattern, varPattern, doublePattern, spacePattern);

            // Enumerate matching tokens that don't consist solely of white space.
            foreach (String s in Regex.Split(formula, pattern, RegexOptions.IgnorePatternWhitespace))
            {
                if (!Regex.IsMatch(s, @"^\s*$", RegexOptions.Singleline))
                {
                    yield return s;
                }
            }

        }
        /// <summary>
        /// checks to see if in our first constructor we have a valid format
        /// </summary>
        /// <exception cref="FormulaFormatException"></exception>
        private void FormatChecker()
        {
            int leftParenCount = 0;
            int rightParenCount = 0;
            if (tokenArray.Length <= 0) // one token rule
                throw new FormulaFormatException("there is not at least one token in the formula");
            for (int i = 0; i < tokenArray.Length; i++) // checks the format 
            {
                bool validToken = false;
                double value = 0;

                validToken = isValidToken(tokenArray[i]);
                if (validToken) // specific token rule
                {
                    if (tokenArray[0] != "(" && !double.TryParse(tokenArray[0], out value) && !Char.IsLetter(tokenArray[0][0]) && tokenArray[0][0] != '_')//starter token rule
                        throw new FormulaFormatException("the starting token rule was violated. Check if your starting token is not an operator or invalid token");
                    if (tokenArray[i] == "(") //balance parentheses rule
                        leftParenCount++;
                    if (tokenArray[i] == ")") // right parentheses rule 
                        rightParenCount++;
                    if (tokenArray[tokenArray.Length - 1] != ")" && !Char.IsLetter(tokenArray[tokenArray.Length - 1][0]) && !double.TryParse(tokenArray[tokenArray.Length - 1], out value) && tokenArray[tokenArray.Length - 1][0] != '_')// checks the ending token rule
                        throw new FormulaFormatException("The ending token rule was violated. Check to make sure your ending token is not an operator or an invalid token");
                    if (tokenArray[i] == "(" || tokenArray[i] == "+" || tokenArray[i] == "-" || tokenArray[i] == "*" || tokenArray[i] == "/")
                    {
                        if (i < (tokenArray.Length - 1) && (tokenArray[i + 1] != "(" && !Char.IsLetter(tokenArray[i + 1][0]) && !double.TryParse(tokenArray[i + 1], out value) && tokenArray[i + 1][0] != '_')) // operator/parenthesis rule
                            throw new FormulaFormatException("The parenthesis/Operator Rule was violated. an operator or parenthesis was not followed by a correct token");
                    }
                     if (tokenArray[i] == ")" || Char.IsLetter(tokenArray[i][0]) || double.TryParse(tokenArray[i], out value) || tokenArray[i] == "_")
                    {
                        if (i < (tokenArray.Length - 1) && (tokenArray[i + 1] != "+" && tokenArray[i + 1] != "-" && tokenArray[i + 1] != "*" && tokenArray[i + 1] != "/" && tokenArray[i + 1] != ")"))// extra following rule.
                            throw new FormulaFormatException("the extra following rule was violated. Check to make sure a number, variable or closing parenthesis is followed by either an operator or close parenthesis.");
                    }
                 
                }
                else if (!validToken)
                    throw new FormulaFormatException("One of the tokens used is not valid");
            }
            if (leftParenCount != rightParenCount) // also part of balance parenthesis rule 
                throw new FormulaFormatException("Missing a close parenthesis");
        }
        /// <summary>
        /// This handles addition with the stacks.
        /// </summary>
        /// <param name="values"></param>
        /// <param name="operatorStack"></param>
        private void additionEvaluator(Stack<double> values, Stack<String> operatorStack)
        {
            sum = 0;

            sum = values.Pop() + values.Pop();
            operatorStack.Pop();
            values.Push(sum);
        }
        /// <summary>
        /// This handles subtraction with the stacks.
        /// </summary>
        /// <param name="values"></param>
        /// <param name="operatorStack"></param>
        private void subtractionEvaluator(Stack<double> values, Stack<String> operatorStack)
        {
            sum = 0;
            double value = values.Pop();
            double value2 = values.Pop();

            sum = value2 - value;
            operatorStack.Pop();
            values.Push(sum);
        }
        /// <summary>
        /// This handles multiplication with the stacks.
        /// </summary>
        /// <param name="values"></param>
        /// <param name="operatorStack"></param>
        private void mulitplicationEvaluator(Stack<double> values, Stack<String> operatorStack, double multiplier)
        {
            sum = 0;

            operatorStack.Pop();
            sum = values.Pop() * multiplier;
            values.Push(sum);
        }
        /// <summary>
        /// This handles division with the stacks.
        /// </summary>
        /// <param name="values"></param>
        /// <param name="operatorStack"></param>
        /// <param name="divisor"></param>
        private void divisionEvaluator(Stack<double> values, Stack<String> operatorStack, double divisor)
        {
            sum = 0;

            if (divisor == 0)
                throw new ArgumentException();
            operatorStack.Pop();
            sum = values.Pop() / divisor;
            values.Push(sum);
        }
        /// <summary>
        /// This used in our equality methods as a way to convert a passed in object to a token array
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private String[] secondaryFormulaTokenGetter(Object? obj)
        {
            if (obj is Formula)// checks that we have a formula object
            {
                List<String> list = new List<String>();
                IEnumerator<String> e = GetTokens(obj.ToString()).GetEnumerator();
                while (e.MoveNext())
                    list.Add(e.Current);
                String[] stringObj = list.ToArray(); // A string array of all tokens.
                return stringObj;
            }
            else
                throw new FormulaFormatException("Object that was passed in is not a formula object.");
        }
        /// <summary>
        /// checks if we have a valid token
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private bool isValidToken(String token)
        {
            double value = 0;
            if (token == "(" || token == ")" || token == "+" || token == "-" || token == "*" || token == "/") // if its operator it is a valid token
                return true;
            if (Char.IsLetter(token[0])) // if its a character,its a valid variable since all variables have  been validated by this point
                return true;
            if (double.TryParse(token, out value)) // if its a double its a valid token
                return true;
            if (token[0] == '_')
                return true;
            else
                return false;
        }
    }

}

/// <summary>
/// Used to report syntactic errors in the argument to the Formula constructor.
/// </summary>
public class FormulaFormatException : Exception
{
    /// <summary>
    /// Constructs a FormulaFormatException containing the explanatory message.
    /// </summary>
    public FormulaFormatException(String message)
        : base(message)
    {
    }
}

/// <summary>
/// Used as a possible return value of the Formula.Evaluate method.
/// </summary>
public struct FormulaError
{
    /// <summary>
    /// Constructs a FormulaError containing the explanatory reason.
    /// </summary>
    /// <param name="reason"></param>
    public FormulaError(String reason)
        : this()
    {
        Reason = reason;
    }

    /// <summary>
    ///  The reason why this FormulaError was created.
    /// </summary>
    public string Reason { get; private set; }
}



// <change>
//   If you are using Extension methods to deal with common stack operations (e.g., checking for
//   an empty stack before peeking) you will find that the Non-Nullable checking is "biting" you.
//
//   To fix this, you have to use a little special syntax like the following:
//
//       public static bool OnTop<T>(this Stack<T> stack, T element1, T element2) where T : notnull
//
//   Notice that the "where T : notnull" tells the compiler that the Stack can contain any object
//   as long as it doesn't allow nulls!
// </change>
