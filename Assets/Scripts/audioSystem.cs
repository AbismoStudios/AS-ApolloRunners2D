using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audioSystem : MonoBehaviour
{
    [Header("Read Only Properties")]
    public GameObject ListenerCameraObject;

    public Component[] sceneAudios;
    public AudioSource[] sceneAudiosToPlay;
    void Start()
    {
       
    }    

    public void AssociateGameAudio()
    {
        AssociateAudioComponents();
        sceneAudios = GetComponentsInChildren(typeof(AudioSource));
        ConvertAudio();
    }

    void AssociateAudioComponents()
    {
        ListenerCameraObject = GameObject.FindWithTag("CameraListener");        
    }

    void ConvertAudio()
    {
        int repeater = sceneAudios.Length;
        sceneAudiosToPlay = new AudioSource[repeater];
        for (int i = 0; i < repeater; i++)
        {
            sceneAudiosToPlay[i] = sceneAudios[i].GetComponent<AudioSource>();
        }
    }

    public void PlayTheSound (int whichSound)
    {
        if (sceneAudiosToPlay[whichSound] != null)
        {
            sceneAudiosToPlay[whichSound].Play();
        }        
    }

    public void StopTheSound (int whichSound)
    {
        if (sceneAudiosToPlay[whichSound] != null)
        {
            sceneAudiosToPlay[whichSound].Stop();
        }        
    }
}
