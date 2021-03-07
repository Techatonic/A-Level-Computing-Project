// Defines used modules
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
public class TeacherSetHomeworkSceneScript : MonoBehaviour {
    // Creates script references
    GeneralFunctions generalFunctions;
    GeneralVariables generalVariables;
    static TeacherSetHomeworkSceneScript instance;

    public Dropdown[] dropdowns; // Array of dropdown objects
    List<GameObject> inputFields = new List<GameObject>(); // List of input field objects
    string[] fieldNames = new string[]{ // Array of field names
        "Class Name",
        "Homework Name",
        "Game",
        "Homework Length",
        "Due Date"
    };

    void Start() {
        // Assigns script references
        generalFunctions = GameObject.Find("General Script Game Object").GetComponent<GeneralFunctions>();
        generalVariables = GameObject.Find("General Script Game Object").GetComponent<GeneralVariables>();
        generalVariables.canvas = GameObject.Find("Canvas"); // Sets canvas equal to the gameobject named "Canvas"
        instance = GameObject.Find("Scene Script Game Object").GetComponent<TeacherSetHomeworkSceneScript>();
        ShowUI(); // Calls ShowUI
    }

    void ShowUI() {
        generalFunctions.ShowImageButton(new Vector2(-475, 275), new Vector2(100, 100), generalVariables.backButtonImage,
            Color.white, 1); // Shows back button
        generalFunctions.ShowText(new Vector2(0, 250), new Vector2(600, 100), "Add Homework", 75, TextAnchor.MiddleCenter,
            generalVariables.colors["lightRed"], true); // Shows title text

        for (int i = 0; i < fieldNames.Length; i++) { // Iterates through number of fields
            generalFunctions.ShowText(new Vector2(-250, -75 * i + 150), new Vector2(300, 80), fieldNames[i], 50,
                TextAnchor.MiddleLeft, generalVariables.colors["lightRed"], true); // Creates field header
        }
        // Creates input fields
        inputFields.Add(generalFunctions.ShowInputField(generalVariables.colors["darkRed"], new Vector2(150, 75),
            new Vector2(400, 60), "Enter homework name", true, 40, generalVariables.colors["lightGreen"],
            InputField.ContentType.Standard, 40));
        inputFields.Add(generalFunctions.ShowInputField(generalVariables.colors["darkRed"], new Vector2(150, -75),
            new Vector2(400, 60), "Enter homework length in minutes", true, 40, generalVariables.colors["lightGreen"],
            InputField.ContentType.IntegerNumber, 40));
        inputFields.Add(generalFunctions.ShowInputField(generalVariables.colors["darkRed"], new Vector2(150, -150),
            new Vector2(400, 60), "Enter due date (YYYY-MM-DD)", true, 40, generalVariables.colors["lightGreen"],
            InputField.ContentType.Standard, 40));

        generalFunctions.ShowButton(generalVariables.canvas, "Set Homework", new Vector2(0, -260), new Vector2(250, 80),
            generalVariables.colors["darkRed"], generalVariables.colors["darkGreen"], 50, true, 21,
            generalVariables.colors["lightGreen"], 6); // Shows button to add homework

        dropdowns[0].AddOptions(generalVariables.classNames); // Adds class names to class dropdown list
        dropdowns[1].AddOptions(generalVariables.gameNames); // Adds game names to game dropdown list
    }
    public static void SetHomework() {
        List<string> errors = instance.ValidateInputs(instance.inputFields);
        // ^ Creates list of error messages and calls ValidateInputs to provide error messages
        if (errors.Count > 0) { // If there are any errors
            instance.generalFunctions.ShowUIBox(errors, 0, new Vector2(0, -50), new Vector2(1000, 525));
            // ^ Show error message
            return; // End function
        }
        instance.StartCoroutine(instance.SendInfoToDatabase()); // Calls SendInfoToDatabase
    }
    List<string> ValidateInputs(List<GameObject> fields) {
        List<string> errors = new List<string>(); // Creates empty list of error messages
        // Retrieves user inputs
        string homeworkName = fields[0].GetComponent<InputField>().text;
        string homeworkLength = fields[1].GetComponent<InputField>().text;
        string dueDate = fields[2].GetComponent<InputField>().text;

        if (homeworkName.Length== 0||homeworkLength.Length==0 || dueDate.Length == 0) { // Makes sure no field is left empty
            errors.Add("Field cannot be left empty."); // Adds error message
        }
        if (homeworkLength.Length > 0) {
            if (int.Parse(homeworkLength) < 5 || int.Parse(homeworkLength) > 120) { // Makes sure hw length is within range
                errors.Add("Limit homeworks to between 5 minutes and 120 minutes."); // Adds error message
            }
        }
        string[] splittedDate = dueDate.Split('-');
        int integer;
        if (splittedDate.Length != 3 || splittedDate[0].Length != 4 || splittedDate[1].Length != 2 ||
            splittedDate[2].Length != 2) { // Checks to make sure date is entered in pre-defined format
            errors.Add("Due Date formatted incorrectly."); // Adds error message
        }
        else if (!int.TryParse(splittedDate[0], out integer) || !int.TryParse(splittedDate[1], out integer) ||
            !int.TryParse(splittedDate[2], out integer)) { // If any part of the date is not an integer
            errors.Add("Due Date formatted incorrectly."); // Adds error message
        }
        return errors; // Returns list of errors
    }
    IEnumerator SendInfoToDatabase() {
        string url = "http://alevelproject2019.000webhostapp.com/teachersethomeworkscript.php?sethw=true" +
            "&classid=" + generalVariables.classIDs[dropdowns[0].value] +
            "&hwname='" + inputFields[0].GetComponent<InputField>().text + "'" +
            "&gameid=" + generalVariables.gameIDs[dropdowns[1].value] +
            "&hwlength=" + inputFields[1].GetComponent<InputField>().text +
            "&duedate='" + inputFields[2].GetComponent<InputField>().text + "'"; // Defines URL to make request to

        UnityWebRequest webpage = UnityWebRequest.Get(url);
        yield return webpage.SendWebRequest(); // Sends web request
        string returnedText = webpage.downloadHandler.text; // Stores returned data

        string message;
        int callNum;
        if (returnedText == "1") { // If homework has been set successfully
            message = "Homework set successfully!"; // Set success message
            callNum = 22; // Set which function to go to when UI box is clicked on
        }
        else {
            message = "An error occurred. Try again."; // Set error message
            callNum = 0; // Set program to do nothing when UI box is clicked on (other than close box)
        }
        generalFunctions.ShowUIBox(new List<string>() { message }, callNum, new Vector2(0, -50), new Vector2(1000, 525));
        // ^ Shows message (either success or error)
    }
    public static void GoToViewHomeworksScene() {
        SceneManager.LoadScene("Teacher - HomeworkMenuScene"); // Loads Homework Menu Scene
    }
}