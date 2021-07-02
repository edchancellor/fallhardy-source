using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject PauseMenuUI;
    public EventSystem myEventSystem;

    public AudioClip select;
    public AudioClip pause;
    public AudioClip unpause;

    private AudioSource audio;
    private bool playAudioOnce = true;

    void Start()
    {
        audio = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            GameObject transition = GameObject.Find("Transition");
            if(transition != null)
            {
                bool transitioning = transition.GetComponent<TransitionOpen>().transitioning;
                if (GameIsPaused && !transitioning)
                {
                    audio.PlayOneShot(unpause, 0.05f);
                    Resume();
                }
                else if (!transitioning)
                {
                    audio.PlayOneShot(pause, 0.05f);
                    StartCoroutine(highlightBtn());
                    Pause();
                }
            }
        }
    }


    void Resume ()
    {
        if(GameObject.Find("MenuButton") != null && GameObject.Find("LevelButton") != null && GameObject.Find("RestartButton") != null)
        {
            GameObject.Find("RestartButton").GetComponent<ButtonSounds>().notFirstTime = true;
            GameObject.Find("MenuButton").GetComponent<ButtonSounds>().notFirstTime = true;
            GameObject.Find("LevelButton").GetComponent<ButtonSounds>().notFirstTime = true;
        }
        PauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    void Pause ()
    {
        PauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void LoadMenu()
    {
        GameObject.Find("Player").GetComponent<CharacterController2D>().jumpSoundAllowed = false;
        if (playAudioOnce)
        {
            playAudioOnce = false;
            audio.PlayOneShot(select, 0.05f);
            GameObject.Find("PauseEventSystem").GetComponent<EventSystem>().sendNavigationEvents = false;
        }
        Debug.Log("Loading Menu");
        TransitionOpen scr = GameObject.Find("Transition").GetComponent<TransitionOpen>();
        scr.CloseScene();
        StartCoroutine(ActualLoadMenu(scr));
    }

    public void RestartLevel()
    {
        GameObject.Find("Player").GetComponent<CharacterController2D>().jumpSoundAllowed = false;
        if (playAudioOnce)
        {
            playAudioOnce = false;
            audio.PlayOneShot(select, 0.05f);
            GameObject.Find("PauseEventSystem").GetComponent<EventSystem>().sendNavigationEvents = false;
        }
        Debug.Log("Restarting Level");
        TransitionOpen scr = GameObject.Find("Transition").GetComponent<TransitionOpen>();
        scr.CloseScene();
        StartCoroutine(ActualRestartMenu(scr));
    }

    public void LevelSelect()
    {
        GameObject.Find("Player").GetComponent<CharacterController2D>().jumpSoundAllowed = false;
        if (playAudioOnce)
        {
            playAudioOnce = false;
            audio.PlayOneShot(select, 0.05f);
            GameObject.Find("PauseEventSystem").GetComponent<EventSystem>().sendNavigationEvents = false;
        }
        Debug.Log("Loading Level Select");
        TransitionOpen scr = GameObject.Find("Transition").GetComponent<TransitionOpen>();
        scr.CloseScene();
        StartCoroutine(ActualLevelSelect(scr));
    }

    private IEnumerator highlightBtn()
    {
        myEventSystem.SetSelectedGameObject(null);
        yield return null;
        myEventSystem.SetSelectedGameObject(myEventSystem.firstSelectedGameObject);
        yield return new WaitForEndOfFrame();
        myEventSystem.SetSelectedGameObject(myEventSystem.firstSelectedGameObject);
        if(GameObject.Find("MenuButton") != null && GameObject.Find("LevelButton") != null)
        {
            GameObject.Find("MenuButton").GetComponent<ButtonSounds>().notFirstTime = false;
            GameObject.Find("LevelButton").GetComponent<ButtonSounds>().notFirstTime = false;
        }
    }

    private IEnumerator ActualLoadMenu(TransitionOpen scr)
    {
        while(scr.closed == false)
        {
            yield return null;
        }
        yield return new WaitForSecondsRealtime(0.5f);
        SceneManager.LoadSceneAsync("Menu");
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    private IEnumerator ActualRestartMenu(TransitionOpen scr)
    {    
        
        while(scr.closed == false)
        {
            yield return null;
        }
        yield return new WaitForSecondsRealtime(0.5f);
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    private IEnumerator ActualLevelSelect(TransitionOpen scr)
    {
        while(scr.closed == false)
        {
            yield return null;
        }
        yield return new WaitForSecondsRealtime(0.5f);
        SceneManager.LoadSceneAsync("Levels");
        Time.timeScale = 1f;
        GameIsPaused = false;
    }
}
