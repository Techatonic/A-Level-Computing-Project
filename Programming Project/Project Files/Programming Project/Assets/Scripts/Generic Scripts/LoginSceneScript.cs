using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class LoginSceneScript : MonoBehaviour {

    string[] textValues = new string[]{ // Headings for input fields
        "Username",
        "Password",
    };
    string[] placeholderValues = new string[]{ // Placeholders
        "Enter username here",
        "Enter password here"
    };
    InputField.ContentType[] contentTypes = new InputField.ContentType[]{ // Input field data types
        InputField.ContentType.Alphanumeric,
        InputField.ContentType.Password
    };
    List<GameObject> inputFields = new List<GameObject>(); // List of input fields

    // Script references
    GeneralFunctions generalFunctions;
    GeneralVariables generalVariables;
    static LoginSceneScript instance;

    void Start() {
        generalFunctions = GameObject.Find("General Script Game Object").GetComponent<GeneralFunctions>();
        // ^ Assigns generalFunctions
        generalVariables = GameObject.Find("General Script Game Object").GetComponent<GeneralVariables>();
        // ^ Assigns generalVariables
        generalVariables.canvas = GameObject.Find("Canvas"); // Sets canvas equal to the gameobject named "Canvas"
        instance = GameObject.Find("Scene Script Game Object").GetComponent<LoginSceneScript>();
        // ^ Assigns instance to own script component
        ShowUI(); // Calls ShowUI
        //CheckSavedLogin(); // Calls CheckSavedLogin   <-- ADD THIS LINE BACK AT THE END
    }

    void ShowUI() {
        generalFunctions.ShowImageButton(new Vector2(-475, 275), new Vector2(100, 100), generalVariables.backButtonImage,
            Color.white, 1); // Show back button
        generalFunctions.ShowText(new Vector2(100, 250), new Vector2(500, 100), "Login", 65, TextAnchor.MiddleCenter,
            generalVariables.colors["lightRed"], false); // Show title text
        for (int i = 0; i < textValues.Length; i++) { // Iterate through number of inputs required
            generalFunctions.ShowText(new Vector2(-300, 100 - 150 * i), new Vector2(325, 80), textValues[i], 40,
                TextAnchor.MiddleCenter, generalVariables.colors["lightRed"], false); // Show field heading
            inputFields.Add(generalFunctions.ShowInputField(Color.red, new Vector2(100, 100-150*i), new Vector2(480, 60),
                placeholderValues[i], false, 35, generalVariables.colors["darkGreen"], contentTypes[i], 40));
            // ^ Show input field
        }
        generalFunctions.ShowButton(generalVariables.canvas, "Sign In", new Vector2(100, -200), new Vector2(240, 80),
            generalVariables.colors["lightGreen"], generalVariables.colors["lightRed"], 35, true, 6, Color.red, 6);
        // ^ Show submit button
    }
    void CheckSavedLogin() {
        // Checks if user's login is already stored on the program
        if (PlayerPrefs.HasKey("Username") && PlayerPrefs.HasKey("AccountType")) {
            LoginSuccess(PlayerPrefs.GetString("AccountType"), PlayerPrefs.GetString("Username")); // Call LoginSuccess
        }
    }
    public static void Login() {
        // Retrieves inputs
        string accountUsername = instance.inputFields[0].GetComponent<InputField>().text;
        string accountPassword = instance.inputFields[1].GetComponent<InputField>().text;
        List<string> errors = instance.ValidateInputs(accountUsername, accountPassword); // List of errors
        if (errors.Count > 0) { // Checks if there are any errors
            instance.generalFunctions.ShowUIBox(errors, 0, new Vector2(-10, 10), new Vector2(780, 540));
            // ^ Show error message
            return;
        }
        instance.StartCoroutine(instance.CheckLoginWithDatabase(accountUsername, accountPassword));
        // ^ Call CheckLoginWithDatabase
    }
    List<string> ValidateInputs(string accountUsername, string accountPassword) {
        List<string> errors = new List<string>();

        // Checks if each input field has been entered correctly.
        // If they haven't then the relevant error is added to the list and shown
        if (accountUsername.Length == 0) {
            errors.Add("Username cannot be empty.");
        }
        if (accountPassword.Length == 0) {
            errors.Add("Password cannot be empty.");
        }
        return errors;
    }

    IEnumerator CheckLoginWithDatabase(string accountUsername, string accountPassword) {
        string url = "http://alevelproject2019.000webhostapp.com/loginscript.php?" +
            "login=true" +
            "&accountusername='" + accountUsername + "'" +
            "&accountpassword='" + accountPassword + "'"; // Defines request URL

        UnityWebRequest webpage = UnityWebRequest.Get(url);

        yield return webpage.SendWebRequest(); // Sends the web request of url
        string downloadText = webpage.downloadHandler.text.Replace("\n", "");
        if (downloadText == "Teacher" || downloadText == "Student") { // Checks if request was successful
            PlayerPrefs.SetString("AccountType", downloadText); // Stores account type on client
            PlayerPrefs.SetString("Username", accountUsername); // Stores account type on client
            LoginSuccess(downloadText, accountUsername); // Calls LoginSuccess
        }
        else { // An error has occurred
            generalFunctions.ShowUIBox(new List<string> { "Login Failed. Try again." }, 0, new Vector2(-10,10),
                new Vector2(780,540)); // Show error message
            Debug.Log(webpage.downloadHandler.text); // Print error (for developer's use)
        }
    }
    void LoginSuccess(string accountType, string username) {
        generalVariables.accountType = accountType; // Store account type in program
        generalVariables.username = username; // Store username in program
        generalFunctions.ShowUIBox(new List<string> { "Login Successful!" }, 7, new Vector2(-10,10), new Vector2(780, 540));
        // ^ Show success message
    }
    public static void GoToNextSceneOnClick() {
        instance.StartCoroutine(instance.GoToNextScene()); // Calls GoToNextScene
    }
    IEnumerator GoToNextScene() {
        yield return StartCoroutine(generalFunctions.GetClassOptions()); // Waits for classes to be downloaded
        if (generalVariables.accountType == "Teacher") { // Checks account type of user
            SceneManager.LoadScene("Teacher - MainMenuScene"); // Load teacher menu scene
        }
        else {
            SceneManager.LoadScene("Student - MainMenuScene"); // Load student menu scene
        }
    }
}