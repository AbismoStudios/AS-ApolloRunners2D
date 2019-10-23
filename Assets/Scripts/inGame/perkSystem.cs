using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class perkSystem : MonoBehaviour
{

    public int whatPlayer;

    public GameObject inGameSystemObj;
    public inGameSystem inGameSystemScrypt;

    public playerController playerControllerScript;

    public GameObject perkTextObj;
    public TextMeshProUGUI perkText;

    public GameObject BeaconPrefab;
    public GameObject NetPrefab;

    public bool playerHavePerk = false;
    private int whatPerk;

    public ParticleSystem burstParticleSystem;

    // Start is called before the first frame update
    void Start()
    {
        Associate();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            perkText.SetText("Testezinho");
        }

        if (playerHavePerk == true)
        {
            //change text
            // perkText.DOColor(Color.white, 0.5f);
        }
    }

    private void Associate()
    {
        inGameSystemObj = GameObject.FindGameObjectWithTag("System");
        inGameSystemScrypt = inGameSystemObj.GetComponent<inGameSystem>();
        playerControllerScript = this.GetComponent<playerController>();

        if (whatPlayer == 1)
        {
            perkTextObj = GameObject.Find("PerkTextShip1");
            perkText = perkTextObj.GetComponent<TextMeshProUGUI>();

            burstParticleSystem = GameObject.Find("BurstParticleShip1").GetComponent<ParticleSystem>();
        }
        else if (whatPlayer == 2)
        {
            perkTextObj = GameObject.Find("PerkTextShip2");
            perkText = perkTextObj.GetComponent<TextMeshProUGUI>();

            burstParticleSystem = GameObject.Find("BurstParticleShip2").GetComponent<ParticleSystem>();
        }
    }

    public void CallPerkTimer()
    {
        StartCoroutine(PerkTimer());
    }

    public void CallThePerk()
    {
        if (playerHavePerk == true)
        {
            Debug.Log("Perk usado");
            if (whatPerk == 0)
            {
                PerkBurst();
            }
            else if (whatPerk == 1)
            {
                PerkNet();
            }
            else if (whatPerk == 2)
            {
                PerkBeacon();
            }

            StartCoroutine(PerkTimer());
        }
        else
        {
            Debug.Log("Perk indisponível");
        }
    }

    public IEnumerator PerkTimer()
    {
        playerHavePerk = false;
        perkText.DOColor(Color.white, 0.0f);
        perkTextObj.transform.DOScale(0.38f, 0.5f);
        perkText.SetText("5");
        yield return new WaitForSeconds(1.0f);
        perkText.SetText("4");
        yield return new WaitForSeconds(1.0f);
        perkText.SetText("3");
        yield return new WaitForSeconds(1.0f);
        perkText.SetText("2");
        yield return new WaitForSeconds(1.0f);
        perkText.SetText("1");
        yield return new WaitForSeconds(1.0f);
        playerHavePerk = true;
        perkTextObj.transform.DOScale(0.5f, 0.5f);
        RandomizerPerk();
    }

    void RandomizerPerk()
    {
        perkText.DOColor(Color.yellow, 0.5f);
        whatPerk = 0;
        whatPerk = Random.Range(0, 3);

        if (whatPerk == 0)
        {
            perkText.SetText("Burst");
        }
        else if (whatPerk == 1)
        {
            perkText.SetText("Net");
        }
        else if (whatPerk == 2)
        {
            perkText.SetText("Beacon");
        }
    }

    //PerkList
    private void PerkBurst()
    {
        playerControllerScript.modeAceleration = 4;
        burstParticleSystem.Play();
    }
    private void PerkNet()
    {
        if (whatPlayer == 1)
        {
            Vector3 tempPerkPosition = new Vector3(inGameSystemScrypt.posPlayerOne.x - 44, inGameSystemScrypt.posPlayerOne.y, inGameSystemScrypt.posPlayerOne.z);
            GameObject TempPerkNet = Instantiate(NetPrefab, tempPerkPosition, Quaternion.identity);
            TempPerkNet.tag = "PlayerOne";
        }
        else if (whatPlayer == 2)
        {
            Vector3 tempPerkPosition = new Vector3(inGameSystemScrypt.posPlayerTwo.x - 44, inGameSystemScrypt.posPlayerTwo.y, inGameSystemScrypt.posPlayerTwo.z);
            GameObject TempPerkNet = Instantiate(NetPrefab, tempPerkPosition, Quaternion.identity);
            //GameObject TempPerkNet = Instantiate(NetPrefab, inGameSystemScrypt.player, Quaternion.identity);
            TempPerkNet.tag = "PlayerTwo";
        }
    }
    private void PerkBeacon()
    {
        if (whatPlayer == 1)
        {
            Vector3 tempPerkPosition = new Vector3(inGameSystemScrypt.posPlayerOne.x + 20, inGameSystemScrypt.posPlayerOne.y - 1.5f, inGameSystemScrypt.posPlayerOne.z);
            GameObject TempPerkBeacon = Instantiate(BeaconPrefab, tempPerkPosition, Quaternion.identity);
            TempPerkBeacon.tag = "PlayerOne";
        }
        else if (whatPlayer == 2)
        {
            Vector3 tempPerkPosition = new Vector3(inGameSystemScrypt.posPlayerTwo.x + 20, inGameSystemScrypt.posPlayerTwo.y - 1.5f, inGameSystemScrypt.posPlayerTwo.z);
            GameObject TempPerkBeacon = Instantiate(BeaconPrefab, tempPerkPosition, Quaternion.identity);
            TempPerkBeacon.tag = "PlayerTwo";
        }
    }
}
