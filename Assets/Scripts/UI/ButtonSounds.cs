using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonSounds : MonoBehaviour, IPointerEnterHandler,  ISelectHandler
{
    public AudioClip select;
    public AudioClip menuMove;

    private AudioSource audio;
    public bool notFirstTime = true;
    
    // Start is called before the first frame update
    void Start()
    {
        audio = gameObject.GetComponent<AudioSource>();
        if(gameObject.name != "Start Game" && gameObject.name != "RestartButton" && gameObject.name != "FirstDown")
        {
            notFirstTime = false;
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerEnter(PointerEventData ped)
    {
        audio.PlayOneShot(menuMove, 0.05f);
        Debug.Log("play");
    }

    public void OnSelect (BaseEventData eventData) 
    {
        if(notFirstTime)
        {
            notFirstTime = false;
        }
        else
        {
            audio.PlayOneShot(menuMove, 0.05f);
        }
    }
}
