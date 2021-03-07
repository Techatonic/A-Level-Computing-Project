using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class TeacherAddClassSceneScript : MonoBehaviour {
    // Define coordinates of input fields
    Vector2[] inputFieldPositions = new Vector2[]{
        new Vector2(-175, 200),
        new Vector2(-275, 125),
        new Vector2(365, 200)
    };
    // Define placeholder values for input field
    string[] placeholderValues = new string[]{
        "Enter school year",
        "Enter student name",
        "Enter class name"
    };
    // Define box sizes for input fields
    Vector2[] inputFieldSizes = new Vector2[]{
        new Vector2(240,50),
        new Vector2(440,50),
        new Vector2(320,50)
    };
    // Define lists
    List<GameObject> inputFields = new List<GameObject>();
    List<GameObject> studentOptionButtons = new List<GameObject>();
    List<GameObject> studentSelectedObjects = new List<GameObject>();
    // Define content type of each input field
    InputField.ContentType[] contentTypes = new InputField.ContentType[]{
        InputField.ContentType.IntegerNumber,
        InputField.ContentType.Name,
        InputField.ContentType.Standard
    };
    // Defines list of content objects
    List<GameObject> contentObjects = new List<GameObject>();
    // Defines script references
    GeneralFunctions generalFunctions;
    GeneralVariables generalVariables;
    static TeacherAddClassSceneScript instance;
    // Defines lists
    List<List<string>> students = new List<List<string>>();
    List<List<string>> studentsOnAutoComplete = new List<List<string>>();
    List<string> chosenStudents = new List<string>();

    void Start() {
        // Assigns scripts and canvas
        generalFunctions = GameObject.Find("General Script Game Object").GetComponent<GeneralFunctions>();
        generalVariables = GameObject.Find("General Script Game Object").GetComponent<GeneralVariables>();
        generalVariables.canvas = GameObject.Find("Canvas"); // Sets canvas equal to the gameobject named "Canvas"
        instance = GameObject.Find("Scene Script Game Object").GetComponent<TeacherAddClassSceneScript>();
        ShowUI(); // Calls ShowUI
    }
    void ShowUI() {
        // Show back button
        generalFunctions.ShowImageButton(new Vector2(-475, 285), new Vector2(100, 100), generalVariables.backButtonImage,
            Color.white, 1);
        // Show title text
        generalFunctions.ShowText(new Vector2(0, 300), new Vector2(500, 100), "Add Class", 65, TextAnchor.MiddleCenter,
            generalVariables.colors["lightRed"], false);
        // Shows headings for input fields
        generalFunctions.ShowText(new Vector2(-400, 200), new Vector2(175, 60), "School Year", 40, TextAnchor.MiddleLeft,
            generalVariables.colors["lightRed"], false);
        generalFunctions.ShowText(new Vector2(110, 200), new Vector2(165, 60), "Class Name", 40, TextAnchor.MiddleLeft,
            generalVariables.colors["lightRed"], false);
        for (int i = 0; i < inputFieldPositions.Length; i++) { // Iterate through number of input fields
            inputFields.Add(generalFunctions.ShowInputField(Color.red, inputFieldPositions[i], inputFieldSizes[i],
                placeholderValues[i], false, 35, generalVariables.colors["darkGreen"], contentTypes[i], 40));
            // ^ Create input field
        }
        inputFields[0].GetComponent<InputField>().onEndEdit.AddListener(delegate { // Add event listener
            List<string> errors = ValidateInputs(inputFields[0].GetComponent<InputField>().text, "1", false);
            if (errors.Count > 0) {  // If there are any errors, show the UI box with the errors
                generalFunctions.ShowUIBox(errors, 0, new Vector2(-10, 10), new Vector2(780, 540)); // and end the function.
                return;
            }
            StartCoroutine(GetNamesFromDatabase(inputFields[0].GetComponent<InputField>().text));
        });
        // Add event listener
        inputFields[1].GetComponent<InputField>().onValueChanged.AddListener(delegate { ChangeStudentButtons(); });
        // Create submit button
        generalFunctions.ShowButton(generalVariables.canvas, "Add Class", new Vector2(0, -300), new Vector2(260, 60),
            generalVariables.colors["lightGreen"], generalVariables.colors["lightRed"], 35, true, 10, Color.red, 3);

        // Show left panel
        GameObject[] leftObjects = generalFunctions.ShowScrollRect(new Vector2(-275, -75), new Vector2(500, 320),
            Color.white, new Vector2(0, -590), new Vector2(480, 310));
        contentObjects.Add(leftObjects[1]);

        // Show right panel and heading
        generalFunctions.ShowText(new Vector2(275, 125), new Vector2(320, 60), "Students Selected", 60,
            TextAnchor.MiddleCenter, generalVariables.colors["darkGreen"], true);

        GameObject[] rightObjects = generalFunctions.ShowScrollRect(new Vector2(275, -75), new Vector2(500, 320),
            Color.white, new Vector2(0, -590), new Vector2(480, 310));
        contentObjects.Add(rightObjects[1]);
    }
    IEnumerator GetNamesFromDatabase(string schoolYear) {
        students.Clear(); // Clears list of students
        string url = "http://alevelproject2019.000webhostapp.com/teacheraddclassscript.php?getnames=true&" +
            "accountusername=" + generalVariables.username +
            "&schoolyear=" + schoolYear; // Defines URL
        UnityWebRequest webpage = UnityWebRequest.Get(url);
        yield return webpage.SendWebRequest(); // Sends URL request
        string returnedText = webpage.downloadHandler.text; // Stores returned text
        string[] studentsSplitted = returnedText.Split('|'); // Splits text by student
        for (int studentNum = 0; studentNum < studentsSplitted.Length - 1; studentNum++) { // Iterates through results
            students.Add(new List<string>(studentsSplitted[studentNum].Split(','))); // Adds student to list
        }
        chosenStudents.Clear(); // Clears list of chosen students
        while (studentSelectedObjects.Count > 0) { // Iterates through selected students, removing all of them
            Destroy(studentSelectedObjects[0]);
            studentSelectedObjects.RemoveAt(0);
        }
        ChangeStudentButtons(); // Calls ChangeStudentButtons
        inputFields[1].GetComponent<InputField>().text = ""; // Reverts input field text to an empty string
        contentObjects[1].GetComponent<RectTransform>().sizeDelta = new Vector2(500, 320);
        // ^ Reverts scroll rect size to default
    }
    void ChangeStudentButtons() { // Changes the students listed in left panel
        foreach (GameObject o in studentOptionButtons) { // Iterates through students in left panel and removes them
            Destroy(o.gameObject);
        }
        string userInput = inputFields[1].GetComponent<InputField>().text; // Retrieves user input
        string schoolYear = inputFields[0].GetComponent<InputField>().text; // Retrieves user input
        studentOptionButtons.Clear(); // Clears list of student options
        studentsOnAutoComplete.Clear(); // Clears list of students shown on left panel
        contentObjects[0].GetComponent<RectTransform>().sizeDelta = new Vector2(500, 320);
        // ^ Reverts scroll rect size to default
        foreach (List<string> student in students) { // Iterates through each student
            if ((student[0] + " " + student[1]).ToLower().Contains(userInput.ToLower())) {
                // ^ Checks if the user input is part of this student's name
                if (chosenStudents.Contains(student[2])) { // Checks if this student has already been selected
                    continue; // Move on to next iteration
                }
                if (studentsOnAutoComplete.Count >= 5) { // If size of auto complete box > 5
                    contentObjects[0].GetComponent<RectTransform>().sizeDelta += new Vector2(0, 60); // Expand scroll rect
                }
                string buttonText = student[0] + " " + student[1] + "  -  " + student[2]; // Define name
                float contentHeight = contentObjects[0].GetComponent<RectTransform>().sizeDelta.y; // Get height of rect
                GameObject buttonObject = generalFunctions.ShowButton(contentObjects[0], buttonText,
                    new Vector2(0, -10 - contentHeight / 2 - 60 * studentsOnAutoComplete.Count), new Vector2(480, 50),
                    Color.white, generalVariables.colors["darkGreen"],35,true,9,generalVariables.colors["lightGreen"], 3);
                // ^ Create button object listing student as an option
                buttonObject.name = studentsOnAutoComplete.Count.ToString(); // Change name of button object
                RectTransform rectTransform = buttonObject.GetComponent<RectTransform>();
                generalFunctions.SetAnchors(rectTransform, new Vector2(0.5f, 1),new Vector2(0.5f, 1),new Vector2(0.5f, 1));

                studentsOnAutoComplete.Add(student); // Add student to list of students on auto complete
                studentOptionButtons.Add(buttonObject); // Add student button object to list
            }
        }
    }
    public static void SelectStudent(string objectName) { // Called when student option button is clicked
        if (instance.chosenStudents.Count >= 5) { // If number of chosen students > 5, expand scroll rect
            instance.contentObjects[1].GetComponent<RectTransform>().sizeDelta += new Vector2(0, 60);
        }
        List<string> chosenStudent = instance.studentsOnAutoComplete[int.Parse(objectName)]; // Retrieves student details
        string buttonText = chosenStudent[0] + " " + chosenStudent[1] + "  -  " + chosenStudent[2]; // Gets student name
        float contentHeight = instance.contentObjects[1].GetComponent<RectTransform>().sizeDelta.y;
        GameObject newObject = instance.generalFunctions.ShowButton(instance.contentObjects[1],
            buttonText, new Vector2(0, -10 - contentHeight / 2 - 60 * instance.chosenStudents.Count), new Vector2(480, 50),
            instance.generalVariables.colors["white"], instance.generalVariables.colors["darkGreen"], 35, true, 0,
            instance.generalVariables.colors["lightGreen"], 3);
        // ^ Creates new text object
        newObject.name = instance.chosenStudents.Count.ToString(); // Change name of object
        RectTransform rectTransform = newObject.GetComponent<RectTransform>();
        instance.generalFunctions.SetAnchors(rectTransform, new Vector2(0.5f, 1),new Vector2(0.5f, 1),new Vector2(0.5f, 1));
        instance.chosenStudents.Add(chosenStudent[2]); // Adds student to list of chosen students
        instance.studentSelectedObjects.Add(newObject); // Adds student object to list of selected objects
        instance.ChangeStudentButtons(); // Call ChangeStudentButtons
    }
    public static void AddClass() {
        // Retrieves user-entered value
        string schoolYear = instance.inputFields[0].GetComponent<InputField>().text;
        string className = instance.inputFields[2].GetComponent<InputField>().text;
        List<string> errors = instance.ValidateInputs(schoolYear, className, true);
        if (errors.Count > 0) {  // If there are any errors, show the UI box with the errors
            instance.generalFunctions.ShowUIBox(errors, 0, new Vector2(-10, 10), new Vector2(780, 540));
            return;
        }
        instance.StartCoroutine(instance.SendInfoToDatabase(schoolYear, className)); // Calls SendInfoToDatabase
    }
    List<string> ValidateInputs(string schoolYear, string className, bool checkStudentList) {
        List<string> errors = new List<string>();
        // Checks if the input field has been entered correctly.
        // Requires a school year between 1 and 15 to be entered
        if (schoolYear == "" || int.Parse(schoolYear) > 15 || int.Parse(schoolYear) < 1) {
            errors.Add("School year out of range.");
        }
        if (className == "") {
            errors.Add("Class name cannot be empty.");
        }
        if (!className.Replace(" ", string.Empty).All(char.IsLetterOrDigit)) {
            errors.Add("Class name - only letters, numbers and spaces are permitted");
        }
        if (checkStudentList && chosenStudents.Count == 0) {
            errors.Add("No students selected.");
        }
        return errors; // Return list of error messages
    }
    IEnumerator SendInfoToDatabase(string schoolYear, string className) {
        // Sets url and adds GET values
        string url = "http://alevelproject2019.000webhostapp.com/teacheraddclassscript.php?" +
            "addclass=true" +
            "&teacherusername='" + generalVariables.username + "'" +
            "&schoolyear=" + schoolYear +
            "&classname='" + className + "'";
        // Adds student usernames to url
        foreach (string username in chosenStudents) {
            url += "&studentusernames[]='" + username + "'";
        }

        UnityWebRequest webpage = UnityWebRequest.Get(url);
        yield return webpage.SendWebRequest(); // Sends the web request of url

        if (webpage.downloadHandler.text == "1") { // Checks if request was successful
            yield return StartCoroutine(generalFunctions.GetClassOptions()); // Call GetClassOptions
            generalFunctions.ShowUIBox(new List<string> { "Class Created!" },11,new Vector2(-10, 10),new Vector2(780, 540));
        }
        else { // An error has occurred
            generalFunctions.ShowUIBox(new List<string> { "An error occurred. Please try again.",
                webpage.downloadHandler.text }, 0, new Vector2(-10, 10), new Vector2(780, 540));
            Debug.Log(webpage.downloadHandler.text); // Print error to console for developer's use
        }
    }
    public static void GoToNextScene() {
        SceneManager.LoadScene("Teacher - MainMenuScene"); // Load Main Menu scene
    }
}