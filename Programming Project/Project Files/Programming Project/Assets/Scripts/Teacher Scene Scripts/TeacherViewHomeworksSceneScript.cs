// Define used modules
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
public class TeacherViewHomeworksSceneScript : MonoBehaviour {

    // Creates script references
    static TeacherViewHomeworksSceneScript instance;
    GeneralFunctions generalFunctions;
    GeneralVariables generalVariables;

    // Scroll Rect Variables
    GameObject contentObject;
    int numberOfRows = 0;
    List<GameObject> homeworkResultsList = new List<GameObject>();

    // Dropdown Variables
    public Dropdown dropdown;
    List<string> homeworkIDs = new List<string>();
    List<string> homeworkNames = new List<string>();

    string[] columnTexts = new string[]{ // Array of texts
        "Name",
        "Time Spent (mins)",
        "Homework Completed?"
    };

    void Start() {
        // Assigns script references
        instance = GameObject.Find("Scene Script Game Object").GetComponent<TeacherViewHomeworksSceneScript>();
        generalFunctions = GameObject.Find("General Script Game Object").GetComponent<GeneralFunctions>();
        generalVariables = GameObject.Find("General Script Game Object").GetComponent<GeneralVariables>();
        generalVariables.canvas = GameObject.Find("Canvas"); // Sets canvas equal to the gameobject named "Canvas"
        ShowUI(); // Calls ShowUI
        StartCoroutine("GethomeworkResultsList"); // Calls GethomeworkResultsList
        dropdown.AddOptions(new List<string>() { "" }); // Adds empty option to dropdown
    }

    void ShowUI() {
        generalFunctions.ShowImageButton(new Vector2(-475, 275), new Vector2(100, 100), generalVariables.backButtonImage,
            Color.white, 1); // Shows back button
        generalFunctions.ShowText(new Vector2(0, 275), new Vector2(600, 100), "View Homeworks", 75, TextAnchor.MiddleCenter,
            generalVariables.colors["lightRed"], true); // Shows title text
        contentObject = generalFunctions.ShowScrollRect(new Vector2(0, -125), new Vector2(900, 400), Color.white,
            new Vector2(0, 0), new Vector2(880, 380))[1]; // Creates scroll rect

        for (int i = 0; i < columnTexts.Length; i++) { // Iterates through indices in columnTexts array
            generalFunctions.ShowText(new Vector2(275 * i - 275, 100), new Vector2(275, 80), columnTexts[i], 30,
                TextAnchor.MiddleCenter, generalVariables.colors["darkRed"], true); // Creates text object
        }
    }
    IEnumerator GethomeworkResultsList() {
        string url = "http://alevelproject2019.000webhostapp.com/viewhomeworks.php?gethw=true" +
            "&accounttype=teacher" +
            "&username=" + generalVariables.username; // Defines URL to query

        UnityWebRequest webpage = UnityWebRequest.Get(url);
        yield return webpage.SendWebRequest(); // Sends web request

        string[] homeworks = webpage.downloadHandler.text.Split('|'); // Stores result, split by homework
        for (int i = 0; i < homeworks.Length - 1; i++) { // Iterates through results
            string[] data = homeworks[i].Split(','); // Splits results into [ID, Name]
            homeworkIDs.Add(data[0]);
            homeworkNames.Add(data[1]);
        }
        dropdown.AddOptions(homeworkNames); // Adds list of homework names as dropdown options
    }
    public void OnDropdownValueChange() {
        foreach (GameObject individualHomework in homeworkResultsList) { // Iterates through list of homework results
            Destroy(individualHomework); // Remove homework result
        }
        homeworkResultsList.Clear(); // Clear list of homework results
        numberOfRows = 0; // Resets number of rows in scroll rect to 0
        if (dropdown.value == 0) { // Ends function is dropdown is set to empty
            return;
        }
        StartCoroutine(RetrieveHomeworkDetails()); // Calls ShowHomeworkDetails
    }
    IEnumerator RetrieveHomeworkDetails() {
        string homeworkID = homeworkIDs[dropdown.value - 1]; // Retrieves ID of homework
        string url = "http://alevelproject2019.000webhostapp.com/viewhomeworks.php?gethwdetails=true" +
            "&homeworkid=" + homeworkID; // Defines URL to access
        UnityWebRequest webpage = UnityWebRequest.Get(url);
        yield return webpage.SendWebRequest(); // Sends web request

        string[] dataDetails = webpage.downloadHandler.text.Split('|'); // Stores result, split by student
        int homeworkLength = int.Parse(dataDetails[0]); // Stores length of homework set
        for (int i = 1; i < dataDetails.Length - 1; i++) { // Iterates through student homework results
            List<string> homeworkDetails = new List<string>(dataDetails[i].Split(',')); // Stores details of homework
            homeworkDetails[1] = Mathf.FloorToInt(float.Parse(homeworkDetails[1]) / 60).ToString();
            // ^ Gets length of time spent doing homework in minutes
            if (int.Parse(homeworkDetails[1]) >= homeworkLength) {
                homeworkDetails.Add("Yes"); // Homework completed
            }
            else {
                homeworkDetails.Add("No"); // Homework not completed
            }
            AddHomework(homeworkDetails); // Calls AddHomework
        }
    }
    void AddHomework(List<string> homeworkDetails) {
        // Adjusts size of scroll rect depending on number of homework results
        if (numberOfRows == 6) {
            contentObject.GetComponent<RectTransform>().sizeDelta += new Vector2(0, 40);
        }
        else if (numberOfRows > 6) {
            contentObject.GetComponent<RectTransform>().sizeDelta += new Vector2(0, 60);
        }
        float contentHeight = contentObject.GetComponent<RectTransform>().sizeDelta.y;

        for (int x = 0; x < homeworkDetails.Count; x++) { // Iterates through homework details
            GameObject buttonObject = generalFunctions.ShowButton(contentObject,
                homeworkDetails[x], new Vector2(275 * x - 275, -10 - contentHeight / 2 - 60 * numberOfRows),
                new Vector2(250,50),instance.generalVariables.colors["white"],instance.generalVariables.colors["darkGreen"],
                35, true, 0, instance.generalVariables.colors["white"], 0); // Creates button showing result
            buttonObject.name = numberOfRows.ToString();
            RectTransform rectTransform = buttonObject.GetComponent<RectTransform>();
            generalFunctions.SetAnchors(rectTransform, new Vector2(0.5f, 1), new Vector2(0.5f, 1), new Vector2(0.5f, 1));
            homeworkResultsList.Add(buttonObject); // Adds button object to list of homework results
        }
        numberOfRows++; // Increments number of rows variable by one
    }
}