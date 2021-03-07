using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;                 // Imported modules
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
public class GeneralFunctions : MonoBehaviour {

    System.Action[] functionsToCall = new System.Action[]{ // List of functions to be called
        /* 0*/() => { },
        /* 1*/() => {OnBackButtonClick();},
        /* 2*/() => {LoadingSceneScript.CloseApplication(); },
        /* 3*/() => {StartSceneScript.GoToNextScene(EventSystem.current.currentSelectedGameObject.name); },
        /* 4*/() => {CreateAccountSceneScript.CreateAccount(); },
        /* 5*/() => {CreateAccountSceneScript.GoToNextScene(); },
        /* 6*/() => {LoginSceneScript.Login(); },
        /* 7*/() => {LoginSceneScript.GoToNextSceneOnClick(); },
        /* 8*/() => {TeacherMainMenuSceneScript.GoToNextScene(EventSystem.current.currentSelectedGameObject.name); },
        /* 9*/() => {TeacherAddClassSceneScript.SelectStudent(EventSystem.current.currentSelectedGameObject.name); },
        /*10*/() => {TeacherAddClassSceneScript.AddClass(); },
        /*11*/() => {TeacherAddClassSceneScript.GoToNextScene(); },
        /*12*/() => {StudentMainMenuSceneScript.GoToNextScene(EventSystem.current.currentSelectedGameObject.name); },
        /*13*/() => {StudentChooseGameSceneScript.MoveGameButtons(EventSystem.current.currentSelectedGameObject.name); },
        /*14*/() => {StudentChooseGameSceneScript.GoToNextScene(EventSystem.current.currentSelectedGameObject.name); },
        /*15*/() => {GameLinearSpaceshipWarsScript.StartGame(); },
        /*16*/() => {GameLinearSpaceshipWarsScript.FireLaser(); },
        /*17*/() => {GameLinearSpaceshipWarsScript.ContinueGame(); },
        /*18*/() => {EndGameSceneScript.GoToNextSceneOnClick(EventSystem.current.currentSelectedGameObject.name); },
        /*19*/() => {TeacherViewClassesSceneScript.GoToNextScene(EventSystem.current.currentSelectedGameObject.name); },
        /*20*/() => {TeacherHomeworkMenuSceneScript.GoToNextScene(EventSystem.current.currentSelectedGameObject.name); },
        /*21*/() => {TeacherSetHomeworkSceneScript.SetHomework(); },
        /*22*/() => {TeacherSetHomeworkSceneScript.GoToViewHomeworksScene(); },
        /*23*/() => {GameSpeedMathsSceneScript.StartGame(); },
        /*24*/() => {GameSpeedMathsSceneScript.AnswerQuestion(EventSystem.current.currentSelectedGameObject); },
        /*25*/() => {GameAlgebraMillionaireSceneScript.FiftyFifty(); },
        /*26*/() => {GameAlgebraMillionaireSceneScript.PhoneAFriend(); },
        /*27*/() => {GameAlgebraMillionaireSceneScript.AskTheAudience(); },
        /*28*/() => {GameAlgebraMillionaireSceneScript.TakeTheMoney(); },
        /*29*/() => {GameAlgebraMillionaireSceneScript.AnswerQuestion(EventSystem.current.currentSelectedGameObject); },
        /*30*/() => {GameAlgebraMillionaireSceneScript.CloseObjects(); },
    };

    GeneralVariables generalVariables; // Link to general variables script

    void Start() {
        generalVariables = transform.GetComponent<GeneralVariables>();
    }

    public IEnumerator GetClassOptions() {
        string url; // The url to make request to
        generalVariables.classIDs.Clear();   // Clears list of classes
        generalVariables.classNames.Clear(); //          ^
        if (generalVariables.accountType == "Teacher") { // Checks if user is a teacher
            url = "alevelproject2019.000webhostapp.com/getclassesscript.php?" +
                "getclassesteacher=true&" +
                "teacherusername=" + generalVariables.username;  // Defines teacher-specific url
        }
        else {
            url = "alevelproject2019.000webhostapp.com/getclassesscript.php?" +
                "getclassesstudent=true&" +
                "studentusername=" + generalVariables.username; // Defines student-specific url
        }

        UnityWebRequest webpage = UnityWebRequest.Get(url);

        yield return webpage.SendWebRequest(); // Makes request to webpage. Waits for response
        string downloadData = webpage.downloadHandler.text; // Data retrieved from website
        string[] splitData = downloadData.Split('|');       // Data retrieved, split by class
        for (int i = 0; i < splitData.Length - 1; i++) { // Iterate through class data
            generalVariables.classIDs.Add(splitData[i].Split(',')[0]);   // Add id to list of class ids
            generalVariables.classNames.Add(splitData[i].Split(',')[1]); // Add name to list of class names
        }
    }

