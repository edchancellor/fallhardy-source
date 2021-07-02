using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Victory : MonoBehaviour
{
    private int screenNo;
    private bool arrowVisible;
    private bool initialised;
    private bool continueReady;
    private bool firstTime;
    private bool ready;

    public GameObject text1;
    public GameObject Fallhardy;
    public GameObject Arrow1;
    public GameObject text2;

    
    public AudioClip select;

    private AudioSource audio;
    
    
    // Start is called before the first frame update
    void Start()
    {
        screenNo = 0;
        arrowVisible = false;
        initialised = false;
        continueReady = false;
        firstTime = true;
        ready = false;
        audio = gameObject.GetComponent<AudioSource>();

        Color zm = text1.GetComponent<Text>().color;
        zm.a = 0f;
        text1.GetComponent<Text>().color = zm;

        Color xm = Fallhardy.GetComponent<Text>().color;
        xm.a = 0f;
        Fallhardy.GetComponent<Text>().color = xm;

        Color ym = Arrow1.GetComponent<Text>().color;
        ym.a = 0f;
        Arrow1.GetComponent<Text>().color = ym;

        Color am = text2.GetComponent<Text>().color;
        am.a = 0f;
        text2.GetComponent<Text>().color = am;

    }

    // Update is called once per frame
    void Update()
    {
        if(firstTime)
        {
            firstTime = false;
            StartCoroutine(waitAMo());
        }

        else if(!initialised && ready)
        {
            StartCoroutine(initialiseScreen());
        }
        
        else if ((Input.GetKeyDown("space") || Input.GetKeyDown(KeyCode.Return)) && continueReady && ready)
        {
            audio.PlayOneShot(select, 0.05f);
            screenNo ++;
            initialised = false;
            continueReady = false;
            Debug.Log("Next");
        }

        else if(continueReady && ready)
        {
            if(!arrowVisible)
            {
                StartCoroutine(makeArrowVisible());
            }
        }

        

    }

    IEnumerator initialiseScreen()
    {
        initialised = true;
        switch (screenNo)
        {
            case 0: Color zm = text1.GetComponent<Text>().color;
                    zm.a = 0f;
                    text1.GetComponent<Text>().color = zm;
                    Color xm = Fallhardy.GetComponent<Text>().color;
                    xm.a = 0f;
                    Fallhardy.GetComponent<Text>().color = xm;

                    yield return new WaitForSeconds(0.1f);
                    zm.a = 0.25f;
                    text1.GetComponent<Text>().color = zm;
                    xm.a = 0.25f;
                    Fallhardy.GetComponent<Text>().color = xm;

                    yield return new WaitForSeconds(0.1f);
                    zm.a = 0.5f;
                    text1.GetComponent<Text>().color = zm;
                    xm.a = 0.5f;
                    Fallhardy.GetComponent<Text>().color = xm;

                    yield return new WaitForSeconds(0.1f);
                    zm.a = 0.75f;
                    text1.GetComponent<Text>().color = zm;
                    xm.a = 0.75f;
                    Fallhardy.GetComponent<Text>().color = xm;

                    yield return new WaitForSeconds(0.1f);
                    zm.a = 1f;
                    text1.GetComponent<Text>().color = zm;
                    xm.a = 1f;
                    Fallhardy.GetComponent<Text>().color = xm;
                    yield return new WaitForSeconds(0.5f);
                    continueReady = true;
            break;
            case 1: Color zm1 = text1.GetComponent<Text>().color;
                    zm1.a = 1f;
                    text1.GetComponent<Text>().color = zm1;
                    Color xm1 = Fallhardy.GetComponent<Text>().color;
                    xm1.a = 1f;
                    Fallhardy.GetComponent<Text>().color = xm1;

                    yield return new WaitForSeconds(0.1f);
                    zm1.a = 0.75f;
                    text1.GetComponent<Text>().color = zm1;
                    xm1.a = 0.75f;
                    Fallhardy.GetComponent<Text>().color = xm1;

                    yield return new WaitForSeconds(0.1f);
                    zm1.a = 0.5f;
                    text1.GetComponent<Text>().color = zm1;
                    xm1.a = 0.5f;
                    Fallhardy.GetComponent<Text>().color = xm1;

                    yield return new WaitForSeconds(0.1f);
                    zm1.a = 0.25f;
                    text1.GetComponent<Text>().color = zm1;
                    xm1.a = 0.25f;
                    Fallhardy.GetComponent<Text>().color = xm1;

                    yield return new WaitForSeconds(0.1f);
                    zm1.a = 0f;
                    text1.GetComponent<Text>().color = zm1;
                    xm1.a = 0f;
                    Fallhardy.GetComponent<Text>().color = xm1;

                    // FADE IN
                    Color am = text2.GetComponent<Text>().color;
                    am.a = 0f;
                    text2.GetComponent<Text>().color = am;

                    yield return new WaitForSeconds(0.1f);
                    am.a = 0.25f;
                    text2.GetComponent<Text>().color = am;

                    yield return new WaitForSeconds(0.1f);
                    am.a = 0.5f;
                    text2.GetComponent<Text>().color = am;

                    yield return new WaitForSeconds(0.1f);
                    am.a = 0.75f;
                    text2.GetComponent<Text>().color = am;

                    yield return new WaitForSeconds(0.1f);
                    am.a = 1f;
                    text2.GetComponent<Text>().color = am;
                    
                    Vector3 pos = Arrow1.transform.position;
                    Debug.Log(pos);
                    pos.y = pos.y +30;
                    Arrow1.transform.position = pos;
                    yield return new WaitForSeconds(0.5f);
                    continueReady = true;

            break;
            case 2: Color zm2 = text2.GetComponent<Text>().color;
                    zm2.a = 1f;
                    text2.GetComponent<Text>().color = zm2;

                    yield return new WaitForSeconds(0.1f);
                    zm2.a = 0.75f;
                    text2.GetComponent<Text>().color = zm2;

                    yield return new WaitForSeconds(0.1f);
                    zm2.a = 0.5f;
                    text2.GetComponent<Text>().color = zm2;

                    yield return new WaitForSeconds(0.1f);
                    zm2.a = 0.25f;
                    text2.GetComponent<Text>().color = zm2;

                    yield return new WaitForSeconds(0.1f);
                    zm2.a = 0f;
                    text2.GetComponent<Text>().color = zm2;

                    yield return new WaitForSeconds(0.5f);
                    GameObject jb = GameObject.Find("Jukebox");
                    if (jb != null)
                    {
                        jb.GetComponent<AudioSource>().Stop();
                    }

                    SceneManager.LoadSceneAsync("Author");
            break;


            default:Debug.Log("Done");
            break;
        }
        yield return null;
    }

    IEnumerator makeArrowVisible()
    {
        arrowVisible = true;
        Color ym = Arrow1.GetComponent<Text>().color;
        ym.a = 1f;
        Arrow1.GetComponent<Text>().color = ym;
        yield return new WaitForSeconds(0.5f);
        ym.a = 0f;
        Arrow1.GetComponent<Text>().color = ym;
        yield return new WaitForSeconds(0.5f);
        arrowVisible = false;
    }

    IEnumerator waitAMo()
    {
        yield return new WaitForSeconds(0.25f);
        ready = true;
        yield return null;
    }
}

