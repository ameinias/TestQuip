/*
 Adapted from http://www.theappguruz.com/blog/unity-xml-parsing-unity
 Git: https://github.com/theappguruz/Unity--XML-Parsing-In-Unity--Demo-Project

 Quip Adaption by gblekkenhorst
 Git: https://github.com/ameinias

    Choose the most specific "quip", based on how many tags match a 
    list of variables, from an XML file. Update the XML file from 
    a form. 

    The XML file must be saved in the Resources folder. 

 */


using UnityEngine;
using System.Collections;
using System.Xml;
using System.Collections.Generic;
using UnityEngine.UI;

public class QuipperController : MonoBehaviour
{

    // GUI
    public Text allQuipDisplay;
    public Text matchDisplayText;
    public InputField TagText;
    public InputField QuipText;


    // XML Parsing
    public TextAsset quipXml; // XML of quip data. Must be saved in Assets/Resources.
    private string path;
    private XmlDocument xmlDoc;
    private WWW www;

    //Lists
    [Tooltip("To retrieve a quip, all variables listed here must return true. " +
        "The quip may have more variables that return false. ")]
    public List<string> variables;


    public Dictionary<string, bool> variabools = new Dictionary<string, bool>();
    private List<Quip> quipList;
    private  List<Quip> relevantQuips;



    // Hold each piece of dialog
    struct Quip
    {
        public int Id;
        public string name;
        public string tags;
    };





    void Awake()
    {
        quipList = new List<Quip>();
        relevantQuips = new List<Quip>();

    }

    void Start()
    {

        allQuipDisplay.text = "";
        Refresh();



    }

    void Refresh() {

        loadXMLFromAssest();
        readXml();
        CheckForRelevantQuip();
    }

    private void loadXMLFromAssest()
    {
        xmlDoc = new XmlDocument();


        xmlDoc.LoadXml(quipXml.text);
    }
    
    private void readXml()
    {
     
        foreach (XmlElement node in xmlDoc.SelectNodes("Quips/Quip"))
        {
            Quip tempPlayer = new Quip();
            tempPlayer.Id = int.Parse(node.GetAttribute("id"));
            tempPlayer.name = node.SelectSingleNode("name").InnerText;
            tempPlayer.tags = node.SelectSingleNode("tags").InnerText;
            quipList.Add(tempPlayer);
            displayPlayeData(tempPlayer);
        }
    }
    
    string[] SplitTags(string stringToSplit) {

        string[] array = stringToSplit.Split(',');
   

        return array;
    }


    // Hooked up to Button, displays quip in matchDisplayText
    // Quip must match at least one variable
    public void CheckForRelevantQuip()
    {
        // string bestMatch = "";
        Quip bestQuip = new Quip();
        int highestMatchSoFar = 0;
        if (relevantQuips.Count > 0) relevantQuips.Clear();
        int allQuips = 0;
        foreach (Quip quippet in quipList)
        {


            int tempMatch = GetRelevance(quippet.tags, quippet.name);
        
                    if (tempMatch > highestMatchSoFar)
                    {
                        highestMatchSoFar = tempMatch;
                        relevantQuips.Clear();
                relevantQuips.Add(quippet);

            }

                    else if (tempMatch == highestMatchSoFar) { relevantQuips.Add(quippet); }
                
            }
        

            Debug.Log("Found " + relevantQuips.Count + " relevant quips.");
                int rando = Random.Range(0, relevantQuips.Count);
                bestQuip = relevantQuips[rando];
        matchDisplayText.text = bestQuip.name;

        
    }


    // Depreciated, saved for hoarding purposes. 
    public void CheckForRelevantQuipBackUp()
    {

        Quip bestQuip = new Quip();
        int highestMatchSoFar = 0;
        if (relevantQuips.Count > 0) relevantQuips.Clear();
        int allQuips = 0;
        foreach (Quip quippet in quipList)
        {


            int tempMatch = GetRelevance(quippet.tags, quippet.name);


            if (tempMatch >= highestMatchSoFar)
            {
                allQuips++;
                if (tempMatch > highestMatchSoFar)
                {
                    highestMatchSoFar = tempMatch;
                    relevantQuips.Clear();

                }
                else { Debug.Log("lower, no kill " + tempMatch + "<" + highestMatchSoFar + " of " + allQuips); }
                relevantQuips.Add(quippet);


            }
        }

        if (highestMatchSoFar > 1)
        {
            int rando = Random.Range(0, relevantQuips.Count);
            bestQuip = relevantQuips[rando];
            matchDisplayText.text =  bestQuip.name;
        }

        else { matchDisplayText.text = "No matches"; }

    }

    // Coiunt how many tags match the list of variables,
    // so the most specific quip with the highest numer of
    // tags is used. 
    int GetRelevance(string tags, string name)
    {
        string[] tagArray = SplitTags(tags);
        int numMatch = 0;


        foreach (string token in tagArray)
        {
            if (variables.Contains(token))
            {
                numMatch++;
                
            } else {
                  return 0;
                // exits if it hits a tag that makes the 
                // quip irrelevant - ALL the quips tags must be variables,
                // but not all the variables need to be in the quips tags. 
            }
        }

        return numMatch;
    }


    // Displays All Quips in a textbox for testing
    private void displayPlayeData(Quip tempPlayer)
    {
        allQuipDisplay.text += tempPlayer.name + "\t\t (" + tempPlayer.tags + ") \n";

        }



    // Create a new Quip from the Tag and Quip text
    // box, and save it into the XML file.
    public void createElement()
    {

        Debug.Log("create");
        Quip tempPlayer = new Quip();
        tempPlayer.Id = quipList.Count + 1;
        tempPlayer.name = QuipText.text;
        tempPlayer.tags = TagText.text;
        quipList.Add(tempPlayer);
        displayPlayeData(tempPlayer);

        XmlNode parentNode = xmlDoc.SelectSingleNode("Quips");
        XmlElement element = xmlDoc.CreateElement("Quip");
        element.SetAttribute("id", tempPlayer.Id.ToString());
        element.AppendChild(createNodeByName("name", tempPlayer.name));
        element.AppendChild(createNodeByName("tags", tempPlayer.tags));
        parentNode.AppendChild(element);
        xmlDoc.Save(getPath() + ".xml");

        Debug.Log("New Quip " + quipList[quipList.Count-1].Id + " added to qipList" );
        TagText.text = "";
       QuipText.text="";
        Refresh();
    }

    // Used by CreateElement
    private XmlNode createNodeByName(string name, string innerText)
    {
        Debug.Log("createNode");
        XmlNode node = xmlDoc.CreateElement(name);
        node.InnerText = innerText;
        return node;
    }

    // Used to find and save the XML file. 
    private string getPath()
    {
#if UNITY_EDITOR
        return Application.dataPath + "/Resources/" + quipXml.name;
#elif UNITY_ANDROID
            return Application.persistentDataPath+fileName;
#elif UNITY_IPHONE
            return GetiPhoneDocumentsPath()+"/"+fileName;
#else
            return Application.dataPath +"/"+ fileName;
#endif
    }
    private string GetiPhoneDocumentsPath()
    {
        // Strip "/Data" from path
        string path = Application.dataPath.Substring(0, Application.dataPath.Length - 5);
        // Strip application name
        path = path.Substring(0, path.LastIndexOf('/'));
        return path + "/Documents";
    }
}