// Defines used modules
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class StudentViewHomeworksSceneScript : MonoBehaviour {
    // Create script reference variables
    GeneralFunctions generalFunctions;
    GeneralVariables generalVariables;
    static StudentViewHomeworksSceneScript instance;

    string[] tableHeaders = new string[] { // Array of table header strings
        "Name",
        "Class",
        "Game",
        "Length",
        "Due Date"
    };
    // Scroll rect variables
    GameObject contentObject;
    List<GameObject> homeworkList = new List<GameObject>();
    int numberOfRows = 0;

    void Start() {
        // Assign script reference variables
        generalFunctions = GameObject.Find("General Script Game Object").GetComponent<GeneralFunctions>();
        generalVariables = GameObject.Find("General Script Game Object").GetComponent<GeneralVariables>();
        instance = GameObject.Find("Scene Script Game Object").GetComponent<StudentViewHomeworksSceneScript>();
        generalVariables.canvas = GameObject.Find("Canvas"); // Sets canvas equal to the gameobject named "Canvas"
        ShowUI(); // Calls ShowUI
        StartCoroutine(GetHomeworks()); // Calls GetHomework
    }

    void ShowUI() {
        generalFunctions.ShowImageButton(new Vector2(-475, 275), new Vector2(100, 100), generalVariables.backButtonImage,
            Color.white, 1); // Shows back button image
        generalFunctions.ShowText(new Vector2(0, 250), new Vector2(600, 100), "View Homeworks", 80, TextAnchor.MiddleCenter,
            generalVariables.colors["lightRed"], true); // Shows title text

        for (int i = 0; i < tableHeaders.Length; i++) { // Iterates through table headers
            generalFunctions.ShowText(new Vector2(200 * i - 400, 175), new Vector2(180, 60), tableHeaders[i], 40,
                TextAnchor.MiddleCenter, generalVariables.colors["darkRed"], true); // Shows table header text
        }
        contentObject = generalFunctions.ShowScrollRect(new Vector2(0, -50), new Vector2(1000, 400), Color.white,
            new Vector2(0, 0), new Vector2(980, 380))[1]; // Shows scroll rect where the results are placed
    }
    IEnumerator GetHomeworks() {
        string url = "alevelproject2019.000webhostapp.com/viewhomeworks.php?gethw=true" +
            "&username=" + generalVariables.username +
            "&accounttype=student"; // Defines URL to make request to

        UnityWebRequest webpage = UnityWebRequest.Get(url);
        yield return webpage.SendWebRequest(); // Sends web request to URL and waits for response
        string[] homeworks = webpage.downloadHandler.text.Split('|'); // Stores returned text, split by homework
        for (int i = 0; i < homeworks.Length - 1; i++) { // Iterates through homeworks
            string[] data = homeworks[i].Split(','); // Splits homework into each piece of information
            data[1] = generalVariables.classNames[generalVariables.classIDs.IndexOf(data[1])];
            // ^ Redefines class ID as class name
            data[2] = generalVariables.gameNames[generalVariables.gameIDs.IndexOf(data[2])];
            // ^ Redefines game ID as game name
            AddHomework(data); // Calls AddHomework, sending the data retrieved as an argument
        }
    }
    void AddHomework(string[] scoreData) {
        // Resizes scroll rect based on number of rows in table
        if (numberOfRows == 6) {
            contentObject.GetComponent<RectTransform>().sizeDelta += new Vector2(0, 40);
        }
        else if (numberOfRows > 6) {
            contentObject.GetComponent<RectTransform>().sizeDelta += new Vector2(0, 60);
        }
        float contentHeight = contentObject.GetComponent<RectTransform>().sizeDelta.y;

        for (int x = 0; x < scoreData.Length; x++) { // Iterates through columns of table
            GameObject buttonObject = generalFunctions.ShowButton(contentObject,
                scoreData[x], new Vector2(200 * x - 400, -10 - contentHeight / 2 - 60 * numberOfRows), new Vector2(180, 50),
                instance.generalVariables.colors["white"], instance.generalVariables.colors["darkGreen"], 35, true, 0,
                instance.generalVariables.colors["white"], 0); // Creates new button to show data
            buttonObject.name = numberOfRows.ToString();
            RectTransform rectTransform = buttonObject.GetComponent<RectTransform>();
            generalFunctions.SetAnchors(rectTransform, new Vector2(0.5f, 1), new Vector2(0.5f, 1), new Vector2(0.5f, 1));
            homeworkList.Add(buttonObject); // Adds newly-created object to list of homework cells
        }
        numberOfRows++; // Increments number of rows as a new row has just been created
    }
}