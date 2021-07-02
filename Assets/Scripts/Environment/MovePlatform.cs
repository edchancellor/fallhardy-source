using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlatform : MonoBehaviour
{
    public Vector3 highest;
    public Vector3 lowest;
    public float speed;
    public bool upwards = true;
    Rigidbody2D rb;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        rb.velocity = new Vector3(0, speed, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (upwards)
        {
            if (gameObject.transform.position.y >= highest.y)
            {
                upwards = false;
                rb.velocity = new Vector3(0, -speed, 0);
            }
        }
        else if (!upwards)
        {
            if (gameObject.transform.position.y <= lowest.y)
            {
                upwards = true;
                rb.velocity = new Vector3(0, speed, 0);
            }
        }
        
    }
}
