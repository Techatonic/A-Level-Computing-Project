// Defines used modules
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq; // Allows extra list manipulation

public class GameAlgebraMillionaireSceneScript : MonoBehaviour {
    // Creates script references
    GeneralFunctions generalFunctions;
    GeneralVariables generalVariables;
    GeneralGameScript generalGameScript;
    static GameAlgebraMillionaireSceneScript instance;

    // Creates money bracket variables
    int[] priceBrackets = new int[] { 100, 200, 300, 500, 1000, 2000, 4000, 8000, 16000, 32000, 64000, 125000, 250000, 500000, 1000000 };
    List<GameObject> priceObjects = new List<GameObject>();

    int currentScore = 0; // Defines user's current score

    // Creates question object variables
    GameObject questionPanel;
    GameObject questionObject;
    
    // Creates answer variables
    List<GameObject> answerObjects = new List<GameObject>();
    List<int> answers = new List<int>();
    List<int> answerIndices = new List<int>();
    // Creates correct answer variables
    int correctAnswer;
    int correctAnswerIndex;

    float timePerQuestion = 30f; // Defines length of time per question
    bool lifelineBeingUsed = false; // Defines whether the user has played a lifeline

    string[] phrases = new string[] { // Defines possible phrases for phone a friend
        "Absolutely no idea. Sorry! If I were to hazard a guess, I'd say ANSWER but I really don't know.",
        "Not very sure but I think it might be ANSWER. I'm about 50% sure",
        "I'm pretty sure it's ANSWER but I'm not 100%",
        "It's ANSWER. 100%."
    };
    List<Sprite> buttonImages = new List<Sprite>();      // List of images for use in lifeline buttons
    List<GameObject> lifelines = new List<GameObject>(); // List of lifeline button objects
    GameObject takeTheMoneySign; // Button object for taking the money

    Vector2[] buttonSizes = new Vector2[]{ // Size of lifeline buttons
        new Vector2(120,80),
        new Vector2(120,80),
        new Vector2(120,80),
        new Vector2(80,120)
    };
    List<GameObject> objectsToClose = new List<GameObject>();// List of objects created by lifeline that needs to be destroyed when closed
    IEnumerator currentCountdownCoroutine; // Creates variable defining the current countdown coroutine being executed

