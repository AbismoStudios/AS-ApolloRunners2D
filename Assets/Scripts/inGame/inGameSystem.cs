using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class inGameSystem : MonoBehaviour
{
    private GameObject playerOneShip;
    private GameObject playerTwoShip;

    public playerController playerOneScript;
    public playerController playerTwoScript;

    public perkSystem playerOnePerkSystemScript;
    public perkSystem playerTwoPerkSystemScript;

    private GameObject primarySystemObject;
    public primarySystem primarySystemScript;

    private GameObject audioSystemObj;
    public audioSystem audioSystemScript;    

    private GameObject botCollider;
    private GameObject topCollider;
    public float colliderPossitionY = 50.0f;

    public Vector3 posPlayerOne;
    public Vector3 posPlayerTwo;
    private float distanceBetween;
    private float meanBetweenShips;   

    public float necessaryDistance = 2000.0f;
    private float necessaryDistanceSaver;

    public int playerOneScore = 0;
    public int playerTwoScore = 0;

    public float playerTimerOptionInMin = 2.0f;
    private float totalTimerSeconds;
    private float playerTimerSaver;
    private bool suddenDeathTimer = false;

    private GameObject SSTimer;
    private GameObject SSNecessaryDistance;
    private GameObject SSScorePOne;
    private GameObject SSScorePTwo;

    private GameObject BCTimer;
    private GameObject BCScoreOne;
    private GameObject BCScoreTwo;

    private GameObject BothCameraTextObj;
    private GameObject BlackBackground;
    private int iniciate;

    private TextMeshProUGUI SSTimerText;
    private TextMeshProUGUI SSNecessaryDistanceText;
    private TextMeshProUGUI SSScorePOneText;
    private TextMeshProUGUI SSScorePTwoText;

    private TextMeshProUGUI BCTimerText;
    private TextMeshProUGUI BCScoreOneText;
    private TextMeshProUGUI BCScoreTwoText;

    public TextMeshProUGUI BothCameraText;

    public GameObject bothCameraObj;
    public trackCamera bothCameraScript;

    private int winnerPlayer = 0;
    private bool playingMatch = true;
    public bool someoneBurned = false;
    private bool someoneWon = false;
    
    void Start()
    {
        //AssociateSystem
        primarySystemObject = GameObject.FindWithTag("PrimarySystem");
        primarySystemScript = primarySystemObject.GetComponent<primarySystem>();

        audioSystemObj = GameObject.FindWithTag("CameraListener");
        audioSystemScript = audioSystemObj.GetComponent<audioSystem>();

        colliderPossitionY = 50.0f;

        AssociateShips();
        AssociateTexts();
        CheckSandbox();
        CalculateScore();

        primarySystemScript.AssociatePrimarySystemAudio();

        //distancia da partida sem sandbox
        necessaryDistance = 2000.0f;


        playerTimerSaver = playerTimerOptionInMin;
        necessaryDistanceSaver = necessaryDistance;
        totalTimerSeconds = 0.0f;
        iniciate = 0;

        BothCameraText.text = "";
        SSNecessaryDistanceText.text = "-- " + Math.Round(necessaryDistance,0).ToString() + " --";        
    }

    void MatchStart()
    {
        bothCameraScript.ResetOffset(posPlayerOne, posPlayerTwo);
        someoneBurned = false;
        someoneWon = false;
        StartCoroutine(GameTimerDisplay()); //MatchTimer
        playerOnePerkSystemScript.CallPerkTimer();
        playerTwoPerkSystemScript.CallPerkTimer();        
    }

    void MatchReStart()
    {
        //change player controller script
        playerOneScript.ResetForReMatch();
        playerTwoScript.ResetForReMatch();

        bothCameraScript.ResetOffset(posPlayerOne, posPlayerTwo);

        //reset MatchTimer
        StopCoroutine(GameTimerDisplay());
        suddenDeathTimer = false;
        totalTimerSeconds = 0.0f;
        playerTimerOptionInMin = playerTimerSaver;
        necessaryDistance = necessaryDistanceSaver;        
        playingMatch = true;        
        StartCoroutine(TimerOfRaceStart());
    }

    void CallVictoryScreen(int victoryOne)
    {
        BothCameraText.text = "";
        audioSystemScript.StopTheSound(0);
        if (victoryOne == 1)
        {
            primarySystemScript.whichPlayerIsTheVictoryOne = 1;
            SceneManager.LoadScene("VictoryScreen", LoadSceneMode.Single);            
        }
        else if (victoryOne == 2)
        {
            primarySystemScript.whichPlayerIsTheVictoryOne = 2;
            SceneManager.LoadScene("VictoryScreen", LoadSceneMode.Single);            
        }
        else
        {
            FatalError();
        }
    }
    
    void Update()
    {
        if (Input.anyKey && iniciate == 0)
        {            
            BlackBackground.SetActive(false);
            StartCoroutine(TimerOfRaceStart());
            iniciate++;
        }
    }

    void FixedUpdate()
    {
        if (playingMatch == true)
        {
            DistanceForVictory();
        }        
        CalculateColliders();        
    }

    void AssociateShips()
    {
        playerOneShip = GameObject.FindWithTag("PlayerOne");
        playerTwoShip = GameObject.FindWithTag("PlayerTwo");

        playerOneScript = playerOneShip.GetComponent<playerController>();
        playerTwoScript = playerTwoShip.GetComponent<playerController>();

        playerOnePerkSystemScript = playerOneShip.GetComponent<perkSystem>();
        playerTwoPerkSystemScript = playerTwoShip.GetComponent<perkSystem>();
        

        botCollider = GameObject.Find("BotCollider");
        topCollider = GameObject.Find("TopCollider");        
    }

    void DistanceForVictory()
    {
        posPlayerOne = playerOneShip.transform.position;
        posPlayerTwo = playerTwoShip.transform.position;
        if (posPlayerOne.x >= posPlayerTwo.x)
        {
            distanceBetween = posPlayerOne.x - posPlayerTwo.x;
            if (distanceBetween >= necessaryDistance)
            {
                //player one victory                
                winnerPlayer = 1;
                EndOfMatch(0);
            }
        }
        else
        {
            distanceBetween = posPlayerTwo.x - posPlayerOne.x;
            if (distanceBetween >= necessaryDistance)
            {
                //player two victory
                winnerPlayer = 2;
                EndOfMatch(0);
            }
        }
    }

    void CalculateColliders()
    {
        meanBetweenShips = (posPlayerOne.x + posPlayerTwo.x) / 2;

        topCollider.transform.position = new Vector3(meanBetweenShips, colliderPossitionY, 0.0f);
        botCollider.transform.position = new Vector3(meanBetweenShips, -colliderPossitionY, 0.0f);

        topCollider.transform.localScale = new Vector3((distanceBetween + 100.0f), 1.0f, 3.0f);
        botCollider.transform.localScale = new Vector3((distanceBetween + 100.0f), 1.0f, 3.0f);        
    }

    void CheckSandbox()
    {
        //Not included on this version
    }

    public void EndOfMatch(int whichOneBurned)
    {
        playingMatch = false;
        someoneWon = true;
        
        //stop timer
        Time.timeScale = 0.5f;
        Time.fixedDeltaTime = 0.02F * Time.timeScale;

        if (someoneBurned == true && whichOneBurned != 0)
        {
            if (whichOneBurned == 1)
            {
                winnerPlayer = 2;
            }
            else if (whichOneBurned == 2)
            {
                winnerPlayer = 1;
            }
        }
        
        if (winnerPlayer == 1)
        {
            //call victory screen on player 1
            playerOneScore++;            
        }
        else if (winnerPlayer == 2)
        {
            //call victory screen on player 2
            playerTwoScore++;            
        }
        else
        {
            FatalError();
        }
        CallForStopAceleration();
        StopCoroutine(GameTimerDisplay());
        NextMatch();
    }

    public void NextMatch()
    {
        Time.timeScale = 1;
        Time.fixedDeltaTime = 0.02F;
        //call new match
        CalculateScore();

    }

    void CallForStopAceleration()
    {
        playerOneScript.CallForStopAcelerationOfTheShip();
        playerTwoScript.CallForStopAcelerationOfTheShip();
    }

    IEnumerator TimerOfRaceStart()
    {
        while (true)
        {            
            yield return new WaitForSeconds(1.0f);
            //3
            bothCameraScript.ResetOffset(posPlayerOne, posPlayerTwo);
            BothCameraTextObj.transform.DOScale(2.0f, 0.0f);
            BothCameraTextObj.transform.DOScale(1.0f, 1.0f);
            BothCameraText.text = "3";
            audioSystemScript.PlayTheSound(1);
            someoneBurned = false;
            yield return new WaitForSeconds(1.0f);
            //2
            BothCameraTextObj.transform.DOScale(2.0f, 0.0f);
            BothCameraTextObj.transform.DOScale(1.0f, 1.0f);
            BothCameraText.text = "2";
            audioSystemScript.PlayTheSound(1);
            yield return new WaitForSeconds(1.0f);
            //1
            BothCameraTextObj.transform.DOScale(2.0f, 0.0f);
            BothCameraTextObj.transform.DOScale(1.0f, 1.0f);
            BothCameraText.text = "1";
            audioSystemScript.PlayTheSound(1);
            //liberar player
            playerOneScript.playable = true;
            playerTwoScript.playable = true;
            yield return new WaitForSeconds(1.0f);

            if (someoneBurned == true)
            {
                audioSystemScript.PlayTheSound(3);
                break;
            }
            else
            {
                //GO
                BothCameraText.text = "GO";
                audioSystemScript.PlayTheSound(2);
                audioSystemScript.PlayTheSound(0);
                playerOneScript.modeAceleration = 5.0f;
                playerTwoScript.modeAceleration = 5.0f;
                playerOneScript.canIniciate = true;
                playerTwoScript.canIniciate = true;
                MatchStart();
                yield return new WaitForSeconds(0.2f);
                //pos iniciate        
                playerOneScript.modeAceleration = playerOneScript.shipX;
                playerTwoScript.modeAceleration = playerTwoScript.shipX;
                //mudar velocidade \ true start
                yield return new WaitForSeconds(0.3f);
                BothCameraText.text = "";
                break;
            }            
        }
    }

    //Texts elements
    void AssociateTexts()
    {
        SSTimer = GameObject.Find("SSTimer");
        SSNecessaryDistance = GameObject.Find("SSNecessaryDistance");
        SSScorePOne = GameObject.Find("SSScorePOne");
        SSScorePTwo = GameObject.Find("SSScorePTwo");

        BothCameraTextObj = GameObject.Find("BothCameraText");
        BCScoreOne = GameObject.Find("BCScorePOne");
        BCScoreTwo = GameObject.Find("BCScorePTwo");
        BCTimer = GameObject.Find("BCMatchTimer");

        SSTimerText = SSTimer.GetComponent<TextMeshProUGUI>();
        SSNecessaryDistanceText = SSNecessaryDistance.GetComponent<TextMeshProUGUI>();
        SSScorePOneText = SSScorePOne.GetComponent<TextMeshProUGUI>();
        SSScorePTwoText = SSScorePTwo.GetComponent<TextMeshProUGUI>();

        BothCameraText = BothCameraTextObj.GetComponent<TextMeshProUGUI>();
        BCScoreOneText = BCScoreOne.GetComponent<TextMeshProUGUI>();
        BCScoreTwoText = BCScoreTwo.GetComponent<TextMeshProUGUI>();
        BCTimerText = BCTimer.GetComponent<TextMeshProUGUI>();

        BlackBackground = GameObject.Find("BlackBackground");

        bothCameraObj = GameObject.FindGameObjectWithTag("CameraListener");
        bothCameraScript = bothCameraObj.GetComponent<trackCamera>();
    }

    void CalculateScore()
    {
        if (playerOneScore != 2 && playerTwoScore != 2)
        {
            //change text
            SSScorePOneText.text = playerOneScore.ToString();
            SSScorePTwoText.text = playerTwoScore.ToString();

            BCScoreOneText.text = playerOneScore.ToString();
            BCScoreTwoText.text = playerTwoScore.ToString();
            //next match
            if (playerOneScore > 0 || playerTwoScore > 0)
            {
                MatchReStart();
            }
        }
        else
        {   
            if (playerOneScore == 2)
            {
                CallVictoryScreen(1);
            }
            else if (playerTwoScore == 2)
            {
                CallVictoryScreen(2);
            }            
        }            
    }

    IEnumerator GameTimerDisplay()
    {        
        while (true)
        {
            if (totalTimerSeconds == 0 && playerTimerOptionInMin != 0)
            {
                playerTimerOptionInMin -= 1.0f;
                totalTimerSeconds = 59.0f;
            }
            else
            {
                totalTimerSeconds -= 1.0f;
            }

            if (totalTimerSeconds < 10)
            {
                SSTimerText.text = "0" + Math.Round(playerTimerOptionInMin, 0).ToString() + ":0" + Math.Round(totalTimerSeconds, 0).ToString();
                BCTimerText.text = "0" + Math.Round(playerTimerOptionInMin, 0).ToString() + ":0" + Math.Round(totalTimerSeconds, 0).ToString();
            }
            else
            {
                SSTimerText.text = "0" + Math.Round(playerTimerOptionInMin, 0).ToString() + ":" + Math.Round(totalTimerSeconds, 0).ToString();
                BCTimerText.text = "0" + Math.Round(playerTimerOptionInMin, 0).ToString() + ":" + Math.Round(totalTimerSeconds, 0).ToString();
            }

            if ((playerTimerSaver / 2) == playerTimerOptionInMin && totalTimerSeconds == 0)
            {                
                suddenDeathTimer = true;
            }       

            if (suddenDeathTimer == true)
            {
                necessaryDistance = necessaryDistance - ((necessaryDistance / 100) * 10);
                SSNecessaryDistanceText.text = "-- " + Math.Round(necessaryDistance, 0).ToString() + " --";
            }

            if (totalTimerSeconds == 0 && playerTimerOptionInMin == 0)
            {
                suddenDeathTimer = false;
                break;                
            }

            if (someoneWon == true)
            {
                break;
            }

            yield return new WaitForSeconds(1.0f);            
        }
    }  

    //PerkAssociation
    public void CallForThePerkPrefab(int whatIsHappening)
    {
        if (whatIsHappening == 1)
        {
            //desacelerar player two
            playerTwoScript.modeAceleration = 1;
        }
        else if (whatIsHappening == 2)
        {
            //desacelerar player one
            playerOneScript.modeAceleration = 1;
        }
        else if (whatIsHappening == 3)
        {
            playerTwoScript.modeAceleration = 1;
        }
        else if (whatIsHappening == 4)
        {
            playerOneScript.modeAceleration = 1;
        }
    }

    public void CallPerkAudio (int whichAudio)
    {
        audioSystemScript.PlayTheSound(whichAudio);
    }

    public void FatalError()
    {
        Debug.Log("FatalError");
        Application.Quit();
    }
}