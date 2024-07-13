using Microsoft.Maui.LifecycleEvents;
using Microsoft.Maui.Storage;
using SS;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using FilePicker = Microsoft.Maui.Storage.FilePicker;
using FilePickerFileType = Microsoft.Maui.Storage.FilePickerFileType;
using PickOptions = Microsoft.Maui.Storage.PickOptions;

namespace GUI;

/// <summary>
///     Author:    Matthew Duffin, Jack Marshall
///     Partner:   Jack Marshall and Matthew Duffin
///     Date:      March 3, 2023
///     Course:    CS 3500, University of Utah, School of Computing
///     Copyright: CS 3500, Matthew Duffin and Jack Marshall - This work may not
///     be copied for use in Academic Coursework.
///     We, Matthew Duffin and Jack Marshall, certify that we wrote this code from scratch and
///     did not copy it in part or whole from another source.  All
///     references used in the completion of the assignments are cited
///     in my README file.
///     File Contents
///     This class handles the connection between the spreadsheet model with a spreadsheet GUI.
///     It handles all the things that the user can use to create spreadsheets.
/// </summary>
public partial class MainPage : ContentPage
{
    private readonly int numRows;
    private readonly int numCols;
    private readonly List<Border> borders;
    private readonly List<Label> labels;
    private readonly Dictionary<Entry, string> addressDictionary;
    private Spreadsheet sheet;
    private Entry myEntry;
    private Color myColor;
    private Color firstColor;
    private bool existingFileOpened; // Existing file was opened
    private string oldTextValue;
    private string existingFilePath; // Existing file's filepath is temporarily stored

    /// <summary>
    ///     MainPage method is responsible for initializing private instance variables
    ///     and Spreadsheet's cells (such the DrawTopAndLeftLabels and DrawCells).
    /// </summary>
    public MainPage()
    {
        // Initializing private instance variables used in helper methods
        numRows = 10;
        numCols = 30;
        myColor = Colors.White;
        firstColor = Color.FromRgb(255, 229, 180);
        addressDictionary = new Dictionary<Entry, string>();
        existingFileOpened = false;
        borders = new List<Border>();
        labels = new List<Label>();

        // creates the actual GUI
        InitializeComponent();
        DrawTopAndLeftLabels();
        DrawCells();
    }

    /// <summary>
    ///     Draws the top and left labels
    /// </summary>
    protected void DrawTopAndLeftLabels()
    {
        // Drawing all Top Labels (A, B, C, etc).
        for (int i = 0; i <= numCols; i++)
        {
            var curLabel = new Label
            {
                Text = $"{RowLabel(i)}",
                TextColor = Color.FromRgb(0, 0, 0),
                BackgroundColor = Color.FromRgb(200, 200, 250),
                HorizontalTextAlignment = TextAlignment.Center
            };
            var curBorder = new Border
            {
                Stroke = Color.FromRgb(0, 0, 0),
                StrokeThickness = 1,
                HeightRequest = 20,
                WidthRequest = 75,
                HorizontalOptions = LayoutOptions.Center,
                Content = curLabel
            };
            // Adding border's and its label to GUI, as well as storing them for later access
            labels.Add(curLabel);
            borders.Add(curBorder);
            TopLabels.Add(curBorder);
        }

        // Drawing all Left-side labels (1, 2, 3, etc).
        CellContent.IsReadOnly = true;
        StoreEntry(0, 0, CellContent);
        for (int i = 1; i <= numRows; i++)
        {
            var curLabel = new Label
            {
                Text = $"{i}",
                TextColor = Color.FromRgb(0, 0, 0),
                BackgroundColor = Color.FromRgb(200, 200, 250),
                HorizontalTextAlignment = TextAlignment.Center
            };
            var curBorder = new Border
            {
                Stroke = Color.FromRgb(0, 0, 0),
                StrokeThickness = 1,
                HeightRequest = 25,
                WidthRequest = 75,
                HorizontalOptions = LayoutOptions.Center,
                Content = curLabel
            };
            // Adding border's and its label to GUI, as well as storing them for later access
            labels.Add(curLabel);
            borders.Add(curBorder);
            LeftLabels.Add(curBorder);
        }
    }

