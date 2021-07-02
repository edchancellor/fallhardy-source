using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Instructions : MonoBehaviour
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
    public GameObject ArrowKeys;
    public GameObject text3;
    public GameObject Spears;
    public GameObject text4;
    public GameObject LevelNames;

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

        Color bm = ArrowKeys.GetComponent<Text>().color;
        bm.a = 0f;
        ArrowKeys.GetComponent<Text>().color = bm;

        Color cm = text3.GetComponent<Text>().color;
        cm.a = 0f;
        text3.GetComponent<Text>().color = cm;

        Color dm = Spears.GetComponent<Text>().color;
        dm.a = 0f;
        Spears.GetComponent<Text>().color = dm;

        Color em = text4.GetComponent<Text>().color;
        em.a = 0f;
        text4.GetComponent<Text>().color = em;

        Color fm = LevelNames.GetComponent<Text>().color;
        fm.a = 0f;
        LevelNames.GetComponent<Text>().color = fm;
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
                    Color bm = ArrowKeys.GetComponent<Text>().color;
                    bm.a = 0f;
                    ArrowKeys.GetComponent<Text>().color = bm;

                    yield return new WaitForSeconds(0.1f);
                    am.a = 0.25f;
                    text2.GetComponent<Text>().color = am;
                    bm.a = 0.25f;
                    ArrowKeys.GetComponent<Text>().color = bm;

                    yield return new WaitForSeconds(0.1f);
                    am.a = 0.5f;
                    text2.GetComponent<Text>().color = am;
                    bm.a = 0.5f;
                    ArrowKeys.GetComponent<Text>().color = bm;

                    yield return new WaitForSeconds(0.1f);
                    am.a = 0.75f;
                    text2.GetComponent<Text>().color = am;
                    bm.a = 0.75f;
                    ArrowKeys.GetComponent<Text>().color = bm;

                    yield return new WaitForSeconds(0.1f);
                    am.a = 1f;
                    text2.GetComponent<Text>().color = am;
                    bm.a = 1f;
                    ArrowKeys.GetComponent<Text>().color = bm;
                    
                    Vector3 pos = Arrow1.transform.position;
                    Debug.Log(pos);
                    pos.y = pos.y -30;
                    Arrow1.transform.position = pos;
                    yield return new WaitForSeconds(0.5f);
                    continueReady = true;

            break;
            case 2: Color zm2 = text2.GetComponent<Text>().color;
                    zm2.a = 1f;
                    text2.GetComponent<Text>().color = zm2;
                    Color xm2 = ArrowKeys.GetComponent<Text>().color;
                    xm2.a = 1f;
                    ArrowKeys.GetComponent<Text>().color = xm2;

                    yield return new WaitForSeconds(0.1f);
                    zm2.a = 0.75f;
                    text2.GetComponent<Text>().color = zm2;
                    xm2.a = 0.75f;
                    ArrowKeys.GetComponent<Text>().color = xm2;

                    yield return new WaitForSeconds(0.1f);
                    zm2.a = 0.5f;
                    text2.GetComponent<Text>().color = zm2;
                    xm2.a = 0.5f;
                    ArrowKeys.GetComponent<Text>().color = xm2;

                    yield return new WaitForSeconds(0.1f);
                    zm2.a = 0.25f;
                    text2.GetComponent<Text>().color = zm2;
                    xm2.a = 0.25f;
                    ArrowKeys.GetComponent<Text>().color = xm2;

                    yield return new WaitForSeconds(0.1f);
                    zm2.a = 0f;
                    text2.GetComponent<Text>().color = zm2;
                    xm2.a = 0f;
                    ArrowKeys.GetComponent<Text>().color = xm2;

                    // FADE IN NEED TO CHANGE!!
                    Color am1 = text3.GetComponent<Text>().color;
                    am1.a = 0f;
                    text3.GetComponent<Text>().color = am1;
                    Color bm1 = Spears.GetComponent<Text>().color;
                    bm1.a = 0f;
                    Spears.GetComponent<Text>().color = bm1;

                    yield return new WaitForSeconds(0.1f);
                    am1.a = 0.25f;
                    text3.GetComponent<Text>().color = am1;
                    bm1.a = 0.25f;
                    Spears.GetComponent<Text>().color = bm1;

                    yield return new WaitForSeconds(0.1f);
                    am1.a = 0.5f;
                    text3.GetComponent<Text>().color = am1;
                    bm1.a = 0.5f;
                    Spears.GetComponent<Text>().color = bm1;

                    yield return new WaitForSeconds(0.1f);
                    am1.a = 0.75f;
                    text3.GetComponent<Text>().color = am1;
                    bm1.a = 0.75f;
                    Spears.GetComponent<Text>().color = bm1;

                    yield return new WaitForSeconds(0.1f);
                    am1.a = 1f;
                    text3.GetComponent<Text>().color = am1;
                    bm1.a = 1f;
                    Spears.GetComponent<Text>().color = bm1;
                    
                    yield return new WaitForSeconds(0.5f);
                    continueReady = true;
            break;
            case 3: Color zm3 = text3.GetComponent<Text>().color;
                    zm3.a = 1f;
                    text3.GetComponent<Text>().color = zm3;
                    Color xm3 = Spears.GetComponent<Text>().color;
                    xm3.a = 1f;
                    Spears.GetComponent<Text>().color = xm3;

                    yield return new WaitForSeconds(0.1f);
                    zm3.a = 0.75f;
                    text3.GetComponent<Text>().color = zm3;
                    xm3.a = 0.75f;
                    Spears.GetComponent<Text>().color = xm3;

                    yield return new WaitForSeconds(0.1f);
                    zm3.a = 0.5f;
                    text3.GetComponent<Text>().color = zm3;
                    xm3.a = 0.5f;
                    Spears.GetComponent<Text>().color = xm3;

                    yield return new WaitForSeconds(0.1f);
                    zm3.a = 0.25f;
                    text3.GetComponent<Text>().color = zm3;
                    xm3.a = 0.25f;
                    Spears.GetComponent<Text>().color = xm3;

                    yield return new WaitForSeconds(0.1f);
                    zm3.a = 0f;
                    text3.GetComponent<Text>().color = zm3;
                    xm3.a = 0f;
                    Spears.GetComponent<Text>().color = xm3;

                    // FADE IN NEED TO CHANGE!!
                    Color am4 = text4.GetComponent<Text>().color;
                    am4.a = 0f;
                    text4.GetComponent<Text>().color = am4;
                    Color bm4 = LevelNames.GetComponent<Text>().color;
                    bm4.a = 0f;
                    LevelNames.GetComponent<Text>().color = bm4;

                    yield return new WaitForSeconds(0.1f);
                    am4.a = 0.25f;
                    text4.GetComponent<Text>().color = am4;
                    bm4.a = 0.25f;
                    LevelNames.GetComponent<Text>().color = bm4;

                    yield return new WaitForSeconds(0.1f);
                    am4.a = 0.5f;
                    text4.GetComponent<Text>().color = am4;
                    bm4.a = 0.5f;
                    LevelNames.GetComponent<Text>().color = bm4;

                    yield return new WaitForSeconds(0.1f);
                    am4.a = 0.75f;
                    text4.GetComponent<Text>().color = am4;
                    bm4.a = 0.75f;
                    LevelNames.GetComponent<Text>().color = bm4;

                    yield return new WaitForSeconds(0.1f);
                    am4.a = 1f;
                    text4.GetComponent<Text>().color = am4;
                    bm4.a = 1f;
                    LevelNames.GetComponent<Text>().color = bm4;

                    Vector3 pos1 = Arrow1.transform.position;
                    Debug.Log(pos1);
                    pos1.y = pos1.y -20;
                    Arrow1.transform.position = pos1;
                    
                    yield return new WaitForSeconds(0.5f);
                    continueReady = true;
            break;
            case 4: Color am5 = text4.GetComponent<Text>().color;
                    am5.a = 1f;
                    text4.GetComponent<Text>().color = am5;
                    Color bm5 = LevelNames.GetComponent<Text>().color;
                    bm5.a = 1f;
                    LevelNames.GetComponent<Text>().color = bm5;

                    yield return new WaitForSeconds(0.1f);
                    am5.a = 0.75f;
                    text4.GetComponent<Text>().color = am5;
                    bm5.a = 0.75f;
                    LevelNames.GetComponent<Text>().color = bm5;

                    yield return new WaitForSeconds(0.1f);
                    am5.a = 0.5f;
                    text4.GetComponent<Text>().color = am5;
                    bm5.a = 0.5f;
                    LevelNames.GetComponent<Text>().color = bm5;

                    yield return new WaitForSeconds(0.1f);
                    am5.a = 0.25f;
                    text4.GetComponent<Text>().color = am5;
                    bm5.a = 0.25f;
                    LevelNames.GetComponent<Text>().color = bm5;

                    yield return new WaitForSeconds(0.1f);
                    am5.a = 0f;
                    text4.GetComponent<Text>().color = am5;
                    bm5.a = 0f;
                    LevelNames.GetComponent<Text>().color = bm5;
                    yield return new WaitForSeconds(0.5f);


                    SceneManager.LoadSceneAsync("Scene1");
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
