using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trackCamera : MonoBehaviour
{
    [Header("Associated Ships")]
    public GameObject ship;
    public GameObject secondShip;
    private Vector3 offset;
    private Vector3 bothOffset;

    private playerController shipScript;

    private Vector2 pos1, pos2;
    private Vector3 bothCameraPosition;
    private float posDiferece;
    private float originalCameraSize;

    private Camera thisCamera;
    private bool willShake;

    [Header("Both Camera Properties")]
    public bool bothCamera;

    public GameObject bothCameraObj;
    private Camera bothCameraComponent;
    private int frontShip;

    private bool checkOffsetState = true;
    public GameObject offsetSpot;
    
    void Start()
    {        
        if (bothCamera == true)
        {
            bothCameraComponent = GetComponent<Camera>();
            bothCameraPosition = bothCameraObj.transform.position;

            bothOffset = transform.position - offsetSpot.transform.position;
        }
        else
        {
            offset = transform.position - ship.transform.position;
            Associate();
            willShake = false;            
        }
    }
    
    void Update()
    {
        if (bothCamera == true)
        {
            //ship = 1 \ bothShip = 2
            TakeVector();
            if (posDiferece < 30)
            {
                //both camera activated
                bothCameraComponent.enabled = true;
            }
            else
            {
                bothCameraComponent.enabled = false;
                checkOffsetState = true;
            }
        }           
    }
    void LateUpdate()
    {
        if (bothCamera == false)
        {
            transform.position = ship.transform.position + offset;
            if (shipScript.actualVelocity > 1000.0f && willShake == false)
            {                
                StartCoroutine(CameraShake());
            }
        }
        else if (bothCamera == true)
        {
            if (checkOffsetState == true)
            {
                CheckOffset();
            }
            if (frontShip == 2)
            {
                transform.position = ship.transform.position + bothOffset;
            }
            else if (frontShip == 1)
            {
                transform.position = secondShip.transform.position + bothOffset;
            }
            checkOffsetState = false;
        }
    }    

    void TakeVector()
    {
        //pos1 = ship.transform.position;
        pos1 = new Vector2(ship.transform.position.x, ship.transform.position.y);
        //pos2 = secondShip.transform.position;
        pos2 = new Vector2(secondShip.transform.position.x, secondShip.transform.position.y);
        if (pos1.x > pos2.x)
        {
            posDiferece = pos1.x - pos2.x; 
        }
        else if (pos1.x < pos2.x)
        {
            posDiferece = pos2.x - pos1.x;
        }
        else
        {
            posDiferece = 0;
        }
    }
    void CheckOffset()
    {
        TakeVector();
        if (pos1.x >= pos2.x)
        {
            //pos1 front with ship 1            
            frontShip = 1;            
        }
        else
        {
            //pos2 front with ship 2
            frontShip = 2;
        }        
    }

    void Associate()
    {
        thisCamera = this.GetComponent<Camera>();
        originalCameraSize = thisCamera.orthographicSize;
        shipScript = ship.GetComponent<playerController>();        
    }

    //corotine
    IEnumerator CameraShake()
    {
        willShake = true;
        while (true)
        {
            if (thisCamera.orthographicSize >= 5.0f)
            {
                thisCamera.orthographicSize = thisCamera.orthographicSize * 0.999f;
            }
            else if (thisCamera.orthographicSize < 5.0f)
            {
                thisCamera.orthographicSize = 5.0f;
            }
            
            if (shipScript.actualVelocity < 1000.0f)
            {
                thisCamera.orthographicSize = originalCameraSize;
                break;
            }
            yield return new WaitForSeconds(0.1f);
        }
        willShake = false;
    }
}