    /// <summary>
    ///     Draws the cells for the spreadsheet by layering HorizontalStackLayouts of Entry objects.
    ///     Each Entry, the entry's address, and the entry's content (i.e. the cell's content) is initialized
    ///     and stored for later manipulation and access.
    /// </summary>
    protected void DrawCells()
    {
        // Drawing each cell from spreadsheet as an "Entry" object
        for (int i = 1; i <= numRows; i++)
        {
            var curCell = new HorizontalStackLayout();
            for (int j = 1; j <= numCols; j++)
            {
                myEntry = new Entry
                {
                    Placeholder = "", // Change to "" for submit, change to cellAddress when testing
                    TextColor = Color.FromRgb(0, 0, 0),
                    BackgroundColor = Color.FromRgb(255, 229, 180),
                    HorizontalTextAlignment = TextAlignment.Center,
                    VerticalTextAlignment = TextAlignment.Start,
                    HeightRequest = 25,
                    WidthRequest = 75,
                    IsReadOnly = true
                };
                StoreEntry(j, i, myEntry);
                var curBorder = new Border
                {
                    Stroke = Color.FromRgb(0, 0, 0),
                    StrokeThickness = 1,
                    HeightRequest = 25,
                    WidthRequest = 75,
                    HorizontalOptions = LayoutOptions.Center,
                    Content = myEntry
                };
                // Storing the cell's Entry for later access
                curCell.Add(curBorder);
                borders.Add(curBorder);
            }

            CellStack.Add(curCell);
        }
    }

    /// <summary>
    ///     The StoreEntry method has 3 purposes:
    ///     1) Storing the Entry in a 2D array to access Entry via its spreadsheet location (column, row)
    ///     2) Storing the Entry in a dictionary to return the cell address (A1, B2, etc) for a given Entry
    ///     3) Adding event handlers to the Entry to update the spreadsheet when the Entry is changed
    /// </summary>
    /// <param name="column"></param>
    /// <param name="row"></param>
    /// <param name="currEntry"></param>
    /// <exception cref="IndexOutOfRangeException"></exception>
    private void StoreEntry(int column, int row, Entry currEntry)
    {
        if (column > numCols || row > numRows)
            throw new IndexOutOfRangeException(
                "StoreEntry: Column/Row is greater than number of rows/columns allowed in spreadsheet.");

        // Storing entries (the cell and its content) in a dictionary with its specific address (such as A1, B3, etc.)
        if (column != 0 && row != 0)
        {
            string cellAddress = $"{RowLabel(column)}{row}";
            if (!addressDictionary.ContainsKey(currEntry))
                addressDictionary.Add(currEntry, cellAddress);
        }

        // Identifying the order of which Entry event handlers execute tasks
        currEntry.Focused += OnEntryFocused;
        currEntry.Unfocused += OnEntryUnFocused;
        currEntry.Completed += OnEntryCompleted; // Here, setContentsOfCell is updated
    }

    /// <summary>
    ///     Mimics the horizontal alphabetical layout (A to Z, then AA to AZ, etc)
    ///     for the topLabels HorizontalStackLayout
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    private string RowLabel(int index)
    {
        double columnIndex = index;
        string columnLabel = "";

        while (columnIndex > 0)
        {
            // Checking that when alphabet out of letters, it creates new labels such as after Z, it is AA, AB, AC, etc.
            double currIndex = (columnIndex - 1) % 26;
            char letter = (char)(currIndex + 65);
            columnLabel = letter + columnLabel;
            columnIndex = (columnIndex - (currIndex + 1)) / 26;
        }

        return columnLabel;
    }

