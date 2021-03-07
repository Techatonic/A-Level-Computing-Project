using System.Collections.Generic;
using System.Collections;
using UnityEngine;                    // Imports relevant modules
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
public class LoadingSceneScript : MonoBehaviour {

    GeneralFunctions generalFunctions; // Reference to General Functions script
    GeneralVariables generalVariables; // Reference to General Variables script
    static LoadingSceneScript instance; // Static reference to its own script

    GameObject loadingText; // Loading text object

    void Start() {
        DontDestroyOnLoad(GameObject.Find("General Script Game Object").gameObject);
        // ^ Keeps General Script Game Object in all future scenes
        generalFunctions = GameObject.Find("General Script Game Object").GetComponent<GeneralFunctions>();
        // ^ Assigns generalFunctions
        generalVariables = GameObject.Find("General Script Game Object").GetComponent<GeneralVariables>();
        // ^ Assigns generalVariables
        generalVariables.canvas = GameObject.Find("Canvas"); // Sets canvas equal to the gameobject named "Canvas"
        instance = GameObject.Find("Scene Script Game Object").GetComponent<LoadingSceneScript>();
        // ^ Assigns instance to its own script component in the Scene Script Game Object
        ShowUI(); // Calls ShowUI function to display UI elements
        StartCoroutine("CheckInternetConnection"); // Calls CheckInternetConnection to check if user has connection
        InvokeRepeating("ChangeLoadingText", 0.3f, 0.3f); // Repeatedly calls ChangeLoadingText every 0.3 seconds
    }
    void ShowUI() {
        loadingText = generalFunctions.ShowText(new Vector2(0, 50), new Vector2(250, 100), "Loading", 75,
        TextAnchor.MiddleLeft, generalVariables.colors["lightRed"], true);
        // ^ Shows loading text
    }
    IEnumerator CheckInternetConnection() {
        string url = "alevelproject2019.000webhostapp.com/checkinternetconnection.php";
        // ^ Defines url to access
        UnityWebRequest webpage = UnityWebRequest.Get(url); // Defines webpage request
        yield return webpage.SendWebRequest(); // Sends webpage request and waits for response
        string downloadData = webpage.downloadHandler.text; // Stores webpage output
        if (downloadData == "1") { // If output from webpage is 1, user has internet connection
            StartCoroutine("GetGameNames"); // Call GetGameNames function
            yield break;
        }
        else { // No internet connection
            generalFunctions.ShowUIBox(new List<string>() { "No internet connection. Tap to close application" }, 2,
                new Vector2(0, 0), new Vector2(750, 500)); // Show error message
        }
    }
    IEnumerator GetGameNames() {
        string url = "alevelproject2019.000webhostapp.com/getgamenamesscript.php?getgamenames=true";
        // ^ Defines url to access
        UnityWebRequest webpage = UnityWebRequest.Get(url); // Defines webpage request
        yield return webpage.SendWebRequest(); // Sends webpage request and waits for response
        string downloadData = webpage.downloadHandler.text; // Stores webpage output
        string[] splitData = downloadData.Split('|'); // Splits output into separate games
        for (int i = 0; i < splitData.Length - 1; i++) { // Iterates through list of games
            generalVariables.gameIDs.Add(splitData[i].Split(',')[0]); // Adds game id to list
            generalVariables.gameNames.Add(splitData[i].Split(',')[1]); // Adds game name to list
        }
        SceneManager.LoadScene("StartScene"); // Go to start scene
    }
    void ChangeLoadingText() {
        string str = loadingText.GetComponent<Text>().text; // Define current string
        string dots = new string('.', (str.Split('g')[1].Length + 1) % 4); // Add full stop
        loadingText.GetComponent<Text>().text = "Loading" + dots; // Set new string to text object
    }
    public static void CloseApplication() {
        Application.Quit(); // Close the application
    }
}