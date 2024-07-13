// See https://aka.ms/new-console-template for more information
using FormulaEvaluator;
using System.Collections;
/// <summary>
/// This class tests our formulaEvaluatorClass.
/// It invlovles testing as many edge cases as I could think of including what to do with
/// negative numbers, extra spaces, mulitple paranthesis and so forth. 
/// </summary>
public class FormulaEvaluatorTester
{
    /// <summary>
    /// This main method calls all of my tester helper functions
    /// </summary>
    /// <param name="args"></param>
    static void Main(string[] args)
    {
        FormulaEvaluatorTester.simpleEquations();
        FormulaEvaluatorTester.divideByZero();
        FormulaEvaluatorTester.otherExceptions();
        FormulaEvaluatorTester.mediumEquations();
        FormulaEvaluatorTester.complexEquations();
        FormulaEvaluatorTester.delegateEquations();
    }
    /// <summary>
    /// This calls simple mathmatical equations.
    /// 1-2 operators. Contains tests 1-18
    /// </summary>
    public static void simpleEquations()
    {
        //testHard
        if (Evaluator.Evaluate("2+3*5+(3+4*8)*5+2", null) == 194)
            Console.WriteLine("Professional test case pass");
        //Test01
        if (Evaluator.Evaluate("5+5", null) == 10)
            Console.WriteLine("Test01 Passed");
        //TestOrder of Operations
        if (Evaluator.Evaluate("2+6*3", null) == 20)
            Console.WriteLine("Test order of operations Passed");
        //Test02
        if (Evaluator.Evaluate("5*5", null) == 25)
            Console.WriteLine("Test02 Passed");
        //Test03
        if (Evaluator.Evaluate("5/5", null) == 1)
            Console.WriteLine("Test03 Passed");
        //Test04
        if (Evaluator.Evaluate("5-5", null) == 0)
            Console.WriteLine("Test04 Passed");
        //Test5
        if (Evaluator.Evaluate("5+4", null) == 9)
            Console.WriteLine("Test5 Passed");
        //Test6
        if (Evaluator.Evaluate("5*6", null) == 30)
            Console.WriteLine("Test6 Passed");
        //Test7
        if (Evaluator.Evaluate("5/1", null) == 5)
            Console.WriteLine("Test7 Passed");
        //Test8
        if (Evaluator.Evaluate("5-4", null) == 1)
            Console.WriteLine("Test8 Passed");
        //Test9
        if (Evaluator.Evaluate("(5-5) * 1", null) == 0)
            Console.WriteLine("Test9 Passed");
        //Test10
        if (Evaluator.Evaluate("(5-4) * 1", null) == 1)
            Console.WriteLine("Test10 Passed");
        //Test11
        if (Evaluator.Evaluate("(5+4) * 1", null) == 9)
            Console.WriteLine("Test11 Passed");
        //Test12
        if (Evaluator.Evaluate("(5+5) * 1", null) == 10)
            Console.WriteLine("Test12 Passed");
        //Test13
        if (Evaluator.Evaluate("(5*5) * 1", null) == 25)
            Console.WriteLine("Test13 Passed");
        //Test14
        if (Evaluator.Evaluate("(5/5) * 1", null) == 1)
            Console.WriteLine("Test14 Passed");
        //Test15
        if (Evaluator.Evaluate("(5 + 5) / 1", null) == 10)
            Console.WriteLine("Test15 Passed");
        // Test16
        if (Evaluator.Evaluate("(5 * 5) / 1", null) == 25)
            Console.WriteLine("Test16 Passed");
        // Test17
        if (Evaluator.Evaluate("(5/5) / 1", null) == 1)
            Console.WriteLine("Test17 Passed");
        // Test18
        if (Evaluator.Evaluate("0/5", null) == 0)
            Console.WriteLine("Test18 Passed");
    }
    /// <summary>
    /// Tests that an exception is thrown when dividing by zero
    /// Contains tests 19-20.
    /// </summary>
    public static void divideByZero()
    {
        // test19
        try
        {
            Evaluator.Evaluate("5/0", null);
        }
        catch (ArgumentException)
        {
            Console.WriteLine("Test19 passed");
        }
        // test00
        try
        {
            Evaluator.Evaluate("+", null);
        }
        catch (ArgumentException)
        {
            Console.WriteLine("Test00 passed");
        }
        // test0.1
        try
        {
            Evaluator.Evaluate("2+5*7)", null);
        }
        catch (ArgumentException)
        {
            Console.WriteLine("Test0.1 passed");
        }
        // test0.2
        try
        {
            Evaluator.Evaluate("", null);
        }
        catch (ArgumentException)
        {
            Console.WriteLine("Test0.2 passed");
        }
        // test0.2
        try
        {
            Evaluator.Evaluate("2+5+", null);
        }
        catch (ArgumentException)
        {
            Console.WriteLine("Test0.3 passed");
        }
        //test20
        try
        {
            Evaluator.Evaluate("(5/0) + 3", null);
        }
        catch (ArgumentException)
        {
            Console.WriteLine("Test20 passed");
        }
    }
    /// <summary>
    /// Tests other exceptions like negative numbers being passed through or invalid formulas
    /// Contains tests 21-25
    /// </summary>
    public static void otherExceptions()
    {
       //test21
        try
        {
            Evaluator.Evaluate("(5/0 + 3", null);
        }
        catch (ArgumentException)
        {
            Console.WriteLine("Test21 passed");
        }
       //test22
        try
        {
            Evaluator.Evaluate("-5/5", null);
        }
        catch (ArgumentException)
        {
            Console.WriteLine("Test22 passed");
        }
       //test23
        try
        {
            Evaluator.Evaluate("5+(-5)", null);
        }
        catch (ArgumentException)
        {
            Console.WriteLine("Test23 passed");
        }
       //test24
        try
        {
            Evaluator.Evaluate("-5+(-5)", null);
        }
        catch (ArgumentException)
        {
            Console.WriteLine("Test24 passed");
        }
        //Test25
        try
        {
            Evaluator.Evaluate("-32 * 41", null);
        }
        catch (ArgumentException)
        {
            Console.WriteLine("Test25 passed");
        }
    }
    /// <summary>
    /// Tests medium equations.
    /// 3 operators or several sets of parenthesis.
    /// Contains tests 26-32
    /// </summary>
    public static void mediumEquations()
    {
        // Test26
        if (Evaluator.Evaluate("((1))", null) == 1)
            Console.WriteLine("Test26 Passed");
        // Test27
        if (Evaluator.Evaluate("(2 + 3) * 5 + 2", null) == 27)
            Console.WriteLine("Test27 Passed");
        // Test28
        if (Evaluator.Evaluate("((14 +32) - 2)", null) == 44)
            Console.WriteLine("Test28 Passed");
        // Test29
        if (Evaluator.Evaluate("((14  +32) -    2)", null) == 44)
            Console.WriteLine("Test29 Passed");
        // Test30
        if (Evaluator.Evaluate("24/(2+2)", null) == 6)
            Console.WriteLine("Test30 Passed");
        // Test31
        if (Evaluator.Evaluate("(12+12)/(2+2)", null) == 6)
            Console.WriteLine("Test31 Passed");
        // Test32
        if (Evaluator.Evaluate("(12+12)/(2*2)", null) == 6)
            Console.WriteLine("Test32 Passed");
    }
    /// <summary>
    /// Contains complex mathmatical equations including 3+ operators
    /// or many parenthesis. 
    /// Contains tests 33-42
    /// </summary>
    public static void complexEquations()
    {
        // Test33
        if (Evaluator.Evaluate("((5/5) * (13*4))/1", null) == 52)
            Console.WriteLine("Test33 Passed");

        // Test34
        if (Evaluator.Evaluate("(((14*32)/2) * (13*4))*3", null) == 34944)
            Console.WriteLine("Test34 Passed");
        // Test35
        if (Evaluator.Evaluate("(((14*32)/2) + (13*4))*3", null) == 828)
            Console.WriteLine("Test35 Passed");
        // Test36
        if (Evaluator.Evaluate("((14*32)/2) - (13*4))", null) == 172)
            Console.WriteLine("Test36 Passed");
        // Test37
        if (Evaluator.Evaluate("(24/(4/2))", null) == 12)
            Console.WriteLine("Test37 Passed");
        // Test38
        if (Evaluator.Evaluate("(24-(4/2))", null) == 22)
            Console.WriteLine("Test38 Passed");
        // Test39
        if (Evaluator.Evaluate("(24+(4/2))", null) == 26)
            Console.WriteLine("Test39 Passed");
        // Test40
        if (Evaluator.Evaluate("(24) * (3+1) - (4)", null) == 92)
            Console.WriteLine("Test40 Passed");
        // Test41
        if (Evaluator.Evaluate("(((24*3) / 3) + (3+(1/1))) - (4+ (4*4))", null) == 8)
            Console.WriteLine("Test41 Passed");
        //Test42 
        if (Evaluator.Evaluate("(((24*3) / 3) + (3+(1/1))) + (4+ (4*4))", null) == 48)
         Console.WriteLine("Test42 Passed");
    }
    /// <summary>
    /// Tests any equations with variables included.
    /// Contains tests 43 - 56 
    /// </summary>
    public static void delegateEquations()
    {
        //Test43
        if (Evaluator.Evaluate("5+A1", variableValues) == 26)
            Console.WriteLine("Test43 Passed");
        //Test44
        if (Evaluator.Evaluate("5-B2", variableValues) == 5)
            Console.WriteLine("Test44 Passed");
        //Test45
        if (Evaluator.Evaluate("(5+B2)*A1", variableValues) == 105)
            Console.WriteLine("Test45 Passed");
        //Test46
        if (Evaluator.Evaluate("5+D4", variableValues) == 7)
            Console.WriteLine("Test46 Passed");
        //Test46
        if (Evaluator.Evaluate("A1*3-8/2+4*(8-9*2)/14*B2", variableValues) == 8)
            Console.WriteLine("BADUM");
        // test47
        try
        {
            Evaluator.Evaluate("5+C3", variableValues);
        }
        catch (ArgumentException)
        {
            Console.WriteLine("Test47 passed");
        }
        // test48
        try
        {
            Evaluator.Evaluate("5/B2", variableValues);
        }
        catch (ArgumentException)
        {
            Console.WriteLine("Test48 passed");
        }
        // test49
        try
        {
            Evaluator.Evaluate("5/B4", variableValues);
        }
        catch (ArgumentException)
        {
            Console.WriteLine("Test49 passed");
        }
        //Test50
        if (Evaluator.Evaluate("D4+D4", variableValues) == 4)
            Console.WriteLine("Test50 Passed");
        //Test51
        if (Evaluator.Evaluate("A1-D4", variableValues) == 19)
            Console.WriteLine("Test51 Passed");
        //Test52
        if (Evaluator.Evaluate("D4/D4", variableValues) == 1)
            Console.WriteLine("Test52 Passed");
        //Test53
        if (Evaluator.Evaluate("D4*A1", variableValues) == 42)
            Console.WriteLine("Test53 Passed");
        //Test54
        if (Evaluator.Evaluate("(D4*A1)+B2", variableValues) == 42)
            Console.WriteLine("Test54 Passed");
        //Test55
        if (Evaluator.Evaluate("( D4*A1) + B2", variableValues) == 42)
            Console.WriteLine("Test55 Passed");
        //Test56
        if (Evaluator.Evaluate("( (A1-(3*3))/ (B2+D4))", variableValues) == 6)
            Console.WriteLine("Test56, Ultimate test, Passed");
    }
    /// <summary>
    /// Our delegate containing variable definitions.
    /// </summary>
    /// <param name="variable"></param> the value our library will send back to be checked. 
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>Throws this if the variable does not exist in the list
    public static int variableValues(String variable)
    {
        int result = 0;
        Dictionary<String, int> dictionary = new Dictionary<String, int>();

        dictionary.Add("A1", 4);
        dictionary.Add("B2", 0);
        dictionary.Add("C3", -5);
        dictionary.Add("D4", 2);

        if (dictionary.ContainsKey(variable) == true)
        {
            dictionary.TryGetValue(variable, out result);
        }
        else
            throw new ArgumentException();

        return result;
    }

}

