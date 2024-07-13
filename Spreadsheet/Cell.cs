using SpreadsheetUtilities;

namespace SS
{   /// <summary>
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
    ///   This class represents a single cell in our spreadsheet class.
    /// </summary>
    internal class Cell
    {
        private String name;
        private Object content;
        private Object value;

        /// <summary>
        /// The constructor for our cell
        /// </summary>
        /// <param name="name">the name of the cell</param>
        /// <param name="content">the content of the cell</param>
        /// <param name="value">the value of the content</param>
        public Cell(String name, Object content, Func<string, double> lookup)
        {
            this.name = name;
            this.content = content;
            this.value = content;
            if (content is Formula)
                this.value = new FormulaError("hasnt been set yet");
            ChangeContent(content, lookup); // sets our value up.
        }
        /// <summary>
        /// Gets the content of the object for the user
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public Object GetContent()
        {
            return content;
        }
        /// <summary>
        /// lets the user change the contents of the cell
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public void ChangeContent(Object content, Func<string, double> lookup)
        {
            this.content = content;
            ChangeValue(lookup);
        }
        /// <summary>
        /// This method changes our value to equal the content
        /// </summary>
        public void ChangeValue(Func<string, double> lookup)
        {
            
            // checks what type of object our content is and sets value to that object
            if (content is String)
            {
                value = (string)content;
            }
            if (content is double) value = (double)content;
            if (content is Formula)
            {
                
               Formula f = (Formula)content;
               value = f.Evaluate(lookup);
            }
        }
        /// <summary>
        /// gets the value of an object
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public Object GetValue()
        {
            return value;
        }
    }
}