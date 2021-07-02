using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1Hand : MonoBehaviour
{
    private int round;
    private bool startingPosition;
    public Vector3 startPlace;
    public Vector3 startPlace2;

    private float RotateSpeed = 2f;
    private float Radius = 0.5f;
    private Vector3 _centre;
    private float _angle;
    private GameObject _player;
    private bool handCrush;
    private float killerspeed;
    private bool crushOnce;
    public GameObject associatedEar;
    private bool playerOnHand;
    private bool dieOnce = false;
    public bool upwards;

    public AudioClip thump;

    private AudioSource audio;

    // Start is called before the first frame update
    void Start()
    {
        round = 1;
        _player = GameObject.Find("Player");
        handCrush = false;
        killerspeed = 0f;
        crushOnce = true;
        playerOnHand = false;
        upwards = false;
        audio = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        // Phase 1
        if (round == 1 && !startingPosition)
        {
            StartCoroutine(GoToStartingPosition());
            startingPosition = true;
        }

        else if (round == 2)
        {
            // Check if the player is close to the hand.
            float playerXCoord = _player.transform.position.x;
            float difference = Mathf.Abs(_centre.x - playerXCoord);
            if(difference < 9 && !(playerXCoord < 5 && playerXCoord > -6))
            {
                Debug.Log("Close");
                if (killerspeed < 5.5f)
                {
                    killerspeed = killerspeed + 0.05f;
                }

                float xCoord = Mathf.Round(playerXCoord);
                if ((xCoord == Mathf.Round(_centre.x) || playerOnHand) && !handCrush)
                {
                    killerspeed = 0f;
                    handCrush = true;
                }
                else if (!handCrush)
                {
                    if (_centre.x < -13 && xCoord < -13)
                    {
                        killerspeed = 0f;
                        handCrush = true;
                    }
                    else if(_centre.x > 12 && xCoord > 12)
                    {
                        killerspeed = 0f;
                        handCrush = true;
                    }
                    else
                    {
                        _centre = Vector3.MoveTowards(_centre, new Vector3(xCoord, 0, 0), killerspeed * Time.deltaTime);
                    }
                    
                    normalMove();
                }

                else if (handCrush)
                {
                    if (crushOnce)
                    {
                        crushOnce = false;
                        StartCoroutine(Crush());
                    }
                }
            }
            else
            {
                killerspeed = 0f;
                if (!handCrush)
                {
                    normalMove();
                }
            }

            if(associatedEar.GetComponent<RightEar>().triggered == true)
            {
                Debug.Log("Enter round 3 of hand");
                round = 3;
            }
            
        }

        else if (round == 3)
        {
            if (!dieOnce)
            {
                dieOnce = true;
                StartCoroutine(die());
            }
        }
    }

    IEnumerator GoToStartingPosition()
    {
        while (transform.localPosition != startPlace)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, startPlace, 3f * Time.deltaTime);
            yield return null;
        }
        while (transform.localPosition != startPlace2)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, startPlace2, 3f * Time.deltaTime);
            yield return null;
        }
        Debug.Log("Hand in start position");
        round = 2;

        _centre = new Vector3(transform.position.x, transform.position.y - Radius, 0);
        yield return null;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerOnHand = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerOnHand = false;
        }
    }
    IEnumerator Crush()
    {
        yield return new WaitForSeconds(0.5f);
        //gameObject.GetComponent<Jaw>().goingDown = true;
        Vector3 handStart = gameObject.transform.position;

        while (gameObject.transform.position.y > -9.5)
        {
            if(round == 3)
            {
                break;
            }
            gameObject.transform.localPosition = Vector3.MoveTowards(gameObject.transform.localPosition, new Vector3(handStart.x, -9.5f, 0), 9f * Time.deltaTime);
            yield return null;
        }
        audio.PlayOneShot(thump, 0.1f);
        //gameObject.GetComponent<Jaw>().goingDown = false;
        yield return new WaitForSeconds(0.5f);
        //gameObject.GetComponent<Jaw>().goingDown = false;
        upwards = true;
        while (gameObject.transform.position.y < handStart.y)
        {
            if (round == 3)
            {
                upwards = false;
                break;
            }
            gameObject.transform.localPosition = Vector3.MoveTowards(gameObject.transform.localPosition, handStart, 3f * Time.deltaTime);
            yield return null;
        }
        upwards = false;
        handCrush = false;
        Debug.Log("HandCrush complete.");
        crushOnce = true;
        yield return null;
    }

    IEnumerator die()
    {
        yield return new WaitForSeconds(1f);
        gameObject.GetComponent<Jaw>().goingDown = true;
        Vector3 handStart = gameObject.transform.position;
        while (gameObject.transform.position.y > -36)
        {
            gameObject.transform.localPosition = Vector3.MoveTowards(gameObject.transform.localPosition, new Vector3(handStart.x, -36, 0), 8.5f * Time.deltaTime);
            yield return null;
        }
        Debug.Log("Hand should die");
        gameObject.SetActive(false);
        yield return null;
    }

    private void normalMove()
    {
        _angle += RotateSpeed * Time.deltaTime;

        var offset = new Vector3(Mathf.Sin(_angle), Mathf.Cos(_angle), 0) * Radius;


        /*if (upwards && _centre.y < upperlimit.y)
        {
            _centre = Vector3.MoveTowards(_centre, upperlimit, updownspeed * Time.deltaTime);
        }
        else if (upwards && _centre.y >= upperlimit.y)
        {
            upwards = false;
        }
        else if (!upwards && _centre.y > lowerlimit.y)
        {
            _centre = Vector3.MoveTowards(_centre, lowerlimit, updownspeed * Time.deltaTime);
        }
        else if (!upwards && _centre.y <= lowerlimit.y)
        {
            upwards = true;
        }*/

        transform.position = _centre + offset;
    }
}
