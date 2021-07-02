using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class CheckLevelCode : MonoBehaviour
{
    public AudioClip AcceptedSound;
    public AudioClip RejectedSound;
    private bool unlocked = true;
    private bool acceptOnce = true;

    private AudioSource audio;

    // Start is called before the first frame update
    void Start()
    {
        audio = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CheckInput()
    {
        GameObject one = GameObject.Find("FirstLetter");
        GameObject two = GameObject.Find("SecondLetter");
        GameObject three = GameObject.Find("ThirdLetter");
        GameObject four = GameObject.Find("FourthLetter");
        GameObject five = GameObject.Find("FifthLetter");
        GameObject six = GameObject.Find("SixthLetter");

        string input = one.GetComponent<Text>().text + two.GetComponent<Text>().text + three.GetComponent<Text>().text + four.GetComponent<Text>().text + five.GetComponent<Text>().text + six.GetComponent<Text>().text;

        Debug.Log(input);

        switch(input)
        {
            case "SIMPLE": loadLevel("Scene1");
            break;
            case "MIDWAY": loadLevel("Scene2");
            break;
            case "BRIDGE": loadLevel("Scene3");
            break;
            case "PARTED": loadLevel("Scene4");
            break;
            case "NEEDLE": loadLevel("Scene5");
            break;
            case "PLUNGE": loadLevel("Scene6");
            break;
            case "TUMBLE": loadLevel("Scene7");
            break;
            case "BOUNCE": loadLevel("Scene8");
            break;
            case "STACKS": loadLevel("Scene9");
            break;
            case "SKEWER": loadLevel("Scene10");
            break;
            case "DANGER": loadLevel("Scene11");
            break;
            default: invalidInput();
            break;
        }
    }

    private void loadLevel(string scene)
    {
        if (acceptOnce)
        {
            acceptOnce = false;
            audio.PlayOneShot(AcceptedSound, 0.05f);
            GameObject.Find("EventSystem").GetComponent<EventSystem>().sendNavigationEvents = false;
        }
        GameObject accept = GameObject.Find("Accepted");
        accept.GetComponent<Text>().enabled = true;
        TransitionOpen scr = GameObject.Find("Transition").GetComponent<TransitionOpen>();
        StartCoroutine(ActualLoad(scr, scene));
    }

    IEnumerator ActualLoad(TransitionOpen scr, string scene)
    {
        yield return new WaitForSeconds(1f);
        scr.CloseScene();
        while(scr.closed == false)
        {
            yield return null;
        }
        yield return new WaitForSeconds(0.02f);
        SceneManager.LoadSceneAsync(scene);
    }

    private void invalidInput()
    {
        if (unlocked)
        {
            unlocked = false;
            audio.PlayOneShot(RejectedSound, 0.05f);
            GameObject deny = GameObject.Find("Denied");
            deny.GetComponent<Text>().enabled = true;
            StartCoroutine(WaitABit(deny));
        }
    }

    IEnumerator WaitABit(GameObject deny)
    {
        yield return new WaitForSeconds(1.5f);
        deny.GetComponent<Text>().enabled = false;
        unlocked = true;
        yield return null;
    }

    public void LoadMenu()
    {
        if (acceptOnce)
        {
            acceptOnce = false;
            audio.PlayOneShot(AcceptedSound, 0.05f);
            GameObject.Find("EventSystem").GetComponent<EventSystem>().sendNavigationEvents = false;
        }
        
        Debug.Log("Loading Menu");
        TransitionOpen scr = GameObject.Find("Transition").GetComponent<TransitionOpen>();
        scr.CloseScene();
        StartCoroutine(ActualLoadMenu(scr));
    }

    private IEnumerator ActualLoadMenu(TransitionOpen scr)
    {
        while(scr.closed == false)
        {
            yield return null;
        }
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadSceneAsync("Menu");
    }
}
