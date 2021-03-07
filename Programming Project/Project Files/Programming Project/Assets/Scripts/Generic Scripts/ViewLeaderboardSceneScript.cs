// Defines modules used
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
public class ViewLeaderboardSceneScript : MonoBehaviour {
    // Creates script references
    GeneralFunctions generalFunctions;
    GeneralVariables generalVariables;
    static ViewLeaderboardSceneScript instance;

    public Dropdown[] dropdownObjects; // Creates list of dropdown objects
    GameObject contentObject; // Creates content object
    int numberOfRows = 0; // Defines number of rows in content object
    List<GameObject> tableBlocks = new List<GameObject>(); // List of table cells

    void Start() {
        // Assign script references
        instance = GameObject.Find("Scene Script Game Object").GetComponent<ViewLeaderboardSceneScript>();
        generalFunctions = GameObject.Find("General Script Game Object").GetComponent<GeneralFunctions>();
        generalVariables = GameObject.Find("General Script Game Object").GetComponent<GeneralVariables>();
        generalVariables.canvas = GameObject.Find("Canvas"); // Sets canvas equal to the gameobject named "Canvas"
        ShowUI(); // Calls ShowUI
    }
    void ShowUI() {
        generalFunctions.ShowImageButton(new Vector2(-475, 275), new Vector2(100, 100), generalVariables.backButtonImage,
            Color.white, 1); // Creates back button
        generalFunctions.ShowText(new Vector2(0, 290),new Vector2(600, 100),"View Leaderboards",75,TextAnchor.MiddleCenter,
            generalVariables.colors["lightRed"], false); // Creates title text
        // Creates dropdown headings
        generalFunctions.ShowText(new Vector2(-250, 215), new Vector2(350, 75), "Class Name", 50, TextAnchor.MiddleCenter,
            generalVariables.colors["lightRed"], false);
        generalFunctions.ShowText(new Vector2(250, 215), new Vector2(350, 75), "Game Name", 50, TextAnchor.MiddleCenter,
            generalVariables.colors["lightRed"], false);
        // Crates leaderboard headings
        generalFunctions.ShowText(new Vector2(-250, 60), new Vector2(350, 75), "Name", 50, TextAnchor.MiddleCenter,
            generalVariables.colors["darkRed"], false);
        generalFunctions.ShowText(new Vector2(250, 60), new Vector2(350, 75), "Score", 50, TextAnchor.MiddleCenter,
            generalVariables.colors["darkRed"], false);

        // Adds dropdown options to each dropdown object
        dropdownObjects[0].AddOptions(generalVariables.classNames);
        dropdownObjects[1].AddOptions(generalVariables.gameNames);

        contentObject = generalFunctions.ShowScrollRect(new Vector2(0, -125), new Vector2(1000, 320), Color.white,
            new Vector2(0, 0), new Vector2(980, 310))[1]; // Creates scroll rect for leaderboard
    }
    public void OnDropdownValueChange() {
        foreach (GameObject tableBlock in tableBlocks) { // Iterates through table cells
            Destroy(tableBlock); // Deletes cell
        }
        tableBlocks.Clear(); // Clears list
        numberOfRows = 0; // Resets number of rows to 0

        if (dropdownObjects[0].value == 0) { // If no class has been selected
            return; // End function
        }
        string chosenClass = generalVariables.classIDs[dropdownObjects[0].value - 1]; // Retrieves chosen class
        string chosenGame = generalVariables.gameIDs[dropdownObjects[1].value]; // Retrieves chosen game
        StartCoroutine(GetInfoFromDatabase(chosenClass, chosenGame)); // Calls GetInfoFromDatabase with relevant parameters
    }
    IEnumerator GetInfoFromDatabase(string classID, string gameID) {
        string url = "alevelproject2019.000webhostapp.com/viewleaderboardscript.php?" +
                "getscores=true&" +
                "classid=" + classID +
                "&gameid=" + gameID; // Defines url to request

        UnityWebRequest webpage = UnityWebRequest.Get(url);
        yield return webpage.SendWebRequest(); // Makes request to url
        string downloadData = webpage.downloadHandler.text; // Stores returned text
        if (downloadData == "") { // If returned text is empty
            AddRow(new string[] { "No Entries", "No Entries" }); // Show message saying no rows
            yield break; // End function
        }
        string[] splitData = downloadData.Split('|'); // Split returned string by student
        for (int i = 0; i < splitData.Length - 1; i++) { // Iterate through data
            AddRow(splitData[i].Split(',')); // Add row with data
        }
    }
    void AddRow(string[] scoreData) {
        if (numberOfRows >= 5) { // If number of rows is greater than five
            contentObject.GetComponent<RectTransform>().sizeDelta += new Vector2(0, 60); // Increase size of scroll rect
        }
        float contentHeight = contentObject.GetComponent<RectTransform>().sizeDelta.y;

        for (int x = 0; x <= 1; x++) { // Iterates between 0 and 1
            GameObject buttonObject = generalFunctions.ShowButton(contentObject,
                scoreData[x], new Vector2(500 * x - 250, -10 - contentHeight / 2 - 60 * numberOfRows), new Vector2(450, 50),
                instance.generalVariables.colors["white"], instance.generalVariables.colors["darkGreen"], 35, true, 0,
                instance.generalVariables.colors["white"], 0); // Creates button object
            RectTransform rectTransform = buttonObject.GetComponent<RectTransform>();
            generalFunctions.SetAnchors(rectTransform, new Vector2(0.5f, 1), new Vector2(0.5f, 1), new Vector2(0.5f, 1));
            tableBlocks.Add(buttonObject); // Sets anchors of new object
        }
        numberOfRows++; // Increments number of rows
    }
}