    /// <summary>
    ///     Called when an Entry is clicked, altered, then user pressed "Enter"
    ///     Keeps new text entered by user
    ///     see reference 3
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void OnEntryCompleted(object sender, EventArgs e)
    {
        // Getting entry's address, i.e. the cell's name (such as A6)
        string text = ((Entry)sender).Text;
        string currAddress = "";
        object value = null;
        // Retrieving the cell address of a specific cell
        if (addressDictionary.ContainsKey((Entry)sender))
        {
            addressDictionary.TryGetValue((Entry)sender, out currAddress);

            // If Entry is located on grid, must have an address, therefore...
            // Set the current address as a cell's name, and the text as the value/contents
            try
            {
                sheet.SetContentsOfCell(currAddress, text);
            }
            catch (Exception exception)
            {
                if (exception is FormulaFormatException)
                    await DisplayAlert("Alert", "Invalid Formula, please input a valid Formula", "OK");
                if (exception is InvalidCastException)
                    await DisplayAlert("Alert", "A circular exception was thrown.", "Ok");
            }

            addressDictionary.TryGetValue((Entry)sender, out string name); // Gets the name of the entry
            entrySelected(name,
                sheet.GetCellContents(name).ToString()); // Records the contents at the top of the screen
            value = sheet.GetCellValue(currAddress);
        }
        else // If Entry not found, then set cell contents and catch such exceptions...
        {
            try
            {
                sheet.SetContentsOfCell(CellName.Text, text);
            }
            catch (Exception exception)
            {
                if (exception is FormulaFormatException)
                    await DisplayAlert("Alert", "Invalid Formula, please input a valid Formula", "OK");
                if (exception is InvalidCastException)
                    await DisplayAlert("Alert", "A circular exception was thrown.", "Ok");
            }

            currAddress = CellName.Text;
        }

        // Checks to see if we are entering in the content entry on top of screen, and if so do something different
        if ((Entry)sender == CellContent)
        {
            entrySelected(currAddress, text);
            updateGUI();
            return;
        }

        // Updating the cell's old text, current text, GUI displays, etc.
        value = sheet.GetCellValue(currAddress);
        oldTextValue = value.ToString();
        text = ((Entry)sender).Text = value.ToString();
        updateGUI();
    }

    /// <summary>
    ///     Updates our front end GUI to match the backend Spreadsheet.
    ///     Ensures spreadsheet is showing correct values after each
    ///     time a cell's contents are altered.
    /// </summary>
    private void updateGUI()
    {
        foreach (var entry in addressDictionary.Keys)
        {
            object value = sheet.GetCellValue(addressDictionary[entry]);
            entry.Text = value.ToString();
        }
    }

    /// <summary>
    ///     Called when an Entry is clicked on.
    ///     Functions:
    ///     1) Cell's old text is stored in case cell not saved (user didn't press enter)
    ///     2) Highlighting the cell the user clicked on
    ///     3) Ensuring user is editing an existing spreadsheet (created or opened one)
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void OnEntryFocused(object sender, EventArgs e)
    {
        oldTextValue ??= ""; // If old text value is null, set it to empty string
        oldTextValue = ((Entry)sender).Text;
        myColor = ((Entry)sender).BackgroundColor;

        foreach (var pair in addressDictionary)
            if (pair.Value == "A1" && pair.Key != (Entry)sender)
                pair.Key.BackgroundColor = firstColor;

        //Affects the highlighting of the cells
        if ((Entry)sender != CellContent)
            ((Entry)sender).BackgroundColor = Colors.LightBlue;

        // For every cell, if selected when spreadsheet doesn't exist, an alert displays that tells user to create new spreadsheet.
        if (addressDictionary.ContainsKey((Entry)sender))
        {
            addressDictionary.TryGetValue((Entry)sender, out string name); // gets name of entry

            if (sheet == null)
            {
                sheet = new Spreadsheet(s => true, normalizer, "six");
                await DisplayAlert("Alert", "No SpreadSheet Open, You need to hit the new File option ", "OK");
            }
            else
            {
                entrySelected(name, sheet.GetCellContents(name).ToString());
            }
        }
    }

    /// <summary>
    ///     Called when user clicks off of an Entry without pressing "Enter" first
    ///     Old text value is restored to Entry since its technically "not confirmed"
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnEntryUnFocused(object sender, EventArgs e)
    {
        if (((Entry)sender).Text != oldTextValue) ((Entry)sender).Text = oldTextValue;

        //Reverses the highlighted cell
        if ((Entry)sender != CellContent)
            ((Entry)sender).BackgroundColor = myColor;
    }

