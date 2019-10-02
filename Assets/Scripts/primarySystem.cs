using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class primarySystem : MonoBehaviour
{
    public GameObject audioSystemObj;
    public audioSystem audioSystemScript;

    public string playerVictoryScreenText;
    public string actualSceneName;
    public int situationSceneSystem = 0;

    public int whichPlayerIsTheVictoryOne;
    
    void Start()
    {        
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
        if (actualSceneName == "Menu" && situationSceneSystem == 0)
        {
            //Not included on Alpha
            //situationSceneSystem++;
        }  
        if (actualSceneName == "Game" && situationSceneSystem == 0)
        {

        }
    }

    public void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        actualSceneName = SceneManager.GetActiveScene().name;
    }

    public void ButtonIniciateGame()
    {
        audioSystemScript.PlayTheSound(1);
        audioSystemScript.StopTheSound(0);
        StartCoroutine(ToTheGame());
    }

    public void SkipVictoryScreen()
    {
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
    }    

    IEnumerator ToTheGame()
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("Game", LoadSceneMode.Single);
    }
}