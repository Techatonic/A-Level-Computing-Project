// Defines used modules
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameSpeedMathsSceneScript : MonoBehaviour {
    // Creates script reference variables
    GeneralFunctions generalFunctions;
    GeneralVariables generalVariables;
    GeneralGameScript generalGameScript;
    static GameSpeedMathsSceneScript instance;

    GameObject scoreObject;    // Creates variable for text object showing the score
    GameObject questionObject; // Creates variable for text object showing the question
    int[] currentQuestion = new int[] { }; // [Operation, Value1, Value2, Answer]
    // ^ Array storing information about the current question
    int gameLength = 60; // Defines length of the game
    char[] exponentUnicodeValues = new char[] {'\u2070','\u00B9','\u00B2','\u00B3','\u2074','\u2075','\u2076',
        '\u2077','\u2078','\u2079'}; // Defines array of unicode values representing each exponent

    List<GameObject> answerOptions = new List<GameObject>();  // Creates list of game objects storing the buttons for the answers
    List<GameObject> livesRemaining = new List<GameObject>(); // Creates a list of game objects storing the heart image objects

    void Start() {
        // Assigns script reference variables
        generalFunctions = GameObject.Find("General Script Game Object").GetComponent<GeneralFunctions>();
        generalVariables = GameObject.Find("General Script Game Object").GetComponent<GeneralVariables>();
        generalGameScript = GameObject.Find("Scene Script Game Object").GetComponent<GeneralGameScript>();
        instance = GameObject.Find("Scene Script Game Object").GetComponent<GameSpeedMathsSceneScript>();
        generalVariables.canvas = GameObject.Find("Canvas"); // Sets canvas equal to the gameobject named "Canvas"
        ShowUI(); // Calls ShowUI
    }

    void ShowUI() {
        generalGameScript.timerObject = generalFunctions.ShowText(new Vector2(-300, 275), new Vector2(350, 100),
            gameLength.ToString() + ":00", 80, TextAnchor.MiddleLeft, generalVariables.colors["lightRed"], true); // Shows timer text
        scoreObject = generalFunctions.ShowText(new Vector2(0, 275), new Vector2(350, 100), "Score: 0", 80, 
            TextAnchor.MiddleCenter, generalVariables.colors["darkRed"], true); // Shows score text
        questionObject = generalFunctions.ShowText(new Vector2(0, 175), new Vector2(350, 100), "", 80, TextAnchor.MiddleCenter,
            generalVariables.colors["darkGreen"], true); // Shows text for question
        ShowPanel(); // Calls ShowPanel to show answer buttons and background

        for (int i = 250; i <= 450; i += 100) { // Iterates through x-coordinate positions
            livesRemaining.Add(generalFunctions.ShowImage(new Vector2(i, 275), new Vector2(100, 100), new Vector2(1, 1),
                Resources.Load<Sprite>("HeartImage"), Color.white)); // Shows life image in calculated position
        }
        generalFunctions.ShowUIBox(new List<string>{
            "Try to solve as many questions as possible within the time limit.",
            "Be careful, though, because you only can only have three incorrect attempts."},
            23, new Vector2(0, -60), new Vector2(1000, 550)); // Shows instructions UI box
    }
    void ShowPanel() {
        GameObject panelObject = generalFunctions.ShowImage(new Vector2(0, -112.5f), new Vector2(952, 445), new Vector2(1, 1), null, 
            generalVariables.colors["white"]); // Creates and shows panel object

        GridLayoutGroup gridLayoutComponent = panelObject.AddComponent<GridLayoutGroup>(); // Adds grid layout component to panel
        gridLayoutComponent.padding = new RectOffset(13, 13, 13, 13);
        // ^ Sets padding between the cells and the edges of the panel as 13 pixels in each direction
        gridLayoutComponent.cellSize = new Vector2(300, 130); // Sets each cell size to be a width of 300 and a height of 130
        gridLayoutComponent.spacing = new Vector2(13, 13); // Sets the spacing between each of the cells as a width and height of 13 pixels
        gridLayoutComponent.constraint = GridLayoutGroup.Constraint.FixedColumnCount; // Constrains the number of columns to a fixed amount
        gridLayoutComponent.constraintCount = 3; // Sets number of columns to 3

        for (int i = 0; i < 9; i++) { // Iterates between 0 and 8
            answerOptions.Add(generalFunctions.ShowButton(panelObject, "", new Vector2(0, 0), new Vector2(300, 130),
                generalVariables.colors["darkGreen"], generalVariables.colors["darkRed"], 50, true, 24,
                generalVariables.colors["white"], 0));
            // ^ Creates and shows answer button, adding it to the grid, and adding it to list of answer option objects
        }
    }
    public static void StartGame() {
        instance.StartCoroutine(instance.generalGameScript.StartCountdown(instance.gameLength, 0));
        // Calls StartCountdown, taking game length and 0 as arguments
        instance.NewQuestion(); // Calls NewQuestion
    }
    void NewQuestion() {
        int operation = 4;//Random.Range(0, 6); // Randomises which operation to use 
        // ^ operation value corresponds to index of [Addition, Subtraction, Multiplication, Division, Exponent, Factorial]
        int value1 = 0; // Pre-defines value1 to be 0
        int value2 = 0; // Pre-defines value2 to be 0
        int answer; // Creates integer answer - representing the answer to the question

        while (Mathf.Abs(value1) <= 1 || Mathf.Abs(value2) <= 1) { // Continue until value1 and value2 are not between -1 and 1
            value1 = Random.Range(-20, 21); // Randomises value between -20 and 20
            value2 = Random.Range(-20, 21); // Randomises value between -20 and 20
        }
        if (operation == 3) { // If operation selected is divison
            int temp = value1; // Temporarily stores value1 so value1 can be changed with its previous value stored
            value1 *= value2; // Multiplies value1 by value2 so it becomes the multiple
            value2 = temp; // Sets value2 to the original value of value1
        }
        if (operation == 4) { // If operation is exponent
            while (true) { // Iterate forever until it is broken out of
                value1 = Random.Range(-12, 13); // value1 is randomised between -12 and 12
                if (Mathf.Abs(value1) > 1) { // Makes sure value isn't between -1 and 1
                    break; // Breaks out of infinite loop if value 1 if value1 is not between -1 and 1
                }
            }
            value2 = Random.Range(2, 6); // value2 is randomised between 2 and 5
        }
        if (operation == 5) { // If operation is factorial
            value1 = Random.Range(2, 9); // value1 is randomised between 2 and 8
        }

        switch (operation) {
            case 0: // Addition
                if (value2 > 0) { // If second value is positive
                    questionObject.GetComponent<Text>().text = value1 + " + " + value2; // Define text of question object with addition
                }
                else {
                    questionObject.GetComponent<Text>().text = value1 + " - " + Mathf.Abs(value2); // Define question text with subtraction
                }
                answer = value1 + value2; // Defines answer value for question
                break; // Breaks out of switch
            case 1: // Subtraction
                if (value2 > 0) {
                    questionObject.GetComponent<Text>().text = value1 + " - " + value2; // Define text of question object with subtraction
                } else {
                    questionObject.GetComponent<Text>().text = value1 + " + " + Mathf.Abs(value2); // Define question text with addition
                }
                answer = value1 - value2; // Defines answer value for question
                break; // Breaks out of switch
            case 2: // Multiplication
                questionObject.GetComponent<Text>().text = value1 + " * " + value2; // Define text of question object
                answer = value1 * value2; // Defines answer value for question
                break; // Breaks out of switch
            case 3: // Divison
                questionObject.GetComponent<Text>().text = value1 + " " + '\u00F7' + " " + value2;
                // ^ Define text of question object
                answer = value1 / value2; // Defines answer value for question
                break; // Breaks out of switch
            case 4: // Exponent
                answer = (int)Mathf.Pow(Mathf.Abs(value1), value2); // Get answer value
                if (value1 < 0) { // If base is negative and exponent is odd
                    if (value2 % 2 == 1) {
                        answer = -answer; // Negate the answer
                    }
                    questionObject.GetComponent<Text>().text = "(" + value1.ToString() + ")" + exponentUnicodeValues[value2];
                    // ^ Define text of question object with parentheses
                } else {
                    questionObject.GetComponent<Text>().text = value1.ToString() + exponentUnicodeValues[value2];
                    // ^ Define text of question object
                }
                break; // Breaks out of switch
            case 5: // Factorial
                questionObject.GetComponent<Text>().text = value1 + " !"; // Define text of question object
                int total = 1;
                for (int i = 1; i <= value1; i++) {
                    // Iterates through values between 1 and value1, multiplying the answer each time by the iterable variable
                    total *= i;
                }
                answer = total; // Equates answer and total
                break; // Breaks out of switch
            default:
                answer = 0; // Sets answer to 0 in the errorneous situation that none of the case are true
                break; // Breaks out of switch
        }

        currentQuestion = new int[] { operation, value1, value2, answer }; // Stores data about question globally

        int correctAnswerOption = Random.Range(0, 9); // Randomly chooses the index of the answer option with the right answer
        for (int i = 0; i < answerOptions.Count; i++) { // Iterates through number of answer options
            if (i == correctAnswerOption) { // If the current iteration is the correct answer index
                answerOptions[i].GetComponentInChildren<Text>().text = answer.ToString(); // Sets answer option text to correct answer
            }
            else { // Current iteration is not the correct answer index
                int rangeNum = 2 * Mathf.Abs(answer); // Sets range of possible values to twice the magnitude of the answer
                if (Mathf.Abs(answer) < 10) {
                    rangeNum = 20; // Sets range of answer to 20 if the answer value is too small
                }
                answerOptions[i].GetComponentInChildren<Text>().text = Random.Range(-rangeNum, rangeNum + 1).ToString();
                // ^ Sets answer option text to a random value in the range specified
            }
        }
    }
    public static void AnswerQuestion(GameObject buttonPressed) { // Called when any of the answers is clicked
        if (buttonPressed.GetComponentInChildren<Text>().text == instance.currentQuestion[3].ToString()) {
            // ^ Checks if button clicked is the correct answer
            instance.generalGameScript.score += 1; // Incrememnts score by one
            instance.scoreObject.GetComponent<Text>().text = "Score: " + instance.generalGameScript.score.ToString(); // Updates score text
            instance.NewQuestion(); // Calls NewQuestion to ask a new question
        }
        else {
            Destroy(instance.livesRemaining[instance.livesRemaining.Count - 1]);
            instance.livesRemaining.RemoveAt(instance.livesRemaining.Count - 1); // Removes right-most life from list
            if (instance.livesRemaining.Count == 0) { // Checks if number of lives is equal to 0
                GeneralGameScript.EndGame(); // Calls EndGame since the user is out of lives
            }
            else {
                instance.NewQuestion(); // Game hasn't ended to NewQuestion is called to ask a new question
            }
        }
    }
}