    /// <summary>
    ///     Creates a new spreadsheet when the Window's "New" button is clicked
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void FileMenuNewASync(object sender, EventArgs e)
    {
        // If spreadsheet is currently open, check for changes. If changes --> ask user if they want to save it
        if (sheet != null)
            if (sheet.Changed)
            {
                // Displaying a dialog box to ask user if they want to save their current spreadsheet. See reference 5
                bool answer = await DisplayAlert("Save Spreadsheet", "Would you like to save your current spreadsheet?",
                    "Yes", "No");
                if (answer)
                {
                    SaveFile();
                    return;
                }
            }

        //If no spreadsheet open, opened spreadsheet not changed, or user doesn't want to save current spreadsheet, create a new one
        NewFile();
        sheet = new Spreadsheet(s => true, normalizer, "six");
    }

    /// <summary>
    ///     Opens an existing spreadsheet when the Window's "Open" button is clicked
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void FileMenuOpenAsync(object sender, EventArgs e)
    {
        // If spreadsheet is currently open, check for changes. If changes --> ask user if they want to save it
        if (sheet != null)
            if (sheet.Changed)
            {
                // Displaying a dialog box to ask user if they want to save their current spreadsheet. See reference 5
                bool answer = await DisplayAlert("Save Spreadsheet", "Would you like to save your current spreadsheet?",
                    "Yes", "No");

                /**This is repeated code from the SaveFile() method due to
                 * NET MAUI primitive functionality, when we called SaveFile()
                 * it would jump to the NewFile() call and also display the alert at the same time.
                 * When we use the repeated code, it does not do this. This occurs due to the 
                 * method being Async, and using a alerts the require "await", which causes all
                 * code in the method to execute, therefore causing both windows to open at once and awaiting
                 * for user input on both.
                 **/
                if (answer)
                {
                    // When previously opened file is saved, it is saved at the same filepath
                    if (existingFileOpened)
                    {
                        sheet.Save(existingFilePath);
                        return;
                    }

                    string name = "no entry";
                    bool nameIllegal = true;
                    string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

                    //User is repeatedly asked to answer prompt below if user enters nothing or whitespace for the file's name.
                    while (nameIllegal)
                    {
                        name = await DisplayPromptAsync("Saving File...",
                            "What should the file's name be? (Don't include .sprd at end!)");
                        if (name == null)
                            break;
                        if (File.Exists(desktopPath + "\\" + name + ".sprd"))
                        {
                            answer = await DisplayAlert("Save Spreadsheet",
                                "The current File will be overwritten, as it already exists. Is this ok?", "Yes", "No");
                            if (!answer)
                                return;
                        }

                        // See reference 10 for use of Path.GetInvalidPathChars()
                        bool illegalFileNamecharacters = !string.IsNullOrEmpty(name) &&
                                                         name.IndexOfAny(Path.GetInvalidPathChars()) >= 0;

                        if (!string.IsNullOrWhiteSpace(name) && !illegalFileNamecharacters)
                            nameIllegal = false; // If user enters something, name is no longer illegal
                    }

                    if (name != null)
                    {
                        // UnauthorizedAccessException when attempting to save file, so temporarily defining file path to save to desktop directory.
                        // See reference 9, which is a current Piazza post explaining this.
                        existingFilePath = desktopPath;
                        sheet.Save(desktopPath + "\\" + name + ".sprd");
                    }
                }
            }


        //This should be the FindFileMethod, but for some reason it takes to long to open the file explorer when I do this and causes an exception.
        NewFile();
        var result = await FilePicker.PickAsync(new PickOptions
        {
            PickerTitle = "Pick xml file please Please"
        });

        if (result == null)
            return;

        existingFilePath = result.FullPath;
        existingFileOpened = true;


        // ***** Logic for retrieving a previously saved spreadsheet, loading it into "sheet", making sure it's valid, and then displaying it ****

        try
        {
            sheet = new Spreadsheet(existingFilePath, s => true, normalizer,
                "six"); // generate a file based on the file path thats given in find file
        }
        catch (Exception)
        {
            await DisplayAlert("Alert", "Error Opening File", "OK");
        }

        updateGUI(); // this should then update the gui based on the new spreadsheet.
        CellContent.IsReadOnly = false;
        foreach (var pair in addressDictionary) pair.Key.IsReadOnly = false;
    }

