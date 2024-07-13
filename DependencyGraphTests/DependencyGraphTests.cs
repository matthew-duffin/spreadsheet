using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
///   This class tests our dependency graphs
namespace DevelopmentTests
{
    /// <summary>
    ///This is a test class for DependencyGraphTest and is intended
    ///to contain all DependencyGraphTest Unit Tests
    ///</summary>
    [TestClass()]
    public class DependencyGraphTest
    {
        /// <summary>
        ///Empty graph should contain nothing
        ///</summary>
        [TestMethod()]
        public void SimpleEmptyTest()
        {
            DependencyGraph t = new DependencyGraph();
            Assert.AreEqual(0, t.Size);
        }
        /// <summary>
        ///adds a "" character which should still increase the size
        ///</summary>
        [TestMethod()]
        public void SimpleBlankTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("", "");
            Assert.AreEqual(1, t.Size);
        }
        /// <summary>
        ///Empty graph should contain nothing
        ///</summary>
        [TestMethod()]
        public void SimpleEmptyRemoveTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("x", "y");
            Assert.AreEqual(1, t.Size);
            t.RemoveDependency("x", "y");
            Assert.AreEqual(0, t.Size);
        }
        /// <summary>
        ///Empty graph should contain nothing
        ///</summary>
        [TestMethod()]
        public void EmptyEnumeratorTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("x", "y");
            IEnumerator<string> e1 = t.GetDependees("y").GetEnumerator();
            Assert.IsTrue(e1.MoveNext());
            Assert.AreEqual("x", e1.Current);
            IEnumerator<string> e2 = t.GetDependents("x").GetEnumerator();
            Assert.IsTrue(e2.MoveNext());
            Assert.AreEqual("y", e2.Current);
            t.RemoveDependency("x", "y");
            Assert.IsFalse(t.GetDependees("y").GetEnumerator().MoveNext());
            Assert.IsFalse(t.GetDependents("x").GetEnumerator().MoveNext());
        }
        /// <summary>
        ///Replace on an empty DG shouldn't fail
        ///</summary>
        [TestMethod()]
        public void SimpleReplaceTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("x", "y");
            Assert.AreEqual(t.Size, 1);
            t.RemoveDependency("x", "y");
            t.ReplaceDependents("x", new HashSet<string>());
            t.ReplaceDependees("y", new HashSet<string>());
        }
        ///<summary>
        ///It should be possible to have more than one DG at a time.
        ///</summary>
        [TestMethod()]
        public void StaticTest()
        {
            DependencyGraph t1 = new DependencyGraph();
            DependencyGraph t2 = new DependencyGraph();
            t1.AddDependency("x", "y");
            Assert.AreEqual(1, t1.Size);
            Assert.AreEqual(0, t2.Size);
        }
        /// <summary>
        ///Non-empty graph contains something
        ///</summary>
        [TestMethod()]
        public void SizeTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            t.AddDependency("a", "c");
            t.AddDependency("c", "b");
            t.AddDependency("b", "d");
            Assert.AreEqual(4, t.Size);
        }
        /// <summary>
        ///Non-empty graph contains something
        ///</summary>
        [TestMethod()]
        public void EnumeratorTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            t.AddDependency("a", "c");
            t.AddDependency("c", "b");
            t.AddDependency("b", "d");
            IEnumerator<string> e = t.GetDependees("a").GetEnumerator();
            Assert.IsFalse(e.MoveNext());
            e = t.GetDependees("b").GetEnumerator();
            Assert.IsTrue(e.MoveNext());
            String s1 = e.Current;
            Assert.IsTrue(e.MoveNext());
            String s2 = e.Current;
            Assert.IsFalse(e.MoveNext());
            Assert.IsTrue(((s1 == "a") && (s2 == "c")) || ((s1 == "c") && (s2 == "a")));
            e = t.GetDependees("c").GetEnumerator();
            Assert.IsTrue(e.MoveNext());
            Assert.AreEqual("a", e.Current);
            Assert.IsFalse(e.MoveNext());
            e = t.GetDependees("d").GetEnumerator();
            Assert.IsTrue(e.MoveNext());
            Assert.AreEqual("b", e.Current);
            Assert.IsFalse(e.MoveNext());
        }
        /// <summary>
        ///Non-empty graph contains something
        ///</summary>
        [TestMethod()]
        public void ReplaceThenEnumerate()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("x", "b");
            t.AddDependency("a", "z");
            t.ReplaceDependents("b", new HashSet<string>());
            t.AddDependency("y", "b");
            t.ReplaceDependents("a", new HashSet<string>() { "c" });
            t.AddDependency("w", "d");
            t.ReplaceDependees("b", new HashSet<string>() { "a", "c" });
            t.ReplaceDependees("d", new HashSet<string>() { "b" });
            IEnumerator<string> e = t.GetDependees("a").GetEnumerator();
            Assert.IsFalse(e.MoveNext());
            e = t.GetDependees("b").GetEnumerator();
            Assert.IsTrue(e.MoveNext());
            String s1 = e.Current;
            Assert.IsTrue(e.MoveNext());
            String s2 = e.Current;
            Assert.IsFalse(e.MoveNext());
            Assert.IsTrue(((s1 == "a") && (s2 == "c")) || ((s1 == "c") && (s2 == "a")));
            e = t.GetDependees("c").GetEnumerator();
            Assert.IsTrue(e.MoveNext());
            Assert.AreEqual("a", e.Current);
            Assert.IsFalse(e.MoveNext());
            e = t.GetDependees("d").GetEnumerator();
            Assert.IsTrue(e.MoveNext());
            Assert.AreEqual("b", e.Current);
            Assert.IsFalse(e.MoveNext());
        }
        /// <summary>
        ///Using lots of data
        ///</summary>
        [TestMethod()]
        public void StressTest()
        {
            // Dependency graph
            DependencyGraph t = new DependencyGraph();
            // A bunch of strings to use
            const int SIZE = 200;
            string[] letters = new string[SIZE];
            for (int i = 0; i < SIZE; i++)
            {
                letters[i] = ("" + (char)('a' + i));
            }
            // The correct answers
            HashSet<string>[] dents = new HashSet<string>[SIZE];
            HashSet<string>[] dees = new HashSet<string>[SIZE];
            for (int i = 0; i < SIZE; i++)
            {
                dents[i] = new HashSet<string>();
                dees[i] = new HashSet<string>();
            }
            // Add a bunch of dependencies
            for (int i = 0; i < SIZE; i++)
            {
                for (int j = i + 1; j < SIZE; j++)
                {
                    t.AddDependency(letters[i], letters[j]);
                    dents[i].Add(letters[j]);
                    dees[j].Add(letters[i]);
                }
            }
            // Remove a bunch of dependencies
            for (int i = 0; i < SIZE; i++)
            {
                for (int j = i + 4; j < SIZE; j += 4)
                {
                    t.RemoveDependency(letters[i], letters[j]);
                    dents[i].Remove(letters[j]);
                    dees[j].Remove(letters[i]);
                }
            }
            // Add some back
            for (int i = 0; i < SIZE; i++)
            {
                for (int j = i + 1; j < SIZE; j += 2)
                {
                    t.AddDependency(letters[i], letters[j]);
                    dents[i].Add(letters[j]);
                    dees[j].Add(letters[i]);
                }
            }
            // Remove some more
            for (int i = 0; i < SIZE; i += 2)
            {
                for (int j = i + 3; j < SIZE; j += 3)
                {
                    t.RemoveDependency(letters[i], letters[j]);
                    dents[i].Remove(letters[j]);
                    dees[j].Remove(letters[i]);
                }
            }
            // Make sure everything is right
            for (int i = 0; i < SIZE; i++)
            {
                Assert.IsTrue(dents[i].SetEquals(new
        HashSet<string>(t.GetDependents(letters[i]))));
                Assert.IsTrue(dees[i].SetEquals(new
        HashSet<string>(t.GetDependees(letters[i]))));
            }

        }
        /// <summary>
        ///Non-empty graph contains something as well as a valid count of dependees. 
        ///</summary>
        [TestMethod()]
        public void SimpleNumberOfDependeesTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            t.AddDependency("a", "c");
            t.AddDependency("c", "b");
            t.AddDependency("b", "d");
            Assert.AreEqual(4, t.Size);
            Assert.AreEqual(0, t["a"]);
            Assert.AreEqual(2, t["b"]);
            Assert.AreEqual(1, t["c"]);
            Assert.AreEqual(1, t["d"]);
        }
        /// <summary>
        ///Non-empty graph contains something as well as a valid count of dependees. 
        ///After performing removes we should get the same thing
        ///</summary>
        [TestMethod()]
        public void difficultNumberOfDependeesTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            t.AddDependency("a", "c");
            t.AddDependency("c", "b");
            t.AddDependency("b", "d");
            Assert.AreEqual(4, t.Size);
            Assert.AreEqual(0, t["a"]);
            Assert.AreEqual(2, t["b"]);
            Assert.AreEqual(1, t["c"]);
            Assert.AreEqual(1, t["d"]);
            t.RemoveDependency("a", "b");
            Assert.AreEqual(1, t["b"]);
        }
        /// <summary>
        ///Non-empty graph contains something as well as a valid count of dependees. 
        ///After performing removes we should get the same thing
        ///</summary>
        [TestMethod()]
        public void advancedNumberOfDependeesTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            t.AddDependency("a", "c");
            t.AddDependency("c", "b");
            t.AddDependency("b", "d");
            Assert.AreEqual(4, t.Size);
            Assert.AreEqual(0, t["a"]);
            Assert.AreEqual(2, t["b"]);
            Assert.AreEqual(1, t["c"]);
            Assert.AreEqual(1, t["d"]);
            t.RemoveDependency("a", "b");
            Assert.AreEqual(1, t["b"]);
            t.AddDependency("a", "b");
            t.AddDependency("b", "b");
            t.AddDependency("c", "b");
            t.AddDependency("d", "b");
            t.RemoveDependency("b", "a");
            t.RemoveDependency("b", "b");
            t.RemoveDependency("b", "c");
            t.RemoveDependency("b", "d");
            Assert.AreEqual(0, t["b"]);
        }
        /// <summary>
        ///test our getters hasDependent and hasDependee
        ///</summary>
        [TestMethod()]
        public void SimpleGettersTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            t.AddDependency("a", "c");
            t.AddDependency("c", "b");
            t.AddDependency("b", "d");
            Assert.IsTrue(t.HasDependents("b"));
            Assert.IsTrue(t.HasDependents("a"));
            Assert.IsFalse(t.HasDependents("d"));
            Assert.IsTrue(t.HasDependents("c"));
            Assert.IsTrue(t.HasDependees("b"));
            Assert.IsFalse(t.HasDependees("a"));
            Assert.IsTrue(t.HasDependees("d"));
            Assert.IsTrue(t.HasDependees("c"));

        }
        /// <summary>
        ///test our getters hasDependent and hasDependee
        ///</summary>
        [TestMethod()]
        public void difficultGettersTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            t.AddDependency("a", "c");
            t.AddDependency("c", "b");
            t.AddDependency("b", "d");
            Assert.IsTrue(t.HasDependents("b"));
            Assert.IsTrue(t.HasDependents("a"));
            Assert.IsFalse(t.HasDependents("d"));
            Assert.IsTrue(t.HasDependents("c"));
            t.RemoveDependency("c", "b");
            t.RemoveDependency("a", "c");
            Assert.IsFalse(t.HasDependents("c"));
            Assert.IsFalse(t.HasDependees("c"));
        }
        /// <summary>
        ///test our getters hasDependent and hasDependee
        ///</summary>
        [TestMethod()]
        public void advancedGettersTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            t.AddDependency("a", "c");
            t.AddDependency("c", "b");
            t.AddDependency("b", "d");
            Assert.IsTrue(t.HasDependents("b"));
            Assert.IsTrue(t.HasDependents("a"));
            Assert.IsFalse(t.HasDependents("d"));
            Assert.IsTrue(t.HasDependents("c"));
            t.RemoveDependency("c", "b");
            t.RemoveDependency("a", "c");
            Assert.IsFalse(t.HasDependents("c"));
            Assert.IsFalse(t.HasDependees("c"));
            t.AddDependency("c", "e");
            Assert.IsTrue(t.HasDependents("c"));
            Assert.IsTrue(t.HasDependees("e"));
        }/// <summary>
         ///test our all cases of our addDependency Method
         ///</summary>
        [TestMethod()]
        public void simpleAddTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            t.AddDependency("z", "c");
            t.AddDependency("z", "b");
            IEnumerator<string> e = t.GetDependees("b").GetEnumerator();
            Assert.IsTrue(e.MoveNext());
            String s1 = e.Current;
            Assert.IsTrue(e.MoveNext());
            String s2 = e.Current;
            Assert.IsTrue(s1 == "a" && s2 == "z");
            IEnumerator<string> f = t.GetDependents("z").GetEnumerator(); // makes sure the other table is edited to match
            Assert.IsTrue(f.MoveNext());
            String s5 = f.Current;
            Assert.IsTrue(f.MoveNext());
            String s6 = f.Current;
            Assert.IsTrue(s5 == "c"&& s6 == "b");
        }
        /// <summary>
        ///test our all cases of our addDependency Method
        ///</summary>
        [TestMethod()]
        public void advancedAddTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            t.AddDependency("a", "c");
            t.AddDependency("c", "b");
            t.AddDependency("b", "d");
            t.AddDependency("a", "t");
            t.AddDependency("a", "d");
            t.AddDependency("z", "c");
            IEnumerator<string> e = t.GetDependents("a").GetEnumerator();
            Assert.IsTrue(e.MoveNext());
            String s1 = e.Current;
            Assert.IsTrue(e.MoveNext());
            String s2 = e.Current;
            Assert.IsTrue(e.MoveNext());
            String s3 = e.Current;
            Assert.IsTrue(e.MoveNext());
            String s4 = e.Current;
            Assert.IsTrue(s1 == "b" && s2 == "c" && s3 == "t" && s4 == "d");
            IEnumerator<string> f = t.GetDependees("c").GetEnumerator(); // makes sure the other table is edited to match
            Assert.IsTrue(f.MoveNext());
            String s5 = f.Current;
            Assert.IsTrue(f.MoveNext());
            String s6 = f.Current;
            Assert.IsTrue(s5 == "a" && s6 == "z");
            Assert.IsTrue(t.Size == 7);
        }
        /// <summary>
        ///test our all cases of our removeDependency Method
        ///</summary>
        [TestMethod()]
        public void advancedRemoveTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            t.AddDependency("a", "c");
            t.AddDependency("c", "b");
            t.AddDependency("b", "d");
            t.AddDependency("a", "t");
            t.AddDependency("a", "d");
            t.AddDependency("z", "c");
            t.RemoveDependency("a", "b");
            t.RemoveDependency("b", "c");
            Assert.IsTrue(t.Size == 5);
            t.RemoveDependency("c", "d");
            Assert.IsTrue(t.Size == 5); // nothing should be removed
            t.RemoveDependency("t", "z");
            Assert.IsTrue(t.Size == 5); // nothing should be removed
        }
        /// <summary>
        ///test to see that duplicate dependency is not added
        ///</summary>
        [TestMethod()]
        public void duplicateAddTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            t.AddDependency("a", "c");
            t.AddDependency("c", "b");
            t.AddDependency("b", "d");
            t.AddDependency("a", "b"); // our duplicate
            Assert.IsTrue(t.Size == 4);
        }
        /// <summary>
        ///test that nothing is removed when removing a non existent dependency
        ///</summary>
        [TestMethod()]
        public void removeNonExistentTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            t.AddDependency("a", "c");
            t.AddDependency("c", "b");
            t.AddDependency("b", "d");
            t.RemoveDependency("a", "x");
            t.RemoveDependency("y", "l");
            t.RemoveDependency("x", "c");
            Assert.IsTrue(t.Size == 4);
        }
        /// <summary>
        ///test our replace dependents method 
        ///</summary>
        [TestMethod()]
        public void replaceDependentsTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            t.AddDependency("a", "c");
            t.AddDependency("c", "b");
            t.AddDependency("b", "d");
            t.ReplaceDependents("a", new HashSet<string>() { "z", "y", "g", "f" });
            IEnumerator<string> e = t.GetDependents("a").GetEnumerator();
            List<String> s = new List<String>();
            while(e.MoveNext())
                s.Add(e.Current);
            Assert.IsTrue(s.Contains("z"));
            Assert.IsTrue(s.Contains("y"));
            Assert.IsTrue(s.Contains("g"));
            Assert.IsTrue(s.Contains("f"));
            Assert.IsTrue(t.HasDependees("z"));// checks the other table has been edited properly
            Assert.IsTrue(t.HasDependees("y"));
            Assert.IsTrue(t.HasDependees("g"));
            Assert.IsTrue(t.HasDependees("f"));
            Assert.IsFalse(t.HasDependees("c"));
        }
        /// <summary>
        ///test our replace dependents method if we ask to replace the items on something not in the table
        ///</summary>
        [TestMethod()]
        public void replaceDependentsOnItemNotInTableTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.ReplaceDependents("a", new HashSet<string>() { "z", "y", "g", "f" });
            IEnumerator<string> e = t.GetDependents("a").GetEnumerator();
            List<String> s = new List<String>();
            while (e.MoveNext())
                s.Add(e.Current);
            Assert.IsTrue(s.Contains("z"));
            Assert.IsTrue(s.Contains("y"));
            Assert.IsTrue(s.Contains("g"));
            Assert.IsTrue(s.Contains("f"));
            Assert.IsTrue(t.HasDependees("z"));// checks the other table has been edited properly
            Assert.IsTrue(t.HasDependees("y"));
            Assert.IsTrue(t.HasDependees("g"));
            Assert.IsTrue(t.HasDependees("f"));
        }
        /// <summary>
        ///test our replace dependees method if we ask to replace the items on something not in the table
        ///</summary>
        [TestMethod()]
        public void replaceDependeesOnItemNotInTableTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.ReplaceDependees("a", new HashSet<string>() { "z", "y", "g", "f" });
            IEnumerator<string> e = t.GetDependees("a").GetEnumerator();
            List<String> s = new List<String>();
            while (e.MoveNext())
                s.Add(e.Current);
            Assert.IsTrue(s.Contains("z"));
            Assert.IsTrue(s.Contains("y"));
            Assert.IsTrue(s.Contains("g"));
            Assert.IsTrue(s.Contains("f"));
            Assert.IsTrue(t.HasDependents("z"));// checks the other table has been edited properly
            Assert.IsTrue(t.HasDependents("y"));
            Assert.IsTrue(t.HasDependents("g"));
            Assert.IsTrue(t.HasDependents("f"));
        }
        /// <summary>
        ///test our replace dependees method 
        ///</summary>
        [TestMethod()]
        public void replaceDependeesTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            t.AddDependency("a", "c");
            t.AddDependency("c", "b");
            t.AddDependency("b", "d");
            t.ReplaceDependees("b", new HashSet<string>() { "z", "y", "g", "f" });
            IEnumerator<string> e = t.GetDependees("b").GetEnumerator();
            List<String> s = new List<String>();
            while (e.MoveNext())
                s.Add(e.Current);
            Assert.IsTrue(s.Contains("z"));
            Assert.IsTrue(s.Contains("y"));
            Assert.IsTrue(s.Contains("g"));
            Assert.IsTrue(s.Contains("f"));
            Assert.IsTrue(t.HasDependents("z"));// checks the other table has been edited properly
            Assert.IsTrue(t.HasDependents("y"));
            Assert.IsTrue(t.HasDependents("g"));
            Assert.IsTrue(t.HasDependents("f"));
            Assert.IsFalse(t.HasDependents("c"));
        }
        /// <summary>
        ///Checks if false is returned when a value doesnt have dependents 
        /// or if a value doesnt have dependees. 
        ///</summary>
        [TestMethod()]
        public void hasDependentsDependeesFalseTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            Assert.IsFalse(t.HasDependents("b"));
            Assert.IsFalse(t.HasDependees("a"));
        }
        /// <summary>
        ///Checks if false is returned when a value doesnt exist in our table
        ///</summary>
        [TestMethod()]
        public void hasDependentsDependeesNonExistantTest()
        {
            DependencyGraph t = new DependencyGraph();
            Assert.IsFalse(t.HasDependents("b"));
            Assert.IsFalse(t.HasDependees("a"));
        }
        /// <summary>
        ///Using lots of data, up to 1000 items
        ///</summary>
        [TestMethod()]
        public void Stress02Test()
        {
            // Dependency graph
            DependencyGraph t = new DependencyGraph();
            // A bunch of strings to use
            const int SIZE = 1000;
            string[] letters = new string[SIZE];
            for (int i = 0; i < SIZE; i++)
            {
                letters[i] = ("" + (char)('a' + i));
            }
            // The correct answers
            HashSet<string>[] dents = new HashSet<string>[SIZE];
            HashSet<string>[] dees = new HashSet<string>[SIZE];
            for (int i = 0; i < SIZE; i++)
            {
                dents[i] = new HashSet<string>();
                dees[i] = new HashSet<string>();
            }
            // Add a bunch of dependencies
            for (int i = 0; i < SIZE; i++)
            {
                for (int j = i + 1; j < SIZE; j++)
                {
                    t.AddDependency(letters[i], letters[j]);
                    dents[i].Add(letters[j]);
                    dees[j].Add(letters[i]);
                }
            }
            // Remove a bunch of dependencies
            for (int i = 0; i < 250; i++)
            {
                for (int j = i + 4; j < 250; j += 4)
                {
                    t.RemoveDependency(letters[i], letters[j]);
                    dents[i].Remove(letters[j]);
                    dees[j].Remove(letters[i]);
                }
            }
            // Add some back
            for (int i = 0; i < 100; i++)
            {
                for (int j = i + 1; j < 100; j += 2)
                {
                    t.AddDependency(letters[i], letters[j]);
                    dents[i].Add(letters[j]);
                    dees[j].Add(letters[i]);
                }
            }
 
            // Make sure everything is right
            for (int i = 0; i < SIZE; i++)
            {
                Assert.IsTrue(dents[i].SetEquals(new
        HashSet<string>(t.GetDependents(letters[i]))));
                Assert.IsTrue(dees[i].SetEquals(new
        HashSet<string>(t.GetDependees(letters[i]))));
            }
        }
        /// <summary>
        ///test the example given in the header comment of the start of the project
        ///</summary>
        [TestMethod()]
        public void secondaryAddTest()
        {
            DependencyGraph t = new DependencyGraph(); // tests the dependents first
            t.AddDependency("a", "b");
            t.AddDependency("a", "c");
            t.AddDependency("b", "d");
            t.AddDependency("d", "d");
            IEnumerator<string> e = t.GetDependents("a").GetEnumerator();
            Assert.IsTrue(e.MoveNext());
            String s1 = e.Current;
            Assert.IsTrue(e.MoveNext());
            String s2 = e.Current;
            e = t.GetDependents("b").GetEnumerator();
            Assert.IsTrue(e.MoveNext());
            String s3 = e.Current;
            e = t.GetDependents("d").GetEnumerator();
            Assert.IsTrue(e.MoveNext());
            String s4 = e.Current;
            e = t.GetDependents("c").GetEnumerator();
            Assert.IsFalse(e.MoveNext());
            Assert.IsTrue(s1 == "b" && s2 == "c" && s3 == "d" && s4 == "d");

            e = t.GetDependees("b").GetEnumerator();// here down tests the dependees
            Assert.IsTrue(e.MoveNext());
            String s5 = e.Current;
            e = t.GetDependees("c").GetEnumerator();
            Assert.IsTrue(e.MoveNext());
            String s6 = e.Current;
            e = t.GetDependees("d").GetEnumerator();
            Assert.IsTrue(e.MoveNext());
            String s7 = e.Current;
            Assert.IsTrue(e.MoveNext());
            String s8 = e.Current;
            e = t.GetDependees("a").GetEnumerator();
            Assert.IsFalse(e.MoveNext());
            Assert.IsTrue(s5 == "a" && s6 == "a" && s7 == "b" && s8 == "d");
        }
        ///<summary>
        ///It should be possible to have more than one DG at a time. This will test that functionality.
        ///</summary>
        [TestMethod()]
        public void multipleGraphsTestsReplaceDependents()
        {
            DependencyGraph t1 = new DependencyGraph();
            DependencyGraph t2 = new DependencyGraph();
            t1.AddDependency("x", "y");
            t2.AddDependency("z", "x");
            t2.AddDependency("x", "y");
            Assert.IsTrue(t1.Size == 1);
            Assert.IsTrue(t2.Size == 2);
            t1.ReplaceDependents("x", new HashSet<string>() { "z", "y", "g", "f" });
            t2.ReplaceDependents("z", new HashSet<string>() { "a", "b", "c", "d" });
            IEnumerator<string> e = t1.GetDependents("x").GetEnumerator();//checks the first dependency graph
            List<String> s = new List<String>();
            while (e.MoveNext())
                s.Add(e.Current);
            Assert.IsTrue(s.Contains("z"));
            Assert.IsTrue(s.Contains("y"));
            Assert.IsTrue(s.Contains("g"));
            Assert.IsTrue(s.Contains("f"));
            Assert.IsTrue(t1.HasDependees("z"));// checks the other table has been edited properly
            Assert.IsTrue(t1.HasDependees("y"));
            Assert.IsTrue(t1.HasDependees("g"));
            Assert.IsTrue(t1.HasDependees("f"));

             e = t2.GetDependents("z").GetEnumerator();//checks the second dependency graph
            List<String>g = new List<String>();
            while (e.MoveNext())
                g.Add(e.Current);
            Assert.IsTrue(g.Contains("a"));
            Assert.IsTrue(g.Contains("b"));
            Assert.IsTrue(g.Contains("c"));
            Assert.IsTrue(g.Contains("d"));
            Assert.IsTrue(t2.HasDependees("a"));// checks the other table has been edited properly
            Assert.IsTrue(t2.HasDependees("b"));
            Assert.IsTrue(t2.HasDependees("c"));
            Assert.IsTrue(t2.HasDependees("d"));
        }
        ///<summary>
        ///It should be possible to have more than one DG at a time. This will test that functionality.
        ///</summary>
        [TestMethod()]
        public void multipleGraphsTestsReplaceDependees()
        {
            DependencyGraph t1 = new DependencyGraph();
            DependencyGraph t2 = new DependencyGraph();
            t1.AddDependency("x", "y");
            t2.AddDependency("z", "x");
            t2.AddDependency("x", "y");
            Assert.IsTrue(t1.Size == 1);
            Assert.IsTrue(t2.Size == 2);
            t1.ReplaceDependees("y", new HashSet<string>() { "z", "y", "g", "f" });
            t2.ReplaceDependees("x", new HashSet<string>() { "a", "b", "c", "d" });
            IEnumerator<string> e = t1.GetDependees("y").GetEnumerator();//checks the first dependency graph
            List<String> s = new List<String>();
            while (e.MoveNext())
                s.Add(e.Current);
            Assert.IsTrue(s.Contains("z"));
            Assert.IsTrue(s.Contains("y"));
            Assert.IsTrue(s.Contains("g"));
            Assert.IsTrue(s.Contains("f"));
            Assert.IsTrue(t1.HasDependents("z"));// checks the other table has been edited properly
            Assert.IsTrue(t1.HasDependents("y"));
            Assert.IsTrue(t1.HasDependents("g"));
            Assert.IsTrue(t1.HasDependents("f"));
  
            e = t2.GetDependees("x").GetEnumerator();//checks the second dependency graph
            List<String> g = new List<String>();
            while (e.MoveNext())
                g.Add(e.Current);
            Assert.IsTrue(g.Contains("a"));
            Assert.IsTrue(g.Contains("b"));
            Assert.IsTrue(g.Contains("c"));
            Assert.IsTrue(g.Contains("d"));
            Assert.IsTrue(t2.HasDependents("a"));// checks the other table has been edited properly
            Assert.IsTrue(t2.HasDependents("b"));
            Assert.IsTrue(t2.HasDependents("c"));
            Assert.IsTrue(t2.HasDependents("d"));

        }
    }


}