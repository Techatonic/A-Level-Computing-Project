using UnityEngine;
using System.Collections.Generic;
public class GeneralVariables : MonoBehaviour {
    public GameObject canvas; // Canvas (area for all UI elements)
    public string username = null; // Username set to null
    public Font arcenaFont; // Font
    public Sprite backButtonImage; // Image of back button

    public Dictionary<string, Color32> colors = new Dictionary<string, Color32>(){
        {"lightGreen", new Color32(112,173,71,255)},
        {"darkGreen", new Color32(18,92,10,255)},     // Defines the rgb values of commonly used
        {"lightRed", new Color32(255,47,47,255)},      // colours in the program. This will be
        {"darkRed", new Color32(255,0,0,255)},         // added to over time if new colours are used
        {"lightGrey",new Color32(128,128,128,255)},
        {"darkGrey",new Color32(64,64,64,255)},
        {"white", new Color32(255,255,255,255)},
        {"black",new Color32(0,0,0,255)}
    };
    public List<string> gameIDs = new List<string>();   // List of game ids
    public List<string> gameNames = new List<string>(); // List of game names
    public string accountType; // Account type variable (student or teacher)

    public List<string> classIDs = new List<string>();   // List of class ids
    public List<string> classNames = new List<string>(); // List of class names

    public static List<int> sceneOrder = new List<int>();
    // List for order of scenes viewed (for use with back button)
    // Only allows accepted scenes (no game scenes, start scene)


    //Only Student variables - Begin
    public string gamePlaying; // Most recently played game
    public int scoreToSave; // Score value to save
    public float playingTime; // Time spent by the student on the game
    // Only Student variables - End

    // Only Teacher - Begin
    // Only Teacher - End

    void Start() {
        arcenaFont = Resources.Load<Font>("ARCENA");
        backButtonImage = Resources.Load<Sprite>("BackButton");
    }
    /*
    void PrintColours() {
        int xPosition = -6;
        GameObject canvas = new GameObject();
        canvas.AddComponent<Canvas>();
        foreach (KeyValuePair<string,Color32> entry in colors) {
            GameObject newImage = new GameObject();
            newImage.transform.SetParent(canvas.transform);
            newImage.AddComponent<RectTransform>();
            newImage.transform.localPosition = new Vector3(xPosition, 0, 0);
            newImage.GetComponent<RectTransform>().sizeDelta = new Vector3(1, 1, 1);
            newImage.AddComponent<Image>().color = entry.Value;
            xPosition += 4;
        }
    }
    */

    /*void PrintVariables() {
        Debug.Log("Canvas: " + canvas);
        Debug.Log("username: " + username);
        Debug.Log("arcenaFont: " + arcenaFont);
        Debug.Log("backButtonImage: " + backButtonImage);
        Debug.Log("colors: " + colors);
        Debug.Log("gameIDs: " + gameIDs);
        Debug.Log("gameNames: " + gameNames);
        Debug.Log("accountType: " + accountType);
        Debug.Log("classIDs: " + classIDs);
        Debug.Log("classNames: " + classNames);
        Debug.Log("sceneOrder: " + sceneOrder);
        Debug.Log("gamePlaying: " + gamePlaying);
        Debug.Log("scoreToSave: " + scoreToSave);
        Debug.Log("playingTime: " + playingTime);
    }*/
}