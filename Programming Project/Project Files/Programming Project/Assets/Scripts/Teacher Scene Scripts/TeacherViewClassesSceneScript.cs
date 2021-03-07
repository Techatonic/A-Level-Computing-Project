using UnityEngine;
using UnityEngine.SceneManagement; // Import module for changing scenes
public class TeacherViewClassesSceneScript : MonoBehaviour {
    // Create script reference variables
    static TeacherViewClassesSceneScript instance;
    GeneralFunctions generalFunctions;
    GeneralVariables generalVariables;

    string[] buttonTexts = new string[] { // Array of button texts
        "View Leaderboards",
        "Homeworks"
    };

    void Start() {
        // Assign script references
        instance = GameObject.Find("Scene Script Game Object").GetComponent<TeacherViewClassesSceneScript>();
        generalFunctions = GameObject.Find("General Script Game Object").GetComponent<GeneralFunctions>();
        generalVariables = GameObject.Find("General Script Game Object").GetComponent<GeneralVariables>();
        generalVariables.canvas = GameObject.Find("Canvas"); // Sets canvas equal to the gameobject named "Canvas"
        ShowUI(); // Calls ShowUI
    }
    void ShowUI() {
        generalFunctions.ShowImageButton(new Vector2(-475, 275), new Vector2(100, 100), generalVariables.backButtonImage,
            Color.white, 1); // Shows back button
        generalFunctions.ShowText(new Vector2(0, 200), new Vector2(600, 100), "View Classes", 75, TextAnchor.MiddleCenter,
            generalVariables.colors["lightRed"], false); // Shows title text
        for (int i = 0; i < buttonTexts.Length; i++) { // Iterate through list of button texts
            // Create button
            GameObject newButton = generalFunctions.ShowButton(generalVariables.canvas, buttonTexts[i], new Vector2(-250 +
                500 * i, -50), new Vector2(360, 120), generalVariables.colors["darkRed"],
                generalVariables.colors["lightGreen"], 50, true, 19, generalVariables.colors["darkGreen"], 5);
            newButton.name = buttonTexts[i]; // Assign name of button object to its text
        }
    }
    public static void GoToNextScene(string objectName) {
        GeneralVariables.sceneOrder.Add(SceneManager.GetActiveScene().buildIndex);// Add scene to list of visited scenes
        if (objectName == "View Leaderboards") { // If button chosen was view leaderboards
            SceneManager.LoadScene("ViewLeaderboardScene"); // Load View Leaderboard Scene
        }
        else if (objectName == "Homeworks") { // If button chosen was homeworks
            SceneManager.LoadScene("Teacher - HomeworkMenuScene"); // Load Homework Menu Scene
        }
    }
}