    public static void OnBackButtonClick() {
        if (GeneralVariables.sceneOrder.Count == 0) {
            return;
        }
        if (GameObject.Find("UI Box")) {
            return;
        }
        int sceneToGoTo = GeneralVariables.sceneOrder[GeneralVariables.sceneOrder.Count - 1];
        // ^ Defines which scene to move to
        GeneralVariables.sceneOrder.RemoveAt(GeneralVariables.sceneOrder.Count - 1);
        // ^ Removes scene that the program is about to go to from sceneOrder
        SceneManager.LoadScene(sceneToGoTo); // Loads new scene
    }

    public GameObject ShowText(Vector2 textPosition, Vector2 textBoxSize, string text, int fontSize, TextAnchor alignment,
        Color32 textColor, bool bestFit) {
        GameObject textObject = new GameObject(); // Creates text object
        textObject.transform.SetParent(generalVariables.canvas.transform); // Sets the canvas as a parent of the text
        Text textComponent = textObject.AddComponent<Text>(); // Adds a text component
        RectTransform rectTransform = textObject.GetComponent<RectTransform>();
        // ^ Gets RectTransform component of textObject
        SetRectTransformValues(rectTransform, textPosition, textBoxSize, new Vector2(1, 1));
        // ^ Calls SetRectTransformValues which sets the position, size and scale of the text object
        SetTextValues(textComponent, text, generalVariables.arcenaFont, fontSize, alignment, textColor, bestFit);
        // ^ Calls SetTextValues which sets the text, font, font size, alignment and colour of the text
        textObject.layer = 5;
        return textObject;
    }

    public GameObject ShowInputField(Color32 boxColor, Vector2 inputFieldPosition, Vector2 inputFieldSize,
            string placeholderTextValue, bool bestFit, int fontSize, Color32 textColor, InputField.ContentType contentType,
            int characterLimit) {
        // Create input field object
        GameObject inputField = new GameObject();
        inputField.transform.SetParent(generalVariables.canvas.transform);
        inputField.layer = 5;

        Image inputFieldImage = inputField.AddComponent<Image>();
        inputFieldImage.color = boxColor;

        RectTransform inputFieldTransform = inputField.GetComponent<RectTransform>();
        SetRectTransformValues(inputFieldTransform, inputFieldPosition, inputFieldSize, new Vector2(1, 1));

        // Create placeholder child of input field
        GameObject placeholder = new GameObject();
        placeholder.transform.SetParent(inputField.transform);
        placeholder.layer = 5;
        RectTransform placeholderTransform = placeholder.AddComponent<RectTransform>();
        SetRectTransformValues(placeholderTransform, new Vector2(0, 0), inputFieldSize - new Vector2(20, 5),
            new Vector2(1, 1));
        Text placeholderText = placeholder.AddComponent<Text>();
        SetTextValues(placeholderText, placeholderTextValue, generalVariables.arcenaFont, fontSize, TextAnchor.MiddleLeft,
            textColor, bestFit);

        // Create text child of input field
        GameObject userTextObject = new GameObject();
        userTextObject.transform.SetParent(inputField.transform);
        userTextObject.layer = 5;
        RectTransform userTextTransform = userTextObject.AddComponent<RectTransform>();
        SetRectTransformValues(userTextTransform, new Vector2(0, 0), inputFieldSize - new Vector2(20, 5), new Vector2(1, 1));

        Text userText = userTextObject.AddComponent<Text>();
        SetTextValues(userText, "", generalVariables.arcenaFont, fontSize, TextAnchor.MiddleLeft, textColor, bestFit);
        userText.fontStyle = FontStyle.Italic;


        // Create input field component of input field object
        InputField inputFieldComponent = inputField.AddComponent<InputField>();
        inputFieldComponent.targetGraphic = inputFieldImage; // Sets input field image
        inputFieldComponent.textComponent = userText;        // Sets input field text
        inputFieldComponent.placeholder = placeholderText;   // Sets placeholder text
        inputFieldComponent.contentType = contentType;       // Sets input field type
        inputFieldComponent.characterLimit = characterLimit; // Sets max character limit
        return inputField;
    }
    public GameObject ShowImage(Vector2 position, Vector2 sizeDelta, Vector2 scale, Sprite sprite, Color32 color) {
        GameObject image = new GameObject();
        image.transform.SetParent(generalVariables.canvas.transform);
        RectTransform rectTransform = image.AddComponent<RectTransform>();
        SetRectTransformValues(rectTransform, position, sizeDelta, scale);

        Image imageComponent = image.AddComponent<Image>();
        if (sprite != null) { // Checks if the parameter sprite has a value
            imageComponent.sprite = sprite; // Asigns the sprite attribute
        }
        else {
            imageComponent.color = color; // Assigns the color attribute
        }
        return image;
    }
    
