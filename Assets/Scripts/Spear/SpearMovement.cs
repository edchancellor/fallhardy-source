using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearMovement : MonoBehaviour
{
    // Start is called before the first frame update
    bool embedded = false;
    public BoxCollider2D spearhead;
    public BoxCollider2D shaft;
    public BoxCollider2D shaftcollision;
    Rigidbody2D rb;
    public AudioClip embed;

    private AudioSource audio;

    void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        rb.velocity = new Vector3(8,0,0);
    }

    void Start()
    {
        audio = gameObject.GetComponent<AudioSource>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!embedded && collision.tag != "Player" && collision.tag != "Door" && collision.tag != "Orb")
        {
            Debug.Log(collision.name);
            
            if(collision.name == "Boss1_Right_Ear" || collision.name == "Boss1_Left_Ear" || collision.name == "Boss1_Middle")
            {
                Debug.Log("Gotcha");
                collision.gameObject.GetComponent<RightEar>().activateEar();
            }
            
            audio.PlayOneShot(embed, 0.2f);
            embedded = true;
            Debug.Log(rb.velocity);
            rb.velocity = new Vector3(0, 0, 0);
            Animator anim = gameObject.GetComponent<Animator>();
            anim.SetInteger("AnimationState", 1);
            Vector3 pos = gameObject.transform.position;
            pos.x = pos.x + 0.1f;
            gameObject.transform.position = pos;
            rb.bodyType = RigidbodyType2D.Kinematic;
            spearhead.enabled = false;
            shaft.enabled = true;
            shaftcollision.enabled = true;
            if (collision.gameObject.tag == "Spear")
            {
                transform.parent = collision.gameObject.transform.parent.transform;
            }
            else
            {
                transform.parent = collision.gameObject.transform;
            }
            
        }
    }
}
