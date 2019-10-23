using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class playerController : MonoBehaviour
{
    [Header("Player Association")]
    public int player;
    public int whatShip;

    [Space(10)]
    [Header("Read Only Properties")]
    private Rigidbody2D shipBody;

    private float playerHorizontal;
    private float playerVertical;

    private GameObject inGameSystemObj;
    public inGameSystem gameSystemScript;

    public perkSystem playerPerkScript; 
    
    private float modeBreaker;
    public float modeAceleration;
    private int changeAceleration = 0;
    private float changeAcelerationTimer;
    public bool acelerating = false;
    public float shipX;
    private float shipY;
    public float actualVelocity;

    public bool playable;
    public bool canIniciate;
    public int alreadyIniciated = 0;

    private float rotationResetSpeed = 3.0f;
    public Quaternion originalRotationValue;
    private Quaternion actualRotation;
    private bool collided = false;

    private GameObject playerCameraText1;
    private GameObject playerCameraText2;
    private bool associated = false;
    private TextMeshProUGUI scoreText;
    private TextMeshProUGUI velocityText;

    public GameObject yourShip;
    public GameObject anotherShip;

    private Vector2 yourOriginalPosition;
    
    void Start()
    {
        //rigidbod
        shipBody = GetComponent<Rigidbody2D>();

        //Score of the ship and another texts
        AssociateGameObjects();

        //ship balance
        switch (whatShip)
        {
            case 1:
                shipX = 2.5f;
                shipY = 0.7f;
                modeBreaker = -30.0f;
                changeAcelerationTimer = 10.0f;
                break;
            case 2:
                shipX = 1.3f;
                shipY = 0.7f;
                modeBreaker = -10.0f;
                changeAcelerationTimer = 5.0f;
                break;
            default:
                Debug.Log("Ship not found");
                break;
        }
        modeAceleration = shipX;
        originalRotationValue = transform.rotation;

        playable = false;
        alreadyIniciated = 0;

        StartCoroutine(CheckVelocity());
    }  
    
    public void ResetForReMatch()
    {
        playable = false;
        alreadyIniciated = 0;
        shipBody.velocity = Vector2.zero;
        shipBody.angularVelocity = 0.0f;
        transform.rotation = originalRotationValue;
    }
    
    void FixedUpdate()
    {
        if (playable == true)
        {
            if (player == 1)
            {
                //Arcade inputs
                playerHorizontal = Input.GetAxis("HORIZONTAL0");
                playerVertical = Input.GetAxis("VERTICAL0");
                if ((Input.GetKey(KeyCode.D)) || playerHorizontal > 0.0f)
                {
                    if (canIniciate == false && alreadyIniciated == 0)
                    {                        
                        BurnedRaceStart(1);
                    }
                    shipBody.AddForce(new Vector2(modeAceleration, 0.0f), ForceMode2D.Impulse);
                    acelerating = true;
                    if (modeAceleration > 0 && changeAceleration == 0)
                    {
                        StartCoroutine(ABreaker());
                    }
                    else if (modeAceleration < 1)
                    {
                        StopCoroutine(ABreaker());
                        modeAceleration = 1;
                    }
                    if ((Input.GetKey(KeyCode.W)) || playerVertical > 0.0f)
                    {
                        shipBody.AddForce(new Vector2(0.0f, shipY), ForceMode2D.Impulse);
                        if (actualRotation.z < 10)
                        {
                            transform.Rotate(0.0f, 0.0f, 1.0f, Space.Self);
                        }
                    }
                    else if ((Input.GetKey(KeyCode.S)) || playerVertical < 0.0f) 
                    {                        
                        shipBody.AddForce(new Vector2(0.0f, (-shipY)),ForceMode2D.Impulse);
                        if (actualRotation.z > -10)
                        {                            
                            transform.Rotate(0.0f, 0.0f, -1.0f, Space.Self);
                        }
                    }
                    if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S) && playerVertical == 0.0f)
                    {
                        if (actualRotation.z > 0)
                        {
                            transform.Rotate(0.0f, 0.0f, -2.0f, Space.Self);
                        }
                        else if (actualRotation.z < 0)
                        {
                            transform.Rotate(0.0f, 0.0f, 2.0f, Space.Self);
                        }
                    }
                }
                else
                {
                    if (shipBody.velocity.x > 1)
                    {
                        shipBody.AddForce(new Vector2(modeBreaker, 0.0f), ForceMode2D.Force);
                    }
                        acelerating = false;
                    if (changeAceleration == 0 && modeAceleration < shipX)
                    {
                        StartCoroutine(ABreaker());
                    }
                }

                //perk
                if ((Input.GetKeyDown(KeyCode.H)) || (Input.GetKeyDown(KeyCode.LeftControl)))
                {
                    playerPerkScript.CallThePerk();
                }
            }
            else if (player == 2)
            {
                //Arcade inputs
                playerHorizontal = Input.GetAxis("HORIZONTAL1");
                playerVertical = Input.GetAxis("VERTICAL1");
                if ((Input.GetKey(KeyCode.RightArrow)) || playerHorizontal > 0.0f)
                {
                    if (canIniciate == false && alreadyIniciated == 0)
                    {
                        BurnedRaceStart(2);
                    }
                    shipBody.AddForce(new Vector2(modeAceleration, 0.0f), ForceMode2D.Impulse);
                    acelerating = true;
                    if (modeAceleration > 0 && changeAceleration == 0)
                    {
                        StartCoroutine(ABreaker());
                    }
                    else if (modeAceleration < 1)
                    {
                        StopCoroutine(ABreaker());
                        modeAceleration = 1;
                    }
                    if ((Input.GetKey(KeyCode.UpArrow)) || playerVertical > 0.0f)
                    {
                        shipBody.AddForce(new Vector2(0.0f, shipY), ForceMode2D.Impulse);
                        if (actualRotation.z < 10)
                        {
                            transform.Rotate(0.0f, 0.0f, 1.0f, Space.Self);
                        }
                    }
                    else if ((Input.GetKey(KeyCode.DownArrow)) || playerVertical < 0.0f)
                    {
                        shipBody.AddForce(new Vector2(0.0f, (-shipY)), ForceMode2D.Impulse);
                        if (actualRotation.z > -10)
                        {
                            transform.Rotate(0.0f, 0.0f, -1.0f, Space.Self);
                        }
                    }
                    if (!Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.DownArrow) && playerVertical == 0.0f)
                    {
                        if (actualRotation.z > 0)
                        {
                            transform.Rotate(0.0f, 0.0f, -2.0f, Space.Self);
                        }
                        else if (actualRotation.z < 0)
                        {
                            transform.Rotate(0.0f, 0.0f, 2.0f, Space.Self);
                        }
                    }
                }
                else
                {
                    if (shipBody.velocity.x > 1)
                    {
                        shipBody.AddForce(new Vector2(modeBreaker, 0.0f), ForceMode2D.Force);
                    }
                    acelerating = false;
                    if (changeAceleration == 0 && modeAceleration < shipX)
                    {
                        StartCoroutine(ABreaker());
                    }
                }

                //perk
                if ((Input.GetKeyDown(KeyCode.J)) || (Input.GetKeyDown(KeyCode.RightControl)))
                {
                    playerPerkScript.CallThePerk();
                }
            }
            else
            {
                Debug.Log("Player Unknown");
            }
        }
        else
        {            
            if (gameSystemScript.someoneBurned == false)
            {
                playable = false;
                alreadyIniciated = 0;
                shipBody.velocity = Vector2.zero;
                shipBody.angularVelocity = 0.0f;
                transform.rotation = originalRotationValue;
                this.transform.position = yourOriginalPosition;
            }            
        }

        if (actualVelocity > 500.0f && actualVelocity < 1500.0f)
        {
            int angularRandomizer = UnityEngine.Random.Range(0, 2);
            if (angularRandomizer == 0)
            {
                transform.Rotate(0.0f, 0.0f, 0.5f, Space.Self);
            }
            else if (angularRandomizer == 1)
            {
                transform.Rotate(0.0f, 0.0f, -0.5f, Space.Self);
            }
        }
        else if (actualVelocity > 1500.0f)
        {
            int angularRandomizer = UnityEngine.Random.Range(0, 2);
            if (angularRandomizer == 0)
            {
                transform.Rotate(0.0f, 0.0f, 1.5f, Space.Self);
            }
            else if (angularRandomizer == 1)
            {
                transform.Rotate(0.0f, 0.0f, -1.5f, Space.Self);
            }
        }

        //reset in Z        
        Vector3 tempPosition = transform.position;
        if (tempPosition.z != 0)
        {
            tempPosition.z = 0;
            transform.position = new Vector3(transform.position.x, transform.position.y, tempPosition.z);
        }
        
    }

    void Update()
    {
        if (associated == true)
        {   
            Vector3 posYourShip = yourShip.transform.position;
            Vector3 posAnotherShip = anotherShip.transform.position;
            float atualDistance = posYourShip.x - posAnotherShip.x;            
            scoreText.text = Math.Round(atualDistance, 2).ToString();
        }

        //reset Rotation
        if (transform.rotation != originalRotationValue && collided == true)
        {            
            StartCoroutine(ResetRotation());
        }
        //check start line timer    

        actualRotation = transform.rotation;
    }

    void AssociateGameObjects()
    {
        if (player == 1)
        {
            playerCameraText1 = GameObject.Find("TextShip1");
            scoreText = playerCameraText1.GetComponent<TextMeshProUGUI>();
            yourShip = GameObject.Find("Player1Ship");
            anotherShip = GameObject.Find("Player2Ship");

            playerCameraText2 = GameObject.Find("VelocityShip1");
            velocityText = playerCameraText2.GetComponent<TextMeshProUGUI>();

            associated = true;
        }
        else if (player == 2)
        {
            playerCameraText1 = GameObject.Find("TextShip2");
            scoreText = playerCameraText1.GetComponent<TextMeshProUGUI>();
            yourShip = GameObject.Find("Player2Ship");
            anotherShip = GameObject.Find("Player1Ship");

            playerCameraText2 = GameObject.Find("VelocityShip2");
            velocityText = playerCameraText2.GetComponent<TextMeshProUGUI>();
                       
            associated = true;
        }
        else
        {
            Debug.Log("Association Error");
        }

        yourOriginalPosition = this.transform.position;
        inGameSystemObj = GameObject.FindWithTag("System");
        gameSystemScript = inGameSystemObj.GetComponent<inGameSystem>();
        playerPerkScript = this.GetComponent<perkSystem>();        
    }

    void BurnedRaceStart(int which)
    {
        alreadyIniciated++;
        gameSystemScript.someoneBurned = true;
        playable = false;
        CallForStopAcelerationOfTheShip();
        if (which == 1)
        {
            //player 1 burned \ player 2 win
            gameSystemScript.BothCameraText.text = "Burned";            
        }
        else if (which == 2)
        {
            //player 2 burned \ player 1 win
            gameSystemScript.BothCameraText.text = "Burned";
        }
        else
        {
            gameSystemScript.FatalError();
        }
        gameSystemScript.EndOfMatch(which);
    }

    public void CallForStopAcelerationOfTheShip()
    {
        StartCoroutine(StopAceleration());
    }
    

    public IEnumerator StopAceleration()
    {
        yield return new WaitForSeconds(2.0f);
        modeAceleration = 0;
    }

    IEnumerator CheckVelocity()
    {
        Vector3 checkVelocity = yourShip.transform.position;
        yield return new WaitForSeconds(0.5f);
        Vector3 checkVelocity2 = yourShip.transform.position;
        float distanceOfTheShips = checkVelocity2.x - checkVelocity.x;
        distanceOfTheShips = (distanceOfTheShips * 2.0f) * 3.6f;
        actualVelocity = distanceOfTheShips;
        //km/h
        velocityText.text = Math.Round(distanceOfTheShips, 0).ToString() + " Km/h";        
        StartCoroutine(CheckVelocity());
    }

    IEnumerator ABreaker()
    {
        changeAceleration++;
        if (acelerating == true)
        {
            modeAceleration = modeAceleration * 0.90f;
            //0.25f
        }
        else
        {
            modeAceleration = modeAceleration * 2.01f;
        }        
        yield return new WaitForSeconds(changeAcelerationTimer);
        changeAceleration = 0;
    }

    IEnumerator ResetRotation()
    {
        collided = false;
        yield return new WaitForSeconds(rotationResetSpeed);
        transform.rotation = originalRotationValue;        
    }


    //colliders 
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Collider")
        {
            collided = true;
        }
    }        
}