    /// <summary>
    ///     Saves the spreadsheet when the Window's "Save" button is clicked
    ///     For when save button clicked, save/overwrite file.
    ///     Also, something where file is saved with different name? (i.e. sheet.sprd, sheet1.sprd, etc).
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void FileMenuSaveASync(object sender, EventArgs e)
    {
        // If a sheet hasn't been created or opened yet, do nothing
        if (sheet != null)
        {
            if (existingFilePath != null)
            {
                // If saving an existing, modified spreadsheet
                if (existingFileOpened && sheet.Changed)
                {
                    // Spreadsheet is overwritten at existing filepath

                    await DisplayAlert("Alert", "file saved ", "OK");
                    SaveFile();

                    existingFileOpened = false;
                    existingFilePath = null;
                }
                else if (sheet.Changed)
                {
                    // Spreadsheet changed when no previous spreadsheet opened
                    await DisplayAlert("Alert", "file saved ", "OK");
                    SaveFile();

                    existingFileOpened = false;
                    existingFilePath = null;
                }
                else if (existingFileOpened && !sheet.Changed)
                {
                    // Existing spreadsheet is unmodified --> do nothing
                    await DisplayAlert("Alert", "File not saved because there was no edits ", "OK");
                }
                else
                {
                    // Spreadsheet doesn't exist, is unmodified, or left blank --> SaveFile()
                    await DisplayAlert("Alert", "file saved ", "OK");
                    SaveFile();
                }
            }
            else
            {
                SaveFile();
            }
        }
    }

