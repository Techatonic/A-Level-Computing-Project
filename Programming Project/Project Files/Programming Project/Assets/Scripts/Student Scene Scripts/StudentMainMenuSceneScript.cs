using UnityEngine;
using UnityEngine.SceneManagement;

public class StudentMainMenuSceneScript : MonoBehaviour {
    // Create script variables
    GeneralFunctions generalFunctions;
    GeneralVariables generalVariables;
    static StudentMainMenuSceneScript instance;

    string[] buttonTexts = new string[]{ // Define text to go on buttons
        "Play Game",
        "View Leaderboards",
        "View Homeworks"
    };
    string[] sceneNames = new string[]{ // Define names of scenes to go to from button
        "Student - ChooseGameScene",
        "ViewLeaderboardScene",
        "Student - ViewHomeworksScene"
    };

    void Start() {
        // Assigns script references
        generalFunctions = GameObject.Find("General Script Game Object").GetComponent<GeneralFunctions>();
        generalVariables = GameObject.Find("General Script Game Object").GetComponent<GeneralVariables>();
        instance = GameObject.Find("Scene Script Game Object").GetComponent<StudentMainMenuSceneScript>();
        generalVariables.canvas = GameObject.Find("Canvas"); // Sets canvas equal to the gameobject named "Canvas"
        ShowUI(); // Calls ShowUI
    }

    void ShowUI() {
        generalFunctions.ShowText(new Vector2(0, 200), new Vector2(400, 100), "Main Menu", 75, TextAnchor.MiddleCenter,
            generalVariables.colors["lightRed"], false); // Shows title text

        for (int i = 0; i < buttonTexts.Length; i++) { // Iterates through number of buttons
            GameObject newButton = generalFunctions.ShowButton(generalVariables.canvas, buttonTexts[i],
                new Vector2(-375 + 375 * i, -50), new Vector2(320, 120), generalVariables.colors["lightGreen"],
                generalVariables.colors["lightRed"], 50, true, 12, generalVariables.colors["darkRed"], 6);
            // ^ Creates new button
            newButton.name = i.ToString(); // Sets name of button object to index of buttonTexts
        }
    }
    public static void GoToNextScene(string objectName) {
        GeneralVariables.sceneOrder.Add(SceneManager.GetActiveScene().buildIndex); // Adds current scene to list of scenes
        SceneManager.LoadScene(instance.sceneNames[int.Parse(objectName)]); // Loads next scene
    }
}