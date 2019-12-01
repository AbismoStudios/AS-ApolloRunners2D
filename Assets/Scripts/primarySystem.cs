using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    public Image tutorialImagePT;
    public Image tutorialImageEN;

    private bool tutorialScreen;
    public string whichLang = "EN";

    public TextMeshProUGUI btnStartText;    
    public TextMeshProUGUI btnQuitText;
    public TextMeshProUGUI btnLangText;

    void Start()
    {
        Cursor.visible = false;
        tutorialScreen = false;
        whichLang = "EN";
        ChangeLanguage();
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

    public void ButtonLang()
    {
        audioSystemScript.PlayTheSound(1);
        StartCoroutine(MenuTimer(4));
    }

    void CheckLanguage()
    {
        if (whichLang == "EN")
        {
            whichLang = "PT";
        }
        else if (whichLang == "PT")
        {
            whichLang = "EN";
        }
        ChangeLanguage();
    }

    void ChangeLanguage()
    {
        if (whichLang == "EN")
        {
            btnStartText.text = "Start";
            btnQuitText.text = "Quit";
            btnLangText.text = "EN";
        }
        else if (whichLang == "PT")
        {
            btnStartText.text = "Iniciar";
            btnQuitText.text = "Sair";
            btnLangText.text = "PT";
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

            if (whichLang == "PT")
            {
                Color tempColorTutorial = tutorialImagePT.color;
                tempColorTutorial.a = 0;
                tutorialImagePT.color = tempColorTutorial;
            }
            else if (whichLang == "EN")
            {
                Color tempColorTutorial = tutorialImageEN.color;
                tempColorTutorial.a = 0;
                tutorialImageEN.color = tempColorTutorial;
            }

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

            if (whichLang == "PT")
            {
                Color tempColorTutorial = tutorialImagePT.color;
                tempColorTutorial.a = 1;
                tutorialImagePT.color = tempColorTutorial;
            }
            else if (whichLang == "EN")
            {
                Color tempColorTutorial = tutorialImageEN.color;
                tempColorTutorial.a = 1;
                tutorialImageEN.color = tempColorTutorial;
            }            
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
        else if (whichButton == 4)
        {
            CheckLanguage();
        }
    }
}