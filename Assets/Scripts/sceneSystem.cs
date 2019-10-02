using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class sceneSystem : MonoBehaviour
{
    [Header("Read Only Properties")]
    public primarySystem primarySystemScript;
    public GameObject primarySystemObject;
    public string actualSceneNameOnSceneSystem;
   
    void Start()
    {
        actualSceneNameOnSceneSystem = SceneManager.GetActiveScene().name;
        primarySystemObject = GameObject.FindWithTag("PrimarySystem");
        primarySystemScript = primarySystemObject.GetComponent<primarySystem>();

        primarySystemScript.actualSceneName = actualSceneNameOnSceneSystem;
    }
}