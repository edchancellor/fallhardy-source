using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LevelSelect : MonoBehaviour
{
    private int index = 0;
    private int length = 25;

    private bool Cooldown = false;
    private bool scrolling = false;
    private bool scrollup = false;
    private bool scrolldown = false;
    private bool cooldownpress = true;
    private bool reset = false;

    public AudioClip scroll;

    private AudioSource audio;

    // Start is called before the first frame update
    void Start()
    {
        audio = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (EventSystem.current.currentSelectedGameObject.tag == gameObject.tag)
        {
            gameObject.GetComponent<Text>().color = new Color32(64, 133, 60, 255);
        }
        
        if (EventSystem.current.currentSelectedGameObject.tag == gameObject.tag && !Cooldown && (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.UpArrow)))
        {
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                increaseIndex();
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                decreaseIndex();
            }
            
            Cooldown = true;
            Debug.Log("Start cooldown");
            StartCoroutine(WaitABit(1f));
        }
        else if (EventSystem.current.currentSelectedGameObject.tag != gameObject.tag)
        {
            gameObject.GetComponent<Text>().color = new Color32(200, 200, 200, 200);
        }

        else if (Cooldown)
        {
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                increaseIndex();
                reset = true;
                Debug.Log("bug");
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                decreaseIndex();
                reset = true;
            }
        }

        if (scrolling && Input.GetKey("up") && !scrolldown)
        {
            decreaseIndex();
            scrolling = false;
            scrollup = true;
            StartCoroutine(WaitABit(0.2f));
        }
        else if (scrolling && Input.GetKey("down") && !scrollup)
        {
            increaseIndex();
            scrolling = false;
            scrolldown = true;
            StartCoroutine(WaitABit(0.2f));
        }
        else if (scrolling)
        {
            scrolling = false;
            scrollup = false;
            scrolldown = false;
            Cooldown = false;
        }
    }

    private string [] alphabet = {"A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z"};

    public void increaseIndex()
    {
        audio.PlayOneShot(scroll, 0.05f);
        index ++;
        if (index > length)
        {
            index = 0;
        }
        gameObject.GetComponent<Text>().text = alphabet[index];
    }

    public void decreaseIndex()
    {
        audio.PlayOneShot(scroll, 0.05f);
        index --;
        if (index < 0)
        {
            index = length;
        }
        gameObject.GetComponent<Text>().text = alphabet[index];
    } 

    private IEnumerator WaitABit(float val)
    {
        float starter = val;
        while(starter > 0)
        {
            if (reset)
            {
                starter = val;
                reset = false;
            }
            starter = starter - 0.01f;
            
            yield return null;
        }

        if(EventSystem.current.currentSelectedGameObject.tag == gameObject.tag)
        {
            Debug.Log(EventSystem.current.currentSelectedGameObject.tag);
            Debug.Log(gameObject.tag);
            scrolling = true;
        }
        else
        {
            Cooldown = false;
        }
        yield return null;
    }
}
