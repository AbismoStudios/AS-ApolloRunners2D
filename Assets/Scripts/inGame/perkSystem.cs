using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class perkSystem : MonoBehaviour
{

    public int whatPlayer;

    public GameObject inGameSystemObj;
    public inGameSystem inGameSystemScrypt;

    public GameObject perkTextObj;
    public TextMeshProUGUI perkText;
    
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
    }

    private void Associate()
    {
        inGameSystemObj = GameObject.FindGameObjectWithTag("System");
        inGameSystemScrypt = inGameSystemObj.GetComponent<inGameSystem>();

        if (whatPlayer == 1)
        {
            perkTextObj = GameObject.Find("PerkTextShip1");
            perkText = perkTextObj.GetComponent<TextMeshProUGUI>();
        }
        else if (whatPlayer == 2)
        {

        }
    }
}
