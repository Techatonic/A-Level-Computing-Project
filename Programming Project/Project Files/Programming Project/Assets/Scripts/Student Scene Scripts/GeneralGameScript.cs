using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class GeneralGameScript : MonoBehaviour {

    public float startTime; // Timestamp for beginning of countdown
    public int score; // Defines score variable
    public GameObject timerObject; // Creates timer object variable

    // Script reference variables
    GeneralFunctions generalFunctions;
    GeneralVariables generalVariables;
    static GeneralGameScript instance;

    Action[] actions = new Action[] { // List for what to do when the countdown runs out
        /*0*/ () => {EndGame(); },
        /*1*/ () => {GameAlgebraMillionaireSceneScript.CountdownFinished(); },
    };

    void Start() {
        // Assigns script references
        generalFunctions = GameObject.Find("General Script Game Object").GetComponent<GeneralFunctions>();
        generalVariables = GameObject.Find("General Script Game Object").GetComponent<GeneralVariables>();
        instance = GameObject.Find("Scene Script Game Object").GetComponent<GeneralGameScript>();
        score = 0;
    }

    public IEnumerator StartCountdown(float countdownValue, int action) {
        startTime = Time.time; // Assigns beginning of countdown
        while (countdownValue > 0) { // While still time left
            timerObject.GetComponent<Text>().text = Math.Round(countdownValue, 2).ToString().Replace('.', ':');
            // ^ Show new timer text on screen
            yield return new WaitForSeconds(0.09f); // Wait for a short time
            countdownValue -= 0.09f; // Decrease time left
        }
        actions[action](); // Execute action for end of countdown
    }
    public static void EndGame() {
        instance.generalVariables.scoreToSave = instance.score; // Assign final score
        instance.generalVariables.playingTime = Time.time - instance.startTime; // Assign time spent playing the game
        SceneManager.LoadScene("EndGameScene"); // Loads End Game scene
    }

    /*void RunTests() {
        generalVariables.canvas = GameObject.Find("Canvas"); // Assigns canvas variable
        timerObject = generalFunctions.ShowText(new Vector2(0, 0), new Vector2(250, 125), "", 100, TextAnchor.MiddleCenter,
            generalVariables.colors["darkGreen"], true); // Defines new timer object
        StartCoroutine(StartCountdown(15, 0)); // runs countdown
    }*/

}