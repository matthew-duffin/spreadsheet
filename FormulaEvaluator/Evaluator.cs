using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows.Markup;
using static System.Net.Mime.MediaTypeNames;

namespace FormulaEvaluator
{
    /// <summary>
    /// Author:    Matthew Duffin
    /// Partner:   None
    /// Date:      January 14, 2023
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
    ///    This a Library class evaluates arithmetic expressions using standard infix notation.
    ///    It will be used as a way to evaluate mathmatical expressions in our spreadsheet project

    /// </summary>
    public class Evaluator
    {
        /// <summary>
        /// This is a libarary class that evaluates formuals using a look up delegate and a formula evaluator method. 
        /// </summary>
        /// <param name="variable_name"></param>
        /// <returns></returns>
        public delegate int Lookup(String variable_name);

        /// <summary>
        ///  This function takes in a string representing a Mathematical operation using infix notation
        ///  it then evaluates the Mathematical expression by breaking it up into an array of tokens
        ///  and then performing the operation included in the string. It ignores whitespace. 
        /// </summary>
        /// <param name="expression"> Expression is the string value of the Mathematical expression we are attempting to solve</param>
        /// <param name="variableEvaluator"></param>
        /// <returns> an integer Value that is the answer to the expression passed into the function</returns>
        /// <exception cref="NotImplementedException"></exception>
        public static int Evaluate(String expression, Lookup variableEvaluator)
        {
            Stack<int>    values                = new Stack<int>();
            Stack<String> operatorStack = new Stack<String>();
            int           valuesPeekValue       = 0;
            int           secondValuesPeekValue = 0;
            String[]      tokenArray            = Regex.Split(expression, "(\\()|(\\))|(-)|(\\+)|(\\*)|(/)");
            int           token                 = 0;
            int           sum                   = 0;
            int           variableValue         = 0;
            String        variable              = "";

            // loops through our expression
            for (int i = 0; i < tokenArray.Length; i++)
            {
                sum = 0;

                if (tokenArray[i] == " " || tokenArray[i] == "") // handles any whitespace that is included in the token array 
                    continue;
                variable = tokenArray[i].Trim(); // removes white space that might be added to variables.

                //The first condition
                if (int.TryParse(tokenArray[i], out token)) // checks if we have an int
                {
                    if (operatorStack.Count() > 0 && operatorStack.Peek() == ("*") && values.Count() > 0) // checks if we have any operators and values, and if the top of the stack is *
                    {
                        mulitplicationEvaluator(values, operatorStack, token);
                        continue;
                    }
                    else if (operatorStack.Count() > 0 && operatorStack.Peek() == ("/") && values.Count() > 0)// checks if we have any operators and values, and if the top of the stack is /
                    {
                        divisionEvaluator(values, operatorStack, token);
                        continue;
                    }
                    else
                    {
                        values.Push(token);
                        continue;
                    }
                }
                //The second condition in the table
                if (!String.IsNullOrEmpty(variable) && Char.IsLetter(variable[0]) && Char.IsNumber(variable[variable.Length-1]))// checks if the first character in the string is a letter and the last character is a number
                {
                    try
                    {
                        variableValue = variableEvaluator(variable);
                    }
                    catch (ArgumentException)// checks for negative numbers or invalid variables 
                    {
                        throw new ArgumentException();
                    }
                    if (operatorStack.Count() > 0 && operatorStack.Peek() == ("*") && values.Count() > 0) // checks if we have any operators and values, and if the top of the stack is *
                    {
                        mulitplicationEvaluator(values, operatorStack, variableValue);
                        continue;
                    }
                    else if (operatorStack.Count() > 0 && operatorStack.Peek() == ("/") && values.Count() > 0)// checks if we have any operators and values, and if the top of the stack is /
                    {
                        divisionEvaluator(values, operatorStack, variableValue);
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
                    if (tokenArray.Contains(")") && !tokenArray.Contains("(")) // checks to see if we have an extra close parenthesis. 
                        throw new ArgumentException();
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
                        if (operatorStack.Peek() != "(")
                            throw new ArgumentException();
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
                            if (secondValuesPeekValue == 0)
                                throw new ArgumentException();
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
                else 
                    throw new ArgumentException();
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
            if (operatorStack.Count >= 1)// this checks for negative numbers, extra characters, or any other invalid formulas. 
                throw new ArgumentException();
            return sum;
        }
        /// <summary>
        /// This handles addition with the stacks.
        /// </summary>
        /// <param name="values"></param>
        /// <param name="operatorStack"></param>
        private static void additionEvaluator(Stack<int>values, Stack<String> operatorStack)
        {
            int sum = 0;

            sum = values.Pop() + values.Pop();
            operatorStack.Pop();
            values.Push(sum);
        }
        /// <summary>
        /// This handles subtraction with the stacks.
        /// </summary>
        /// <param name="values"></param>
        /// <param name="operatorStack"></param>
        private static void subtractionEvaluator(Stack<int> values, Stack<String> operatorStack)
        {
            int sum = 0;
            int value = values.Pop();
            int value2 = values.Pop();

            sum = value2 - value;
            operatorStack.Pop();
            values.Push(sum);
        }
        /// <summary>
        /// This handles multiplication with the stacks.
        /// </summary>
        /// <param name="values"></param>
        /// <param name="operatorStack"></param>
        private static void mulitplicationEvaluator(Stack<int> values, Stack<String> operatorStack,int multiplier)
        {
            int sum = 0;
            int valuesPeekValue = 0;
            String operatorPeekValue = "";

            valuesPeekValue = values.Pop();
            operatorPeekValue = operatorStack.Pop();
            sum = valuesPeekValue * multiplier;
            values.Push(sum);
        }
        /// <summary>
        /// This handles division with the stacks.
        /// </summary>
        /// <param name="values"></param>
        /// <param name="operatorStack"></param>
        /// <param name="divisor"></param>
        private static void divisionEvaluator(Stack<int> values, Stack<String> operatorStack, int divisor)
        {
            int sum = 0;
            int valuesPeekValue = 0;
            String operatorPeekValue = "";

            if (divisor == 0)
                throw new ArgumentException();
            valuesPeekValue = values.Pop();
            operatorPeekValue = operatorStack.Pop();
            sum = valuesPeekValue / divisor;
            values.Push(sum);
        }
    }
}