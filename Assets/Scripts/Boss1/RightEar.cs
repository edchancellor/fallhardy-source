using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightEar : MonoBehaviour
{
    public string type;
    public bool triggered = false;
    public AudioClip hit;
    public AudioClip victory;

    private AudioSource audio;
    
    void Start()
    {
        GameObject.Find("BrokenFlash").GetComponent<SpriteRenderer>().enabled = false;
        GameObject.Find("BrokenFlash2").GetComponent<SpriteRenderer>().enabled = false;
        GameObject.Find("BrokenFlash3").GetComponent<SpriteRenderer>().enabled = false;
        audio = gameObject.GetComponent<AudioSource>();
    }
    
    /*private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("hi there " + collision.gameObject.tag);
        if (collision.gameObject.tag == "Spear" && !triggered)
        {
            triggered = true;
            if (type == "Right")
            {
                GameObject head = GameObject.Find("Boss1_Head");
                head.GetComponent<Boss1Head>().rightButton = true;
            }
            else if (type == "Left")
            {
                GameObject head = GameObject.Find("Boss1_Head");
                head.GetComponent<Boss1Head>().leftButton = true;
            }
            else if (type == "Middle")
            {
                GameObject head = GameObject.Find("Boss1_Head");
                head.GetComponent<Boss1Head>().middleButton = true;
            }
            
        }
    }*/

    public void activateEar()
    {
            triggered = true;
            if (type == "Right")
            {
                audio.PlayOneShot(hit, 0.1f);
                GameObject head = GameObject.Find("Boss1_Head");
                Boss1Head b1h = head.GetComponent<Boss1Head>();
                b1h.recoil1 = true;
                b1h.rightButton = true;
                gameObject.GetComponent<Animator>().SetTrigger("Broken");
                GameObject.Find("BrokenFlash2").GetComponent<SpriteRenderer>().enabled = true;
            }
            else if (type == "Left")
            {
                audio.PlayOneShot(hit, 0.1f);
                GameObject head = GameObject.Find("Boss1_Head");
                Boss1Head b1h = head.GetComponent<Boss1Head>();
                b1h.recoil2 = true;
                b1h.leftButton = true;
                gameObject.GetComponent<Animator>().SetTrigger("Broken");
                GameObject.Find("BrokenFlash").GetComponent<SpriteRenderer>().enabled = true;
            }
            else if (type == "Middle")
            {
                audio.PlayOneShot(hit, 0.1f);
                audio.PlayOneShot(victory, 0.1f);
                GameObject head = GameObject.Find("Boss1_Head");
                head.GetComponent<Boss1Head>().middleButton = true;
                gameObject.GetComponent<Animator>().SetTrigger("Broken");
                GameObject.Find("BrokenFlash3").GetComponent<SpriteRenderer>().enabled = true;
            }
    }
}
