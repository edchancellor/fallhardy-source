using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftRightPlatform : MonoBehaviour
{
    public Vector3 left;
    public Vector3 right;
    public float speed;
    Vector2 nextPos;//
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        nextPos = right;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.localPosition == left)
        {
            nextPos = right;
        }
        else if (transform.localPosition == right)
        {
            nextPos = left;
        }
        Move();

    }

    private void Move()
    {
        
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, nextPos, speed * Time.deltaTime);
    }
}
