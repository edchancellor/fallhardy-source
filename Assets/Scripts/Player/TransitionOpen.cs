using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TransitionOpen : MonoBehaviour
{
    public GameObject g1;
    public GameObject g2;
    public GameObject g3;
    public GameObject g4;
    public GameObject g5;
    public GameObject g6;
    public GameObject g7;
    public GameObject g8;
    public GameObject g9;
    public GameObject g10;
    public GameObject g11;
    public GameObject g12;
    public GameObject g13;
    public GameObject g14;
    public GameObject g15;
    public GameObject g16;
    public GameObject g17;
    public GameObject g18;
    public GameObject g19;

    public bool closed = false;
    public bool transitioning = true;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Open());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CloseScene()
    {
        StartCoroutine(Close());
    }

    IEnumerator Close()
    {
        transitioning = true;
        g4.GetComponent<Image>().enabled = true;
        g3.GetComponent<Image>().enabled = true;
        g2.GetComponent<Image>().enabled = true;
        g1.GetComponent<Image>().enabled = true;
        yield return new WaitForSecondsRealtime(0.05f);

        g8.GetComponent<Image>().enabled = true;
        g7.GetComponent<Image>().enabled = true;
        g6.GetComponent<Image>().enabled = true;
        g5.GetComponent<Image>().enabled = true;
        yield return new WaitForSecondsRealtime(0.05f);

        g12.GetComponent<Image>().enabled = true;
        g11.GetComponent<Image>().enabled = true;
        g10.GetComponent<Image>().enabled = true;
        g9.GetComponent<Image>().enabled = true;
        yield return new WaitForSecondsRealtime(0.05f);

        g16.GetComponent<Image>().enabled = true;
        g15.GetComponent<Image>().enabled = true;
        g14.GetComponent<Image>().enabled = true;
        g13.GetComponent<Image>().enabled = true;
        yield return new WaitForSecondsRealtime(0.05f);

        g19.GetComponent<Image>().enabled = true;
        g18.GetComponent<Image>().enabled = true;
        g17.GetComponent<Image>().enabled = true;
        yield return new WaitForSecondsRealtime(0.05f);
        closed = true;
        yield return null;
    }

    IEnumerator Open()
    {
        transitioning = true;
        if(SceneManager.GetActiveScene().buildIndex != 14) // This if statement for slow loading of scene 7
        {
            yield return new WaitForSecondsRealtime(0.2f);
        }
        yield return new WaitForSecondsRealtime(0.1f);
        g19.GetComponent<Image>().enabled = false;
        g18.GetComponent<Image>().enabled = false;
        g17.GetComponent<Image>().enabled = false;
        yield return new WaitForSecondsRealtime(0.05f);

        g16.GetComponent<Image>().enabled = false;
        g15.GetComponent<Image>().enabled = false;
        g14.GetComponent<Image>().enabled = false;
        g13.GetComponent<Image>().enabled = false;
        yield return new WaitForSecondsRealtime(0.05f);

        g12.GetComponent<Image>().enabled = false;
        g11.GetComponent<Image>().enabled = false;
        g10.GetComponent<Image>().enabled = false;
        g9.GetComponent<Image>().enabled = false;
        yield return new WaitForSecondsRealtime(0.05f);

        g8.GetComponent<Image>().enabled = false;
        g7.GetComponent<Image>().enabled = false;
        g6.GetComponent<Image>().enabled = false;
        g5.GetComponent<Image>().enabled = false;
        yield return new WaitForSecondsRealtime(0.05f);

        g4.GetComponent<Image>().enabled = false;
        g3.GetComponent<Image>().enabled = false;
        g2.GetComponent<Image>().enabled = false;
        g1.GetComponent<Image>().enabled = false;
        transitioning = false;
        yield return null;
    }
}
