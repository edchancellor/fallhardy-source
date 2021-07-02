using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Gate : MonoBehaviour
{
    public AudioClip gate;

    private AudioSource audio;
    
    // Start is called before the first frame update
    void Start()
    {
        TilemapRenderer tm = gameObject.GetComponent<TilemapRenderer>();
        tm.enabled = false;
        audio = gameObject.GetComponent<AudioSource>();
    }

    public void close()
    {
        StartCoroutine(actualClose(3));
    }

    public void open()
    {
        audio.PlayOneShot(gate, 0.1f);
        StartCoroutine(actualClose(-3));
    }

    IEnumerator actualClose(float distance)
    {
        Vector3 currentPos = transform.localPosition;
        Vector3 newPos = new Vector3(currentPos.x, currentPos.y - distance, currentPos.z);

        while(transform.localPosition != newPos)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, newPos, 6f * Time.deltaTime);
            yield return null;
        }
    }
}
