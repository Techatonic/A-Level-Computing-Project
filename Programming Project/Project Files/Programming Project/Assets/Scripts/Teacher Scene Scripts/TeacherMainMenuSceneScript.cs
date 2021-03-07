using UnityEngine;
using UnityEngine.SceneManagement;

public class TeacherMainMenuSceneScript : MonoBehaviour {
    // Create script references
    GeneralFunctions generalFunctions;
    GeneralVariables generalVariables;

    string[] buttonTexts = new string[]{ // Define text to go in buttons
        "View Classes",
        "Add Class"
    };

    void Start() {
        // Assign script references
        generalFunctions = GameObject.Find("General Script Game Object").GetComponent<GeneralFunctions>();
        generalVariables = GameObject.Find("General Script Game Object").GetComponent<GeneralVariables>();
        generalVariables.canvas = GameObject.Find("Canvas"); // Sets canvas equal to the gameobject named "Canvas"
        ShowUI(); // Calls ShowUI
    }
    void ShowUI() {
        generalFunctions.ShowText(new Vector2(0, 200), new Vector2(400, 100), "Main Menu", 75, TextAnchor.MiddleCenter,
            generalVariables.colors["lightRed"], false); // Shows title text

        for (int i = 0; i < buttonTexts.Length; i++) { // Iterates through number of buttons
            GameObject buttonObject = generalFunctions.ShowButton(generalVariables.canvas, buttonTexts[i],
                new Vector2(-250 + 500 * i, -50), new Vector2(360, 120), generalVariables.colors["lightGreen"],
                generalVariables.colors["lightRed"], 50, true, 8, generalVariables.colors["darkRed"], 6);
            // ^ Creates new button
            buttonObject.name = buttonTexts[i]; // Assigns name of button object
        }
    }
    public static void GoToNextScene(string objectName) {
        GeneralVariables.sceneOrder.Add(SceneManager.GetActiveScene().buildIndex); // Adds current scene to scene list
        if (objectName == "View Classes") { // Checks which button was clicked on
            SceneManager.LoadScene("Teacher - ViewClassesScene"); // Goes to ViewClasses scene
        }
        else {
            SceneManager.LoadScene("Teacher - AddClassScene"); // Goes to AddClass scene
        }
    }
}