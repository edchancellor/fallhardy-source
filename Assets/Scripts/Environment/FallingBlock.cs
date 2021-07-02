using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingBlock : MonoBehaviour
{
    Rigidbody2D rb;
    bool falling;
    bool interactable = true;
    float speed = -1f;
    public BoxCollider2D floorDetection;
    public AudioClip impact;

    private AudioSource audio;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        falling = false;
        audio = gameObject.GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (falling && interactable)
        {
            
            //rb.bodyType = RigidbodyType2D.Kinematic;
            rb.velocity = new Vector3(0, speed, 0);
            speed = speed - 0.1f;
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        string name = col.tag;
        bool hitGround = name == "Ground" || name == "ShiftingBlock" || name == "MovingBlock" || name == "FallingBlock";
        float height = col.transform.position.y;

        float diff = Math.Abs(height - gameObject.transform.position.y);

        if (!hitGround && interactable && col.tag != "Wall")
        {
            StartCoroutine(waitABit());
        }
        else if(falling && hitGround && interactable && height < gameObject.transform.position.y) // This line might be a problem
        {
            if(name == "Ground" || (diff < 1.2f && diff > 0.8f))
            {
                if(gameObject.transform.position.y > -6.5)
                {
                    audio.PlayOneShot(impact, 0.05f);
                }
                
                Debug.Log(diff);
                falling = false;
                interactable = false;
                //rb.bodyType = RigidbodyType2D.Static;
                rb.velocity = new Vector2(0,0);
                Vector2 pos = gameObject.transform.position;
                pos.y = (float)Math.Round(pos.y * 2f) * 0.5f;
                gameObject.transform.position = pos;

                transform.parent = col.gameObject.transform;
                floorDetection.enabled = false;
            }

        }
    }



    IEnumerator waitABit()
    {
        yield return new WaitForSeconds(1f);
        falling = true;
        floorDetection.enabled = true;
        rb.bodyType = RigidbodyType2D.Kinematic;
    }

}
