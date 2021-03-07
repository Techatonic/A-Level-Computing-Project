using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameLinearSpaceshipWarsScript : MonoBehaviour {
    // Create script reference variables
    GeneralFunctions generalFunctions;
    GeneralVariables generalVariables;
    static GameLinearSpaceshipWarsScript instance;
    GeneralGameScript generalGameScript;

    GameObject gunObject; // Variable for gun object
    GameObject alliedSpaceshipObject; // Variable for allied spaceship object
    List<GameObject> enemyShipObjects = new List<GameObject>(); // List of game objects for enemy ships
    GameObject scoreObject; // Variable for score object
    List<GameObject> coefficients = new List<GameObject>(); // Variable for list of coefficients
    int gameLength = 60; // Time limit for game


    void Start() {
        // Assigns script references
        generalFunctions = GameObject.Find("General Script Game Object").GetComponent<GeneralFunctions>();
        generalVariables = GameObject.Find("General Script Game Object").GetComponent<GeneralVariables>();
        generalGameScript = GameObject.Find("Scene Script Game Object").GetComponent<GeneralGameScript>();
        generalVariables.canvas = GameObject.Find("Canvas"); // Sets canvas equal to the gameobject named "Canvas"
        instance = GameObject.Find("Scene Script Game Object").GetComponent<GameLinearSpaceshipWarsScript>();
        ShowUI(); // Calls ShowUI
    }

    void ShowUI() {
        // Show Grid
        for (float xAndY = -25; xAndY <= 25; xAndY += 5) { // Iterates through axis values
            // X Axis
            GameObject newObject = GameObject.CreatePrimitive(PrimitiveType.Cube); // Creates cube
            Transform transform = newObject.GetComponent<Transform>();
            transform.position = new Vector3(xAndY, 0, 0.1f); // Assigns position of cube
            transform.localScale = new Vector3(0.5f, 50, 0.01f); // Assigns size of cube
            newObject.GetComponent<Renderer>().material.color = new Color32(30, 30, 30, 100); // Assigns colour of cube

            GameObject textObject = new GameObject(); // Creates text object to display coordinate
            textObject.transform.position = new Vector2(xAndY + 0.5f, -0.5f); // Assigns position
            textObject.transform.localScale = new Vector2(1, 1); // Assigns size of cube
            TextMesh text = textObject.AddComponent<TextMesh>();
            text.text = xAndY.ToString();
            text.fontSize = 10;

            if (xAndY == 0) { // If it's the central axis, make it thicker
                transform.localScale = new Vector3(1, 50, 0.01f);
                textObject.transform.position = new Vector2(xAndY + 0.75f, -0.5f); // Move text to make way for thicker line
            }

            // Y Axis
            GameObject newObjectY = GameObject.CreatePrimitive(PrimitiveType.Cube); // Creates cube
            Transform transformY = newObjectY.GetComponent<Transform>();
            transformY.position = new Vector3(0, xAndY, 0.1f); // Assigns position of cube
            transformY.localScale = new Vector3(50, 0.5f, 0.01f); // Assigns size of cube
            if (xAndY == 0) { // If it's the central axis, make it thicker
                transformY.localScale = new Vector3(50, 1, 0.01f);
            }
            newObjectY.GetComponent<Renderer>().material.color = new Color32(30, 30, 30, 100); // Assigns colour of cube

            GameObject textObjectY = new GameObject(); // Creates text object to display coordinate
            TextMesh textY = textObjectY.AddComponent<TextMesh>();
            textY.anchor = TextAnchor.UpperRight;
            textObjectY.transform.position = new Vector2(-1, xAndY - 0.5f); // Assigns position of text object
            textObjectY.transform.localScale = new Vector2(1, 1); // Assigns size of text object
            textY.text = xAndY.ToString();
            textY.fontSize = 10;
        }
        // Create gun object
        gunObject = generalFunctions.ShowSprite(new Vector2(0, 0), Resources.Load<Sprite>("Space Gun"));
        gunObject.AddComponent<LineRenderer>(); // Adds component to be used for firing lasers

        // Create allied spaceship object
        alliedSpaceshipObject = generalFunctions.ShowSprite(new Vector2(0, 0), Resources.Load<Sprite>("Allied Spaceship"));

        // Create GUI - right hand side
        generalGameScript.timerObject = generalFunctions.ShowText(new Vector2(400, 250), new Vector2(300, 100),
            gameLength.ToString() + ":00", 100, TextAnchor.MiddleLeft, generalVariables.colors["lightRed"], true);
        // Show Score
        scoreObject = generalFunctions.ShowText(new Vector2(350, 125), new Vector2(300, 100), "Score: 0", 75,
            TextAnchor.MiddleCenter, generalVariables.colors["lightRed"], true);

        // Show Equation
        generalFunctions.ShowText(new Vector2(150, 0), new Vector2(100, 100), "y = ", 75, TextAnchor.MiddleCenter,
            generalVariables.colors["lightRed"], false);
        coefficients.Add(generalFunctions.ShowInputField(generalVariables.colors["lightRed"], new Vector2(260, -5),
            new Vector2(125, 75),"",false,50,generalVariables.colors["lightGreen"],InputField.ContentType.DecimalNumber,5));
        generalFunctions.ShowText(new Vector2(390, 0), new Vector2(100, 100), "x + ", 75, TextAnchor.MiddleCenter,
            generalVariables.colors["lightRed"], false);
        coefficients.Add(generalFunctions.ShowInputField(generalVariables.colors["lightRed"], new Vector2(495, -5),
            new Vector2(125, 75),"",false,50,generalVariables.colors["lightGreen"],InputField.ContentType.DecimalNumber,5));
        // Show Fire Laser button
        generalFunctions.ShowButton(generalVariables.canvas, "Fire Laser", new Vector2(350, -200), new Vector2(200, 100),
            generalVariables.colors["lightRed"], generalVariables.colors["lightGreen"], 50, true, 16,
            generalVariables.colors["darkGreen"], 6);

        // Shows instructions as UI box
        generalFunctions.ShowUIBox(new List<string>{
            "Defend you spaceship by shooting lasers from the gun to destroy the enemy spaceships.",
            "The direction of the lasers is dependent on the linear equation you enter.",
            "Destroy as many enemy spaceships as possible in 120 seconds.",
            "If an enemy spaceship reaches your spaceship, it's game over.",
            "Click on this box to start the game."
            },
            15, new Vector2(325, -50), new Vector2(470, 500));
    }
    public static void StartGame() {
        instance.StartCoroutine(instance.generalGameScript.StartCountdown(instance.gameLength, 0)); // Starts countdown
        instance.InvokeRepeating("CreateEnemy", 1f, 5f); // Repeatedly calls CreateEnemy every 5 seconds
    }
    void CreateEnemy() {
        Vector2 enemyStartPosition; // Create Vector2 for the new enemy's starting position
        float randomPosition = Random.Range(-25.0f, 25.0f); // Creates random position along axis
        int posOrNeg = Random.Range(0, 2) * 50 - 25;
        // ^ Randomly decides whether to be positioned on positive or negative axis (output is either -25 or 25)
        int xOrY = Random.Range(0, 2); // Randomly decides whether the position should be in the corner of the x or y axis
        if (xOrY == 0) { // If xOrY decides to be placed with a constant y position where |y| = 25
            enemyStartPosition = new Vector2(randomPosition, posOrNeg); // Defines start position
        }
        else {
            enemyStartPosition = new Vector2(posOrNeg, randomPosition); // Defines start position
        }
        GameObject newEnemy = generalFunctions.ShowSprite(enemyStartPosition, Resources.Load<Sprite>("Enemy Spaceship"));
        // ^ Creates new enemy object
        newEnemy.transform.up = alliedSpaceshipObject.transform.position - newEnemy.transform.position;
        // ^ Rotates enemy and points it towards the allied spaceship
        newEnemy.tag = "Enemy"; // Adds Enemy tag to object
        newEnemy.AddComponent<BoxCollider2D>(); // Adds collider component
        enemyShipObjects.Add(newEnemy); // Adds object to list
    }
    void Update() {
        foreach (GameObject enemyShip in enemyShipObjects) { // Iterates through enemy ships
            enemyShip.transform.position = Vector3.MoveTowards(enemyShip.transform.position,
                alliedSpaceshipObject.transform.position, Time.deltaTime); // Moves ship towards allied spaceship
            if(enemyShip.transform.position==alliedSpaceshipObject.transform.position){//If the enemy ship reaches the allies
                GeneralGameScript.EndGame(); // End the game
            }
        }
    }
    public static void FireLaser() {
        // Retrieve user inputs
        string gradientStr = instance.coefficients[0].GetComponent<InputField>().text;
        string yInterceptStr = instance.coefficients[1].GetComponent<InputField>().text;
        if (yInterceptStr == "") { // If user hasn't inputted a value for y-intercept, set it to 0 by default
            yInterceptStr = "0";
        }
        List<string> errors = instance.ValidateInputs(gradientStr, yInterceptStr); // Check for errors in inputs
        if (errors.Count > 0) { // If there are any errors
            Time.timeScale = 0; // Pause game
            instance.generalFunctions.ShowUIBox(errors, 17, new Vector2(325, -50),
                new Vector2(470, 500)); // Show error message
            return;
        }

        float gradient = float.Parse(gradientStr); // Convert gradient to float value
        float yIntercept = float.Parse(yInterceptStr); // Convert y-intercept to float value
        float startYPos = -25 * gradient + yIntercept; // Get start y-position
        float endYPos = 25 * gradient + yIntercept; // Get end y-position

        instance.gunObject.transform.position = new Vector2(0, yIntercept); // Move gun object

        LineRenderer lineRenderer = instance.gunObject.GetComponent<LineRenderer>(); // Get line component
        lineRenderer.positionCount = 3; // Add components to line
        Vector3 gunPos = new Vector3(-0.6f, 0, 0); // Move line position relative to gun object
        if (gradient < 0) {
            gunPos.x *= -1; // Flip relative position of line if graident is negative
        }
        lineRenderer.SetPositions(new Vector3[]{ // Create line going from left to right
            new Vector3(-25, startYPos, 0),
            instance.gunObject.transform.position + gunPos, 
                                                // ^ Moves it from center to where the bullet hole is
            new Vector3(25, endYPos, 0),
        });
        lineRenderer.startWidth = 0.3f; // Assign start width of line
        lineRenderer.endWidth = 0.3f; // Assign end width of line
        Material newMaterial = new Material(Shader.Find("Standard"));
        newMaterial.color = Color.red; // Assign colour of line
        lineRenderer.enabled = true;
        instance.Invoke("TurnOffLaser", 0.25f); // Call TurnOffLaser in short period of time

        instance.gunObject.transform.up = new Vector3(25, endYPos, 0) - instance.gunObject.transform.position;
        // Rotates gun object towards firing position

        instance.CheckKill(lineRenderer); // Call CheckKill
    }
    List<string> ValidateInputs(string gradient, string yIntercept) {
        List<string> errors = new List<string>();
        if (gradient == "") { // If gradient field is empty
            errors.Add("Field cannot be empty. If you want a value of zero, type 0."); // Add error message
        }
        if (gradient != "") {
            if (float.Parse(gradient) > -0.1f && float.Parse(gradient) < 0.1f) { // If value of gradient is zero
                errors.Add("Gradient cannot be equal to 0."); // Add error message
            }
        }
        if (yIntercept != "") {
            float intercept = float.Parse(yIntercept);
            if (intercept < -25f || intercept > 25f) { // If y-intercept value is out of range
                errors.Add("Intercept value out of bounds."); // Add error message
            }
        }
        return errors; // Return errors
    }
    public static void ContinueGame() {
        Time.timeScale = 1; // Continue the game by changing speed back to 1
    }
    void CheckKill(LineRenderer lineRenderer) {
        RaycastHit2D hitInfo = Physics2D.Linecast(lineRenderer.GetPosition(0), lineRenderer.GetPosition(2));
        // ^ Draws a virtual line between the beginning and end of laser
        if (hitInfo.collider != null) { // If something was 'hit' by laser
            if (hitInfo.collider.tag == "Enemy") { // If object hit was an enemy
                enemyShipObjects.Remove(hitInfo.transform.gameObject); // Remove enemy ship from list of ships
                Destroy(hitInfo.transform.gameObject); // Destroy ship
                generalGameScript.score++; // Add score by one
                scoreObject.GetComponent<Text>().text = "Score: " + generalGameScript.score.ToString();
                // ^ Update score object
            }
        }
    }
    void TurnOffLaser() {
        gunObject.GetComponent<LineRenderer>().enabled = false; // Hides laser
    }
}