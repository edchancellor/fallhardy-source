using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class InitiateBoss1 : MonoBehaviour
{

    public AudioClip thump;
    public AudioClip boss_music;
    public AudioClip normal;

    void Awake()
    {
        AudioSource jukebox = GameObject.Find("Jukebox").GetComponent<AudioSource>();
        jukebox.clip = normal;
        if (!jukebox.isPlaying)
        {
            jukebox.Play();   
        }
    }
    
    bool onlyOnce = true;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (onlyOnce)
        {
            AudioSource jukebox = GameObject.Find("Jukebox").GetComponent<AudioSource>();
            jukebox.clip = boss_music;
            if (!jukebox.isPlaying)
            {
                jukebox.Play();   
            }
            onlyOnce = false;
            Debug.Log("Boss fight 1 start");
            GameObject head = GameObject.Find("Boss1_Head");
            GameObject left = GameObject.Find("Boss1_Left");
            GameObject right = GameObject.Find("Boss1_Right");
            GameObject left_eye = GameObject.Find("Left_Eye");
            GameObject right_eye = GameObject.Find("Right_Eye");
            GameObject gate = GameObject.Find("Gate");
            TilemapRenderer tm = gate.GetComponent<TilemapRenderer>();
            tm.enabled = true;
            Gate gscript = gate.GetComponent<Gate>();
            gscript.close();

            head.AddComponent<Boss1Head>();
            left.AddComponent<Boss1Hand>();
            left.GetComponent<Boss1Hand>().startPlace = new Vector3(-9.5f, 3, 0);
            left.GetComponent<Boss1Hand>().startPlace2 = new Vector3(-11f, 0, 0);
            left.GetComponent<Boss1Hand>().associatedEar = GameObject.Find("Boss1_Left_Ear");
            right.AddComponent<Boss1Hand>();
            right.GetComponent<Boss1Hand>().startPlace = new Vector3(8.5f, 3, 0);
            right.GetComponent<Boss1Hand>().startPlace2 = new Vector3(10f, 0, 0);
            right.GetComponent<Boss1Hand>().associatedEar = GameObject.Find("Boss1_Right_Ear");

            left.GetComponent<Boss1Hand>().thump = thump;
            right.GetComponent<Boss1Hand>().thump = thump;
            head.GetComponent<Boss1Head>().thump = thump;
            left_eye.GetComponent<Animator>().SetTrigger("Awaken");
            right_eye.GetComponent<Animator>().SetTrigger("Awaken");

            gameObject.SetActive(false);
        }
    }
}
