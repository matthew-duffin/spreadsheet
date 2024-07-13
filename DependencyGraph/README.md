```
Author:     Matthew Duffin
Partner:    None
Course:     CS 3500, University of Utah, School of Computing
GitHub ID:  matthew-duffin
Repo:       https://github.com/uofu-cs3500-spring23/spreadsheet-matthew-duffin
Date:       26-January-2023 6:36pm (when submission was completed) 
Project:    Dependency Graph
Copyright:  CS 3500 and Matthew Duffin - This work may not be copied for use in Academic Coursework.
```

# Comments to Evaluators:
I found this project to be much easier than the previous assignment. No issues to report. I found the hardest part to be finding away to get almost all methods 
to run in constant time. The way I solved this problem is by using the correct data structures, a hash set and a dictionary. This allowed me to get constant time on	
all operations

# Assignment Specific Topics
The two data structures I decided to use are two dictionaries, one for dependents and another for dependees, which then have hash sets as there values.	
This allowed for constant time in all methods except replace. This is because replace has to go through and change every item in the list. I eventually learned about hash sets	
that have constant removal and contains times. This allowed me to solve that problem. 

# Consulted Peers:
None


# References:
1. HashSet<T> Class - https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.hashset-1?view=net-7.0
  