    public GameObject ShowUIBox(List<string> messages, int callFunctionNum, Vector2 position, Vector2 size) {
        if (GameObject.Find("UI Box")) { // Checks if a UI box already exists
            return GameObject.Find("UI Box");
        }
        // Create UI box object 
        GameObject uiBox = ShowImage(position, size, new Vector2(1, 1), null, generalVariables.colors["lightGreen"]);
        uiBox.name = "UI Box";
        // Create text child of UI box
        GameObject textChild = ShowText(position,size,string.Join("\n",messages),40,TextAnchor.MiddleCenter,
            generalVariables.colors["darkRed"],true);
        textChild.transform.SetParent(uiBox.transform);

        // Create button component of UI box
        Button button = uiBox.AddComponent<Button>();
        button.targetGraphic = uiBox.GetComponent<Image>();
        button.onClick.AddListener(delegate () { // Creates an event listener for when the box is clicked on
            Destroy(uiBox); // Destroys the UI box object
            functionsToCall[callFunctionNum]();
        });
        return uiBox;
    }

    public GameObject ShowButton(GameObject parent, string buttonText, Vector2 buttonPosition, Vector2 buttonSize,
                Color32 buttonColor, Color32 textColor, int fontSize, bool bestFit, int callFunctionNum,
                Color32 outlineColor, int outlineThickness) {
        // Create button object
        GameObject buttonObject = ShowImage(new Vector2(0,0), buttonSize, new Vector2(1, 1), null, buttonColor);
        buttonObject.transform.SetParent(parent.transform);
        buttonObject.GetComponent<RectTransform>().localPosition = buttonPosition;

        // Creates button text object
        GameObject buttonTextObject = ShowText(new Vector2(0,0), buttonSize - new Vector2(20, 5), buttonText, fontSize,
            TextAnchor.MiddleCenter, textColor, true);
        buttonTextObject.transform.SetParent(buttonObject.transform);
        buttonTextObject.GetComponent<RectTransform>().localPosition = new Vector2(0, 0);

        // Creates button component
        Button buttonComponent = buttonObject.AddComponent<Button>();
        buttonComponent.targetGraphic = buttonObject.GetComponent<Image>();
        buttonComponent.onClick.AddListener(delegate {  // Defines event listener for when button is clicked
            functionsToCall[callFunctionNum]();
        });

        // Creates outline component
        Outline buttonOutline = buttonObject.AddComponent<Outline>();
        buttonOutline.effectColor = outlineColor;
        buttonOutline.effectDistance = new Vector2(outlineThickness, -outlineThickness);
        return buttonObject;
    }
    public GameObject ShowSprite(Vector2 position, Sprite sprite) {
        GameObject spriteObject = new GameObject();
        spriteObject.transform.position = new Vector3(position.x, position.y, 0);
        SpriteRenderer spriteRenderer = spriteObject.AddComponent<SpriteRenderer>(); // Add sprite renderer component
        spriteRenderer.sprite = sprite; // Set sprite
        return spriteObject;
    }

