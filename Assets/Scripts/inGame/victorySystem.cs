using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class victorySystem : MonoBehaviour
{
    [Header("Read Only Properties")]
    public string victoryOnePlayer;
    public GameObject ImagePlayerOne;
    public GameObject ImagePlayerTwo;

    private GameObject PlayerVictoryTextObj;
    private GameObject TimerTextObj;
    private TextMeshProUGUI PlayerVictoryText;
    private TextMeshProUGUI TimerText;

    private GameObject primarySystemObject;
    public primarySystem primarySystemScript;

    private bool canSkip;
    
    void Start()
    {
        canSkip = false;
        AssociateObjects();
        if (primarySystemScript.whichPlayerIsTheVictoryOne == 1)
        {
            ImagePlayerOne.SetActive(true);
            ImagePlayerTwo.SetActive(false);
            PlayerVictoryText.text = "Player One";
        }
        else if (primarySystemScript.whichPlayerIsTheVictoryOne == 2)
        {
            ImagePlayerOne.SetActive(false);
            ImagePlayerTwo.SetActive(true);
            PlayerVictoryText.text = "Player Two";
        }
        StartCoroutine(VictoryTimer());
    }
    
    void Update()
    {
        if (Input.anyKey && canSkip == true)
        {            
            primarySystemScript.SkipVictoryScreen();
        }
    }

    void AssociateObjects()
    {
        primarySystemObject = GameObject.FindWithTag("PrimarySystem");
        primarySystemScript = primarySystemObject.GetComponent<primarySystem>();

        ImagePlayerOne = GameObject.Find("ImagePlayerOne");
        ImagePlayerTwo = GameObject.Find("ImagePlayerTwo");

        PlayerVictoryTextObj = GameObject.Find("PlayerVictoryText");
        TimerTextObj = GameObject.Find("TimerText");

        PlayerVictoryText = PlayerVictoryTextObj.GetComponent<TextMeshProUGUI>();
        TimerText = TimerTextObj.GetComponent<TextMeshProUGUI>();

        primarySystemScript.AssociatePrimarySystemAudio();
    }

    IEnumerator VictoryTimer()
    {
        TimerText.text = "3";
        yield return new WaitForSeconds(1.0f);
        TimerText.text = "2";
        yield return new WaitForSeconds(1.0f);
        TimerText.text = "1";
        yield return new WaitForSeconds(1.0f);
        TimerText.text = "Pressione qualquer botão para sair";
        canSkip = true;
    }
}