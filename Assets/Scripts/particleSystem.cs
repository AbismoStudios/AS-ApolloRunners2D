using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class particleSystem : MonoBehaviour
{
    public GameObject playerShip;
    public playerController playerShipScript;
    public ParticleSystem shipParticle;

    private float particleShipSpeed;
    private float particleShipLifetime;
    // Start is called before the first frame update
    void Start()
    {
        Associate();
    }

    // Update is called once per frame
    void Update()
    {
        particleShipSpeed = (playerShipScript.actualVelocity / 100) * 5;
        particleShipLifetime = (playerShipScript.actualVelocity / 100) * 2;
        var sSpeed = shipParticle.main;
        
        if (playerShipScript.acelerating == true && playerShipScript.playable == true)
        {
            if (shipParticle.isPlaying == false)
            {
                shipParticle.Play();
            }
            if (particleShipSpeed <= 250.0f)
            {
                sSpeed.simulationSpeed = particleShipSpeed;
            }
            else
            {
                sSpeed.simulationSpeed = 250.0f;
            }
            
            if (particleShipLifetime <= 5)
            {
                sSpeed.startLifetime = particleShipLifetime;
            }
            else
            {
                sSpeed.startLifetime = 5.0f;
            }
        }

        if (playerShipScript.acelerating == false || playerShipScript.playable == false)
        {
            sSpeed.simulationSpeed = 0.0f;
            sSpeed.startLifetime = 0.0f;
            shipParticle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            //includeChildren, ParticleSystemStopBehavior.StopEmittingAndClear
        }
    }

    void Associate()
    {
        playerShip = this.GetComponent<GameObject>();
        playerShipScript = GetComponent<playerController>();
        shipParticle = GetComponentInChildren<ParticleSystem>();
    }
}
