using UnityEngine;
using UnityEngine.SceneManagement;
public class TeacherHomeworkMenuSceneScript : MonoBehaviour {
    string[] buttonTexts = new string[]{ // Defines text for buttons
        "View Homeworks",
        "Set Homework"
    };
    // Creates script reference variables
    GeneralFunctions generalFunctions;
    GeneralVariables generalVariables;
    static TeacherHomeworkMenuSceneScript instance;

    void Start() {
        // Assigns script references
        generalFunctions = GameObject.Find("General Script Game Object").GetComponent<GeneralFunctions>();
        generalVariables = GameObject.Find("General Script Game Object").GetComponent<GeneralVariables>();
        generalVariables.canvas = GameObject.Find("Canvas"); // Sets canvas equal to the gameobject named "Canvas"
        instance = GameObject.Find("Scene Script Game Object").GetComponent<TeacherHomeworkMenuSceneScript>();
        ShowUI(); // Calls ShowUI
    }
    void ShowUI() {
        generalFunctions.ShowImageButton(new Vector2(-475, 275), new Vector2(100, 100), generalVariables.backButtonImage,
            Color.white, 1); // Shows back button
        generalFunctions.ShowText(new Vector2(0, 200), new Vector2(600, 100), "Homeworks Menu", 75, TextAnchor.MiddleCenter,
            generalVariables.colors["lightRed"], false); // Shows title text

        for (int i = 0; i < buttonTexts.Length; i++) { // Iterates through number of buttons
            GameObject button = generalFunctions.ShowButton(generalVariables.canvas, buttonTexts[i], new Vector2(-250 +
                500 * i, -50), new Vector2(360, 120), generalVariables.colors["lightGreen"],
                generalVariables.colors["lightRed"], 50, true, 20, generalVariables.colors["darkRed"], 6);
            // ^ Shows a button with text stated in its index of buttonTexts
            button.name = buttonTexts[i];
        }
    }
    public static void GoToNextScene(string objectName) {
        GeneralVariables.sceneOrder.Add(SceneManager.GetActiveScene().buildIndex); // Adds current scene to list
        if (objectName == "View Homeworks") { // If View Homeworks button was chosen
            SceneManager.LoadScene("Teacher - ViewHomeworksScene"); // Load View Homeworks Scene
        }
        else if(objectName == "Set Homework"){ // If Set Homework button was chosen
            SceneManager.LoadScene("Teacher - SetHomeworkScene"); // Load Set Homework Scene
        }
    }
}