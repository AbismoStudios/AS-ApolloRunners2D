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

    // Start is called before the first frame update
    void Start()
    {
        thisPerk = this.gameObject;
        thisRigidbody = this.GetComponent<Rigidbody2D>();
        inGameSystemObj = GameObject.FindWithTag("System");
        gameSystemScript = inGameSystemObj.GetComponent<inGameSystem>();
        situation = 0;

        if (whatPerk == 1)
        {

        }        
        else if (whatPerk == 2)
        {
            //beacon            
            //thisRigidbody.velocity = new Vector3(100.0f, 0.0f, 0.0f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        thisVector = this.transform.position;

        if (whatPerk == 1 && situation == 0)
        {
            //net 
            StartCoroutine(NetTimer());
            situation++;
        }        
        else if (whatPerk == 2)
        {
            //beacon
            thisRigidbody.AddForce(new Vector2(50.0f, 0.0f), ForceMode2D.Impulse);
            if (thisVector.x > (gameSystemScript.posPlayerOne.x + 100) && thisVector.x > (gameSystemScript.posPlayerTwo.x + 100))
            {
                //shipParticle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                
                Destroy(this);
            }
        }
    }

    IEnumerator NetTimer()
    {
        yield return new WaitForSeconds(2.0f);
        Destroy(this);
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
            }
            else if (this.tag == "Player Two")
            {
                if (collision.gameObject.tag == "PlayerOne")
                {
                    gameSystemScript.CallForThePerkPrefab(2);
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
            }
            else if (this.tag == "PlayerTwo")
            {
                if (collision.gameObject.tag == "PlayerOne")
                {
                    gameSystemScript.CallForThePerkPrefab(4);
                }
            }
        }        
    }
}
