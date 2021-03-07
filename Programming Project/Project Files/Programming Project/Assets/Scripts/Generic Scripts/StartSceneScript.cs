using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSceneScript : MonoBehaviour {
    GeneralFunctions generalFunctions; // Reference to script
    GeneralVariables generalVariables; // Reference to script
    static StartSceneScript instance; // Reference to own script
    List<Sprite> images = new List<Sprite>(); // List of images to be shown

    string[] buttonTexts = new string[]{ // List of names for buttons
        "Login",
        "Create Account"
    };

    void Start() { // Function runs automatically when the script loads        
        generalFunctions = GameObject.Find("General Script Game Object").GetComponent<GeneralFunctions>();
        // ^ Defines reference to script
        generalVariables = GameObject.Find("General Script Game Object").GetComponent<GeneralVariables>();
        // ^ Defines reference to script
        instance = GameObject.Find("Scene Script Game Object").GetComponent<StartSceneScript>();
        // ^ Defines reference to own script
        generalVariables.canvas = GameObject.Find("Canvas"); // Sets canvas equal to the gameobject named "Canvas"
        images.Add(Resources.Load<Sprite>("Home Screen Image 1")); // Retrieves image and adds to list of images
        images.Add(Resources.Load<Sprite>("Home Screen Image 2")); // Retrieves image and adds to list of images
        ShowUI(); // Calls ShowUI function
    }

    void ShowUI() {
        generalFunctions.ShowText(new Vector2(0, 200), new Vector2(700, 125), "Moving Maths", 100,
            TextAnchor.MiddleCenter, generalVariables.colors["lightRed"], false);
        // ^ Shows title text
        for (int i = 0; i < buttonTexts.Length; i++) { // Iterates through list of buttons
            GameObject button = generalFunctions.ShowButton(generalVariables.canvas, buttonTexts[i],
                new Vector2(-200 + 400 * i, 0), new Vector2(320, 120), generalVariables.colors["darkRed"],
                generalVariables.colors["lightGreen"], 50, true, 3, generalVariables.colors["darkGreen"], 6);
            // ^ Create button
            button.name = buttonTexts[i]; // Assigns name of button object
        }
        for (int i = 0; i < images.Count; i++) { // Iterates through list of images
            generalFunctions.ShowImage(new Vector2(800 * i - 400, -200), new Vector2(200, 200), new Vector2(1, 1), images[i],
                Color.white);
            // ^ Shows image
        }
    }
    public static void GoToNextScene(string objectName) { // Function which loads the next scene
        GeneralVariables.sceneOrder.Add(SceneManager.GetActiveScene().buildIndex);
        // ^ Add current scene to scene list
        if (objectName == "Login") { // If button clicked was login
            SceneManager.LoadScene("LoginScene"); // Load login scene
        }
        else {
            SceneManager.LoadScene("CreateAccountScene"); // Load create account scene
        }
    }
}