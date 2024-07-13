
using Microsoft.VisualBasic;
using SpreadsheetUtilities;
using SS;
using System;
using System.Data.SqlTypes;
using System.Reflection.Metadata;
using System.Xml;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace SS
{
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
    ///   This class handles certain parts building to a functional spreadsheet application.  
    /// Our basic spreadsheet class that has all spreadsheet functionality
    /// </summary>
    public class Spreadsheet : AbstractSpreadsheet
    {
        private Dictionary<String, Cell> cells;
        private DependencyGraph cellDependencies;
        private string? filePath;
        private Object? prevValue;
        private bool changed;
        public string? exceptionMessage;

        /// <summary>
        /// determines if our spreadsheet has been changed
        /// </summary>
        public override bool Changed { get => changed; protected set => changed = false; }

        /// <summary>
        /// The constructor for our spreadsheet object. It creates a dictionary of our cells, and a dependency graph of cell dependencies. 
        /// </summary>
        public Spreadsheet() :
        this(s => true, s => s, "default")
        {
        }
        /// <summary>
        /// The constructor for our spreadsheet object. It creates a dictionary of our cells, and a dependency graph of cell dependencies
        /// can use a validator, normalizer and version name. 
        /// </summary>
        public Spreadsheet(Func<string, bool> isValid, Func<string, string> normalize, string version) : base(isValid, normalize, version)
        {
            cells = new Dictionary<String, Cell>();
            cellDependencies = new DependencyGraph();
            changed = false;
        }
        /// <summary>
        /// The constructor for our spreadsheet object. It creates a dictionary of our cells, and a dependency graph of cell dependencies. Can use a
        /// file path 
        /// </summary>
        public Spreadsheet(String filePath, Func<string, bool> isValid, Func<string, string> normalize, string version) : base(isValid, normalize, version)
        {
            cells = new Dictionary<String, Cell>();
            cellDependencies = new DependencyGraph();
            this.filePath = filePath;
            changed = false;
            try
            {
                // Create an XmlReader inside this block, and automatically Dispose() it at the end.
                using (XmlReader reader = XmlReader.Create(filePath))
                {
                    string name = "";
                    while (reader.Read())
                    {
                        if (reader.IsStartElement()) // checks if we are at the start element
                        {
                            switch (reader.Name)
                            {
                                case "spreadsheet": // if we find version, we set its value to the version string
                                    {
                                        if (version != reader["version"])
                                            throw new SpreadsheetReadWriteException("version names did not match");
                                        break;
                                    }
                                case "cell":
                                    break; // no more direct info to read on the cell
                                case "name":
                                    reader.Read();
                                    name = reader.Value;
                                    break; // no more direct info to read on the name

                                case "contents":
                                    reader.Read();
                                    SetContentsOfCell(name, reader.Value);
                                    break;
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw new SpreadsheetReadWriteException("There was an error reading from the file.");
            }

        }
        /// <summary>
        /// Gets the contents of a cell
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <exception cref="InvalidNameException"></exception>
        public override object GetCellContents(string name)
        {
            name = Normalize(name);
            Cell? cell;
            //we dont use the method cell setter here because we dont want to actually change the cell if it does exist
            if(cells.TryGetValue(name, out cell))
                return cell.GetContent();
            else 
            {
                try //creates the cell
                {
                    SpreadSheetNameChecker(name);
                    if (!IsValid(name))
                        throw new InvalidNameException();
                    cell = new Cell(name, string.Empty, valueGetter); // adds an empty string since the cell is empty. 
                    cells.Add(name, cell);
                }
                catch (InvalidNameException)
                {
                    throw new InvalidNameException();
                }
            }
            
            return cell.GetContent(); // the contents should be either a string, formula or double
        }
        /// <summary>
        /// Gets the names of all empty cells.
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<string> GetNamesOfAllNonemptyCells()
        {
            return cells.Keys;
        }
        /// <summary>
        /// Sets the contents of cells to a double
        /// </summary>
        /// <param name="name">name of the cell</param>
        /// <param name="number">contents of the cell</param>
        /// <returns></returns>
        /// <exception cref="InvalidNameException"></exception>
        protected override IList<string> SetCellContents(string name, double number)
        {
            List<string> whoDependsOnMe = new List<string>();

            cellSetter(name, number); // adds all the necessary connections
            IEnumerable<String> s = GetCellsToRecalculate(FindWhoDependsOnMe(name));
            cellDependencies.ReplaceDependents(name, new HashSet<string>());
            whoDependsOnMe.Add(name); // adds the first value as the cell we looked at initially 
            foreach (var x in s)
            {
                whoDependsOnMe.Add(x);
            }
            return whoDependsOnMe;
        }
        /// <summary>
        /// Sets the contents of a cell to a string
        /// </summary>
        /// <param name="name"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        /// <exception cref="InvalidNameException"></exception>
        /// <exception cref="ArgumentNullException "></exception>
        protected override IList<string> SetCellContents(string name, string text)
        {
            List<string> whoDependsOnMe = new List<string>();

            cellSetter(name, text); // adds all the necessary connections

            IEnumerable<String> s = GetCellsToRecalculate(name);
            cellDependencies.ReplaceDependents(name, new HashSet<string>());
            foreach (var x in s)
            {
                whoDependsOnMe.Add(x);
            }
            return whoDependsOnMe;
        }

        /// <summary>
        /// sets the contents of a cell to formula
        /// </summary>
        /// <param name="name"></param>
        /// <param name="formula"></param>
        /// <returns></returns>
        /// <exception cref="InvalidNameException"></exception>
        /// <exception cref="ArgumentNullException "></exception>
        /// <exception cref="CircularException"></exception>
        protected override IList<string> SetCellContents(string name, Formula formula)
        {
            List<string> whoDependsOnMe = new List<string>();

            cellSetter(name, formula); // adds all the necessary connections
            IEnumerator<string> e1 = formula.GetVariables().GetEnumerator();
            while (e1.MoveNext())
            {
                cellDependencies.AddDependency(name, e1.Current);
            }
            IEnumerable<String> s = GetCellsToRecalculate(FindWhoDependsOnMe(name));
            whoDependsOnMe.Add(name);
            foreach (var x in s)
            {
                whoDependsOnMe.Add(x);
            }
            return whoDependsOnMe;
        }
        /// <summary>
        /// gets the direct dependents of a cell. 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        protected override IEnumerable<string> GetDirectDependents(string name)
        {
            return cellDependencies.GetDependees(name);
        }
        /// <summary>
        /// Determines which cells depend on a certain cell. 
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        private HashSet<String> FindWhoDependsOnMe(string name)
        {
            HashSet<String> whoDependsOnMe = new HashSet<String>();
            IEnumerator<String> DependentCells = GetDirectDependents(name).GetEnumerator();
            while (DependentCells.MoveNext()) //adds all of the dependees to a hash set
            {
                whoDependsOnMe.Add(DependentCells.Current);
            }
            return whoDependsOnMe;
        }
        /// <summary>
        /// Sets the cells and adds them to both the dependency graph and the cell dictionary
        /// </summary>
        /// <param name="name"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// <exception cref="InvalidNameException"></exception>
        private void cellSetter(string name, object obj)
        {
            prevValue = "";
            Cell? cell;
            //checks if the cell already exists
            if (!cells.ContainsKey(name))
            {
                if (obj is string)
                {
                    if (String.IsNullOrWhiteSpace((string)obj)) // if its an empty string, we dont want to add it to cells hash set, so we return
                    {
                        cell = new Cell(name, obj, valueGetter); // this is done to check we have a valid cell name
                        return;
                    }
                }
                cell = new Cell(name, obj, valueGetter);
                cells.Add(name, cell);
                changed = true;
            }
            else
            {
                if (cells.TryGetValue(name, out cell))
                { // if the cell already exists, we pull it out here. 
                    prevValue = cell.GetContent();
                    cell.ChangeContent(obj, valueGetter);
                    changed = true;
                    Object x = cell.GetContent();
                    if (x is string)
                    {
                        if (String.IsNullOrWhiteSpace((string)x))
                        {
                            cells.Remove(name);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// This method takes in an object for our content and determines which protected method it needs to go to.
        ///
        /// </summary>
        /// <param name="name"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public override IList<string> SetContentsOfCell(string name, string content)
        {
            IList<string> contents;
            SpreadSheetNameChecker(name);
            name = Normalize(name);
            if (!IsValid(name))
                throw new InvalidNameException();
            if (double.TryParse(content, out double token))
            {
                contents = SetCellContents(name, token);
            }
            else if (content.Length > 0 && content[0] == '=') // if the first character is an '=' we know that it is a formula. 
            {
                content = content.Remove(0, 1); // we need to remove the '=' as our formula object will call this an invalid formula. 
                Formula f = new Formula(content, Normalize, IsValid);
                contents = SetCellContents(name, f);
            }
            else
                contents = SetCellContents(name, content);
            foreach (string variable in contents)
            {
                if (variable != name)
                {
                    if (cells.TryGetValue(variable, out Cell? cell))
                    {
                        cell.ChangeValue(valueGetter);
                        
                    }
                        
                }
            }

            return contents;
        }
        /// <summary>
        /// This method reads the 
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        /// <exception cref="SpreadsheetReadWriteException"></exception>
        public override string GetSavedVersion(string filename)
        {
            String version = "";
            try
            {
                // Create an XmlReader inside this block, and automatically Dispose() it at the end.
                using (XmlReader reader = XmlReader.Create(filename))
                {
                    while (reader.Read())
                    {
                        if (reader.IsStartElement()) // checks if we are at the start element
                        {
                            switch (reader.Name)
                            {
                                case "spreadsheet": // if we find version, we set its value to the version string
                                    {
                                        String? isNull = reader.GetAttribute("version");
                                        if (isNull == null)
                                            throw new SpreadsheetReadWriteException("no attribute for version");
                                        else
                                            version = isNull;
                                        break;
                                    }
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw new SpreadsheetReadWriteException("There was an error reading from the file.");
            }
            return version;
        }
        /// <summary>
        /// Saves our spreadsheet by writing to a file in this format
        /// 
        ///  /// <spreadsheet version="version information goes here">
        /// 
        /// <cell>
        /// <name>cell name goes here</name>
        /// <contents>cell contents goes here</contents>    
        /// </cell>
        /// 
        /// </spreadsheet>
        /// 
        /// </summary>
        /// <param name="filename"></param>
        /// <exception cref="SpreadsheetReadWriteException"></exception>
        public override void Save(string filename)
        {
            Cell? cell;
            Object? obj;
            // We want some non-default settings for our XML writer.
            // Specifically, use indentation to make it more (human) readable.
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Async = true;
            settings.Indent = true;
            settings.IndentChars = "  ";


            // Some if statement that checks if a valid path to use.
            // Could be unauthorized access, path doesn't exist, etc.
            // Some logic: if path name not in this format --> return.

            // this method is called from a while loop, where it keeps calling this,
            // keeps displaying alerts of bad path,  until user exits out (decides
            // to not save), or gives a valid file path to save it in.
            // therefore, try/catch block never encounters exception.

            try
            {
                using (XmlWriter writer = XmlWriter.Create(filename, settings))
                {

                    writer.WriteStartDocument();
                    writer.WriteStartElement("spreadsheet");
                    writer.WriteAttributeString("version", Version);

                    IEnumerator<string> e1 = GetNamesOfAllNonemptyCells().GetEnumerator(); // get all empty cells.
                    while (e1.MoveNext())
                    {
                        writer.WriteStartElement("cell");
                        writer.WriteStartElement("name");
                        writer.WriteString(e1.Current);// writes the cell key as the name. 
                        writer.WriteEndElement(); // ends the name block 

                        if (cells.TryGetValue(e1.Current, out cell))
                        {
                            obj = cell.GetContent(); // pulls out the content of the cell. 
                            if (obj is string)
                            {
                                writer.WriteStartElement("contents");
                                writer.WriteString((string)obj);
                            }
                            if (obj is double)
                            {
                                writer.WriteStartElement("contents");
                                writer.WriteString(obj.ToString());
                            }
                            if (obj is Formula)
                            {
                                Formula f = (Formula)obj;
                                writer.WriteStartElement("contents");
                                writer.WriteString("=" + f.ToString());
                            }
                            writer.WriteEndElement();
                            writer.WriteEndElement(); // ends the cell block
                        }
                    }
                    writer.WriteEndElement(); // Ends the Spreadsheet block
                    writer.WriteEndDocument(); // ends the document
                    changed = false; // since its been saved the document has no longer been changed
                }
            }

            catch (Exception e)
            {
                exceptionMessage = e.Message;
                throw new SpreadsheetReadWriteException("there was an issue writing to the file");
                // Exceptions: System.IO.DirectoryNotFoundException --> invalid path
                // Exceptions: System.IO.IOException --> file is read only, or some other IO error
            }

        }
        /// <summary>
        /// Gets the value of a cell opposed to the object 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <exception cref="InvalidNameException"></exception>
        public override object GetCellValue(string name)
        {
            Cell? cell;
            name = Normalize(name);
            cells.TryGetValue(name, out cell);
            if (cell == null)
                return "";
            return cell.GetValue();
        }
        /// <summary>
        /// A lookup delegate for our evaluate method that is passed into the cell class. 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public double valueGetter(String name)
        {
            Cell? cell;
            Object obj = new object();
            if (cells.TryGetValue(name, out cell))// gets the cell we are getting the value of 
            {
                if (cell.GetContent() is double)
                    obj = cell.GetContent();
                else if (cell.GetContent() is string) // if its a string, this will result in a formula error. 
                    throw new ArgumentException();
                else if (cell.GetContent() is Formula)
                {
                    obj = cell.GetValue();
                    if (obj is FormulaError) // if the object is a formula error, this will result in a new formula error when you try to evaluate it
                        throw new ArgumentException();
                }
            }
            else
                throw new ArgumentException();
            return (double)obj; // only a number shoud be returned if we will be able to evaluate a valid formula
        }
        /// <summary>
        /// Checks the validity of the cells name
        /// </summary>
        /// <param name="name"></param>
        /// <exception cref="ArgumentException">Throws this exception if the name is invalid</exception>
        private static void SpreadSheetNameChecker(string name)
        {
            char[] characters = name.ToArray();
            if (characters.Length <= 1)
                throw new InvalidNameException();
            if (Char.IsLetter(characters[0]))// checks if the first character is a variable or a number
            {
                for (int i = 1; i < characters.Length; i++)// loops through all of the characters in the string to check for variable validity 
                {
                    if (!Char.IsNumber(characters[i]) && !Char.IsLetter(characters[i]))
                        throw new InvalidNameException();
                }
            }
            else
                throw new InvalidNameException();
        }
    }
}