    void Start() {
        // Assigns script reference variables
        generalFunctions = GameObject.Find("General Script Game Object").GetComponent<GeneralFunctions>();
        generalVariables = GameObject.Find("General Script Game Object").GetComponent<GeneralVariables>();
        generalGameScript = GameObject.Find("Scene Script Game Object").GetComponent<GeneralGameScript>();
        instance = GameObject.Find("Scene Script Game Object").GetComponent<GameAlgebraMillionaireSceneScript>();

        generalVariables.canvas = GameObject.Find("Canvas"); // Sets canvas equal to the gameobject named "Canvas"
        // Adds the button sprites
        buttonImages.Add(Resources.Load<Sprite>("5050 Sign"));
        buttonImages.Add(Resources.Load<Sprite>("PhoneAFriend Sign"));
        buttonImages.Add(Resources.Load<Sprite>("AskTheAudience Sign"));
        buttonImages.Add(Resources.Load<Sprite>("TakeTheMoney Sign"));
        ShowUI(); // Calls ShowUI
    }
    void ShowUI() {
        GameObject panel = generalFunctions.ShowImage(new Vector2(400, 0), new Vector2(300, 650), new Vector2(1, 1), null, 
            generalVariables.colors["lightGrey"]); // Creates background for price brackets area
        for (int i = 0; i < priceBrackets.Length; i++) { // Iterates through number of price brackets
            priceObjects.Add(generalFunctions.ShowButton(panel, priceBrackets[i].ToString(), new Vector2(0, -298.5f + 42.75f * i),
            new Vector2(275, 40), generalVariables.colors["darkGrey"], generalVariables.colors["white"], 50, true, 0,
                generalVariables.colors["lightGreen"], 0)); // Creates price brackets button and adds it to priceBrackets lit
        }
        generalGameScript.timerObject = generalFunctions.ShowText(new Vector2(0, 285), new Vector2(500, 130), "30:00", 75,
            TextAnchor.MiddleLeft, generalVariables.colors["white"], true); // Creates and shows timer object

        for (int i = 0; i < buttonImages.Count; i++) { // Iterates through number of button images
            GameObject button = generalFunctions.ShowImageButton(new Vector2(i * 150 - 310, -269), buttonSizes[i],
                buttonImages[i], generalVariables.colors["white"], 25 + i); // Shows image of lifeline as button
            if (i < 3) { // If i<3, then the button is a lifeline
                lifelines.Add(button); // Adds button object to list of lifelines
            }
            else { // If i=4, then the button is for taking the money
                takeTheMoneySign = button; // Assigns takeTheMoneySign
            }
        }
        ShowQuestionAndAnswerButtons(); // Calls ShowQuestionAndAnswerButtons to display question panel
        NewQuestion(); // Creates and displays first question
    }
    void ShowQuestionAndAnswerButtons() {
        questionPanel = generalFunctions.ShowImage(new Vector2(-160, 150), new Vector2(600, 150), new Vector2(1, 1), null, 
            generalVariables.colors["black"]); // Shows background for question and answer area
        // Adds white outline to question panel with thickness of 6
        Outline outline = questionPanel.AddComponent<Outline>();
        outline.effectColor = generalVariables.colors["white"];
        outline.effectDistance = new Vector2(6, -6);

        questionObject = generalFunctions.ShowText(new Vector2(-160, 150), new Vector2(550, 140), "", 60,
            TextAnchor.MiddleCenter, generalVariables.colors["white"], true); // Creates and shows question object

        for (int x = 0; x <= 1; x++) { // Iterates between 0 and 1 inclusive
            for (int y = 0; y <= 1; y++) { // Iterates between 0 and 1 inclusive
                answerObjects.Add(generalFunctions.ShowButton(questionPanel, "", new Vector2(-150 + 300 * x, -169 - 100 * y),
                    new Vector2(300, 100), generalVariables.colors["black"], generalVariables.colors["white"], 60, true, 29,
                    generalVariables.colors["white"], 6)); // Shows answer button object with thick white outline
            }
        }
    }
    void NewQuestion() {
        answers.Clear(); // Clears list of current answer possibilities
        answerIndices = new List<int> { 0, 1, 2, 3 }; // Resets available answer indices
        foreach (GameObject answerObject in answerObjects) { // Iterates through answer objects
            answerObject.SetActive(true); // Displays the answer object
        }
        // Defines values in equation
        int a = 0;
        int b = 0;
        int c = 0;
        int d = 0;
        int x = 0;
        if (currentScore < 5) { // Question should be easy. In the form ax+b = c
            // Randomly assigns values in equation
            a = Random.Range(1, 21);
            x = Random.Range(2, 21);
            b = Random.Range(1, 21);
            c = a * x + b;

            questionObject.GetComponent<Text>().text = a.ToString() + "x + " + b.ToString() + " = " + c.ToString() + "\n" +
                "Solve for x."; // Redefines question text to show new question
        }
        else if (currentScore < 10) { // Question should be of medium difficulty. In the form a(bx+c) = d and can involve negatives
            // Randomly assigns values in equation. (Second multiplier in each line randomly assigns it to be either positive or negative)
            a = Random.Range(1, 6) * (Random.Range(0, 2) * 2 - 1);
            b = Random.Range(1, 11) * (Random.Range(0, 2) * 2 - 1);
            x = Random.Range(2, 21) * (Random.Range(0, 2) * 2 - 1);
            c = Random.Range(1, 21) * (Random.Range(0, 2) * 2 - 1);
            d = a * (b * x + c);

            if (c > 0) { // If c is positive
                questionObject.GetComponent<Text>().text = a.ToString() + "(" + b.ToString() + "x + " + c.ToString() + ") = "
                    + d.ToString() + "\n" + "Solve for x."; // Show text with plus sign
            }
            else { // c is negative
                questionObject.GetComponent<Text>().text = a.ToString() + "(" + b.ToString() + "x - " +
                    Mathf.Abs(c).ToString() + ") = " + d.ToString() + "\n" + "Solve for x."; // Show text with minus sign and absolute value of c
            }
        }
        else { // Question should be of hard difficulty. In the form a(bx+c)^2 = d
            // Randomly assigns values in equation.
            a = Random.Range(1, 11);
            b = Random.Range(1, 6);
            x = Random.Range(2, 11);
            c = Random.Range(-b * x + 1, 12 - b * x + 1);
            d = a * (int)Mathf.Pow(b * x + c, 2);

            if (c > 0) { // If c is positive
                questionObject.GetComponent<Text>().text = a.ToString() + "(" + b.ToString() + "x + " + c.ToString() + ")" +
                    '\u00B2' + "= " + d.ToString() + "\n" + "Solve for x."; // Shows text with plus sign
            }
            else { // c is negative
                questionObject.GetComponent<Text>().text = a.ToString() + "(" + b.ToString() + "x - " + Mathf.Abs(c).ToString()
                    + ")" + '\u00B2' + "= " + d.ToString() + "\n" + "Solve for x."; // Shows text with minus sign and uses absolute value of c
            }
        }
        correctAnswer = x; // Defines correct answer as x
        Debug.Log(correctAnswer);
        correctAnswerIndex = Random.Range(0, answerObjects.Count); // Randomly assigns index of the correct answer

        for (int i = 0; i < answerObjects.Count; i++) { // Iterates through numer of answer objects
            if (i == correctAnswerIndex) { // If the current iteration relates to the correct answer
                answerObjects[i].GetComponentInChildren<Text>().text = correctAnswer.ToString(); // Sets text of object to right answer
                answers.Add(correctAnswer); // Adds value to answers list
            }
            else { // Current iteration doesn't relate to the correct answer
                int newAnswer; // Creates variable for random answer
                while (true) { // Continue iterating until broken out of
                    newAnswer = Random.Range(1, correctAnswer * 2 + 1); // Generates random answer
                    if (answers.Contains(newAnswer) == false && newAnswer != correctAnswer) { 
                        // ^ If the value isn't already an answer and it isn't the correct answer
                        break; // Breaks out of loop
                    }
                }
                answerObjects[i].GetComponentInChildren<Text>().text = newAnswer.ToString(); // Sets text of object to random answer
                answers.Add(newAnswer); // Adds answer value to list of answers
            }
        }

        if (currentCountdownCoroutine != null) { // If a countdown is running
            StopCoroutine(currentCountdownCoroutine); // Stop the countdown
        }
        currentCountdownCoroutine = generalGameScript.StartCountdown(timePerQuestion, 1); // Define countdown function
        StartCoroutine(currentCountdownCoroutine); // Starts countdown
    }