    public void SetRectTransformValues(RectTransform rectTransform, Vector2 localPosition, Vector2 sizeDelta,
        Vector2 localScale) {
        rectTransform.localPosition = localPosition; // changes local position of object
        rectTransform.sizeDelta = sizeDelta; // changes width and height of object
        rectTransform.localScale = localScale; // changes scale relative to the object's parent
    }
    public void SetAnchors(RectTransform rectTransform, Vector2 minAnchor, Vector2 maxAnchor, Vector2 pivot) {
        rectTransform.anchorMin = minAnchor; //
        rectTransform.anchorMax = maxAnchor; // Set anchors and pivot of object
        rectTransform.pivot = pivot;         //
    }
    public void SetTextValues(Text textObject, string text, Font font, int fontSize, TextAnchor alignment, Color32 color,
        bool bestFit) {
        textObject.text = text; // sets text of object
        textObject.font = font; // sets font of text
        textObject.fontSize = fontSize; // sets font size of text
        textObject.alignment = alignment; // sets alignment value of text
        textObject.color = color; // sets colour of the text
        if (bestFit) { // Checks if the boolean parameter bestFit is true
            textObject.resizeTextForBestFit = true;
            textObject.resizeTextMinSize = 10;
            textObject.resizeTextMaxSize = fontSize;
        }
    }
    public GameObject[] ShowScrollRect(Vector2 panelPosition, Vector2 panelSize, Color32 color, Vector2 contentPosition,
        Vector2 contentSize) {

        // Create panel object
        GameObject panelObject = ShowImage(panelPosition, panelSize, new Vector2(1, 1), null, color);
        panelObject.layer = 5;
        // Create content object
        GameObject contentObject = new GameObject();
        contentObject.layer = 5;
        contentObject.transform.SetParent(panelObject.transform);
        RectTransform contentObjectTransform = contentObject.AddComponent<RectTransform>();
        SetRectTransformValues(contentObjectTransform, contentPosition, contentSize, new Vector2(1, 1));
        SetAnchors(contentObjectTransform, new Vector2(0.5f, 1), new Vector2(0.5f, 1), new Vector2(0.5f, 1));

        // Add scroll rect to panel
        ScrollRect scrollRect = panelObject.AddComponent<ScrollRect>();
        scrollRect.content = contentObjectTransform; // Sets content to content object
        scrollRect.horizontal = false; // Prevents horizontal scrolling
        scrollRect.vertical = true; // Allows vertical scrolling
        // Mask the panel
        panelObject.AddComponent<Mask>();

        return new GameObject[] { panelObject, contentObject };
    }
    public GameObject ShowImageButton(Vector2 imagePosition, Vector2 imageSize, Sprite sprite, Color32 color,
        int callFunctionNum) {
        // Create image object
        GameObject imageObject = ShowImage(imagePosition, imageSize, new Vector2(1, 1), sprite, color);
        imageObject.layer = 5;
        Button buttonComponent = imageObject.AddComponent<Button>();
        buttonComponent.targetGraphic = imageObject.GetComponent<Image>();
        buttonComponent.onClick.AddListener(delegate {
            functionsToCall[callFunctionNum]();
        });
        return imageObject;
    }
    /*void RunTests() {
        generalVariables.username = "TestUsernameTeacher";
        generalVariables.accountType = "Teacher";
        StartCoroutine("GetClassOptions");
        generalVariables.username = "TestUsernameStudent";
        generalVariables.accountType = "Student";
        StartCoroutine("GetClassOptions");
        OnBackButtonClick();
       
        ShowText(new Vector2(-100, 0), new Vector2(300, 75), "Good morning", 50, TextAnchor.MiddleLeft, Color.white,
            false);
        ShowText(new Vector2(50, 0), new Vector2(300, 75), "Good morning. How are you today", 50, TextAnchor.MiddleLeft,
            Color.white, false);
        ShowText(new Vector2(50, 0), new Vector2(300, 75), "Good morning. How are you today", 50, TextAnchor.MiddleLeft,
            Color.white, true);
        ShowInputField(Color.red, new Vector2(0, -150), new Vector2(420, 170), "Placeholder text", false, 40, Color.green,
            InputField.ContentType.Alphanumeric, 10);
        ShowInputField(Color.red, new Vector2(0, -150), new Vector2(420, 170), "Placeholder text", false, 40, Color.green,
            InputField.ContentType.IntegerNumber, 10);
        ShowImage(new Vector2(50, 50), new Vector2(100, 100), new Vector2(1, 1),
            Resources.Load<Sprite>("Home Screen Image 1"), Color.white);
        ShowUIBox(new List<string> { "Hello", "Hi" }, 0, new Vector2(-250, 0), new Vector2(500, 500));
        ShowUIBox(new List<string> { "Hello", "Hi" }, 0, new Vector2(250, 0), new Vector2(500, 500));
        ShowButton(generalVariables.canvas, "Good day", new Vector2(0, 0), new Vector2(600, 400), Color.blue, Color.red,
            40, true, 2, Color.white, 3);
        ShowSprite(new Vector2(3, 3), Resources.Load<Sprite>("Home Screen Image 2"));
        ShowScrollRect(new Vector2(0, 0), new Vector2(500, 500), Color.grey, new Vector2(0, 0), new Vector2(450, 450));
        ShowImageButton(new Vector2(0, 100), new Vector2(200, 200), Resources.Load<Sprite>("Home Screen Image 2"),
            Color.white,2);
    }*/
}