    /// <summary>
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void ThemeASync(object sender, EventArgs e)
    {
        // Color settings on widgets for White theme
        if (((MenuFlyoutItem)sender).Text == "White (Default)")
        {
            CellStack.BackgroundColor = Colors.LightYellow;
            TheGrid.BackgroundColor = Colors.LightGrey;
            CellNameLabel.TextColor = Colors.Black;
            CellName.TextColor = Colors.Black;
            CellContentLabel.TextColor = Colors.Black;
            CellContent.TextColor = Colors.Black;
            CellValue.TextColor = Colors.Black;
            CellValueLabel.TextColor = Colors.Black;
            foreach (var pair in addressDictionary)
            {
                if (pair.Value == "A1") firstColor = Color.FromRgb(255, 229, 180);
                pair.Key.BackgroundColor = Color.FromRgb(255, 229, 180);
                pair.Key.TextColor = Colors.Black;
            }

            foreach (var currLabel in labels)
            {
                currLabel.BackgroundColor = Color.FromRgb(200, 200, 250);
                currLabel.TextColor = Colors.Black;
            }

            foreach (var currBorder in borders)
            {
                currBorder.Stroke = Colors.Black;
                currBorder.BackgroundColor = Colors.LightYellow;
            }
        }

        // Color settings on widgets for Dark theme
        if (((MenuFlyoutItem)sender).Text == "Dark")
        {
            CellStack.BackgroundColor = Color.FromRgb(27, 27, 27);
            TheGrid.BackgroundColor = Color.FromRgb(20, 20, 20);
            CellNameLabel.TextColor = Colors.White;
            CellName.TextColor = Colors.White;
            CellContentLabel.TextColor = Colors.White;
            CellContent.TextColor = Colors.White;
            CellValue.TextColor = Colors.White;
            CellValueLabel.TextColor = Colors.White;

            foreach (var pair in addressDictionary)
            {
                if (pair.Value == "A1") firstColor = Color.FromRgb(27, 27, 27);
                pair.Key.BackgroundColor = Color.FromRgb(27, 27, 27);
                pair.Key.TextColor = Colors.White;
            }

            foreach (var currLabel in labels)
            {
                currLabel.BackgroundColor = Color.FromRgb(27, 27, 27);
                currLabel.TextColor = Colors.White;
            }

            foreach (var currBorder in borders)
            {
                currBorder.Stroke = Color.FromRgb(27, 27, 27);
                currBorder.BackgroundColor = Color.FromRgb(27, 27, 27);
            }
        }

        // Color settings on widgets for RaInBoW theme
        if (((MenuFlyoutItem)sender).Text == "RaInBoW")
        {
            var r = new Random();

            CellStack.BackgroundColor = Color.FromRgb(r.Next(0, 255), r.Next(0, 255), r.Next(0, 255));
            TheGrid.BackgroundColor = Color.FromRgb(r.Next(0, 255), r.Next(0, 255), r.Next(0, 255));
            CellNameLabel.TextColor = Color.FromRgb(r.Next(0, 255), r.Next(0, 255), r.Next(0, 255));
            CellName.TextColor = Color.FromRgb(r.Next(0, 255), r.Next(0, 255), r.Next(0, 255));
            CellContentLabel.TextColor = Color.FromRgb(r.Next(0, 255), r.Next(0, 255), r.Next(0, 255));
            CellContent.TextColor = Color.FromRgb(r.Next(0, 255), r.Next(0, 255), r.Next(0, 255));
            CellValue.TextColor = Color.FromRgb(r.Next(0, 255), r.Next(0, 255), r.Next(0, 255));
            CellValueLabel.TextColor = Color.FromRgb(r.Next(0, 255), r.Next(0, 255), r.Next(0, 255));

            // Randomizing color for all widgets. For each row of cells, a random color chosen
            int randomRed = r.Next(0, 255);
            int randomGreen = r.Next(0, 255);
            int randomBlue = r.Next(0, 255);
            int i = 0;
            foreach (var pair in addressDictionary)
            {
                if (i % numCols == 0) // Every 5, Change color
                {
                    randomRed = r.Next(0, 255);
                    randomGreen = r.Next(0, 255);
                    randomBlue = r.Next(0, 255);
                }

                if (pair.Value == "A1")
                    firstColor = pair.Key.BackgroundColor = Color.FromRgb(randomRed, randomGreen, randomBlue);
                pair.Key.TextColor = Colors.White;
                pair.Key.BackgroundColor = Color.FromRgb(randomRed, randomGreen, randomBlue);
                i++;
            }

            foreach (var currLabel in labels)
            {
                if (i % numRows == 0) // Every 5, Change color
                {
                    randomRed = r.Next(0, 255);
                    randomGreen = r.Next(0, 255);
                    randomBlue = r.Next(0, 255);
                }

                currLabel.BackgroundColor = Color.FromRgb(27, 27, 27);
                currLabel.TextColor = Colors.White;

                i++;
            }

            foreach (var currBorder in borders)
            {
                if (i % numRows == 0) // Every 5, Change color
                {
                    randomRed = r.Next(0, 255);
                    randomGreen = r.Next(0, 255);
                    randomBlue = r.Next(0, 255);
                }

                currBorder.Stroke = Color.FromRgb(27, 27, 27);
                currBorder.BackgroundColor = Color.FromRgb(27, 27, 27);

                i++;
            }
        }
    }

