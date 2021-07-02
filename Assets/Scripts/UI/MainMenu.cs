using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public AudioClip select;
    public AudioClip menuMove;
    public AudioClip normal;

    private AudioSource audio;
    private bool playOnce = true;

    public EventSystem myEventSystem;
    // Start is called before the first frame update
    void Start()
    {
        audio = gameObject.GetComponent<AudioSource>();

        GameObject jb = GameObject.Find("Jukebox");
        AudioSource jukebox = jb.GetComponent<AudioSource>();
        if (jb != null)
        {
            jukebox.clip = normal;
            if (!jukebox.isPlaying)
            {
                jukebox.Play();   
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
    
    }


    public void LevelSelect()
    {
        if (playOnce)
        {
            playOnce = false;
            audio.PlayOneShot(select, 0.05f);
            GameObject.Find("MenuEventSystem").GetComponent<EventSystem>().sendNavigationEvents = false;
        }
        Debug.Log("Loading Level Select");
        TransitionOpen scr = GameObject.Find("Transition").GetComponent<TransitionOpen>();
        scr.CloseScene();
        StartCoroutine(ActualLoadLevel(scr));
    }

    public void StartGame()
    {
        if (playOnce)
        {
            playOnce = false;
            audio.PlayOneShot(select, 0.05f);
            GameObject.Find("MenuEventSystem").GetComponent<EventSystem>().sendNavigationEvents = false;
        }
        Debug.Log("Starting Game");
        TransitionOpen scr = GameObject.Find("Transition").GetComponent<TransitionOpen>();
        scr.CloseScene();
        StartCoroutine(ActualStartGame(scr));
    }

    private IEnumerator ActualStartGame(TransitionOpen scr)
    {
        while(scr.closed == false)
        {
            yield return null;
        }
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadSceneAsync("Instructions");
    }

    private IEnumerator ActualLoadLevel(TransitionOpen scr)
    {
        while(scr.closed == false)
        {
            yield return null;
        }
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadSceneAsync("Levels");
    }
}
