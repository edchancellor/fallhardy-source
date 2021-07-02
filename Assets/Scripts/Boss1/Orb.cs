using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orb : MonoBehaviour
{
    public AudioClip charge;
    public AudioClip fire;

    private AudioSource audio;
    
    // Start is called before the first frame update
    void Start()
    {
        audio = gameObject.GetComponent<AudioSource>();
        StartCoroutine(OrbBehaviour());
    }

    // Update is called once per frame
    void Update()
    {
        GameObject head = GameObject.Find("Boss1_Head");
        int headRound = head.GetComponent<Boss1Head>().round;
        Vector3 pos = gameObject.transform.position;
        if (pos.x < -25f || pos.x > 25f || pos.y < -25f || pos.y > 25f)
        {
            gameObject.SetActive(false);
        }
        if (gameObject.transform.parent == null && headRound == 5)
        {
            gameObject.SetActive(false);
        }
    }

    IEnumerator OrbBehaviour()
    {
        audio.PlayOneShot(charge, 0.05f);
        Animator anim = gameObject.GetComponent<Animator>();
        yield return new WaitForSeconds(1f);
        anim.SetInteger("Phase", 1);
        audio.PlayOneShot(charge, 0.05f);
        yield return new WaitForSeconds(1f);
        anim.SetInteger("Phase", 2);
        audio.PlayOneShot(charge, 0.05f);
        yield return new WaitForSeconds(1f);
        audio.PlayOneShot(fire, 0.1f);
        GameObject _player = GameObject.Find("Player");
        Vector3 player_pos = _player.transform.position;

        gameObject.transform.parent = null;
        Vector3 myPos = gameObject.transform.position;
        Debug.Log(player_pos);
        Debug.Log(myPos);

        Vector3 direction = player_pos - myPos;
        direction = direction.normalized;

        Debug.Log("I should go:" + direction);

        Rigidbody2D rigidbody = gameObject.GetComponent<Rigidbody2D>();

        rigidbody.AddForce(direction * 700f);
    }
}