    /// <summary>
    ///     This method saves the file for an existing spreadsheet.
    /// </summary>
    private async void SaveFile()
    {
        // When previously opened file is saved, it is saved at the same filepath
        if (existingFileOpened)
        {
            sheet.Save(existingFilePath);
            return;
        }

        string name = "no entry";
        bool nameIllegal = true;
        string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

        // User is repeatedly asked to answer prompt below if user enters nothing or whitespace for the file's name.
        while (nameIllegal)
        {
            name = await DisplayPromptAsync("Saving File...",
                "What should the file's name be? (Don't include .sprd at end!)");
            if (name == null)
                break;
            if (File.Exists(desktopPath + "\\" + name + ".sprd"))
            {
                bool answer = await DisplayAlert("Save Spreadsheet",
                    "The current File will be overwritten, as it already exists. Is this ok?", "Yes", "No");
                if (!answer) return;
            }

            // See reference 10 for use of Path.GetInvalidPathChars()
            bool illegalFileNamecharacters = !string.IsNullOrEmpty(name) &&
                                             name.IndexOfAny(Path.GetInvalidPathChars()) >= 0;

            // If user enters something, name is no longer illegal
            if (!string.IsNullOrWhiteSpace(name) && !illegalFileNamecharacters)
                nameIllegal = false;
        }

        if (name != null)
        {
            // UnauthorizedAccessException when attempting to save file, so temporarily defining file path to save to desktop directory.
            // See reference 9, which is a current Piazza post explaining this.
            existingFilePath = desktopPath;
            sheet.Save(desktopPath + "\\" + name + ".sprd");
        }
    }

    /// <summary>
    ///     When File needs to be cleared, spreadsheet is emptied and all cells and their dependencies are cleared.
    /// </summary>
    private void NewFile()
    {
        // Resets all values
        existingFileOpened = false;
        existingFilePath = null;
        CellName.Text = "A1";
        CellContent.Text = "";
        CellValue.Text = "";


        oldTextValue = "";
        foreach (var pair in addressDictionary)
        {
            pair.Key.Text = ""; // Clearing text from spreadsheet

            if (pair.Value == "A1")
            {
                // New spreadsheet --> First cell highlighted
                firstColor = pair.Key.BackgroundColor;
                pair.Key.BackgroundColor = Colors.LightBlue;
            }
        }


        // Allowing user to edit cells when a new spreadsheet is created
        foreach (var pair in addressDictionary) pair.Key.IsReadOnly = false;
        CellContent.IsReadOnly = false;
    }

    /// <summary>
    ///     This method displays the name of the cell selected, its contents(editable) and its value at the top of the sheet
    /// </summary>
    private void entrySelected(string name, string content)
    {
        if (name == null)
            CellName.Text = "";
        if (content == null)
            CellContent.Text = "";
        CellName.Text = name;
        CellContent.Text = content;
        CellValue.Text =
            sheet.GetCellValue(name).ToString(); // Updating Cell Value label to display a selected cell's value
    }

    /// <summary>
    ///     This method handles when you select the help button in the menu. It will write the message below as a pop up to the
    ///     user.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void FileHelpNewAsync(object sender, EventArgs e)
    {
        string message = "TO START\n \t" +
                         "To start you have either open or select new in the file menu.\n\n" +
                         "TO SET CONTENTS OF CELLS\n\t" +
                         "To set contents of cells click in either the content pain up at the top of \n\t" +
                         "the grid and enter a value, or click directly into a cell.\n\n" +
                         "SAVING INFO\n\t" +
                         "When you select save as, the file you create will be auto saved to your " +
                         "\n\tdesktop. This was done to prevent an error where MAUI tries to auto\n\t " +
                         "save into the windows 32 file directory.\n\n" +
                         "SPECIAL FEATURE\n\t" +
                         "Added Themes, this includes Light Mode, Dark Mode, and RaInBoW\n\t" +
                         "Mode! RaInBoW theme includes FIDGET MODE, where if the user \n\t" +
                         "changes the theme to RaInBoW Mode before creating a new\n\t" +
                         "spreadsheet! The rainbow color appears to fill up half \n\t" +
                         "of the cell, and the user can swipe their mouse over \n\t" +
                         "all the cells to fill in all of its color!\n\t" +
                         "Other RaInBoW Mode features: Every time you select this mode,\n\t" +
                         "random, new colors will take over your screen!";
        await DisplayAlert("Wiki", message, "OK");
    }

    /// <summary>
    ///     Our example normalizer that will convert the first character to uppercase values.
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    private string normalizer(string s)
    {
        if (!string.IsNullOrEmpty(s) && char.IsLetter(s[0]))
            s = s.ToUpper();
        return s;
    }
}