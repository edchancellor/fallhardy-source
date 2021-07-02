using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InitialiseAccept : MonoBehaviour
{

    public GameObject accept;
    public GameObject deny;
    // Start is called before the first frame update
    void Start()
    {
        accept.GetComponent<Text>().enabled = false;
        deny.GetComponent<Text>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
