using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class StudentChooseGameSceneScript : MonoBehaviour {
    // Create script variables
    GeneralFunctions generalFunctions;
    GeneralVariables generalVariables;
    static StudentChooseGameSceneScript instance;

    List<Sprite> arrowImages = new List<Sprite>(); // Create list of arrow images 
    List<GameObject> gameButtons = new List<GameObject>(); // Create list of button objects
    int currentGameOption = 0; // Index of game button currently in centre of screen
    int numberOfGameOptions; // Number of games
    float movementSpeed = 7.5f; // Speed at which to move buttons
    bool optionsMoving = false; // Defining whether the buttons are moving or not
    float xPositionOfNextButton = 550f; // Determines position of buttons

    void Start() {
        // Assigns script references
        generalFunctions = GameObject.Find("General Script Game Object").GetComponent<GeneralFunctions>();
        generalVariables = GameObject.Find("General Script Game Object").GetComponent<GeneralVariables>();
        instance = GameObject.Find("Scene Script Game Object").GetComponent<StudentChooseGameSceneScript>();
        generalVariables.canvas = GameObject.Find("Canvas"); // Sets canvas equal to the gameobject named "Canvas"
        arrowImages.Add(Resources.Load<Sprite>("Left Arrow")); // Adds arrow image to list
        arrowImages.Add(Resources.Load<Sprite>("Right Arrow")); // Adds arrow image to list
        numberOfGameOptions = generalVariables.gameNames.Count;
        ShowUI(); // Calls ShowUI
    }

    void ShowUI() {
        // Title text
        generalFunctions.ShowText(new Vector2(0, 200), new Vector2(400, 100), "Choose Game", 75, TextAnchor.MiddleCenter,
            generalVariables.colors["lightRed"], false);
        // Show back button
        generalFunctions.ShowImageButton(new Vector2(-510, 285), new Vector2(100, 100), generalVariables.backButtonImage,
            Color.white, 1);
        // Game option buttons
        for (int i = 0; i < numberOfGameOptions * 2; i++) {
            Vector2 buttonPosition = new Vector2(xPositionOfNextButton * 2, -50);
            if (i == 0) { // First option
                buttonPosition.x = 0; // Centre of screen
            }
            else if (i == 1) { // Second option
                buttonPosition.x = xPositionOfNextButton; // Right-hand side of screen
            }
            else if (i == generalVariables.gameNames.Count * 2 - 1) { // Final option
                buttonPosition.x = -xPositionOfNextButton; // Left-hand side of screen
            }
            GameObject newButton = generalFunctions.ShowButton(generalVariables.canvas, generalVariables.gameNames[i %
                generalVariables.gameNames.Count],buttonPosition,new Vector2(400, 200),generalVariables.colors["lightRed"],
                generalVariables.colors["darkGreen"], 60, true, 14, generalVariables.colors["lightGreen"], 6);
            // ^ Creates new button object
            newButton.name = generalVariables.gameNames[i % generalVariables.gameNames.Count];
            // ^ Sets name of button object to game number
            gameButtons.Add(newButton); // Adds button to list
        }
        // Arrow Buttons
        for (int i = 0; i < arrowImages.Count; i++) { // Iterates through number of button images
            GameObject newButton = generalFunctions.ShowImageButton(new Vector2(800 * i - 400, 200), new Vector2(100, 100), 
                arrowImages[i], generalVariables.colors["lightRed"], 13); // Creates new button object
            newButton.name = i.ToString(); // Assigns name of object to its index in arrowImages
        }
    }
    public static void MoveGameButtons(string objectName) {
        if (instance.optionsMoving) { // If arrows are already moving then return
            return;
        }
        int rightOrLeft = int.Parse(objectName); // Gets index value
        int direction = (-2) * rightOrLeft + 1; // Converts index value to 1 for left arrow or -1 for right arrow

        // Shifts position of the four buttons
        instance.StartCoroutine(instance.MoveButton(instance.gameButtons[(instance.currentGameOption - 2 + rightOrLeft +
            instance.numberOfGameOptions * 2) % (instance.numberOfGameOptions * 2)], direction, true)); 
        instance.StartCoroutine(instance.MoveButton(instance.gameButtons[(instance.currentGameOption - 1 + rightOrLeft +
            instance.numberOfGameOptions * 2) % (instance.numberOfGameOptions * 2)], direction, false));
        instance.StartCoroutine(instance.MoveButton(instance.gameButtons[(instance.currentGameOption + rightOrLeft +
            instance.numberOfGameOptions * 2) % (instance.numberOfGameOptions * 2)], direction, false));
        instance.StartCoroutine(instance.MoveButton(instance.gameButtons[(instance.currentGameOption + 1 + rightOrLeft +
            instance.numberOfGameOptions * 2) % (instance.numberOfGameOptions * 2)], direction, false));

        instance.currentGameOption = (instance.currentGameOption - 1 + 2 * rightOrLeft + instance.numberOfGameOptions * 2) %
            (instance.numberOfGameOptions * 2); // Defines new current game option
    }
    IEnumerator MoveButton(GameObject buttonObject, int direction, bool endButton) {
        optionsMoving = true; // Boolean stating the options are now moving
        if (direction == 1 && endButton == true){//If moving to the right and is the button to move into the left-hand space
            buttonObject.transform.localPosition=new Vector2(-xPositionOfNextButton * 2,//Move button to the left-hand space
                buttonObject.transform.localPosition.y);
        }
        int movements = 0; // Count times moved
        while (movements < xPositionOfNextButton / movementSpeed) { // While button hasn't reached end position
            buttonObject.transform.localPosition += new Vector3(movementSpeed * direction, 0, 0); // Move button a bit
            movements++; // Add to movement
            yield return new WaitForSeconds(0.005f); // Wait a small amount of time
        }
        if (direction == -1 && endButton == true) {
            // ^ If buttons moving to the left and is the button to move into the right-hand space
            buttonObject.transform.localPosition = new Vector2(xPositionOfNextButton * 2, 
                buttonObject.transform.localPosition.y); // Move button into right-hand space
        }
        optionsMoving = false; // Option is no longer moving
    }
    public static void GoToNextScene(string objectName) {
        instance.generalVariables.gamePlaying = objectName; // Define new game name
        SceneManager.LoadScene("Game - " + objectName.Replace(" ", string.Empty) + "Scene"); // Load game scene
    }
}