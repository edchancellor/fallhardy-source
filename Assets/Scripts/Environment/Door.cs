using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    public string destination;
    bool onlyLoadOnce = true;
    public AudioClip victory;
    private bool soundOnce = true;

    private AudioSource audio;

    void Start()
    {
        audio = gameObject.GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        CharacterController2D controller = collision.gameObject.GetComponent<CharacterController2D>();
        if (controller != null)
        {
            if (controller.isAlive && onlyLoadOnce)
            {
                
                StartCoroutine(enterScene(controller));
            }
        }
    }

    IEnumerator enterScene(CharacterController2D controller)
    {
        
        StartCoroutine(DoorSound(controller));
        gameObject.GetComponent<Animator>().SetBool("Taken", true);
        GameObject.Find("Player").GetComponent<PlayerMovement>().movementAllowed = false;
        yield return new WaitForSeconds(1f);
        if (controller != null)
        {
            if (controller.isAlive && onlyLoadOnce)
            {
                onlyLoadOnce = false;
                TransitionOpen scr = GameObject.Find("Transition").GetComponent<TransitionOpen>();
                scr.CloseScene();
                StartCoroutine(NextScene(scr));
                yield return null;
            }

        }
    }

    IEnumerator DoorSound(CharacterController2D controller)
    {
        yield return new WaitForSeconds(0.1f);
        if (controller.isAlive && soundOnce)
        {
            soundOnce = false;
            audio.PlayOneShot(victory, 0.05f);
        }
    }

    private IEnumerator NextScene(TransitionOpen scr)
    {
        while(scr.closed == false)
        {
            yield return null;
        }

        AsyncOperation ao = SceneManager.LoadSceneAsync(destination);
        ao.allowSceneActivation = false;
        Debug.Log("Loading New Scene");
        ao.allowSceneActivation = true;
    }
}
