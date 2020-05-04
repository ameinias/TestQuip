# TestQuip1
Choose the most specific "quip", based on how many tags match a list of variables, from an XML file. Update the XML file from a form. 

Intended to be used for barks, but could be adapted to pull any tagged info from an XML file.

# Unity Package
[Skip git and download the .unitypackage]()

# Usage
The XML file must be saved in the root Resources folder if you are using the Add Quip form. 

This tool only chooses a random relevant quip from the XML file and displays in in a Text object, based on variables entered in the inspector. 

Displaying the quips in a dialog system and in-game variable management not included. The variables are analyzed by existing in the list at all. There is no mechanic for analyzing a boolean or weighting variables, or assigning quips to specific NPCs.
 
# Known Issues
If there is only one variable to tag-match, it will also choose from all quips that do not have any tags. But it will not choose tagness
 
# Credits 
  Adapted from [the app gurus xml parser](http://www.theappguruz.com/blog/unity-xml-parsing-unity)
 [the app gurus xml parser git](https://github.com/theappguruz/Unity--XML-Parsing-In-Unity--Demo-Project)

 Quip Adaption by gblekkenhorst
[gblekkenhorst on git](https://github.com/ameinias)
[Relevant Quip on git](https://github.com/ameinias/TestQuip)