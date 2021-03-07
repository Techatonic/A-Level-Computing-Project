using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
public class EndGameSceneScript : MonoBehaviour {
    // Creates script references
    GeneralFunctions generalFunctions;
    GeneralVariables generalVariables;
    static EndGameSceneScript instance;

    string[] buttonTexts = new string[] { // Defines text of buttons
        "Play Again",
        "View Leaderboards",
        "Main Menu"
    };
    bool scoreSent = false; // Defines if the database has returned a result for sending the score data
    bool homeworkSent = false; // Defines if the database has returned a result for sending the homework data

    void Start() {
        // Assigns script references
        generalFunctions = GameObject.Find("General Script Game Object").GetComponent<GeneralFunctions>();
        generalVariables = GameObject.Find("General Script Game Object").GetComponent<GeneralVariables>();
        generalVariables.canvas = GameObject.Find("Canvas"); // Sets canvas equal to the gameobject named "Canvas"
        instance = GameObject.Find("Scene Script Game Object").GetComponent<EndGameSceneScript>();
        ShowUI(); // Calls ShowUI
        StartCoroutine(SendScoreToDatabase(generalVariables.scoreToSave)); // Calls the coroutine SendScoreToDatabase
        StartCoroutine(UpdateHomeworkToDatabase(generalVariables.playingTime)); // Calls UpdateHomeworkToDatabase
    }

    void ShowUI() {
        // Shows end game message
        generalFunctions.ShowText(new Vector2(0, 250), new Vector2(500, 100), "Game Finished", 75, TextAnchor.MiddleCenter,
            generalVariables.colors["lightRed"], true);
        if (generalVariables.gamePlaying == "Algebra Millionaire") { // If the game played was algebra millionaire
            // Show how much 'money' they won
            generalFunctions.ShowText(new Vector2(0, 100), new Vector2(650, 125), "You won £" +generalVariables.scoreToSave,
                100, TextAnchor.MiddleCenter, generalVariables.colors["darkRed"], true);
        }
        else { // If the game was not algebra millionaire
            // Show score text
            generalFunctions.ShowText(new Vector2(0, 100),new Vector2(650, 125),"You scored: "+generalVariables.scoreToSave,
                100, TextAnchor.MiddleCenter, generalVariables.colors["darkRed"], true);
        }
        for (int i = 0; i < buttonTexts.Length; i++) { // Iterates through number of buttons
            // Creates button object
            GameObject newObject = generalFunctions.ShowButton(generalVariables.canvas, buttonTexts[i],
                new Vector2(-360 + 360 * i, -100), new Vector2(320, 120), generalVariables.colors["darkGreen"],
                generalVariables.colors["lightRed"], 50, true, 18, generalVariables.colors["lightRed"], 8);
            newObject.name = i.ToString();
        }
    }
    public static void GoToNextSceneOnClick(string objectName) {
        instance.StartCoroutine(GoToNextScene(objectName)); // Calls GoToNextScene
    }
    public static IEnumerator GoToNextScene(string objectName) {
        // Waits for information to be sent to database
        float timeSpentWaiting = 0.0f;
        while (instance.scoreSent == false || instance.homeworkSent == false) {
            yield return new WaitForSeconds(0.1f);
            timeSpentWaiting += 0.1f;
            if (timeSpentWaiting >= 10f) { // If waiting for more than 10 seconds
                instance.generalFunctions.ShowUIBox(new List<string> { "Error connecting to server. Please try again." }, 0,
                    new Vector2(150, 0), new Vector2(640, 500)); // Show error
                yield break; // Exit function
            }
        }
        if (objectName == "0") { // If button chosen was play again
            SceneManager.LoadScene("Game - " + instance.generalVariables.gamePlaying.Replace(" ", string.Empty) + "Scene");
        }
        else if (objectName == "1") { // If button chosen was view leaderboards
            instance.generalVariables.gamePlaying = null;
            SceneManager.LoadScene("ViewLeaderboardScene");
        }
        else { // If button chosen was main menu
            instance.generalVariables.gamePlaying = null;
            SceneManager.LoadScene("Student - MainMenuScene");
        }
    }
    IEnumerator SendScoreToDatabase(int score) {
        string url = "http://alevelproject2019.000webhostapp.com/sendscoretodatabasescript.php?" +
                     "sendscore=true" +
                     "&accountusername='" + generalVariables.username + "'" +
                     "&gamename='" + generalVariables.gamePlaying + "'" +
                     "&score=" + score; // Defines url to access
        UnityWebRequest webpage = UnityWebRequest.Get(url);
        yield return webpage.SendWebRequest(); // Sends web request
        scoreSent = true;
    }
    IEnumerator UpdateHomeworkToDatabase(float playingTime) {
        int secondsPlayingTime = Mathf.FloorToInt(playingTime); // Round down playing time

        string url = "http://alevelproject2019.000webhostapp.com/updatehomeworktodatabasescript.php?" +
                     "updatehomework=true" +
                     "&accountusername='" + generalVariables.username + "'" +
                     "&gameid='" + generalVariables.gameIDs[generalVariables.gameNames.
                        IndexOf(generalVariables.gamePlaying)] + "'" +
                     "&timespent=" + secondsPlayingTime; // Defines url to access
        UnityWebRequest webpage = UnityWebRequest.Get(url);
        yield return webpage.SendWebRequest(); // Sends web request
        homeworkSent = true;
    }
}