    public static void CountdownFinished() {
        instance.EndGame(); // Calls EndGame because student has run out of time
    }
    public static void FiftyFifty() {
        if (instance.lifelineBeingUsed) { // Another lifeline can't be in the process of being used at the same time
            return; // So exit function
        }
        instance.lifelineBeingUsed = true; // Lifeline is currently being used
        while (true) { // Loop to decide which answers to remove
            int firstChoice = Random.Range(0, instance.answers.Count);  // Randomly assigns answer to remove
            int secondChoice = Random.Range(0, instance.answers.Count); // Randomly assigns answer to remove
            if (firstChoice != instance.correctAnswerIndex && secondChoice != instance.correctAnswerIndex
                && firstChoice != secondChoice) { // Neither value can be the correct answer and they can't be the same value
                // Resets the removed answer texts to an empty string
                instance.answerObjects[firstChoice].SetActive(false);
                instance.answerObjects[secondChoice].SetActive(false);

                instance.answerIndices.Remove(firstChoice);
                instance.answerIndices.Remove(secondChoice);
                instance.answers.RemoveAt(firstChoice); // Removes first answer from list
                if (secondChoice > firstChoice) {
                    secondChoice--; // Decrements secondChoice to make it the correct index (once the first answer is removed)
                }
                instance.answers.RemoveAt(secondChoice); // Removes second answer from list
                Destroy(instance.lifelines[0]); // Destroys 50/50 lifeline button
                instance.lifelineBeingUsed = false; // Lifeline is no longer being used
                return; // Ends function
            }
        }
    }
    public static void PhoneAFriend() {
        // End function if another lifeline is currently being processed
        if (instance.lifelineBeingUsed) {
            return;
        }
        instance.lifelineBeingUsed = true; // Sets lifelineBeingUsed to true

        string phoneAFriendText = ""; // Sets text to be shown to user as empty string
        int randomPhrase = Random.Range(0, instance.phrases.Length); // Randomises phrase to use
        int correctProbability = Random.Range(0, 4); // Randomises probability of responding with correct answer
        int randomAnswer = Random.Range(0, instance.answers.Count); // Randomises answer to show user, if answer shown shouldn't be correct
        if (randomPhrase == 0) { // Friend has no idea
            phoneAFriendText = instance.phrases[0].Replace("ANSWER", instance.answers[randomAnswer].ToString());
            // ^ Sets text to chosen phrases with random answer
        }
        else if (randomPhrase == 1) { // Friend is 50% sure
            if (correctProbability >= 2) { // 50% chance the user gets it wrong - random answer given
                phoneAFriendText = instance.phrases[1].Replace("ANSWER", instance.answers[randomAnswer].ToString());
            }
            else { // 50% chance the user gets it right - correct answer given
                phoneAFriendText = instance.phrases[1].Replace("ANSWER", instance.correctAnswer.ToString());
            }
        }
        else if (randomPhrase == 2) { // Friend is 75% sure
            if (correctProbability == 3) { // 25% chance the user gets it wrong - random answer given
                phoneAFriendText = instance.phrases[0].Replace("ANSWER", instance.answers[randomAnswer].ToString());
            }
            else { // 75% chance the user gets it right - correct answer given
                phoneAFriendText = instance.phrases[0].Replace("ANSWER", instance.correctAnswer.ToString());
            }
        }
        else if (randomPhrase == 3) { // Friend is 100% sure - correct answer is guaranteed
            phoneAFriendText = instance.phrases[3].Replace("ANSWER", instance.correctAnswer.ToString());
        }
        // Shows friend's response
        instance.objectsToClose.Add(instance.generalFunctions.ShowImage(new Vector2(-165, -140), new Vector2(750, 400), new Vector2(1, 1), 
            null, instance.generalVariables.colors["darkRed"])); // Shows background panel
        instance.objectsToClose.Add(instance.generalFunctions.ShowText(new Vector2(-165, -75), new Vector2(700, 250),
            phoneAFriendText, 60, TextAnchor.MiddleCenter, instance.generalVariables.colors["darkGreen"], true));
        // ^ Shows friend's response message
        instance.objectsToClose.Add(instance.generalFunctions.ShowButton(instance.objectsToClose[0],
            "OK", new Vector2(0, -110), new Vector2(250, 100), instance.generalVariables.colors["darkGreen"],
            instance.generalVariables.colors["lightRed"], 60, true, 30, Color.white, 0)); // Shows exit button

        Destroy(instance.lifelines[1]); // Removes phone a friend button
    }
    public static void AskTheAudience() {
        if (instance.lifelineBeingUsed) { // End function if lifeline is currently being processed
            return;
        }
        instance.lifelineBeingUsed = true; // Sets lifelineBeingUSed to true

        int chanceOfSuccess; // Percentage chance that audience get the right answer.
        bool topAnswer; // Determines if the audience will get the right answer or not.
        // In the first ten questions, there is an 80% chance of the audience selecting the correct answer as its highest percentage.
        // In the final five questions, there is a 35% chance of the audience slecting the correct answer.
        if (instance.currentScore < 10) {
            chanceOfSuccess = 80;
        }
        else {
            chanceOfSuccess = 35;
        }
        if (Random.Range(1, 101) <= chanceOfSuccess) { // Selects random value
            topAnswer = true; // Highest percentage will be the correct answer
        }
        else {
            topAnswer = false; // Highest percentage will be not be made to be the correct answer
        }
        List<int> chosenProbabilities = new List<int>(); // List of probabilites
        for (int i = 0; i < instance.answers.Count; i++) { // Iterates through number of answers
            int randomProbability = 0;
            if (i < instance.answers.Count - 1) { // If iterative index is not the final one
                randomProbability = Random.Range(0, 101 - chosenProbabilities.Sum());
                // ^ Selects random probability that won't carry the total probability over 100%
            }
            else { // Final probability must be equal to 100 minus the sum of the other probabilities to make the sum equal to 100%.
                randomProbability = 100 - chosenProbabilities.Sum();
            }
            chosenProbabilities.Add(randomProbability);
        }
        int maxProbability = chosenProbabilities.Max();
        
        List<int> finalProbabilities = new List<int>(); // List of probabilities in order
        while (true) {
            finalProbabilities = chosenProbabilities.OrderBy(x => Random.value).ToList(); // Randomises list of probabilities
            if (topAnswer && finalProbabilities[instance.answerIndices.IndexOf(instance.correctAnswerIndex)] != maxProbability) {
                // ^ Continues to next iteration if criteria is not fulfilled
                continue;
            }
            break;
        }
        
        // Shows bar chart
        instance.objectsToClose.Add(instance.generalFunctions.ShowImage(new Vector2(-150, -50), new Vector2(402, 575), new Vector2(1, 1),
            null, instance.generalVariables.colors["darkGrey"])); // Shows background for bar chart
        for (int i = 0; i < finalProbabilities.Count; i++) { // Iterates through number of probabilities
            float height = 350f * finalProbabilities[i] / 100; // Works out height of bar in pixels based on magnitude of probability
            instance.objectsToClose.Add(instance.generalFunctions.ShowImage(new Vector2(-300 + 100 * i, -150 + height / 2),
                new Vector2(50, height), new Vector2(1, 1), null, instance.generalVariables.colors["lightRed"])); // Shows bar
            instance.objectsToClose.Add(instance.generalFunctions.ShowText(new Vector2(-300 + 100 * i, -200),
                new Vector2(75, 100), instance.answers[i].ToString(), 60, TextAnchor.MiddleCenter,
                instance.generalVariables.colors["darkGreen"], true)); // Shows text under bar, showing the answer being represented
        }
        instance.objectsToClose.Add(instance.generalFunctions.ShowButton(instance.objectsToClose[0], "Press to continue game",
            new Vector2(0, -225), new Vector2(320, 75), instance.generalVariables.colors["darkGreen"],
            instance.generalVariables.colors["lightRed"], 60, true, 30, instance.generalVariables.colors["lightRed"], 3));
        // ^ Shows button to exit the ask the audience panel
        instance.objectsToClose[instance.objectsToClose.Count - 1].GetComponentInChildren<Text>().lineSpacing = 0.75f;
        // ^ Defines spacing between button and rest of panel

        Destroy(instance.lifelines[2]); // Removes ask the audience lifeline as an option
        instance.lifelines.RemoveAt(2); // Removes ask the audience lifeline from list of lifelines
    }
    public static void CloseObjects() {
        foreach (GameObject i in instance.objectsToClose) { // Iterates through list of objects to close
            Destroy(i); // Destroys the object
        }
        instance.objectsToClose.Clear(); // Clears list of objects to close
        instance.lifelineBeingUsed = false; // Sets lifelineBeingUsed to false
    }
    public static void TakeTheMoney() { // Called when the take the money lifeline is clicked
        if (instance.currentScore > 0) {
            instance.generalGameScript.score = instance.priceBrackets[instance.currentScore - 1]; // Determines the score in monetary terms
        }
        else {
            instance.generalGameScript.score = 0; // No questions right so user has got 0
        }
        GeneralGameScript.EndGame(); // Ends the game
    }
    public static void AnswerQuestion(GameObject buttonPressed) { // Called when one of the answer buttons is clicked
        if (buttonPressed.GetComponentInChildren<Text>().text == instance.correctAnswer.ToString()) {
            // ^ Checks if the text of the answer button clicked is the same as the correct answer
            instance.currentScore++; // Increments score
            instance.priceObjects[instance.currentScore - 1].GetComponent<Image>().color = instance.generalVariables.colors
                ["lightGreen"]; // Highlights current price object
            if (instance.currentScore > 1) { // If there is a previous score highlighted
                instance.priceObjects[instance.currentScore - 2].GetComponent<Image>().color = instance.generalVariables.
                    colors["darkGrey"]; // Set previous score to normal colour
            }
            if (instance.currentScore == 15) { // Reached the end of the game
                instance.StartCoroutine("GameWon"); // Calls EndGame();
                return; // Ends function
            }
            instance.NewQuestion(); // Ask new question as game isn't over yet
        }
        else {
            instance.EndGame(); // Calls EndGame because user has got the wrong answer
        }
    }
    void EndGame() {
        if (currentScore < 5) { // If user has made under £1000
            generalGameScript.score = 0; // Score is 0
        }
        else if (currentScore < 10) { // If user has made over £1000 but under £32000
            generalGameScript.score = priceBrackets[4]; // Score is £1000
        }
        else { // User has made over £32000 but under £1000000
            generalGameScript.score = priceBrackets[9]; // Score is £32000
        }
        GeneralGameScript.EndGame(); // Calls EndGame
    }
    IEnumerator GameWon() {
        // Creates particle system dropping virtual money notes from top of screen
        GameObject moneyFallingObject = new GameObject(); // Creates particle system object
        moneyFallingObject.GetComponent<Transform>().position = new Vector2(0, 10); // Defines position of object
        ParticleSystem particleSystem = moneyFallingObject.AddComponent<ParticleSystem>(); // Adds particle system to object
        var particleSystemVariables = particleSystem.main;
        particleSystemVariables.loop = true; // Makes the particle system continue until the scene is changed
        particleSystemVariables.startSpeed = 7.5f; // Sets starting speed of particles (money notes)
        particleSystemVariables.startSize = 2f; // Sets size of particles
        particleSystemVariables.gravityModifier = 0.1f; // Sets amount of gravitational acceleration on particles
        particleSystem.GetComponent<Renderer>().material = Resources.Load<Material>("MoneyParticle"); // Sets sprite of particle as money

        // Removes question, answers and lifeline objects
        Destroy(questionObject);
        foreach (GameObject answerObject in answerObjects) {
            Destroy(answerObject);
        }
        foreach (GameObject lifeline in lifelines) {
            Destroy(lifeline);
        }
        Destroy(questionPanel);
        Destroy(takeTheMoneySign);
        generalGameScript.timerObject.SetActive(false);
        StopCoroutine(currentCountdownCoroutine);

        generalGameScript.score = priceBrackets[currentScore - 1]; // Sets user's score based on price bracket
        yield return new WaitForSeconds(5f); // Waits for five seconds
        GeneralGameScript.EndGame(); // Calls EndGame
    }
}