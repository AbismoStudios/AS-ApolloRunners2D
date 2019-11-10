using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class primarySystem : MonoBehaviour
{
    public GameObject audioSystemObj;
    public audioSystem audioSystemScript;

    public string playerVictoryScreenText;
    public string actualSceneName;
    public int situationSceneSystem = 0;

    public int whichPlayerIsTheVictoryOne;

    public Camera mainMenuCamera;

    [Header("Menu Objects")]    
    public Button bIniciar;
    public Button bTutorial;
    public Button bSair;

    public Image gameLogo;
    public Image tutorialImage;

    private bool tutorialScreen;    
    
    void Start()
    {
        Cursor.visible = false;
        tutorialScreen = false;
        playerVictoryScreenText = "";
        AssociatePrimarySystemAudio();        
    }

    public void AssociatePrimarySystemAudio()
    {
        audioSystemObj = GameObject.FindWithTag("CameraListener");
        audioSystemScript = audioSystemObj.GetComponent<audioSystem>();
        if (actualSceneName == "Menu")
        {
            audioSystemScript.AssociateGameAudio();
            //play menu music 
            audioSystemScript.PlayTheSound(0);
        }
        else if (actualSceneName == "Game")
        {
            audioSystemScript.AssociateGameAudio();
            //play ingame music
        }
        else if (actualSceneName == "VictoryScreen")
        {
            audioSystemScript.AssociateGameAudio();
        }
    }
   
    void Update()
    {
        if (actualSceneName == "Menu" && tutorialScreen == true)
        {
            if (Input.anyKey)
            {
                ChangeMenuImages(1);
            }
        }          

        if ((Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton9)))
        {
            Application.Quit();
        }         
    }

    public void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        actualSceneName = SceneManager.GetActiveScene().name;
    }

    public void ButtonIniciateGame()
    {
        if (tutorialScreen == false)
        {
            audioSystemScript.PlayTheSound(1);
            audioSystemScript.StopTheSound(0);
            StartCoroutine(MenuTimer(1));
        }        
    }

    public void ButtonTutorial()
    {    
        if (tutorialScreen == false)
        {
            audioSystemScript.PlayTheSound(1);
            StartCoroutine(MenuTimer(2));
        }        
    }

    public void ButtonSair()
    {
        if (tutorialScreen == false)
        {
            audioSystemScript.PlayTheSound(1);
            StartCoroutine(MenuTimer(3));
        }        
    }

    public void SkipVictoryScreen()
    {
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
    }
    
    void ChangeMenuImages  (int whichChance)
    {
        if (whichChance == 1)
        {
            bIniciar.interactable = true;
            bTutorial.interactable = true;
            bSair.interactable = true;
            tutorialScreen = false;

            Color tempColor = gameLogo.color;
            tempColor.a = 1;
            gameLogo.color = tempColor;

            Color tempColorTutorial = tutorialImage.color;
            tempColorTutorial.a = 0;
            tutorialImage.color = tempColorTutorial;

            bTutorial.Select();
        }
        else if (whichChance == 2)
        {
            bIniciar.interactable = false;
            bTutorial.interactable = false;
            bSair.interactable = false;
            tutorialScreen = true;

            Color tempColor = gameLogo.color;
            tempColor.a = 0;
            gameLogo.color = tempColor;

            Color tempColorTutorial = tutorialImage.color;
            tempColorTutorial.a = 1;
            tutorialImage.color = tempColorTutorial;
        }
    }

    IEnumerator MenuTimer(int whichButton)
    {
        yield return new WaitForSeconds(0.5f);
        if (whichButton == 1)
        {
            SceneManager.LoadScene("Game", LoadSceneMode.Single);
        }
        else if (whichButton == 2)
        {
            ChangeMenuImages(2);                       
        }
        else if (whichButton == 3)
        {
            Application.Quit();
        }        
    }
}