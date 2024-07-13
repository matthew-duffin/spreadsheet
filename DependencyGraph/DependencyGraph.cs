// Skeleton implementation written by Joe Zachary for CS 3500, September 2013.
// Version 1.1 (Fixed error in comment for RemoveDependency.)
// Version 1.2 - Daniel Kopta 
//               (Clarified meaning of dependent and dependee.)
//               (Clarified names in solution/project structure.)
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.ComponentModel;

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
    ///   This class handles dependencies between cells of a spreadsheet

    /// </summary>
    /// <summary>
    /// (s1,t1) is an ordered pair of strings
    /// t1 depends on s1; s1 must be evaluated before t1
    /// 
    /// A DependencyGraph can be modeled as a set of ordered pairs of strings.  Two ordered pairs
    /// (s1,t1) and (s2,t2) are considered equal if and only if s1 equals s2 and t1 equals t2.
    /// Recall that sets never contain duplicates.  If an attempt is made to add an element to a
    /// set, and the element is already in the set, the set remains unchanged.
    /// 
    /// Given a DependencyGraph DG:
    /// 
    ///    (1) If s is a string, the set of all strings t such that (s,t) is in DG is called dependents(s).
    ///        (The set of things that depend on s)    
    ///        
    ///    (2) If s is a string, the set of all strings t such that (t,s) is in DG is called dependees(s).
    ///        (The set of things that s depends on) 
    //
    // For example, suppose DG = {("a", "b"), ("a", "c"), ("b", "d"), ("d", "d")}
    //     dependents("a") = {"b", "c"}
    //     dependents("b") = {"d"}
    //     dependents("c") = {}
    //     dependents("d") = {"d"}
    //     dependees("a") = {}
    //     dependees("b") = {"a"}
    //     dependees("c") = {"a"}
    //     dependees("d") = {"b", "d"}
    /// </summary>
    public class DependencyGraph
    {
        private Dictionary<String, HashSet<String>> dependents;
        private Dictionary<String, HashSet<String>> dependees;
        private int size;
        private bool hasDependants;
        private bool hasDependees;
        private bool itemRemoved;

        /// <summary>
        /// Creates an empty DependencyGraph.
        /// </summary>
        public DependencyGraph()
        {
            dependents = new Dictionary<String, HashSet<String>>();
            dependees = new Dictionary<String, HashSet<String>>();
            size = 0;
            hasDependants = false;
            hasDependees = false;
            itemRemoved = false;
        }
        /// <summary>
        /// The number of ordered pairs in the DependencyGraph.
        /// </summary>
        public int Size
        {
            get {return size; }
        }
        /// <summary>
        /// The size of dependees(s).
        /// This property is an example of an indexer.  If dg is a DependencyGraph, you would
        /// invoke it like this:
        /// dg["a"]
        /// It should return the size of dependees("a")
        /// </summary>
        public int this[string s]
        {
            get { return getValuesInTable(s, 0).Count; }
        }
        /// <summary>
        /// Reports whether dependents(s) is non-empty.
        /// </summary>
        public bool HasDependents(string s)
        {
            if (getValuesInTable(s, 1).Count != 0)
                hasDependants = true;
            else
                hasDependants = false;
            return hasDependants;
        }
        /// <summary>
        /// Reports whether dependees(s) is non-empty.
        /// </summary>
        public bool HasDependees(string s)
        {
            if (getValuesInTable(s, 0).Count != 0)
                hasDependees = true;
            else
                hasDependees = false;
            return hasDependees;
        }
        /// <summary>
        /// Enumerates dependents(s).
        /// </summary>
        public IEnumerable<string> GetDependents(string s)
        {
            return getValuesInTable(s, 1);
        }
        /// <summary>
        /// Enumerates dependees(s).
        /// </summary>
        public IEnumerable<string> GetDependees(string s)
        {
            return getValuesInTable(s, 0);
        }
        /// <summary>
        /// <para>Adds the ordered pair (s,t), if it doesn't exist</para>
        /// 
        /// <para>This should be thought of as:</para>   
        /// 
        ///   t depends on s
        ///
        /// </summary>
        /// <param name="s"> s must be evaluated first. T depends on S</param>
        /// <param name="t"> t cannot be evaluated until s is</param>        /// 
        public void AddDependency(string s, string t)
        {
            if (!dependents.ContainsKey(s) && !dependees.ContainsKey(t)) // checks to see if neither exist in our table
            {
                addToTable(s, t, 1); // adds to dependents table
                addToTable(t, s, 0); // adds to dependees table
                size++;
            }
            else if (dependents.ContainsKey(s)) // checks to see if s exists but not t
            {
                HashSet<String> list = getValuesInTable(s, 1);
                if (!list.Contains(t))
                {
                    list.Add(t);
                    dependents[s] = list;
                    if (!dependees.ContainsKey(t))
                        addToTable(t, s, 0);
                    else //if t exists, but is not connected to s, we create that connection here
                    {
                        list = getValuesInTable(t, 0);
                        list.Add(s);
                        dependees[t] = list;
                    }
                    size++;
                }
            }
            else if (dependees.ContainsKey(t)) // checks to see if t exists but not s
            {
                HashSet<String> list = getValuesInTable(t, 0);
                if (!list.Contains(s))
                {
                    list.Add(s);
                    dependees[t] = list;
                    if (!dependents.ContainsKey(s))
                        addToTable(s, t, 1);
                    size++;
                }
            }
        }
        /// <summary>
        /// Removes the ordered pair (s,t), if it exists
        /// </summary>
        /// <param name="s"></param> first value in the ordered pair
        /// <param name="t"></param> second value in the ordered pair
        public void RemoveDependency(string s, string t)
        {
            itemRemoved = false;
            if (dependents.ContainsKey(s))
                remove(s, t, 1);
            if (dependees.ContainsKey(t))
                remove(t, s, 0);
            if (dependees.ContainsKey(s))
                remove(s, t, 0);
            if (dependents.ContainsKey(t))
                remove(t, s, 1);
            if (itemRemoved)
                size--;
        }

        /// <summary>
        /// Removes all existing ordered pairs of the form (s,r).  Then, for each
        /// t in newDependents, adds the ordered pair (s,t).
        /// </summary>
        public void ReplaceDependents(string s, IEnumerable<string> newDependents)
        {
            HashSet<String> list = getValuesInTable(s, 1);
            String[] copy = new String[list.Count]; // creates a copy of the list that wont be manipulated. This is also used to access the items at specific indexes. 
            list.CopyTo(copy);
            String? value = "";

            for (int i = 0; i < copy.Length; i++) // removes the connection the with the dependees. 
            {
                if (i != list.Count)
                {
                    if(list.TryGetValue(copy[i], out value))
                        RemoveDependency(value, s);
                }
                else //allows us to remove the dependency, even though the size of the original list has been changed. 
                    RemoveDependency(copy[i], s);
            }
            IEnumerator<string> enumerator = newDependents.GetEnumerator();
            foreach (string j in newDependents)// replaces the dependents with the new values.
            {
                AddDependency(s, j);
            }
        }
        /// <summary>
        /// Removes all existing ordered pairs of the form (r,s).  Then, for each 
        /// t in newDependees, adds the ordered pair (t,s).
        /// </summary>
        public void ReplaceDependees(string s, IEnumerable<string> newDependees)
        {
            HashSet<String> list = getValuesInTable(s, 0);
            String[] copy = new String[list.Count]; // creates a copy of the list that wont be manipulated. 
            list.CopyTo(copy);
            String? value = "";
     
            for (int i = 0; i < copy.Length; i++) // removes the connection the with the dependees. 
            {
                if (i != list.Count)
                {
                    if(list.TryGetValue(copy[i],out value))
                    RemoveDependency(value, s);
                }
                else //allows us to remove the dependency, even though the size of the original list has been changed. 
                    RemoveDependency(copy[i], s);
            }
            foreach (string j in newDependees)
            {
                AddDependency(j, s);
            }
        }
        /// <summary>
        /// adds values to our table
        /// </summary>
        /// <param name="key"></param> key to be added
        /// <param name="value"></param> value to be added.
        /// <param name="selection"></param> 1 if you want to add to dependents dictionary or 0 if you want to add to dependees graph.
        private void addToTable(String key, String value, int selection)
        {
            HashSet<String> list = new HashSet<String>();
            if (selection == 1)
            {
                if (!dependents.ContainsKey(key))
                    dependents.Add(key, list);
                else
                    list = getValuesInTable(key, 1);
                list.Add(value);
            }
            if (selection == 0)
            {
                if (!dependees.ContainsKey(key)) //checks to see if our key is already in the table
                    dependees.Add(key, list);
                else //if it is, get all its values and add the extra value 
                    list = getValuesInTable(key, 0);
                list.Add(value);
            }
        }
        /// <summary>
        /// returns the list of values attached to the keys in our dictionary
        /// </summary>
        /// <param name="key"></param>
        /// <returns>list of values connected to the key</returns>
        private HashSet<String> getValuesInTable(String key, int selector)
        {
            HashSet<String>? list = new HashSet<String>();
            HashSet<String> emptyList = new HashSet<String>();// a second list that is always empty that is returned if the list is null.
            if (selector == 1)
            {
                if (!dependents.TryGetValue(key, out list))
                {
                    if (list == null) // if the list value is returned as null, we return an empty list.
                        return emptyList;
                }
            }
            if (selector == 0)
            {
                dependees.TryGetValue(key, out list);
                if (list == null)// if the list value is returned as null, we return an empty list.
                    return emptyList;
            }

            return list;
        }
        /// <summary>
        /// performs a removal of an item from a list
        /// </summary>
        /// <param name="key"></param> the key our removal value is connected to
        /// <param name="value"></param> the value to be removed
        /// <param name="selector"></param>selects which table we are editing 
        public void remove(String key, String value, int selector)
        {
            HashSet<String> list = new HashSet<string>();
            if (selector == 1)
            {
                list = getValuesInTable(key, 1);
                if (list.Remove(value))
                    itemRemoved = true;
            }
            if (selector == 0)
            {
                list = getValuesInTable(key, 0);
                if (list.Remove(value))
                    itemRemoved = true;
            }
        }
    }
}