using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class CreateAccountSceneScript : MonoBehaviour {

    // Define coordinates for text
    Vector2[] textPositions = new Vector2[]{
        new Vector2(-300, 230),
        new Vector2(-300, 80),
        new Vector2(-300, 5),
        new Vector2(-300, -70),
        new Vector2(-300, -145),
        new Vector2(-300, -220)
    };
    // Define names of the rows in the form
    string[] textValues = new string[]{
        "I am a:",
        "First Name",
        "Last Name",
        "School Name",
        "Account Password",
        "School Year"
    };
    // List of input field objects
    List<GameObject> inputFields = new List<GameObject>();
        
    // Define coordinates of input fields
    Vector2[] inputFieldPositions = new Vector2[]{
        new Vector2(100, 80),
        new Vector2(100, 5),
        new Vector2(100, -70),
        new Vector2(100, -145),
        new Vector2(100, -220)
    };
    // Define placeholder values for input fields
    string[] placeholderValues = new string[]{
        "Enter first name here",
        "Enter last name here",
        "Enter school name here",
        "Enter password here (>=8 letters)",
        "Enter school year here (number)"
    };
    // Create list for toggle objects
    List<GameObject> toggles = new List<GameObject>();
    
    // Define toggle text values
    string[] toggleTextValues = new string[]{
        "Teacher",
        "Student"
    };
    // List of input text objects
    List<GameObject> textObjects = new List<GameObject>();

    // Defines possible input types for each input field
    InputField.ContentType[] contentTypes = new InputField.ContentType[]{
        InputField.ContentType.Name,
        InputField.ContentType.Name,
        InputField.ContentType.Name,
        InputField.ContentType.Password,
        InputField.ContentType.IntegerNumber
    };
    string accountUsername; // Username variable

    GeneralFunctions generalFunctions;
    GeneralVariables generalVariables;
    static CreateAccountSceneScript instance;
    void Start() { // Retrieves script references and calls ShowUI
        generalFunctions = GameObject.Find("General Script Game Object").GetComponent<GeneralFunctions>();
        // ^ Assigns reference to General Functions script
        generalVariables = GameObject.Find("General Script Game Object").GetComponent<GeneralVariables>();
        // ^ Assigns reference to General Variables script
        generalVariables.canvas = GameObject.Find("Canvas"); // Sets canvas equal to the gameobject named "Canvas"
        instance = GameObject.Find("Scene Script Game Object").GetComponent<CreateAccountSceneScript>();
        // ^ Assign reference to its own script
        ShowUI();
    }

    void ShowUI() {
        // Show title text
        generalFunctions.ShowText(new Vector2(100, 300),new Vector2(500, 100),"Create Account",65,TextAnchor.MiddleCenter,
            generalVariables.colors["lightRed"], false);
        AddToggles(); // Show toggles
        for (int i = 0; i < textValues.Length; i++) { // Iterate through list of text positions
            textObjects.Add(generalFunctions.ShowText(textPositions[i], new Vector2(325, 80), textValues[i], 40,
                TextAnchor.MiddleCenter, generalVariables.colors["lightRed"], false)); // Show text object
        }
        for (int i = 0; i < inputFieldPositions.Length; i++) { // Iterate through input fields
            inputFields.Add(generalFunctions.ShowInputField(Color.red, inputFieldPositions[i], new Vector2(480, 60),
                placeholderValues[i], false, 35, generalVariables.colors["darkGreen"], contentTypes[i], 40));
            // ^ Show input field
        }
        FlipExtraField(false); // Call FlipExtraField with parameter false. Means set to teacher account
        generalFunctions.ShowImageButton(new Vector2(-475, 275), new Vector2(100, 100), generalVariables.backButtonImage,
            Color.white, 1); // Show back button
        generalFunctions.ShowButton(generalVariables.canvas, "Sign Up", new Vector2(100, -300), new Vector2(260, 60),
            generalVariables.colors["lightGreen"], generalVariables.colors["lightRed"], 35, true, 4, Color.red, 6);
        // ^ Show sign up button
    }
    void AddToggles() {
        for (int i = 0; i < toggleTextValues.Length; i++) { // Iterate through number of toggles
            // Create toggle object
            GameObject newToggle = new GameObject();
            toggles.Add(newToggle);
            toggles[i].transform.SetParent(generalVariables.canvas.transform);
            RectTransform rectTransform = toggles[i].AddComponent<RectTransform>();
            generalFunctions.SetRectTransformValues(rectTransform, new Vector2(190 * i, 155), new Vector2(150, 150),
                new Vector2(1, 1));

            // Create text child
            GameObject textChild = new GameObject();
            textChild.transform.SetParent(toggles[i].transform);
            Text textComponent = textChild.AddComponent<Text>();
            generalFunctions.SetTextValues(textComponent, toggleTextValues[i], generalVariables.arcenaFont, 40,
                TextAnchor.MiddleCenter, generalVariables.colors["darkGreen"], false);
            RectTransform textRectTransform = textChild.GetComponent<RectTransform>();
            generalFunctions.SetRectTransformValues(textRectTransform, new Vector2(0, 75), new Vector2(120, 100), 
                new Vector2(1, 1));

            // Create image child as background
            GameObject imageChild = new GameObject();
            imageChild.transform.SetParent(toggles[i].transform);
            Image imageComponent = imageChild.AddComponent<Image>();
            imageComponent.color = generalVariables.colors["darkGreen"];
            RectTransform imageRectTransform = imageChild.GetComponent<RectTransform>();
            generalFunctions.SetRectTransformValues(imageRectTransform, new Vector2(0,0), new Vector2(50, 50), 
                new Vector2(1, 1));
            imageComponent.sprite = Resources.Load<Sprite>("ToggleSprite");
            imageComponent.useSpriteMesh = true;

            // Create tick object as child of background image
            GameObject tickChild = new GameObject();
            tickChild.transform.SetParent(imageChild.transform);
            Image tickImageComponent = tickChild.AddComponent<Image>();
            tickImageComponent.sprite = Resources.Load<Sprite>("TickIcon");

            RectTransform tickRectTransform = tickChild.GetComponent<RectTransform>();
            generalFunctions.SetRectTransformValues(tickRectTransform, new Vector2(0, 0), new Vector2(50, 50), 
                new Vector2(1, 1));

            // Create toggle component
            Toggle toggleComponent = toggles[i].AddComponent<Toggle>();
            toggleComponent.targetGraphic = imageComponent;
            toggleComponent.graphic = tickImageComponent;
            toggleComponent.onValueChanged.AddListener(delegate {
                ChangeToggle(toggles.IndexOf(newToggle));
            });
        }
    }
    void ChangeToggle(int toggleIndex) { // Switch toggle from student to teacher and vice versa
        if (toggles[toggleIndex].GetComponent<Toggle>().isOn && toggles[(toggleIndex+1)%2].GetComponent<Toggle>().isOn){
            // Toggle has been switched -> Turn off other toggle
            toggles[(toggleIndex + 1) % 2].GetComponent<Toggle>().isOn = false;
        }
        FlipExtraField(toggles[1].GetComponent<Toggle>().isOn); // Call FlipExtraField
    }
    void FlipExtraField(bool isStudent) { // Shows/hides final input field and text object
        inputFields[inputFields.Count - 1].SetActive(isStudent); // Makes SchoolYear input field visible
        textObjects[textObjects.Count - 1].SetActive(isStudent); // Makes SchoolYear text visible
    }
    public static void CreateAccount() {
        if (GameObject.Find("UI Box")) { // Ends function is UI box is on screen
            return;
        }
        // Retrieves user-entered values
        string firstName = instance.inputFields[0].GetComponent<InputField>().text;
        string lastName = instance.inputFields[1].GetComponent<InputField>().text;
        string schoolName = instance.inputFields[2].GetComponent<InputField>().text;
        string accountPassword = instance.inputFields[3].GetComponent<InputField>().text;
        string schoolYear = instance.inputFields[4].GetComponent<InputField>().text;
        string tableName;
        instance.accountUsername = null; // Nullifies the username
        List<string> errors = instance.ValidateInputs(firstName, lastName, schoolName, accountPassword, schoolYear);
        if (errors.Count > 0) {  // If there are any errors, show the UI box with the errors
            instance.generalFunctions.ShowUIBox(errors, 0, new Vector2(150, 0), new Vector2(640, 500));
            return;
        }
        if (instance.toggles[0].GetComponent<Toggle>().isOn) { // Checks which toggle is chosen
            tableName = "Teachers";
        }
        else {
            tableName = "Students";
        }
        instance.StartCoroutine(instance.GetNameCount(firstName, lastName)); // Calls GetNameCount
        instance.StartCoroutine(instance.SendInfoToDatabase( // Calls SendInfoToDatabase
            tableName, firstName, lastName, schoolName, accountPassword, schoolYear));
    }
    bool ValidateName(string value) { // Validates name inputs
        return value.Split(' ').Length == 1 && value.Length >= 2; // Checks if the name is one word
    }
    List<string> ValidateInputs(string firstName, string lastName, string schoolName, string accountPassword,
        string schoolYear) {
        List<string> errors = new List<string>(); // Creates list of errors

        // Checks if each input field has been entered correctly.
        // If they haven't then the relevant error is added to the list and shown
        if (!toggles[0].GetComponent<Toggle>().isOn && !toggles[1].GetComponent<Toggle>().isOn) {
            errors.Add("Account type not specified.");
        }
        if (!ValidateName(firstName)) {
            errors.Add("First Name has not been entered correctly.");
        }
        if (!ValidateName(lastName)) {
            errors.Add("Last Name has not been entered correctly.");
        }
        if (schoolName == "" || schoolName.Length < 2) {
            errors.Add("School Name has not been entered correctly.");
        }
        if (accountPassword.Length < 8) {
            errors.Add("Password not long enough.");
        }
        // check school year
        if (toggles[1].GetComponent<Toggle>().isOn) {
            // Requires a school year between 1 and 15 to be entered
            if (schoolYear == "" || int.Parse(schoolYear) > 15 || int.Parse(schoolYear) < 1) {
                errors.Add("School year out of range.");
            }
        }
        return errors;
    }

    IEnumerator GetNameCount(string firstName, string lastName) {
        string url = "http://alevelproject2019.000webhostapp.com/createaccountscript.php?" +
                     "getnamecount=true" +
                     "&firstname=" + firstName.Substring(0, 2) +
                     "&lastname=" + lastName.Substring(0, 2); // Defines url to access
        UnityWebRequest webpage = UnityWebRequest.Get(url);
        Debug.Log(url);
        yield return webpage.SendWebRequest(); // Sends request
        Debug.Log(webpage.downloadHandler.text);
        accountUsername = firstName.Substring(0, 2) + lastName.Substring(0, 2) + 
            (int.Parse(webpage.downloadHandler.text) + 1).ToString(); // Create new username
    }

    IEnumerator SendInfoToDatabase(string tableName, string firstName, string lastName, string schoolName, 
        string accountPassword, string schoolYear) {
        // Waits for username to be set from previous function
        float timeSpentWaiting = 0.0f;
        while (accountUsername == null) {
            yield return new WaitForSeconds(0.1f);
            timeSpentWaiting += 0.1f;
            if (timeSpentWaiting >= 10f) { // If waiting for more than 10 seconds
                generalFunctions.ShowUIBox(new List<string> { "Error connecting to server. Please try again." }, 0,
                    new Vector2(150, 0), new Vector2(640, 500)); // Show error
                yield break; // Exit function
            }
        }
        // Sets url and adds GET values
        string url = "http://alevelproject2019.000webhostapp.com/createaccountscript.php?" +
            "createaccount=true" + "&tablename=" + tableName +
            "&firstname='" + firstName + "'" + "&lastname='" + lastName + "'" +
            "&accountusername='" + accountUsername + "'" + "&accountpassword='" + accountPassword + "'" +
            "&schoolname='" + schoolName + "'";

        if (tableName == "Students") {
            url += "&schoolyear=" + schoolYear; // Add extra column is user is a student
        }
        UnityWebRequest webpage = UnityWebRequest.Get(url);
        yield return webpage.SendWebRequest(); // Sends the web request of url
        if (webpage.downloadHandler.text == "1") { // Checks if request was successful
            generalVariables.username = accountUsername; // Sets new username
            generalVariables.accountType = tableName; // Sets user's account type
            generalFunctions.ShowUIBox(new List<string>{ // Shows success UI box
                "Account Created!",
                "Your username is "+accountUsername}, 5, new Vector2(150, 0), new Vector2(640, 500));
        }
        else { // An error has occurred
            generalFunctions.ShowUIBox(new List<string> { "An error occurred. Please try again." }, 0, new Vector2(150, 0),
                new Vector2(640, 500));
            Debug.Log(webpage.downloadHandler.text); // Print error to console (for developer's use)
            accountUsername = null; // Resets username to null
        }
    }

    public static void GoToNextScene() {
        if (instance.generalVariables.accountType == "Teachers") {
            SceneManager.LoadScene("Teacher - MainMenuScene"); // Load teacher menu scene
        }
        else {
            SceneManager.LoadScene("Student - MainMenuScene"); // Load student menu scene
        }
    }
}