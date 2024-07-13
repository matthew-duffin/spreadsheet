using SpreadsheetUtilities;
using SS;
using System.Xml.Linq;
/// <summary>
/// Author:    Matthew Duffin
/// Partner:   None
/// Date:      February 17, 2023
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
///   This class creates tests my spreadsheet classes functionality 
/// 
namespace SS
{
    [TestClass]
    public class SpreadsheetTests
    {
        /// <summary>
        /// Tests that the circular dependency error is thrown. 
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(CircularException))]
        public void CircularDependencyTest()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("x1", "= x1 + x1");
        }
        /// <summary>
        /// Tests that the invalid name exception is thrown. 
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void invalidNameTest()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("x_", "32");
        }

        /// <summary>
        /// Tests that the invalid name exception is thrown. 
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void invalidNameTest2()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("_x%", "=32");
        }
        /// <summary>
        /// Tests that the invalid name exception is thrown. 
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void invalidNameTest3()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("12", "32.0");
        }

        /// <summary>
        /// Tests that we can get the names of all nonEmpty cells. 
        /// </summary>
        [TestMethod]
        public void GetNamesOfNonEmptyCellsFalseTest()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", "     ");
            IEnumerator<string> e1 = sheet.GetNamesOfAllNonemptyCells().GetEnumerator();

            Assert.IsFalse(e1.MoveNext());
        }
        /// <summary>
        /// Tests that we can get the names of all nonEmpty cells. 
        /// </summary>
        [TestMethod]
        public void GetNamesOfNonEmptyCellsFalseTest2()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", "");
            IEnumerator<string> e1 = sheet.GetNamesOfAllNonemptyCells().GetEnumerator();

            Assert.IsFalse(e1.MoveNext());
        }
        /// <summary>
        /// Tests that we can get the names of all nonEmpty cells. should return empty IEnum if no cells are set. 
        /// </summary>
        [TestMethod]
        public void GetNamesOfNonEmptyEmptyTest1()
        {
            Spreadsheet sheet = new Spreadsheet();
            IEnumerator<string> e1 = sheet.GetNamesOfAllNonemptyCells().GetEnumerator();
            Assert.IsFalse(e1.MoveNext());
        }
        /// <summary>
        /// Tests that we can get the names of all nonEmpty cells. 
        /// </summary>
        [TestMethod]
        public void GetNamesOfNonEmptyCellsTrueTest()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("j1", "x");
            sheet.SetContentsOfCell("y2", "42.0");
            sheet.SetContentsOfCell("C3", "= A1 + 23*83");
            IEnumerator<string> e1 = sheet.GetNamesOfAllNonemptyCells().GetEnumerator();

            Assert.IsTrue(e1.MoveNext());
            Assert.AreEqual("j1", e1.Current);
            Assert.IsTrue(e1.MoveNext());
            Assert.AreEqual("y2", e1.Current);
            Assert.IsTrue(e1.MoveNext());
            Assert.AreEqual("C3", e1.Current);
            Assert.IsFalse(e1.MoveNext());
        }
        /// <summary>
        /// Tests that we can get the names of all nonEmpty cells. 
        /// </summary>
        [TestMethod]
        public void GetNamesOfNonEmptyCellsFalseTestAndTrue()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("j1", "x");
            sheet.SetContentsOfCell("y1", "32.0");
            sheet.SetContentsOfCell("C3", "=23*83");
            sheet.SetContentsOfCell("C4", "   ");
            sheet.SetContentsOfCell("z3", "   ");
            sheet.SetContentsOfCell("w12", "23.0");
            IEnumerator<string> e1 = sheet.GetNamesOfAllNonemptyCells().GetEnumerator();

            Assert.IsTrue(e1.MoveNext());
            Assert.AreEqual("j1", e1.Current);
            Assert.IsTrue(e1.MoveNext());
            Assert.AreEqual("y1", e1.Current);
            Assert.IsTrue(e1.MoveNext());
            Assert.AreEqual("C3", e1.Current);
            Assert.IsTrue(e1.MoveNext());
            Assert.AreEqual("w12", e1.Current);
            Assert.IsFalse(e1.MoveNext());
        }
        /// <summary>
        /// Tests that we can get the contents of a cell based only on its name
        /// </summary>
        [TestMethod]
        public void GetCellsContentsTest()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("x1", "The");
            Assert.AreEqual("The", sheet.GetCellContents("x1"));
        }
        /// <summary>
        /// Tests that we can get the contents of a cell based only on its name
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void GetCellsContentsInvalidNameTest()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.GetCellContents("%x1");
        }
        /// <summary>
        /// Tests our get value method and sees if it can throw an invalid name exception
        /// </summary>
        [TestMethod]
        public void getValueEmptyCell()
        {
            Spreadsheet sheet = new Spreadsheet(validator, normalizer, "1.1");
            Assert.AreEqual("", sheet.GetCellValue("x3"));
        }
        /// <summary>
        /// Tests that we can get the contents of a cell based only on its name
        /// </summary>
        [TestMethod]
        public void GetCellsContentsNumberTest()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("x1", "23.0");
            Assert.AreEqual(23.0, sheet.GetCellContents("x1"));
        }
        /// <summary>
        /// Tests that we can get the contents of a cell based only on its name
        /// </summary>
        [TestMethod]
        public void GetCellsContentsFormulaTest()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("a1", "32.0");
            sheet.SetContentsOfCell("x1", "=32 + a1");
            Assert.AreEqual(new Formula("32+a1"), sheet.GetCellContents("x1"));
        }
        /// <summary>
        /// Tests that we can set the contents of a cell to a String
        /// </summary>
        [TestMethod]
        public void setCellsContentsNameAndStringTest()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("x1", "Hello");

            Assert.AreEqual("Hello", (String)sheet.GetCellContents("x1"));
        }
        /// <summary>
        /// Tests that we can set the contents of a cell to a Formula
        /// </summary>
        [TestMethod]
        public void setCellsContentsNameAndFormulaTest()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("x1", "= 2 +2");

            Assert.AreEqual(new Formula("2+2"), (Formula)sheet.GetCellContents("x1"));
        }
        /// <summary>
        /// Tests that we can set the contents of a cell to an invalid name and get an error back
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void getEmptyCellContentsInvalidNameTest()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.GetCellContents("&4");
        }

        /// <summary>
        /// Tests that we can set the contents of a cell to a number
        /// </summary>
        [TestMethod]
        public void setCellsContentsNameAndNumberTest2()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("x1", "23.0");
            sheet.SetContentsOfCell("x1", "1.0");
            Assert.AreEqual(1.0, (double)sheet.GetCellContents("x1"));
        }
        /// <summary>
        /// Tests that we can set the contents of a cell to a String
        /// </summary>
        [TestMethod]
        public void setCellsContentsNameAndStringTest2()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("x1", "32");
            sheet.SetContentsOfCell("x1", "forty");
            Assert.AreEqual("forty", (String)sheet.GetCellContents("x1"));
        }
        /// <summary>
        /// Tests that we can set the contents of a cell to a Formula
        /// </summary>
        [TestMethod]
        public void setCellsContentsNameAndFormulaTest2()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("x1", "= 2*323");
            sheet.SetContentsOfCell("x1", "=A1 + 23");
            Assert.AreEqual(new Formula("A1+23"), (Formula)sheet.GetCellContents("x1"));
        }
        /// <summary>
        /// Tests that we can set the contents of a cell to a String
        /// and that the values returned are correct. 
        /// </summary>
        [TestMethod]
        public void setCellsContentsSetTest()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("x1", "23.0");
            sheet.SetContentsOfCell("y1", "=x1");
            IList<string> h = sheet.SetContentsOfCell("x1", "4.0");
            Assert.IsTrue(h.Contains("y1"));
            Assert.IsTrue(h.Contains("x1")); // checks that the original value was added 
        }
        /// <summary>
        /// Tests that we can set the contents of a cell to a String
        /// and that the values returned are correct. 
        /// </summary>
        [TestMethod]
        public void setCellsContentsSetTest2()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("x1", "3.0");
            sheet.SetContentsOfCell("y1", "6.0");
            sheet.SetContentsOfCell("g1", "= x1 / y1 ");
            sheet.SetContentsOfCell("w1", "=x1");
            sheet.SetContentsOfCell("w2", "= x1*4");
            IList<string> h = sheet.SetContentsOfCell("x1", "4.0");

            Assert.IsTrue(h.Contains("x1"));
            Assert.IsTrue(h.Contains("g1"));
            Assert.IsTrue(h.Contains("w1"));
            Assert.IsTrue(h.Contains("w2"));
            Assert.IsFalse(h.Contains("y1"));
        }
        /// <summary>
        /// Tests that we can set the contents of a cell to a String
        /// and that the values returned are correct. 
        /// </summary>
        [TestMethod]
        public void setCellsContentsSetTest3()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("x1", "3.0");
            sheet.SetContentsOfCell("y1", "6.0");
            sheet.SetContentsOfCell("g1", "= x1 / y1 ");
            sheet.SetContentsOfCell("w1", "=x1");
            sheet.SetContentsOfCell("w2", "= x1*4");
            IList<string> h = sheet.SetContentsOfCell("y1", "4.0");

            Assert.IsTrue(h.Contains("y1"));
            Assert.IsTrue(h.Contains("g1"));
            Assert.IsFalse(h.Contains("w1"));
            Assert.IsFalse(h.Contains("w2"));
            Assert.IsFalse(h.Contains("x1"));
        }
        /// <summary>
        /// Tests that we can set the contents of a cell to a String
        /// and that the values returned are correct. 
        /// </summary>
        [TestMethod]
        public void setCellsContentsSetTest4()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("x1", "3.0");
            sheet.SetContentsOfCell("y1", "6.0");
            sheet.SetContentsOfCell("g1", "=y1 / x ");
            sheet.SetContentsOfCell("w1", "=y1");
            sheet.SetContentsOfCell("w2", "=y1*4");
            IList<string> h = sheet.SetContentsOfCell("y1", "x1");

            Assert.IsTrue(h.Contains("y1"));
            Assert.IsTrue(h.Contains("g1"));
            Assert.IsTrue(h.Contains("w1"));
            Assert.IsTrue(h.Contains("w2"));
            Assert.IsFalse(h.Contains("x1"));
        }
        /// <summary>
        /// Tests that we can set the contents of a cell to a String
        /// and that the values returned are correct. 
        /// </summary>
        [TestMethod]
        public void setCellsContentsSetTest5()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("x1", "3.0");
            sheet.SetContentsOfCell("y1", "6.0");
            sheet.SetContentsOfCell("g1", "=y1/ x ");
            sheet.SetContentsOfCell("w1", "=y1");
            sheet.SetContentsOfCell("w2", "=y1*4");
            IList<string> h = sheet.SetContentsOfCell("y1", "=x+2");

            Assert.IsTrue(h.Contains("y1"));
            Assert.IsTrue(h.Contains("g1"));
            Assert.IsTrue(h.Contains("w1"));
            Assert.IsTrue(h.Contains("w2"));
            Assert.IsFalse(h.Contains("x1"));
        }
        /// <summary>
        /// Tests that we can set the cell to an invalid name that we get an error
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void setCellsContentsInvalidNameTest()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("#x", "");
        }
        /// <summary>
        /// Tests that we can set the contents of a cell to a String
        /// and that the values returned are correct. 
        /// </summary>
        [TestMethod]
        public void getEmptyCellContentsTest()
        {
            Spreadsheet sheet = new Spreadsheet();
            Assert.AreEqual("", sheet.GetCellContents("A1"));
        }
        /// <summary>
        /// Tests our get value method
        /// </summary>
        [TestMethod]
        public void getValueTest()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("x1", "= 2+2");
            Assert.AreEqual(4.0, sheet.GetCellValue("x1"));
        }
        /// <summary>
        /// Tests our get value method
        /// </summary>
        [TestMethod]
        public void getValueDoubleTest()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("x1", "2.0");
            Assert.AreEqual(2.0, sheet.GetCellValue("x1"));
        }
        /// <summary>
        /// Tests our get value method
        /// </summary>
        [TestMethod]
        public void getValueDoubleTest2()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("x1", "2");
            Assert.AreEqual(2.0, sheet.GetCellValue("x1"));
        }
        /// <summary>
        /// Tests our get value method
        /// </summary>
        [TestMethod]
        public void getValueStringTest()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("x1", "hello, I'm Mister Frog");
            Assert.AreEqual("hello, I'm Mister Frog", sheet.GetCellValue("x1"));
        }
        /// <summary>
        /// Tests our get value method
        /// </summary>
        [TestMethod]
        public void getValueTestVariable()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("x1", "2");
            sheet.SetContentsOfCell("x2", "= x1+4");
            Assert.AreEqual(6.0, sheet.GetCellValue("x2"));
        }
        /// <summary>
        /// Tests our get value method
        /// </summary>
        [TestMethod]
        public void getValueTestVariableComplex()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("x1", "2"); //2
            sheet.SetContentsOfCell("x2", "= x1+4");//6
            Assert.AreEqual(6.0, sheet.GetCellValue("x2"));

            sheet.SetContentsOfCell("x3", "=x2*x1/x1");//6
            Assert.AreEqual(6.0, sheet.GetCellValue("x3"));

            sheet.SetContentsOfCell("x4", "= x3*x2-x1"); //34
            Assert.AreEqual(34.0, sheet.GetCellValue("x4"));

            sheet.SetContentsOfCell("x5", "=x4*x1+x2+x3"); //80
            Assert.AreEqual(80.0, sheet.GetCellValue("x5"));

            sheet.SetContentsOfCell("x6", "= x1/x1*x4+x2-x3+x5");//114
            Assert.AreEqual(114.0, sheet.GetCellValue("x6"));

            sheet.SetContentsOfCell("x7", "=x6-x1+x2+x3+x4+x5"); //238
            Assert.AreEqual(238.0, sheet.GetCellValue("x7"));

            sheet.SetContentsOfCell("x8", "= x7+x3*x4+x5-x2+x1*x6");// 744
            Assert.AreEqual(744.0, sheet.GetCellValue("x8"));
        }
        /// <summary>
        /// Tests our get value method
        /// </summary>
        [TestMethod]
        public void getValueTestVariableComplex2()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("x1", "2"); //2
            sheet.SetContentsOfCell("x2", "= x1+4");//6
            Assert.AreEqual(6.0, sheet.GetCellValue("x2"));

            sheet.SetContentsOfCell("x3", "=x2*2/x1");//6
            Assert.AreEqual(6.0, sheet.GetCellValue("x3"));

            sheet.SetContentsOfCell("x4", "= x3*x2-2"); //34
            Assert.AreEqual(34.0, sheet.GetCellValue("x4"));

            sheet.SetContentsOfCell("x5", "=x4*x1+x2+6"); //80
            Assert.AreEqual(80.0, sheet.GetCellValue("x5"));

            sheet.SetContentsOfCell("x6", "= x1/x1*34+x2-x3+x5");//114
            Assert.AreEqual(114.0, sheet.GetCellValue("x6"));

            sheet.SetContentsOfCell("x7", "=x6-x1+x2+x3+x4+80"); //238
            Assert.AreEqual(238.0, sheet.GetCellValue("x7"));

            sheet.SetContentsOfCell("x8", "= x7+x3*x4+x5-x2+x1*114");// 744
            Assert.AreEqual(744.0, sheet.GetCellValue("x8"));
        }
        /// <summary>
        /// Tests our get value method
        /// </summary>
        [TestMethod]
        public void getValueTestVariableComplex3()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("x1", "2"); //2 //4
            sheet.SetContentsOfCell("x2", "= x1+4");//6 //8
            Assert.AreEqual(6.0, sheet.GetCellValue("x2"));

            sheet.SetContentsOfCell("x3", "=x2*x1/x1");//6 //8
            Assert.AreEqual(6.0, sheet.GetCellValue("x3"));

            sheet.SetContentsOfCell("x4", "= x3*x2-x1"); //34 //60
            Assert.AreEqual(34.0, sheet.GetCellValue("x4"));

            sheet.SetContentsOfCell("x5", "=x4*x1+x2+x3"); //80 //256
            Assert.AreEqual(80.0, sheet.GetCellValue("x5"));

            sheet.SetContentsOfCell("x6", "= x1/x1*x4+x2-x3+x5");//114 //316
            Assert.AreEqual(114.0, sheet.GetCellValue("x6"));

            sheet.SetContentsOfCell("x7", "=x6-x1+x2+x3+x4+x5"); //238 //644
            Assert.AreEqual(238.0, sheet.GetCellValue("x7"));

            sheet.SetContentsOfCell("x8", "= x7+x3*x4+x5-x2+x1*x6");// 744 //2636
            Assert.AreEqual(744.0, sheet.GetCellValue("x8"));

            sheet.SetContentsOfCell("x1", "4.0");
            Assert.AreEqual(8.0, sheet.GetCellValue("x2"));
            Assert.AreEqual(8.0, sheet.GetCellValue("x3"));
            Assert.AreEqual(60.0, sheet.GetCellValue("x4"));
            Assert.AreEqual(256.0, sheet.GetCellValue("x5"));
            Assert.AreEqual(316.0, sheet.GetCellValue("x6"));
            Assert.AreEqual(644.0, sheet.GetCellValue("x7"));
            Assert.AreEqual(2636.0, sheet.GetCellValue("x8"));
        }
        /// <summary>
        /// Checks we get an exception from getting from an invalid name.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void getValueInvalidName()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("x", "2");
        }
        /// <summary>
        /// Checks that we can write to a file
        /// </summary>
        [TestMethod]
        public void saveTest()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("x1", "2");
            sheet.Save("firstTest.xml");
        }
        /// <summary>
        /// Checks that we can write to a file
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void saveTestBadFile()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("x1", "2");
            sheet.Save("/some/nonsense/path.xml");
        }
        /// <summary>
        /// Checks that we can write to a file
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void saveTestBadFile2()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("x1", "2");
            sheet.Save("/some/nonsense/path.xml");
        }
        /// <summary>
        /// Checks that we can write to a file
        /// </summary>
        [TestMethod]
        public void changedTest()
        {
            Spreadsheet sheet = new Spreadsheet();
            Assert.IsFalse(sheet.Changed);
            sheet.SetContentsOfCell("x1", "2");
            Assert.IsTrue(sheet.Changed);
            sheet.Save("firstTest.xml");
            Assert.IsFalse(sheet.Changed);
        }
        /// <summary>
        /// Checks that we can write to a file
        /// </summary>
        [TestMethod]
        public void saveTestString()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("x1", "Hello, I'm Mister Frog");
            sheet.Save("firstTest.xml");
        }
        /// <summary>
        /// Checks that we can write to a file
        /// </summary>
        [TestMethod]
        public void saveTestFormula()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("x1", "=x2 + 45 * 60/60");
            sheet.Save("firstTest.xml");
        }
        /// <summary>
        /// Checks that we can write to a file
        /// </summary>
        [TestMethod]
        public void saveTestFormulaComplex()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("x1", "2"); //2 //4
            sheet.SetContentsOfCell("x2", "= x1+4");//6 //8
            Assert.AreEqual(6.0, sheet.GetCellValue("x2"));

            sheet.SetContentsOfCell("x3", "=x2*x1/x1");//6 //8
            Assert.AreEqual(6.0, sheet.GetCellValue("x3"));

            sheet.SetContentsOfCell("x4", "= x3*x2-x1"); //34 //60
            Assert.AreEqual(34.0, sheet.GetCellValue("x4"));

            sheet.SetContentsOfCell("x5", "=x4*x1+x2+x3"); //80 //256
            Assert.AreEqual(80.0, sheet.GetCellValue("x5"));

            sheet.SetContentsOfCell("x6", "= x1/x1*x4+x2-x3+x5");//114 //316
            Assert.AreEqual(114.0, sheet.GetCellValue("x6"));

            sheet.SetContentsOfCell("x7", "=x6-x1+x2+x3+x4+x5"); //238 //644
            Assert.AreEqual(238.0, sheet.GetCellValue("x7"));

            sheet.SetContentsOfCell("x8", "= x7+x3*x4+x5-x2+x1*x6");// 744 //2636
            Assert.AreEqual(744.0, sheet.GetCellValue("x8"));
            sheet.Save("firstTest.xml");
        }
        /// <summary>
        /// Checks that we can create a spreadsheet from constructor 3
        /// </summary>
        [TestMethod]
        public void GetSavedVersionTest()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("a1", "=a2 + 45 * 60/60");
            sheet.Save("firstTest.xml");
            Assert.AreEqual("default", sheet.GetSavedVersion("firstTest.xml"));
        }
        /// <summary>
        /// Checks that we can create a spreadsheet from constructor 3
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void GetSavedVersionBadFileTest()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("a1", "=a2 + 45 * 60/60");
            sheet.Save("firstTest.xml");
            sheet.GetSavedVersion("/some/nonsense/path.xml");
        }
        // ***CONSTRUCTOR 2 Tests*** //
        // ALL TESTS BELOW THIS POINT//
        // TEST THE SECOND CONSTRUCTOR//

        /// <summary>
        /// Tests that the circular dependency error is thrown. 
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(CircularException))]
        public void CircularDependencyConstructor2Test()
        {
            Spreadsheet sheet = new Spreadsheet(validator, normalizer, "1.1");
            sheet.SetContentsOfCell("a1", "= a1 + a1");
        }
        /// <summary>
        /// Tests that the invalid name exception is thrown. 
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void invalidNameTestConstructor2()
        {
            Spreadsheet sheet = new Spreadsheet(validator, normalizer, "1.1");
            sheet.SetContentsOfCell("A_", "32");
        }
        /// <summary>
        /// Tests that the invalid name exception is thrown. 
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void invalidNameTest2Constructor2()
        {
            Spreadsheet sheet = new Spreadsheet(validator, normalizer, "1.1");
            sheet.SetContentsOfCell("_a%", "=32");
        }
        /// <summary>
        /// Tests that the invalid name exception is thrown. 
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void invalidNameTest3Constructor2()
        {
            Spreadsheet sheet = new Spreadsheet(validator, normalizer, "1.1");
            sheet.SetContentsOfCell("X1", "32.0");
        }

        /// <summary>
        /// Tests that we can get the names of all nonEmpty cells. 
        /// </summary>
        [TestMethod]
        public void GetNamesOfNonEmptyCellsFalseTestConstructor2()
        {
            Spreadsheet sheet = new Spreadsheet(validator, normalizer, "1.1");
            sheet.SetContentsOfCell("a1", "     ");
            IEnumerator<string> e1 = sheet.GetNamesOfAllNonemptyCells().GetEnumerator();

            Assert.IsFalse(e1.MoveNext());
        }
        /// <summary>
        /// Tests that we can get the names of all nonEmpty cells. 
        /// </summary>
        [TestMethod]
        public void GetNamesOfNonEmptyCellsFalseTest2Constructor2()
        {
            Spreadsheet sheet = new Spreadsheet(validator, normalizer, "1.1");
            sheet.SetContentsOfCell("A1", "");
            IEnumerator<string> e1 = sheet.GetNamesOfAllNonemptyCells().GetEnumerator();

            Assert.IsFalse(e1.MoveNext());
        }
        /// <summary>
        /// Tests that we can get the names of all nonEmpty cells. should return empty IEnum if no cells are set. 
        /// </summary>
        [TestMethod]
        public void GetNamesOfNonEmptyEmptyTest1Constructor2()
        {
            Spreadsheet sheet = new Spreadsheet(validator, normalizer, "1.1");
            IEnumerator<string> e1 = sheet.GetNamesOfAllNonemptyCells().GetEnumerator();
            Assert.IsFalse(e1.MoveNext());
        }
        /// <summary>
        /// Tests that we can get the names of all nonEmpty cells. 
        /// </summary>
        [TestMethod]
        public void GetNamesOfNonEmptyCellsTrueTestConstructor2()
        {
            Spreadsheet sheet = new Spreadsheet(validator, normalizer, "1.1");
            sheet.SetContentsOfCell("A1", "32.0");
            sheet.SetContentsOfCell("a2", "42.0");
            sheet.SetContentsOfCell("A3", "= A1 + 23*83");
            IEnumerator<string> e1 = sheet.GetNamesOfAllNonemptyCells().GetEnumerator();

            Assert.IsTrue(e1.MoveNext());
            Assert.AreEqual("A1", e1.Current);
            Assert.IsTrue(e1.MoveNext());
            Assert.AreEqual("A2", e1.Current);
            Assert.IsTrue(e1.MoveNext());
            Assert.AreEqual("A3", e1.Current);
            Assert.IsFalse(e1.MoveNext());
        }
        /// <summary>
        /// Tests that we can get the names of all nonEmpty cells. 
        /// </summary>
        [TestMethod]
        public void GetNamesOfNonEmptyCellsFalseTestAndTrueConstructor2()
        {
            Spreadsheet sheet = new Spreadsheet(validator, normalizer, "1.1");
            sheet.SetContentsOfCell("A1", "2.0");
            sheet.SetContentsOfCell("a2", "32.0");
            sheet.SetContentsOfCell("A3", "=23*83");
            sheet.SetContentsOfCell("A4", "   ");
            sheet.SetContentsOfCell("A5", "   ");
            sheet.SetContentsOfCell("A12", "23.0");
            IEnumerator<string> e1 = sheet.GetNamesOfAllNonemptyCells().GetEnumerator();

            Assert.IsTrue(e1.MoveNext());
            Assert.AreEqual("A1", e1.Current);
            Assert.IsTrue(e1.MoveNext());
            Assert.AreEqual("A2", e1.Current);
            Assert.IsTrue(e1.MoveNext());
            Assert.AreEqual("A3", e1.Current);
            Assert.IsTrue(e1.MoveNext());
            Assert.AreEqual("A12", e1.Current);
            Assert.IsFalse(e1.MoveNext());
        }
        /// <summary>
        /// Tests that we can get the contents of a cell based only on its name
        /// </summary>
        [TestMethod]
        public void GetCellsContentsTestConstructor2()
        {
            Spreadsheet sheet = new Spreadsheet(validator, normalizer, "1.1");
            sheet.SetContentsOfCell("a1", "The");
            Assert.AreEqual("The", sheet.GetCellContents("a1"));
        }
        /// <summary>
        /// Tests that we can get the contents of a cell based only on its name
        /// </summary>
        [TestMethod]
        public void GetCellsContentsNumberTestConstructor2()
        {
            Spreadsheet sheet = new Spreadsheet(validator, normalizer, "1.1");
            sheet.SetContentsOfCell("a1", "23.0");
            Assert.AreEqual(23.0, sheet.GetCellContents("a1"));
        }
        /// <summary>
        /// Tests that we can get the contents of a cell based only on its name
        /// </summary>
        [TestMethod]
        public void GetCellsContentsFormulaTestConstructor2()
        {
            Spreadsheet sheet = new Spreadsheet(validator, normalizer, "1.1");
            sheet.SetContentsOfCell("a1", "=32 + 2");
            Assert.AreEqual(new Formula("32+A2"), sheet.GetCellContents("a1"));
        }
        /// <summary>
        /// Tests that we can set the contents of a cell to a String
        /// </summary>
        [TestMethod]
        public void setCellsContentsNameAndStringTestConstructor2()
        {
            Spreadsheet sheet = new Spreadsheet(validator, normalizer, "1.1");
            sheet.SetContentsOfCell("a1", "Hello");

            Assert.AreEqual("Hello", (String)sheet.GetCellContents("a1"));
        }
        /// <summary>
        /// Tests that we can set the contents of a cell to a Formula
        /// </summary>
        [TestMethod]
        public void setCellsContentsNameAndFormulaTestConstructor2()
        {
            Spreadsheet sheet = new Spreadsheet(validator, normalizer, "1.1");
            sheet.SetContentsOfCell("a1", "= a2 +2");

            Assert.AreEqual(new Formula("A2+2"), (Formula)sheet.GetCellContents("a1"));
        }
        /// <summary>
        /// Tests that we can set the contents of a cell to an invalid name and get an error back
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void getEmptyCellContentsInvalidNameTestConstructor2()
        {
            Spreadsheet sheet = new Spreadsheet(validator, normalizer, "1.1");
            sheet.GetCellContents("&4");
        }

        /// <summary>
        /// Tests that we can set the contents of a cell to a number
        /// </summary>
        [TestMethod]
        public void setCellsContentsNameAndNumberTest2Constructor2()
        {
            Spreadsheet sheet = new Spreadsheet(validator, normalizer, "1.1");
            sheet.SetContentsOfCell("a1", "23.0");
            sheet.SetContentsOfCell("a1", "1.0");
            Assert.AreEqual(1.0, (double)sheet.GetCellContents("a1"));
        }
        /// <summary>
        /// Tests that we can set the contents of a cell to a String
        /// </summary>
        [TestMethod]
        public void setCellsContentsNameAndStringTest2Constructor2()
        {
            Spreadsheet sheet = new Spreadsheet(validator, normalizer, "1.1");
            sheet.SetContentsOfCell("a1", "32");
            sheet.SetContentsOfCell("a1", "forty");
            Assert.AreEqual("forty", (String)sheet.GetCellContents("A1"));
        }
        /// <summary>
        /// Tests that we can set the contents of a cell to a Formula
        /// </summary>
        [TestMethod]
        public void setCellsContentsNameAndFormulaTest2Constructor2()
        {
            Spreadsheet sheet = new Spreadsheet(validator, normalizer, "1.1");
            sheet.SetContentsOfCell("a1", "= 2*323");
            sheet.SetContentsOfCell("a1", "=A2 + 23");
            Assert.AreEqual(new Formula("A2+23"), (Formula)sheet.GetCellContents("A1"));
        }
        /// <summary>
        /// Tests that we can set the contents of a cell to a String
        /// and that the values returned are correct. 
        /// </summary>
        [TestMethod]
        public void setCellsContentsSetTestConstructor2()
        {
            Spreadsheet sheet = new Spreadsheet(validator, normalizer, "1.1");
            sheet.SetContentsOfCell("a1", "23.0");
            sheet.SetContentsOfCell("a2", "=a1");
            IList<string> h = sheet.SetContentsOfCell("a1", "4.0");
            Assert.IsTrue(h.Contains("A2"));
            Assert.IsTrue(h.Contains("A1")); // checks that the original value was added 
        }
        /// <summary>
        /// Tests that we can set the contents of a cell to a String
        /// and that the values returned are correct. 
        /// </summary>
        [TestMethod]
        public void setCellsContentsSetTest6Constructor2()
        {
            Spreadsheet sheet = new Spreadsheet(validator, normalizer, "1.1");
            sheet.SetContentsOfCell("a1", "23.0");
            IList<string> h = sheet.SetContentsOfCell("a2", "a1"); ;
            Assert.AreEqual(1, h.Count);
        }
        /// <summary>
        /// Tests that we can set the contents of a cell to a String
        /// and that the values returned are correct. 
        /// </summary>
        [TestMethod]
        public void setCellsContentsSetTest2Constructor2()
        {
            Spreadsheet sheet = new Spreadsheet(validator, normalizer, "1.1");
            sheet.SetContentsOfCell("a1", "3.0");
            sheet.SetContentsOfCell("a2", "6.0");
            sheet.SetContentsOfCell("a3", "= a1 / a2 ");
            sheet.SetContentsOfCell("a4", "=a1");
            sheet.SetContentsOfCell("a523", "= a1*4");
            IList<string> h = sheet.SetContentsOfCell("a1", "4.0");

            Assert.IsTrue(h.Contains("A1"));
            Assert.IsTrue(h.Contains("A3"));
            Assert.IsTrue(h.Contains("A4"));
            Assert.IsTrue(h.Contains("A523"));
            Assert.IsFalse(h.Contains("A2"));
        }
        /// <summary>
        /// Tests that we can set the contents of a cell to a String
        /// and that the values returned are correct. 
        /// </summary>
        [TestMethod]
        public void setCellsContentsSetTest3Constructor2()
        {
            Spreadsheet sheet = new Spreadsheet(validator, normalizer, "1.1");
            sheet.SetContentsOfCell("a1", "3.0");
            sheet.SetContentsOfCell("a2", "6.0");
            sheet.SetContentsOfCell("a3", "= a1 / A2 ");
            sheet.SetContentsOfCell("a4", "=A1");
            sheet.SetContentsOfCell("a526", "= a1*4");
            IList<string> h = sheet.SetContentsOfCell("A2", "4.0");

            Assert.IsTrue(h.Contains("A2"));
            Assert.IsTrue(h.Contains("A3"));
            Assert.IsFalse(h.Contains("A4"));
            Assert.IsFalse(h.Contains("A526"));
            Assert.IsFalse(h.Contains("A1"));
            Assert.IsFalse(h.Contains("a1"));
        }
        /// <summary>
        /// Tests that we can set the contents of a cell to a String
        /// and that the values returned are correct. 
        /// </summary>
        [TestMethod]
        public void setCellsContentsSetTest4Constructor2()
        {
            Spreadsheet sheet = new Spreadsheet(validator, normalizer, "1.1");
            sheet.SetContentsOfCell("a1", "3.0");
            sheet.SetContentsOfCell("A2", "6.0");
            sheet.SetContentsOfCell("A3", "=a2 / a56 ");
            sheet.SetContentsOfCell("A4", "=a2");
            sheet.SetContentsOfCell("A526", "=A2*4");
            IList<string> h = sheet.SetContentsOfCell("A2", "A56");

            Assert.IsTrue(h.Contains("A2"));
            Assert.IsTrue(h.Contains("A3"));
            Assert.IsTrue(h.Contains("A4"));
            Assert.IsTrue(h.Contains("A526"));
            Assert.IsFalse(h.Contains("A1") && h.Contains("a1"));
        }
        /// <summary>
        /// Tests that we can set the contents of a cell to a String
        /// and that the values returned are correct. 
        /// </summary>
        [TestMethod]
        public void setCellsContentsSetTest5Constructor2()
        {
            Spreadsheet sheet = new Spreadsheet(validator, normalizer, "1.1");
            sheet.SetContentsOfCell("A1", "3.0");
            sheet.SetContentsOfCell("a2", "6.0");
            sheet.SetContentsOfCell("A3", "=A2/ a1 ");
            sheet.SetContentsOfCell("a4", "=A2");
            sheet.SetContentsOfCell("a526", "=a2*4");
            IList<string> h = sheet.SetContentsOfCell("A2", "=a1+2");

            Assert.IsTrue(h.Contains("A2"));
            Assert.IsTrue(h.Contains("A3"));
            Assert.IsTrue(h.Contains("A4"));
            Assert.IsTrue(h.Contains("A526"));
            Assert.IsFalse(h.Contains("a1") && h.Contains("A1"));
        }
        /// <summary>
        /// Tests that we can set the cell to an invalid name that we get an error
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void setCellsContentsInvalidNameTestConstructor2()
        {
            Spreadsheet sheet = new Spreadsheet(validator, normalizer, "1.1");
            sheet.SetContentsOfCell("#A", "");
        }
        /// <summary>
        /// Tests that we can set the cell to an invalid name that we get an error
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void setCellsContentsInvalidNameTest2Constructor2()
        {
            Spreadsheet sheet = new Spreadsheet(validator, normalizer, "1.1");
            sheet.SetContentsOfCell("A", "");
        }
        /// <summary>
        /// Tests that we can set the contents of a cell to a String
        /// and that the values returned are correct. 
        /// </summary>
        [TestMethod]
        public void getEmptyCellContentsContructor2Test()
        {
            Spreadsheet sheet = new Spreadsheet(validator, normalizer, "1.1");
            Assert.AreEqual("", sheet.GetCellContents("A1"));
        }
        /// <summary>
        /// Tests our get value method
        /// </summary>
        [TestMethod]
        public void getValueTestConstructor2()
        {
            Spreadsheet sheet = new Spreadsheet(validator, normalizer, "1.1");
            sheet.SetContentsOfCell("a1", "= 2+2");
            Assert.AreEqual(4.0, sheet.GetCellValue("A1"));
        }
        /// <summary>
        /// Tests our get value method
        /// </summary>
        [TestMethod]
        public void getValueDoubleTestConstructor2()
        {
            Spreadsheet sheet = new Spreadsheet(validator, normalizer, "1.1");
            sheet.SetContentsOfCell("a1", "2.0");
            Assert.AreEqual(2.0, sheet.GetCellValue("a1"));
        }
        /// <summary>
        /// Tests our get value method
        /// </summary>
        [TestMethod]
        public void getValueDoubleTest2Constructor2()
        {
            Spreadsheet sheet = new Spreadsheet(validator, normalizer, "1.1");
            sheet.SetContentsOfCell("a1", "2");
            Assert.AreEqual(2.0, sheet.GetCellValue("A1"));
        }
        /// <summary>
        /// Tests our get value method
        /// </summary>
        [TestMethod]
        public void getValueStringTestConstructor2()
        {
            Spreadsheet sheet = new Spreadsheet(validator, normalizer, "1.1");
            sheet.SetContentsOfCell("a1", "hello, I'm Mister Frog");
            Assert.AreEqual("hello, I'm Mister Frog", sheet.GetCellValue("a1"));
        }
        /// <summary>
        /// Tests our get value method
        /// </summary>
        [TestMethod]
        public void getValueTestVariableConstructor2()
        {
            Spreadsheet sheet = new Spreadsheet(validator, normalizer, "1.1");
            sheet.SetContentsOfCell("a1", "2");
            sheet.SetContentsOfCell("a2", "= A1+4");
            Assert.AreEqual(6.0, sheet.GetCellValue("a2"));
        }
        /// <summary>
        /// Tests our get value method
        /// </summary>
        [TestMethod]
        public void getValueTestVariableComplexConstructor2()
        {
            Spreadsheet sheet = new Spreadsheet(validator, normalizer, "1.1");
            sheet.SetContentsOfCell("a1", "2"); //2
            sheet.SetContentsOfCell("a2", "= a1+4");//6
            Assert.AreEqual(6.0, sheet.GetCellValue("a2"));

            sheet.SetContentsOfCell("a3", "=A2*A1/a1");//6
            Assert.AreEqual(6.0, sheet.GetCellValue("A3"));

            sheet.SetContentsOfCell("A4", "= A3*a2-a1"); //34
            Assert.AreEqual(34.0, sheet.GetCellValue("a4"));

            sheet.SetContentsOfCell("A5", "=a4*A1+a2+A3"); //80
            Assert.AreEqual(80.0, sheet.GetCellValue("A5"));

            sheet.SetContentsOfCell("A6", "= A1/A1*A4+A2-A3+A5");//114
            Assert.AreEqual(114.0, sheet.GetCellValue("A6"));

            sheet.SetContentsOfCell("A7", "=a6-A1+A2+A3+a4+a5"); //238
            Assert.AreEqual(238.0, sheet.GetCellValue("a7"));

            sheet.SetContentsOfCell("a8", "= a7+a3*a4+a5-a2+a1*a6");// 744
            Assert.AreEqual(744.0, sheet.GetCellValue("a8"));
        }
        /// <summary>
        /// Tests our get value method
        /// </summary>
        [TestMethod]
        public void getValueTestVariableComplex3Constructor2()
        {
            Spreadsheet sheet = new Spreadsheet(validator, normalizer, "1.1");
            sheet.SetContentsOfCell("a1", "2"); //2
            sheet.SetContentsOfCell("a2", "= a1+4");//6
            Assert.AreEqual(6.0, sheet.GetCellValue("a2"));

            sheet.SetContentsOfCell("a3", "=A2*A1/a1");//6
            Assert.AreEqual(6.0, sheet.GetCellValue("A3"));

            sheet.SetContentsOfCell("A4", "= A3*a2-a1"); //34
            Assert.AreEqual(34.0, sheet.GetCellValue("a4"));

            sheet.SetContentsOfCell("A5", "=a4*A1+a2+A3"); //80
            Assert.AreEqual(80.0, sheet.GetCellValue("A5"));

            sheet.SetContentsOfCell("A6", "= A1/A1*A4+A2-A3+A5");//114
            Assert.AreEqual(114.0, sheet.GetCellValue("A6"));

            sheet.SetContentsOfCell("A7", "=a6-A1+A2+A3+a4+a5"); //238
            Assert.AreEqual(238.0, sheet.GetCellValue("a7"));

            sheet.SetContentsOfCell("a8", "= a7+a3*a4+a5-a2+a1*a6");// 744
            Assert.AreEqual(744.0, sheet.GetCellValue("a8"));

            sheet.SetContentsOfCell("a1", "4.0");
            Assert.AreEqual(8.0, sheet.GetCellValue("a2"));
            Assert.AreEqual(8.0, sheet.GetCellValue("a3"));
            Assert.AreEqual(60.0, sheet.GetCellValue("a4"));
            Assert.AreEqual(256.0, sheet.GetCellValue("a5"));
            Assert.AreEqual(316.0, sheet.GetCellValue("a6"));
            Assert.AreEqual(644.0, sheet.GetCellValue("a7"));
            Assert.AreEqual(2636.0, sheet.GetCellValue("a8"));
        }
        /// <summary>
        /// Tests our get value method
        /// </summary>
        [TestMethod]
        public void getValueTestVariableComplex2Constructor2()
        {
            Spreadsheet sheet = new Spreadsheet(validator, normalizer, "1.1");
            sheet.SetContentsOfCell("a1", "2"); //2
            sheet.SetContentsOfCell("A2", "= a1+4");//6
            Assert.AreEqual(6.0, sheet.GetCellValue("a2"));

            sheet.SetContentsOfCell("A3", "=a2*2/a1");//6
            Assert.AreEqual(6.0, sheet.GetCellValue("A3"));

            sheet.SetContentsOfCell("a4", "=a3 * a2-2"); //34
            Assert.AreEqual(34.0, sheet.GetCellValue("a4"));

            sheet.SetContentsOfCell("a5", "=a4*A1+A2+6"); //80
            Assert.AreEqual(80.0, sheet.GetCellValue("a5"));

            sheet.SetContentsOfCell("a6", "= a1/A1*34+a2-a3+A5");//114
            Assert.AreEqual(114.0, sheet.GetCellValue("A6"));

            sheet.SetContentsOfCell("A7", "=a6-a1+a2+a3+a4+80"); //238
            Assert.AreEqual(238.0, sheet.GetCellValue("A7"));

            sheet.SetContentsOfCell("a8", "= A7+A3*A4+A5-A2+A1*114");// 744
            Assert.AreEqual(744.0, sheet.GetCellValue("a8"));
        }
        /// <summary>
        /// Tests our get value method and sees if it can throw an invalid name exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void getValueInvalidNameConstructor2()
        {
            Spreadsheet sheet = new Spreadsheet(validator, normalizer, "1.1");
            sheet.GetCellContents("x1");
        }
        /// <summary>
        /// Tests our get value method and sees if it can throw an invalid name exception
        /// </summary>
        [TestMethod]
        public void getValueEmptyCellConstructor2()
        {
            Spreadsheet sheet = new Spreadsheet(validator, normalizer, "1.1");
            Assert.AreEqual("", sheet.GetCellValue("a3"));
        }
        /// <summary>
        /// Checks that we can write to a file
        /// </summary>
        [TestMethod]
        public void saveTestConstructor2()
        {
            Spreadsheet sheet = new Spreadsheet(validator, normalizer, "1.1");
            sheet.SetContentsOfCell("a1", "2");
            sheet.Save("firstTest.xml");
        }
        /// <summary>
        /// Checks that we can write to a file
        /// </summary>
        [TestMethod]
        public void changedTestConstructor2()
        {
            Spreadsheet sheet = new Spreadsheet(validator, normalizer, "1.1");
            Assert.IsFalse(sheet.Changed);
            sheet.SetContentsOfCell("a1", "2");
            Assert.IsTrue(sheet.Changed);
            sheet.Save("firstTest.xml");
            Assert.IsFalse(sheet.Changed);
        }
        /// <summary>
        /// Checks that we can write to a file and that if the file name is wrong an exception is thrown
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void saveTestInvalidNameConstructor2()
        {
            Spreadsheet sheet = new Spreadsheet(validator, normalizer, "1.1");
            sheet.SetContentsOfCell("x1", "2");
            sheet.Save("firstTest.xml");
        }
        /// <summary>
        /// Checks that we can write to a file
        /// </summary>
        [TestMethod]
        public void saveTestStringConstructor2()
        {
            Spreadsheet sheet = new Spreadsheet(validator, normalizer, "1.1");
            sheet.SetContentsOfCell("a1", "Hello, I'm Mister Frog");
            sheet.Save("firstTest.xml");
        }
        /// <summary>
        /// Checks that we can write to a file
        /// </summary>
        [TestMethod]
        public void saveTestFormulaConstructor2()
        {
            Spreadsheet sheet = new Spreadsheet(validator, normalizer, "1.1");
            sheet.SetContentsOfCell("a1", "=a2 + 45 * 60/60");
            sheet.Save("firstTest.xml");
        }
        /// <summary>
        /// Checks that we can write to a file
        /// </summary>
        [TestMethod]
        public void saveTestFormulaComplexConstructor2()
        {
            Spreadsheet sheet = new Spreadsheet(validator, normalizer, "1.1");
            sheet.SetContentsOfCell("a1", "2"); //2
            sheet.SetContentsOfCell("A2", "= a1+4");//6
            Assert.AreEqual(6.0, sheet.GetCellValue("a2"));

            sheet.SetContentsOfCell("A3", "=a2*2/a1");//6
            Assert.AreEqual(6.0, sheet.GetCellValue("A3"));

            sheet.SetContentsOfCell("a4", "=a3 * a2-2"); //34
            Assert.AreEqual(34.0, sheet.GetCellValue("a4"));

            sheet.SetContentsOfCell("a5", "=a4*A1+A2+6"); //80
            Assert.AreEqual(80.0, sheet.GetCellValue("a5"));

            sheet.SetContentsOfCell("a6", "= a1/A1*34+a2-a3+A5");//114
            Assert.AreEqual(114.0, sheet.GetCellValue("A6"));

            sheet.SetContentsOfCell("A7", "=a6-a1+a2+a3+a4+80"); //238
            Assert.AreEqual(238.0, sheet.GetCellValue("A7"));

            sheet.SetContentsOfCell("a8", "= A7+A3*A4+A5-A2+A1*114");// 744
            Assert.AreEqual(744.0, sheet.GetCellValue("a8"));

            sheet.Save("firstTest.xml");
        }
        /// <summary>
        /// Checks that we can write to a file
        /// </summary>
        [TestMethod]
        public void GetSavedTestFormulaComplexConstructor2()
        {
            Spreadsheet sheet = new Spreadsheet(validator, normalizer, "1.1");
            sheet.SetContentsOfCell("a1", "2"); //2
            sheet.SetContentsOfCell("A2", "= a1+4");//6
            Assert.AreEqual(6.0, sheet.GetCellValue("a2"));

            sheet.SetContentsOfCell("A3", "=a2*2/a1");//6
            Assert.AreEqual(6.0, sheet.GetCellValue("A3"));

            sheet.SetContentsOfCell("a4", "=a3 * a2-2"); //34
            Assert.AreEqual(34.0, sheet.GetCellValue("a4"));

            sheet.SetContentsOfCell("a5", "=a4*A1+A2+6"); //80
            Assert.AreEqual(80.0, sheet.GetCellValue("a5"));

            sheet.SetContentsOfCell("a6", "= a1/A1*34+a2-a3+A5");//114
            Assert.AreEqual(114.0, sheet.GetCellValue("A6"));

            sheet.SetContentsOfCell("A7", "=a6-a1+a2+a3+a4+80"); //238
            Assert.AreEqual(238.0, sheet.GetCellValue("A7"));

            sheet.SetContentsOfCell("a8", "= A7+A3*A4+A5-A2+A1*114");// 744
            Assert.AreEqual(744.0, sheet.GetCellValue("a8"));

            sheet.Save("firstTest.xml");
            Assert.AreEqual("1.1", sheet.GetSavedVersion("firstTest.xml"));
        }

        /// <summary>
        /// Checks that we can create a spreadsheet from constructor 4
        /// </summary>
        [TestMethod]
        public void GetSavedVersionConstructor2Test()
        {
            Spreadsheet sheet = new Spreadsheet(validator, normalizer, "1.1");
            sheet.SetContentsOfCell("a1", "=a2 + 45 * 60/60");
            sheet.Save("firstTest.xml");
            Assert.AreEqual("1.1", sheet.GetSavedVersion("firstTest.xml"));
        }
        // ***CONSTRUCTOR 3 Tests*** //
        // ALL TESTS BELOW THIS POINT//
        // TEST THE THIRD CONSTRUCTOR//

        /// <summary>
        /// Tests that the circular dependency error is thrown. 
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(CircularException))]
        public void CircularDependencyConstructor3Test()
        {
            Spreadsheet sheet = new Spreadsheet(validator, normalizer, "1.1");
            sheet.Save("firstTest.xml");
            Spreadsheet sheet2 = new Spreadsheet("firstTest.xml", validator, normalizer, "1.1");
            sheet2.SetContentsOfCell("a1", "= a1 + a1");
        }
        /// <summary>
        /// Tests that the invalid name exception is thrown. 
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void invalidNameTestConstructor3()
        {
            Spreadsheet sheet = new Spreadsheet(validator, normalizer, "1.1");
            sheet.SetContentsOfCell("A2", "32");
            sheet.Save("firstTest.xml");

            Spreadsheet sheet2 = new Spreadsheet("firstTest.xml", validator, normalizer, "1.1");
            sheet2.SetContentsOfCell("A_", "32");
        }
        /// <summary>
        /// Tests that the invalid name exception is thrown. 
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void invalidNameTest2Constructor3()
        {
            Spreadsheet sheet = new Spreadsheet(validator, normalizer, "1.1");
            sheet.SetContentsOfCell("A2", "32");
            sheet.Save("firstTest.xml");

            Spreadsheet sheet2 = new Spreadsheet("firstTest.xml", validator, normalizer, "1.1");
            sheet2.SetContentsOfCell("_a%", "=32");
        }
        /// <summary>
        /// Tests that the invalid name exception is thrown. 
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void invalidNameTest3Constructor3()
        {
            Spreadsheet sheet = new Spreadsheet(validator, normalizer, "1.1");
            sheet.SetContentsOfCell("A2", "32");
            sheet.Save("firstTest.xml");

            Spreadsheet sheet2 = new Spreadsheet("firstTest.xml", validator, normalizer, "1.1");
            sheet2.SetContentsOfCell("X1", "32.0");
        }

        /// <summary>
        /// Tests that we can get the names of all nonEmpty cells. 
        /// </summary>
        [TestMethod]
        public void GetNamesOfNonEmptyCellsFalseTestConstructor3()
        {
            Spreadsheet sheet = new Spreadsheet(validator, normalizer, "1.1");
            sheet.SetContentsOfCell("a1", "32");
            IEnumerator<string> e1 = sheet.GetNamesOfAllNonemptyCells().GetEnumerator();
            Assert.IsTrue(e1.MoveNext());
            sheet.Save("firstTest.xml");

            Spreadsheet sheet2 = new Spreadsheet("firstTest.xml", validator, normalizer, "1.1");
            sheet2.SetContentsOfCell("a1", "     ");
            e1 = sheet2.GetNamesOfAllNonemptyCells().GetEnumerator();
            Assert.IsFalse(e1.MoveNext());
        }
        /// <summary>
        /// Tests that we can get the names of all nonEmpty cells. 
        /// </summary>
        [TestMethod]
        public void GetNamesOfNonEmptyCellsFalseTest2Constructor3()
        {
            Spreadsheet sheet = new Spreadsheet(validator, normalizer, "1.1");
            sheet.SetContentsOfCell("a1", "32");
            IEnumerator<string> e1 = sheet.GetNamesOfAllNonemptyCells().GetEnumerator();
            Assert.IsTrue(e1.MoveNext());
            sheet.Save("firstTest.xml");

            Spreadsheet sheet2 = new Spreadsheet("firstTest.xml", validator, normalizer, "1.1");
            sheet2.SetContentsOfCell("a1", "");
            e1 = sheet2.GetNamesOfAllNonemptyCells().GetEnumerator();
            Assert.IsFalse(e1.MoveNext());
        }
        /// <summary>
        /// Tests that we can get the names of all nonEmpty cells. should return empty IEnum if no cells are set. 
        /// </summary>
        [TestMethod]
        public void GetNamesOfNonEmptyEmptyTest1Constructor3()
        {
            Spreadsheet sheet = new Spreadsheet(validator, normalizer, "1.1");
            sheet.Save("secondTest.xml"); // saves an empty sheet

            Spreadsheet sheet2 = new Spreadsheet("secondTest.xml", validator, normalizer, "1.1");
            IEnumerator<string> e1 = sheet2.GetNamesOfAllNonemptyCells().GetEnumerator();
            Assert.IsFalse(e1.MoveNext());
        }
        /// <summary>
        /// Tests that we can get the names of all nonEmpty cells. 
        /// </summary>
        [TestMethod]
        public void GetNamesOfNonEmptyCellsTrueTestConstructor3()
        {
            Spreadsheet sheet = new Spreadsheet(validator, normalizer, "1.1");
            sheet.SetContentsOfCell("A1", "32.0");
            sheet.SetContentsOfCell("a2", "42.0");
            sheet.SetContentsOfCell("A3", "= A1 + 23*83");
            IEnumerator<string> e1 = sheet.GetNamesOfAllNonemptyCells().GetEnumerator();

            Assert.IsTrue(e1.MoveNext());
            Assert.AreEqual("A1", e1.Current);
            Assert.IsTrue(e1.MoveNext());
            Assert.AreEqual("A2", e1.Current);
            Assert.IsTrue(e1.MoveNext());
            Assert.AreEqual("A3", e1.Current);
            Assert.IsFalse(e1.MoveNext());
            sheet.Save("firstTest.xml");

            Spreadsheet sheet2 = new Spreadsheet("firstTest.xml", validator, normalizer, "1.1");
            e1 = sheet2.GetNamesOfAllNonemptyCells().GetEnumerator();

            Assert.IsTrue(e1.MoveNext());
            Assert.AreEqual("A1", e1.Current);
            Assert.IsTrue(e1.MoveNext());
            Assert.AreEqual("A2", e1.Current);
            Assert.IsTrue(e1.MoveNext());
            Assert.AreEqual("A3", e1.Current);
            Assert.IsFalse(e1.MoveNext());
        }
        /// <summary>
        /// Tests that we can get the names of all nonEmpty cells. 
        /// </summary>
        [TestMethod]
        public void GetNamesOfNonEmptyCellsFalseTestAndTrueConstructor3()
        {
            Spreadsheet sheet = new Spreadsheet(validator, normalizer, "1.1");
            sheet.SetContentsOfCell("A1", "2.0");
            sheet.SetContentsOfCell("a2", "32.0");
            sheet.SetContentsOfCell("A3", "=23*83");
            sheet.SetContentsOfCell("A4", "   ");
            sheet.SetContentsOfCell("A5", "   ");
            sheet.SetContentsOfCell("A12", "23.0");
            IEnumerator<string> e1 = sheet.GetNamesOfAllNonemptyCells().GetEnumerator();

            Assert.IsTrue(e1.MoveNext());
            Assert.AreEqual("A1", e1.Current);
            Assert.IsTrue(e1.MoveNext());
            Assert.AreEqual("A2", e1.Current);
            Assert.IsTrue(e1.MoveNext());
            Assert.AreEqual("A3", e1.Current);
            Assert.IsTrue(e1.MoveNext());
            Assert.AreEqual("A12", e1.Current);
            Assert.IsFalse(e1.MoveNext());
            sheet.Save("firstTest.xml");

            Spreadsheet sheet2 = new Spreadsheet("firstTest.xml", validator, normalizer, "1.1");
            e1 = sheet2.GetNamesOfAllNonemptyCells().GetEnumerator();

            Assert.IsTrue(e1.MoveNext());
            Assert.AreEqual("A1", e1.Current);
            Assert.IsTrue(e1.MoveNext());
            Assert.AreEqual("A2", e1.Current);
            Assert.IsTrue(e1.MoveNext());
            Assert.AreEqual("A3", e1.Current);
            Assert.IsTrue(e1.MoveNext());
            Assert.AreEqual("A12", e1.Current);
            Assert.IsFalse(e1.MoveNext());
        }
        /// <summary>
        /// Tests that we can get the contents of a cell based only on its name
        /// </summary>
        [TestMethod]
        public void GetCellsContentsTestConstructor3()
        {
            Spreadsheet sheet = new Spreadsheet(validator, normalizer, "1.1");
            sheet.SetContentsOfCell("a1", "The");
            Assert.AreEqual("The", sheet.GetCellContents("a1"));

            sheet.Save("firstTest.xml");

            Spreadsheet sheet2 = new Spreadsheet("firstTest.xml", validator, normalizer, "1.1");
            Assert.AreEqual("The", sheet.GetCellContents("a1"));
        }
        /// <summary>
        /// Tests that we can get the contents of a cell based only on its name
        /// </summary>
        [TestMethod]
        public void GetCellsContentsNumberTestConstructor3()
        {
            Spreadsheet sheet = new Spreadsheet(validator, normalizer, "1.1");
            sheet.SetContentsOfCell("a1", "23.0");
            Assert.AreEqual(23.0, sheet.GetCellContents("a1"));
            sheet.Save("firstTest.xml");

            Spreadsheet sheet2 = new Spreadsheet("firstTest.xml", validator, normalizer, "1.1");
            Assert.AreEqual(23.0, sheet2.GetCellContents("a1"));
        }
        /// <summary>
        /// Tests that we can get the contents of a cell based only on its name
        /// </summary>
        [TestMethod]
        public void GetCellsContentsFormulaTestConstructor3()
        {
            Spreadsheet sheet = new Spreadsheet(validator, normalizer, "1.1");
            sheet.SetContentsOfCell("a1", "=32 + a2");
            Assert.AreEqual(new Formula("32+A2"), sheet.GetCellContents("a1"));
            sheet.Save("firstTest.xml");

            Spreadsheet sheet2 = new Spreadsheet("firstTest.xml", validator, normalizer, "1.1");
            Assert.AreEqual(new Formula("32+A2"), sheet2.GetCellContents("a1"));
        }
        /// <summary>
        /// Tests that we can set the contents of a cell to a String
        /// </summary>
        [TestMethod]
        public void setCellsContentsNameAndStringTestConstructor3()
        {
            Spreadsheet sheet = new Spreadsheet(validator, normalizer, "1.1");
            sheet.SetContentsOfCell("a1", "Hello");

            Assert.AreEqual("Hello", (String)sheet.GetCellContents("a1"));
            sheet.Save("firstTest.xml");

            Spreadsheet sheet2 = new Spreadsheet("firstTest.xml", validator, normalizer, "1.1");
            Assert.AreEqual("Hello", sheet2.GetCellContents("a1"));

        }
        /// <summary>
        /// Tests that we can set the contents of a cell to a Formula
        /// </summary>
        [TestMethod]
        public void setCellsContentsNameAndFormulaTestConstructor3()
        {
            Spreadsheet sheet = new Spreadsheet(validator, normalizer, "1.1");
            sheet.SetContentsOfCell("a1", "= 2 +2");

            Assert.AreEqual(new Formula("2+2"), (Formula)sheet.GetCellContents("a1"));
            sheet.Save("firstTest.xml");

            Spreadsheet sheet2 = new Spreadsheet("firstTest.xml", validator, normalizer, "1.1");
            Assert.AreEqual(new Formula("2+2"), sheet2.GetCellContents("a1"));
        }

        /// <summary>
        /// Tests that we can set the contents of a cell to a number
        /// </summary>
        [TestMethod]
        public void setCellsContentsNameAndNumberTest2Constructor3()
        {
            Spreadsheet sheet = new Spreadsheet(validator, normalizer, "1.1");
            sheet.SetContentsOfCell("a1", "23.0");
            sheet.SetContentsOfCell("a1", "1.0");
            Assert.AreEqual(1.0, sheet.GetCellContents("a1"));
            sheet.Save("firstTest.xml");

            Spreadsheet sheet2 = new Spreadsheet("firstTest.xml", validator, normalizer, "1.1");
            sheet2.SetContentsOfCell("a1", "23.0");
            sheet2.SetContentsOfCell("a1", "1.0");
            Assert.AreEqual(1.0, sheet2.GetCellContents("a1"));
        }
        /// <summary>
        /// Tests that we can set the contents of a cell to a String
        /// </summary>
        [TestMethod]
        public void setCellsContentsNameAndStringTest2Constructor3()
        {
            Spreadsheet sheet = new Spreadsheet(validator, normalizer, "1.1");
            sheet.SetContentsOfCell("a1", "32");
            sheet.SetContentsOfCell("a1", "forty");
            Assert.AreEqual("forty", (String)sheet.GetCellContents("A1"));
            sheet.Save("firstTest.xml");

            Spreadsheet sheet2 = new Spreadsheet("firstTest.xml", validator, normalizer, "1.1");
            Assert.AreEqual("forty", sheet2.GetCellContents("a1"));
        }
        /// <summary>
        /// Tests that we can set the contents of a cell to a String
        /// and that the values returned are correct. 
        /// </summary>
        [TestMethod]
        public void setCellsContentsSetTestConstructor3()
        {
            Spreadsheet sheet = new Spreadsheet(validator, normalizer, "1.1");
            sheet.SetContentsOfCell("a1", "23.0");
            sheet.SetContentsOfCell("a2", "=a1");
            IList<string> h = sheet.SetContentsOfCell("a1", "4.0");
            Assert.IsTrue(h.Contains("A2"));
            Assert.IsTrue(h.Contains("A1")); // checks that the original value was added 
            sheet.Save("firstTest.xml");

            Spreadsheet sheet2 = new Spreadsheet("firstTest.xml", validator, normalizer, "1.1");
            h = sheet2.SetContentsOfCell("a1", "4.0");
            Assert.IsTrue(h.Contains("A2"));
            Assert.IsTrue(h.Contains("A1"));
        }
        /// <summary>
        /// Tests that we can set the contents of a cell to a String
        /// and that the values returned are correct. 
        /// </summary>
        [TestMethod]
        public void setCellsContentsSetTest6Constructor3()
        {
            Spreadsheet sheet = new Spreadsheet(validator, normalizer, "1.1");
            sheet.SetContentsOfCell("a1", "23.0");
            IList<string> h = sheet.SetContentsOfCell("a2", "a1");
            Assert.AreEqual(1, h.Count);
            sheet.Save("firstTest.xml");

            Spreadsheet sheet2 = new Spreadsheet("firstTest.xml", validator, normalizer, "1.1");
            h = sheet2.SetContentsOfCell("a2", "a1");
            Assert.AreEqual(1, h.Count);
        }
        /// <summary>
        /// Tests that we can set the contents of a cell to a String
        /// and that the values returned are correct. 
        /// </summary>
        [TestMethod]
        public void setCellsContentsSetTest2Constructor3()
        {
            Spreadsheet sheet = new Spreadsheet(validator, normalizer, "1.1");
            sheet.SetContentsOfCell("a1", "3.0");
            sheet.SetContentsOfCell("a2", "6.0");
            sheet.SetContentsOfCell("a3", "= a1 / a2 ");
            sheet.SetContentsOfCell("a4", "=a1");
            sheet.SetContentsOfCell("a523", "= a1*4");
            IList<string> h = sheet.SetContentsOfCell("a1", "4.0");

            Assert.IsTrue(h.Contains("A1"));
            Assert.IsTrue(h.Contains("A3"));
            Assert.IsTrue(h.Contains("A4"));
            Assert.IsTrue(h.Contains("A523"));
            Assert.IsFalse(h.Contains("A2"));
            sheet.Save("firstTest.xml");

            Spreadsheet sheet2 = new Spreadsheet("firstTest.xml", validator, normalizer, "1.1");
            h = sheet2.SetContentsOfCell("a1", "5.0");
            Assert.IsTrue(h.Contains("A1"));
            Assert.IsTrue(h.Contains("A3"));
            Assert.IsTrue(h.Contains("A4"));
            Assert.IsTrue(h.Contains("A523"));
            Assert.IsFalse(h.Contains("A2"));
        }
        /// <summary>
        /// Tests that we can set the contents of a cell to a String
        /// and that the values returned are correct. 
        /// </summary>
        [TestMethod]
        public void setCellsContentsSetTest3Constructor3()
        {
            Spreadsheet sheet = new Spreadsheet(validator, normalizer, "1.1");
            sheet.SetContentsOfCell("a1", "3.0");
            sheet.SetContentsOfCell("a2", "6.0");
            sheet.SetContentsOfCell("a3", "= a1 / A2 ");
            sheet.SetContentsOfCell("a4", "=A1");
            sheet.SetContentsOfCell("a526", "= a1*4");
            IList<string> h = sheet.SetContentsOfCell("A2", "4.0");

            Assert.IsTrue(h.Contains("A2"));
            Assert.IsTrue(h.Contains("A3"));
            Assert.IsFalse(h.Contains("A4"));
            Assert.IsFalse(h.Contains("A526"));
            Assert.IsFalse(h.Contains("A1"));
            Assert.IsFalse(h.Contains("a1"));
            sheet.Save("firstTest.xml");

            Spreadsheet sheet2 = new Spreadsheet("firstTest.xml", validator, normalizer, "1.1");
            h = sheet2.SetContentsOfCell("a2", "4.0");
            Assert.IsTrue(h.Contains("A2"));
            Assert.IsTrue(h.Contains("A3"));
            Assert.IsFalse(h.Contains("A4"));
            Assert.IsFalse(h.Contains("A526"));
            Assert.IsFalse(h.Contains("A1"));
            Assert.IsFalse(h.Contains("a1"));
        }

        /// <summary>
        /// Tests that we can set the contents of a cell to a String
        /// and that the values returned are correct. 
        /// </summary>
        [TestMethod]
        public void getEmptyCellContentsContructor3Test()
        {
            Spreadsheet sheet = new Spreadsheet(validator, normalizer, "1.1");
            sheet.Save("firstTest.xml"); // saves an empty sheet
            Spreadsheet sheet2 = new Spreadsheet("firstTest.xml", validator, normalizer, "1.1");
            Assert.AreEqual("", sheet.GetCellContents("A1"));
        }
        /// <summary>
        /// Tests our get value method
        /// </summary>
        [TestMethod]
        public void getValueTestConstructor3()
        {
            Spreadsheet sheet = new Spreadsheet(validator, normalizer, "1.1");
            sheet.SetContentsOfCell("a1", "= 2+2");
            Assert.AreEqual(4.0, sheet.GetCellValue("A1"));
            sheet.Save("firstTest.xml");

            Spreadsheet sheet2 = new Spreadsheet("firstTest.xml", validator, normalizer, "1.1");
            Assert.AreEqual(4.0, sheet2.GetCellValue("a1"));
        }
        /// <summary>
        /// Tests our get value method
        /// </summary>
        [TestMethod]
        public void getValueDoubleTestConstructor3()
        {
            Spreadsheet sheet = new Spreadsheet(validator, normalizer, "1.1");
            sheet.SetContentsOfCell("a1", "2.0");
            Assert.AreEqual(2.0, sheet.GetCellValue("a1"));
            sheet.Save("firstTest.xml");

            Spreadsheet sheet2 = new Spreadsheet("firstTest.xml", validator, normalizer, "1.1");
            Assert.AreEqual(2.0, sheet2.GetCellValue("A1"));
        }
        /// <summary>
        /// Tests our get value method
        /// </summary>
        [TestMethod]
        public void getValueDoubleTest2Constructor3()
        {
            Spreadsheet sheet = new Spreadsheet(validator, normalizer, "1.1");
            sheet.SetContentsOfCell("a1", "2");
            Assert.AreEqual(2.0, sheet.GetCellValue("A1"));
            sheet.Save("firstTest.xml");

            Spreadsheet sheet2 = new Spreadsheet("firstTest.xml", validator, normalizer, "1.1");
            Assert.AreEqual(2.0, sheet2.GetCellValue("a1"));
        }
        /// <summary>
        /// Tests our get value method
        /// </summary>
        [TestMethod]
        public void getValueStringTestConstructor3()
        {
            Spreadsheet sheet = new Spreadsheet(validator, normalizer, "1.1");
            sheet.SetContentsOfCell("a1", "hello, I'm Mister Frog");
            Assert.AreEqual("hello, I'm Mister Frog", sheet.GetCellValue("a1"));
            sheet.Save("firstTest.xml");

            Spreadsheet sheet2 = new Spreadsheet("firstTest.xml", validator, normalizer, "1.1");
            Assert.AreEqual("hello, I'm Mister Frog", sheet2.GetCellValue("A1"));
        }
        /// <summary>
        /// Tests our get value method
        /// </summary>
        [TestMethod]
        public void getValueTestVariableConstructor3()
        {
            Spreadsheet sheet = new Spreadsheet(validator, normalizer, "1.1");
            sheet.SetContentsOfCell("a1", "2");
            sheet.SetContentsOfCell("a2", "= A1+4");
            Assert.AreEqual(6.0, sheet.GetCellValue("a2"));
            sheet.Save("firstTest.xml");

            Spreadsheet sheet2 = new Spreadsheet("firstTest.xml", validator, normalizer, "1.1");
            Assert.AreEqual(2.0, sheet2.GetCellValue("A1"));
            Assert.AreEqual(6.0, sheet2.GetCellValue("a2"));
        }
        /// <summary>
        /// Tests our get value method
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void SpreadsheetReadWriteErrorConstructor3()
        {
            Spreadsheet sheet = new Spreadsheet(validator, normalizer, "1.1");
            sheet.SetContentsOfCell("a1", "2");
            sheet.SetContentsOfCell("a2", "= A1+4");
            Assert.AreEqual(6.0, sheet.GetCellValue("a2"));
            sheet.Save("firstTest.xml");

            Spreadsheet sheet2 = new Spreadsheet("firstTest.xml", validator, normalizer, "1.2");
            Assert.AreEqual(2.0, sheet2.GetCellValue("A1"));
            Assert.AreEqual(6.0, sheet2.GetCellValue("a2"));
        }
        /// <summary>
        /// Tests our get value method
        /// </summary>
        [TestMethod]
        public void getValueTestVariableComplexConstructor3()
        {
            Spreadsheet sheet = new Spreadsheet(validator, normalizer, "1.1");
            sheet.SetContentsOfCell("a1", "2"); //2
            sheet.SetContentsOfCell("a2", "= a1+4");//6
            Assert.AreEqual(6.0, sheet.GetCellValue("a2"));

            sheet.SetContentsOfCell("a3", "=A2*A1/a1");//6
            Assert.AreEqual(6.0, sheet.GetCellValue("A3"));

            sheet.SetContentsOfCell("A4", "= A3*a2-a1"); //34
            Assert.AreEqual(34.0, sheet.GetCellValue("a4"));

            sheet.SetContentsOfCell("A5", "=a4*A1+a2+A3"); //80
            Assert.AreEqual(80.0, sheet.GetCellValue("A5"));

            sheet.SetContentsOfCell("A6", "= A1/A1*A4+A2-A3+A5");//114
            Assert.AreEqual(114.0, sheet.GetCellValue("A6"));

            sheet.SetContentsOfCell("A7", "=a6-A1+A2+A3+a4+a5"); //238
            Assert.AreEqual(238.0, sheet.GetCellValue("a7"));

            sheet.SetContentsOfCell("a8", "= a7+a3*a4+a5-a2+a1*a6");// 744
            Assert.AreEqual(744.0, sheet.GetCellValue("a8"));
            sheet.Save("firstTest.xml");

            Spreadsheet sheet2 = new Spreadsheet("firstTest.xml", validator, normalizer, "1.1");
            Assert.AreEqual(2.0, sheet2.GetCellValue("A1"));
            Assert.AreEqual(6.0, sheet2.GetCellValue("a2"));
            Assert.AreEqual(6.0, sheet2.GetCellValue("A3"));
            Assert.AreEqual(34.0, sheet2.GetCellValue("a4"));
            Assert.AreEqual(80.0, sheet2.GetCellValue("A5"));
            Assert.AreEqual(114.0, sheet2.GetCellValue("A6"));
            Assert.AreEqual(238.0, sheet2.GetCellValue("a7"));
            Assert.AreEqual(744.0, sheet2.GetCellValue("a8"));

        }
        /// <summary>
        /// Tests our get value method
        /// </summary>
        [TestMethod]
        public void getValueTestVariableComplex3Constructor3()
        {
            Spreadsheet sheet = new Spreadsheet(validator, normalizer, "1.1");
            sheet.SetContentsOfCell("a1", "2"); //2
            sheet.SetContentsOfCell("a2", "= a1+4");//6
            Assert.AreEqual(6.0, sheet.GetCellValue("a2"));

            sheet.SetContentsOfCell("a3", "=A2*A1/a1");//6
            Assert.AreEqual(6.0, sheet.GetCellValue("A3"));

            sheet.SetContentsOfCell("A4", "= A3*a2-a1"); //34
            Assert.AreEqual(34.0, sheet.GetCellValue("a4"));

            sheet.SetContentsOfCell("A5", "=a4*A1+a2+A3"); //80
            Assert.AreEqual(80.0, sheet.GetCellValue("A5"));

            sheet.SetContentsOfCell("A6", "= A1/A1*A4+A2-A3+A5");//114
            Assert.AreEqual(114.0, sheet.GetCellValue("A6"));

            sheet.SetContentsOfCell("A7", "=a6-A1+A2+A3+a4+a5"); //238
            Assert.AreEqual(238.0, sheet.GetCellValue("a7"));

            sheet.SetContentsOfCell("a8", "= a7+a3*a4+a5-a2+a1*a6");// 744
            Assert.AreEqual(744.0, sheet.GetCellValue("a8"));
            sheet.Save("firstTest.xml");

            Spreadsheet sheet2 = new Spreadsheet("firstTest.xml", validator, normalizer, "1.1");
            sheet2.SetContentsOfCell("a1", "4.0");
            Assert.AreEqual(4.0, sheet2.GetCellContents("a1"));
            Assert.AreEqual(8.0, sheet2.GetCellValue("a2"));
            Assert.AreEqual(8.0, sheet2.GetCellValue("a3"));
            Assert.AreEqual(60.0, sheet2.GetCellValue("a4"));
            Assert.AreEqual(256.0, sheet2.GetCellValue("a5"));
            Assert.AreEqual(316.0, sheet2.GetCellValue("a6"));
            Assert.AreEqual(644.0, sheet2.GetCellValue("a7"));
            Assert.AreEqual(2636.0, sheet2.GetCellValue("a8"));
        }
        /// <summary>
        /// Tests our get value method and sees if it can throw an invalid name exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void getValueInvalidNameConstructor3()
        {
            Spreadsheet sheet = new Spreadsheet(validator, normalizer, "1.1");
            sheet.GetCellContents("a1");
            sheet.Save("firstTest.xml");

            Spreadsheet sheet2 = new Spreadsheet("firstTest.xml", validator, normalizer, "1.1");
            sheet.GetCellContents("x1");
        }
        /// <summary>
        /// Tests our get value method and sees if it can throw an invalid name exception
        /// </summary>
        [TestMethod]
        public void getValueEmptyCellConstructor3()
        {
            Spreadsheet sheet = new Spreadsheet(validator, normalizer, "1.1");
            Assert.AreEqual("", sheet.GetCellValue("a3"));
            sheet.Save("firstTest.xml");

            Spreadsheet sheet2 = new Spreadsheet("firstTest.xml", validator, normalizer, "1.1");
            Assert.AreEqual("", sheet2.GetCellValue("a3"));
        }
        /// <summary>
        /// Checks that we can create a spreadsheet from constructor 3
        /// </summary>
        [TestMethod]
        public void FormulaConstructor3Test()
        {
            Spreadsheet sheet = new Spreadsheet(validator, normalizer, "1.1");
            sheet.SetContentsOfCell("a1", "=32 + 45 * 60/60");
            sheet.Save("firstTest.xml");

            Spreadsheet sheet2 = new Spreadsheet("firstTest.xml", validator, normalizer, "1.1");
            Assert.AreEqual(new Formula("A2+45*60/60"), sheet2.GetCellContents("a1"));
        }
        /// <summary>
        /// Checks that we can create a spreadsheet from constructor 3
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void badFileConstructor3Test()
        {
            Spreadsheet sheet = new Spreadsheet(validator, normalizer, "1.1");
            sheet.SetContentsOfCell("a1", "=32 + 45 * 60/60");
            sheet.Save("firstTest.xml");

            Spreadsheet sheet2 = new Spreadsheet("/some/nonsense/path.xml", validator, normalizer, "1.1");
        }
        /// <summary>
        /// Checks that we can create a spreadsheet from constructor 3
        /// </summary>
        [TestMethod]
        public void changedConstructor3Test()
        {
            Spreadsheet sheet = new Spreadsheet(validator, normalizer, "1.1");
            sheet.SetContentsOfCell("a1", "=32 + 45 * 60/60");
            Assert.IsTrue(sheet.Changed);
            sheet.Save("firstTest.xml");
            Assert.IsFalse(sheet.Changed);

            Spreadsheet sheet2 = new Spreadsheet("firstTest.xml", validator, normalizer, "1.1");
            Assert.AreEqual(new Formula("A2+45*60/60"), sheet2.GetCellContents("a1"));
        }
        /// <summary>
        /// Checks that we can create a spreadsheet from constructor 3
        /// </summary>
        [TestMethod]
        public void GetSavedVersionConstructor3Test()
        {
            Spreadsheet sheet = new Spreadsheet(validator, normalizer, "1.1");
            sheet.SetContentsOfCell("a1", "=a2 + 45 * 60/60");
            sheet.Save("firstTest.xml");

            Spreadsheet sheet2 = new Spreadsheet("firstTest.xml", validator, normalizer, "1.1");
            Assert.AreEqual("1.1", sheet.GetSavedVersion("firstTest.xml"));
        }
        //    ***STRESS Tests***      //
        // ALL TESTS BELOW THIS POINT//
        //     ARE STRESS TESTS     //

        /// <summary>
        /// stress test 1
        /// </summary>
        [TestMethod()]
        [TestCategory("31")]
        public void TestStress1()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "=B1+B2");
            s.SetContentsOfCell("B1", "=C1-C2");
            s.SetContentsOfCell("B2", "=C3*C4");
            s.SetContentsOfCell("C1", "=D1*D2");
            s.SetContentsOfCell("C2", "=D3*D4");
            s.SetContentsOfCell("C3", "=D5*D6");
            s.SetContentsOfCell("C4", "=D7*D8");
            s.SetContentsOfCell("D1", "=E1");
            s.SetContentsOfCell("D2", "=E1");
            s.SetContentsOfCell("D3", "=E1");
            s.SetContentsOfCell("D4", "=E1");
            s.SetContentsOfCell("D5", "=E1");
            s.SetContentsOfCell("D6", "=E1");
            s.SetContentsOfCell("D7", "=E1");
            s.SetContentsOfCell("D8", "=E1");
            IList<String> cells = s.SetContentsOfCell("E1", "0");
            Assert.IsTrue(new HashSet<string>() { "A1", "B1", "B2", "C1", "C2", "C3", "C4", "D1", "D2", "D3", "D4", "D5", "D6", "D7", "D8", "E1" }.SetEquals(cells));
        }
        // Repeated for extra weight
        [TestMethod()]
        [TestCategory("32")]
        public void TestStress1a()
        {
            TestStress1();
        }
        [TestMethod()]
        [TestCategory("33")]
        public void TestStress1b()
        {
            TestStress1();
        }
        [TestMethod()]
        [TestCategory("34")]
        public void TestStress1c()
        {
            TestStress1();
        }
        /// <summary>
        /// Tests how often we use evaluate a formula 
        /// </summary>
        [TestMethod()]
        [TestCategory("34")]
        public void TestStress2()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("a2", "= 32+43 +65*100*30+4-60/1");
            for (int i = 0; i < 10_000; i++)
            {
                Assert.AreEqual(195019.0, sheet.GetCellValue("a2"));
            }
        }
        [TestMethod()]
        [TestCategory("34")]
        public void TestStress2b()
        {
            TestStress2();
        }
        [TestMethod()]
        [TestCategory("34")]
        public void TestStress2c()
        {
            TestStress2();
        }
        [TestMethod()]
        [TestCategory("34")]
        public void TestStress2d()
        {
            TestStress2();
        }
        /// <summary>
        /// Tests how often we use evaluate a formula 
        /// </summary>
        [TestMethod()]
        [TestCategory("34")]
        public void TestStress3()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("x1", "2"); //2 //4
            sheet.SetContentsOfCell("x2", "= x1+4");//6 //8
            Assert.AreEqual(6.0, sheet.GetCellValue("x2"));

            sheet.SetContentsOfCell("x3", "=x2*x1/x1");//6 //8
            Assert.AreEqual(6.0, sheet.GetCellValue("x3"));

            sheet.SetContentsOfCell("x4", "= x3*x2-x1"); //34 //60
            Assert.AreEqual(34.0, sheet.GetCellValue("x4"));

            sheet.SetContentsOfCell("x5", "=x4*x1+x2+x3"); //80 //256
            Assert.AreEqual(80.0, sheet.GetCellValue("x5"));

            sheet.SetContentsOfCell("x6", "= x1/x1*x4+x2-x3+x5");//114 //316
            Assert.AreEqual(114.0, sheet.GetCellValue("x6"));

            sheet.SetContentsOfCell("x7", "=x6-x1+x2+x3+x4+x5"); //238 //644
            Assert.AreEqual(238.0, sheet.GetCellValue("x7"));

            sheet.SetContentsOfCell("x8", "= x7+x3*x4+x5-x2+x1*x6");// 744 //2636
            Assert.AreEqual(744.0, sheet.GetCellValue("x8"));

            sheet.SetContentsOfCell("x1", "4.0");

            for (int i = 0; i < 10_000; i++)
            {
                Assert.AreEqual(8.0, sheet.GetCellValue("x2"));
                Assert.AreEqual(8.0, sheet.GetCellValue("x3"));
                Assert.AreEqual(60.0, sheet.GetCellValue("x4"));
                Assert.AreEqual(256.0, sheet.GetCellValue("x5"));
                Assert.AreEqual(316.0, sheet.GetCellValue("x6"));
                Assert.AreEqual(644.0, sheet.GetCellValue("x7"));
                Assert.AreEqual(2636.0, sheet.GetCellValue("x8"));
            }
        }
        [TestMethod()]
        [TestCategory("34")]
        public void TestStress3a()
        {
            TestStress3();
        }
        [TestMethod()]
        [TestCategory("34")]
        public void TestStresb3c()
        {
            TestStress3();
        }
        /// <summary>
        /// Tests how often we use evaluate a formula 
        /// </summary>
        [TestMethod()]
        [TestCategory("34")]
        public void TestStress4()
        {
            Spreadsheet sheet = new Spreadsheet(validator, normalizer, "1.1");
            sheet.SetContentsOfCell("a1", "2"); //2
            sheet.SetContentsOfCell("a2", "= a1+4");//6
            Assert.AreEqual(6.0, sheet.GetCellValue("a2"));

            sheet.SetContentsOfCell("a3", "=A2*A1/a1");//6
            Assert.AreEqual(6.0, sheet.GetCellValue("A3"));

            sheet.SetContentsOfCell("A4", "= A3*a2-a1"); //34
            Assert.AreEqual(34.0, sheet.GetCellValue("a4"));

            sheet.SetContentsOfCell("A5", "=a4*A1+a2+A3"); //80
            Assert.AreEqual(80.0, sheet.GetCellValue("A5"));

            sheet.SetContentsOfCell("A6", "= A1/A1*A4+A2-A3+A5");//114
            Assert.AreEqual(114.0, sheet.GetCellValue("A6"));

            sheet.SetContentsOfCell("A7", "=a6-A1+A2+A3+a4+a5"); //238
            Assert.AreEqual(238.0, sheet.GetCellValue("a7"));

            sheet.SetContentsOfCell("a8", "= a7+a3*a4+a5-a2+a1*a6");// 744
            Assert.AreEqual(744.0, sheet.GetCellValue("a8"));
            sheet.Save("firstTest.xml");

            for (int i = 0; i < 1000; i++)
            {
                sheet.SetContentsOfCell("a8", "= 42");// 744
                sheet.Save("firstTest.xml");// checks the speed of saving 

                Spreadsheet sheet2 = new Spreadsheet("firstTest.xml", validator, normalizer, "1.1");
                sheet2.SetContentsOfCell("a1", "4.0");
                Assert.AreEqual(4.0, sheet2.GetCellContents("a1"));
                Assert.AreEqual(8.0, sheet2.GetCellValue("a2"));
                Assert.AreEqual(8.0, sheet2.GetCellValue("a3"));
                Assert.AreEqual(60.0, sheet2.GetCellValue("a4"));
                Assert.AreEqual(256.0, sheet2.GetCellValue("a5"));
                Assert.AreEqual(316.0, sheet2.GetCellValue("a6"));
                Assert.AreEqual(644.0, sheet2.GetCellValue("a7"));
                Assert.AreEqual(42.0, sheet2.GetCellValue("a8"));
            }
        }
        [TestMethod()]
        [TestCategory("34")]
        public void TestStress4a()
        {
            TestStress4();
        }
        [TestMethod()]
        [TestCategory("34")]
        public void TestStress4b()
        {
            TestStress4();
        }
        /// <summary>
        /// Tests how often we use evaluate a formula 
        /// </summary>
        [TestMethod()]
        [TestCategory("34")]
        public void TestStress5()
        {
            Spreadsheet sheet = new Spreadsheet(validator, normalizer, "1.1");
            sheet.SetContentsOfCell("a1", "2"); //2
            sheet.SetContentsOfCell("a2", "= a1+4");//6
            Assert.AreEqual(6.0, sheet.GetCellValue("a2"));

            sheet.SetContentsOfCell("a3", "=A2*A1/a1");//6
            Assert.AreEqual(6.0, sheet.GetCellValue("A3"));

            sheet.SetContentsOfCell("A4", "= A3*a2-a1"); //34
            Assert.AreEqual(34.0, sheet.GetCellValue("a4"));

            sheet.SetContentsOfCell("A5", "=a4*A1+a2+A3"); //80
            Assert.AreEqual(80.0, sheet.GetCellValue("A5"));

            sheet.SetContentsOfCell("A6", "= A1/A1*A4+A2-A3+A5");//114
            Assert.AreEqual(114.0, sheet.GetCellValue("A6"));

            sheet.SetContentsOfCell("A7", "=a6-A1+A2+A3+a4+a5"); //238
            Assert.AreEqual(238.0, sheet.GetCellValue("a7"));

            sheet.SetContentsOfCell("a8", "= a7+a3*a4+a5-a2+a1*a6");// 744
            Assert.AreEqual(744.0, sheet.GetCellValue("a8"));
            sheet.Save("firstTest.xml");

            Spreadsheet sheet2 = new Spreadsheet("firstTest.xml", validator, normalizer, "1.1");

            for (int i = 0; i < 10_000; i++)
            {
                Assert.AreEqual(2.0, sheet2.GetCellValue("A1"));
                Assert.AreEqual(6.0, sheet2.GetCellValue("a2"));
                Assert.AreEqual(6.0, sheet2.GetCellValue("A3"));
                Assert.AreEqual(34.0, sheet2.GetCellValue("a4"));
                Assert.AreEqual(80.0, sheet2.GetCellValue("A5"));
                Assert.AreEqual(114.0, sheet2.GetCellValue("A6"));
                Assert.AreEqual(238.0, sheet2.GetCellValue("a7"));
                Assert.AreEqual(744.0, sheet2.GetCellValue("a8"));
            }
        }
        //stress test circular exceptions
        [TestMethod()]
        [TestCategory("39")]
        public void TestStress6()
        {
            Spreadsheet s = new Spreadsheet();
            for (int i = 1; i < 200; i++)
            {
                s.SetContentsOfCell("A" + i, ("=A" + (i + 1)));
            }
            try
            {
                s.SetContentsOfCell("A150", "=A50");
                Assert.Fail();
            }
            catch (CircularException)
            {
            }
        }
        /// <summary>
        /// 
        /// </summary>
        [TestMethod()]
        [TestCategory("43")]
        public void TestStress7()
        {
            Spreadsheet s = new Spreadsheet();
            for (int i = 0; i < 500; i++)
            {
                s.SetContentsOfCell("A1" + i, ("=A1" + (i + 1)));
            }
            LinkedList<string> firstCells = new LinkedList<string>();
            LinkedList<string> lastCells = new LinkedList<string>();
            for (int i = 0; i < 250; i++)
            {
                firstCells.AddFirst("A1" + i);
                lastCells.AddFirst("A1" + (i + 250));
            }
            Assert.IsTrue(s.SetContentsOfCell("A1249", "25.0").SequenceEqual(firstCells));
            Assert.IsTrue(s.SetContentsOfCell("A1499", "0.0").SequenceEqual(lastCells));
        }

        /// <summary>
        /// This is a validator where a variable name is only valid if the first character is A or a.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private bool validator(String name)
        {
            char firstCharacter = name[0];

            if (Char.IsLetter(firstCharacter) && (firstCharacter == 'a' || firstCharacter == 'A'))
                return true;
            else return false;
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

    }
}

