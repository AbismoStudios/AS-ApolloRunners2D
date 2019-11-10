using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class perkPrefabSystem : MonoBehaviour
{
    public int whatPerk;
    public int whatPlayerTag;
    private int situation;

    private GameObject thisPerk;
    private Rigidbody2D thisRigidbody;
    private Vector2 thisVector;

    private GameObject inGameSystemObj;
    public inGameSystem gameSystemScript;

    private BoxCollider2D beaconCollider;
    
    void Start()
    {
        thisPerk = this.gameObject;
        thisRigidbody = this.GetComponent<Rigidbody2D>();
        inGameSystemObj = GameObject.FindWithTag("System");
        gameSystemScript = inGameSystemObj.GetComponent<inGameSystem>();
        situation = 0;

        if (whatPerk == 2)
        {
            beaconCollider = this.GetComponent<BoxCollider2D>();
            beaconCollider.enabled = false;
            this.transform.DOScale(3.0f, 0.0f);
            this.transform.DOScale(6.5f, 2.5f);
            ParticleSystem tempParticleObj = GetComponentInChildren<ParticleSystem>();
            tempParticleObj.transform.DOScale(3.0f, 0.0f);
            tempParticleObj.transform.DOScale(6.5f, 2.5f);
        }
    }
    
    void Update()
    {
        thisVector = this.transform.position;

        if (whatPerk == 1 && situation == 0)
        {            
            StartCoroutine(NetTimer());
            situation++;
        }        
        else if (whatPerk == 2)
        {
            if (situation == 0)
            {
                StartCoroutine(BeaconTimer());
                situation++;
            }            
            thisRigidbody.AddForce(new Vector2(75.0f, 0.0f), ForceMode2D.Impulse);
            if (thisVector.x > (gameSystemScript.posPlayerOne.x + 100) && thisVector.x > (gameSystemScript.posPlayerTwo.x + 100))
            {
                //shipParticle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);                
                ParticleSystem tempParticle = this.GetComponentInChildren<ParticleSystem>();
                tempParticle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                this.gameObject.SetActive(false);
                Destroy(this.gameObject);
            }
        }
    }

    IEnumerator NetTimer()
    {
        yield return new WaitForSeconds(0.5f);
        ParticleSystem tempParticle = this.GetComponentInChildren<ParticleSystem>();
        tempParticle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        this.gameObject.SetActive(false);
        Destroy(this.gameObject);
    }

    IEnumerator BeaconTimer()
    {
        yield return new WaitForSeconds(0.5f);
        beaconCollider.enabled = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (whatPerk == 1)
        {
            if (this.tag == "PlayerOne")
            {
                if (collision.gameObject.tag == "PlayerTwo")
                {
                    gameSystemScript.CallForThePerkPrefab(1);
                }
                if (collision.gameObject.tag == "PlayerOne")
                {
                    Collider tempCollider2 = GameObject.FindWithTag("PlayerOne").GetComponent<Collider>();
                    Collider tempCollider1 = this.GetComponent<Collider>();
                    Physics.IgnoreCollision(tempCollider1, tempCollider2);
                }
            }
            else if (this.tag == "Player Two")
            {
                if (collision.gameObject.tag == "PlayerOne")
                {
                    gameSystemScript.CallForThePerkPrefab(2);
                }
                if (collision.gameObject.tag == "PlayerTwo")
                {
                    Collider tempCollider1 = GameObject.FindWithTag("PlayerTwo").GetComponent<Collider>();
                    Collider tempCollider2 = this.GetComponent<Collider>();
                    Physics.IgnoreCollision(tempCollider1, tempCollider2);
                }
            }
        }
        else if (whatPerk == 2)
        {
            if (this.tag == "PlayerOne")
            {
                if (collision.gameObject.tag == "PlayerTwo")
                {
                    gameSystemScript.CallForThePerkPrefab(3);
                }
                if (collision.gameObject.tag == "PlayerOne")
                {
                    Collider tempCollider1 = GameObject.FindWithTag("PlayerOne").GetComponent<Collider>();
                    Collider tempCollider2 = this.GetComponent<Collider>();
                    Physics.IgnoreCollision(tempCollider1, tempCollider2);
                } 
            }
            else if (this.tag == "PlayerTwo")
            {
                if (collision.gameObject.tag == "PlayerOne")
                {
                    gameSystemScript.CallForThePerkPrefab(4);
                }
                if (collision.gameObject.tag == "PlayerTwo")
                {
                    Collider tempCollider1 = GameObject.FindWithTag("PlayerTwo").GetComponent<Collider>();
                    Collider tempCollider2 = this.GetComponent<Collider>();
                    Physics.IgnoreCollision(tempCollider1, tempCollider2);
                }
            }
        }        
    }
}
