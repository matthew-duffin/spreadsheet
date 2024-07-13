Author:     Matthew Duffin, Jack Marshall
Partner:    Matthew Duffin and Jack Marshall
Date:       23-Feb-2023
Course:     CS 3500, University of Utah, School of Computing
GitHub ID:  matthew-duffin
Repo:       https://github.com/uofu-cs3500-spring23/assignment-six---gui---functioning-spreadsheet-cell-low-buy-high
Date:       3-Mar-2023(of when submission is ready to be evaluated)
Solution:   GUI
Copyright:  CS 3500, Jack Marshall, and Matthew Duffin - This work may not be copied for use in Academic Coursework.
```

# Overview of the Spreadsheet functionality

The Spreadsheet program is currently capable of performing mathematical operations. This program now also supports dependency graphs. This allows for determining
which cells need to be completed in what orderings. This program also supports formulas being created as objects. The program now supports a spreadsheet
class the keeps track of individual cells and allows for manipulation of those cells. This spreadsheet now 
supports all basic functions of our model. This includes creating and evaluating cells and there contents,
saving the spreadsheet to an xml file, and getting this saved spreadsheet and creating a new spreadsheet with it. 

The program also has GUI files that add a view and controller that allow users to interact with the spreadsheet model. The SpreadSheet project is now
complete. 
# Examples of good software practice
    1. DRY - I implemented the method Cell setter as its code was used in all my setCell methods.
    2. Separation of Concerns - I initially coded a getValue method into my Cell class, however, I soon ran into trouble and had to delete it 
                                as it was not the concern of the current assignment. 
    3. Strong comments - I feel that my comments are strong and that they accurately describe my code without being overused. 
    4. Branch Merging - Although it was a bit anxiety inducing trying to merge branches initially, I was able to get it figured
                        out and understand its importance. It is a tool that can allow me to make changes to my code while knowing
                        I have good code to fall back on should the changes go wrong. 
    5. Open Close principle - Jack and I did a good job of not editing already working code and instead using helper methods that add onto other code
                               that allow us to interact with all items. 
    6. SOLID - Jack and I used SOLID principles that allowed us to keep our code maintainable and functional. 

# Time Expenditures:

    1. Assignment One:   Predicted Hours:          10     Actual Hours:   14
    2. Assignment Two:   Predicted Hours:          10     Actual Hours:   12
    3. Assignment Three: Predicted Hours:          12     Actual Hours:   11 
    3. Assignment Four:  Predicted Hours:          10     Actual Hours:   8
           - Coding solution 6 hours 
           - Debugging 2 hours
    4. Assignment Five: Predicted Hours:           10     Actual Hours: 16 (assignment was hard this week)
           - Attempting to understand and plan code: 3 hours
           - Coding solution: 7 hours
           - Debugging solution: 6 hours
    5. Assignment Six:   Predicted Hours:          13     Actual Hours: Close to 20
           - Attempting to plan and Understand code: around 5 hours
           - Coding Solution: 11 hours
           - Debugging: 4 hours 
           
           