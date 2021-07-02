using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Author : MonoBehaviour
{
    public GameObject authors;
    public GameObject zapsplat;

    public AudioClip ring;

    private AudioSource audio;

    // Start is called before the first frame update
    void Start()
    {
        audio = gameObject.GetComponent<AudioSource>();
        Text text = authors.GetComponent<Text>();
        text.enabled = false;
        Text ztext = zapsplat.GetComponent<Text>();
        ztext.enabled = false;
        StartCoroutine(Display(text, ztext));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Display(Text text, Text ztext)
    {
        yield return new WaitForSeconds(0.2f);
        audio.PlayOneShot(ring, 0.05f);
        Color zm = text.color;
        zm.a = 0f;
        text.color = zm;
        ztext.color = zm;
        ztext.enabled = true;
        text.enabled = true;

        yield return new WaitForSeconds(0.1f);
        zm.a = 0.25f;
        text.color = zm;
        ztext.color = zm;

        yield return new WaitForSeconds(0.1f);
        zm.a = 0.5f;
        text.color = zm;
        ztext.color = zm;

        yield return new WaitForSeconds(0.1f);
        zm.a = 0.75f;
        text.color = zm;
        ztext.color = zm;

        yield return new WaitForSeconds(0.1f);
        zm.a = 1f;
        text.color = zm;
        ztext.color = zm;

        //PAUSE
        yield return new WaitForSeconds(2.5f);
        zm.a = 1f;
        text.color = zm;
        ztext.color = zm;

        yield return new WaitForSeconds(0.1f);
        zm.a = 0.75f;
        text.color = zm;
        ztext.color = zm;

        yield return new WaitForSeconds(0.1f);
        zm.a = 0.5f;
        text.color = zm;
        ztext.color = zm;

        yield return new WaitForSeconds(0.1f);
        zm.a = 0.25f;
        text.color = zm;
        ztext.color = zm;

        yield return new WaitForSeconds(0.1f);
        zm.a = 0f;
        text.color = zm;
        ztext.color = zm;

        text.enabled = false;
        ztext.enabled = false;
        SceneManager.LoadSceneAsync("Menu");
    }
}
