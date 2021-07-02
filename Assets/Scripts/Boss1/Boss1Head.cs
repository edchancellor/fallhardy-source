using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1Head : MonoBehaviour
{
    public bool rightButton;
    public bool leftButton;
    public bool middleButton;
    public int round;
    private bool startingPosition;
    private Vector3 startPlace;

    private float RotateSpeed = 2f;
    private float Radius = 0.3f;
    private Vector3 _centre;
    private float _angle;
    private float _fixedAngle;

    private Vector3 upperlimit;
    private Vector3 lowerlimit;
    private bool upwards = true;
    private float updownspeed;
    private GameObject _player;
    private bool round4init = false;
    private float killerspeed;
    private bool jawcrush = false;
    private bool foundFixedAngle = false;
    private bool crushOnce = true;
    private bool endOnce = true;
    private bool angryOnce = true;
    GameObject left_eye; 
    GameObject right_eye;
    public bool rotating;
    Color original; 
    public bool recoil1;
    public bool recoil2;

    private bool recoil1Once;
    private bool recoil2Once;
    private bool angry = false;
    private bool endspinning = true;

    private bool orbInstantiated = false;
    private float orbTime;

    public AudioClip thump;

    private AudioSource audio;

    // Start is called before the first frame update
    void Start()
    {
        rightButton = false;
        leftButton = false;
        middleButton = false;
        startingPosition = false;
        round = 1;
        startPlace = new Vector3(-0.5f, 0f, 0f);
        upperlimit = new Vector3(-0.5f, 1f, 0f);
        lowerlimit = new Vector3(-0.5f, -1f, 0f);
        updownspeed = 2f;
        _player = GameObject.Find("Player");
        killerspeed = 5.5f;
        left_eye = GameObject.Find("Left_Eye");
        right_eye = GameObject.Find("Right_Eye");
        rotating = false;
        original = left_eye.GetComponent<SpriteRenderer>().color;
        recoil1 = false;
        recoil2 = false;
        recoil1Once = true;
        recoil2Once = true;
        orbTime = 4f;
        audio = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(recoil1 && recoil1Once)
        {
            recoil1 = false;
            recoil1Once = false;
            StartCoroutine(HitBySpear(2f));
        }

        else if (recoil2 && recoil2Once)
        {
            recoil2 = false;
            recoil2Once = false;
            StartCoroutine(HitBySpear(-2f));
        }
        
        // Phase 1
        else if (round == 1 && !startingPosition && recoil1Once && recoil2Once)
        {
            StartCoroutine(GoToStartingPosition());
            startingPosition = true;
        }

        // Phase 2
        else if (round == 2 && recoil1Once && recoil2Once)
        {
            normalMove();

            // Move on to next phase?
            if (leftButton || rightButton)
            {
                Debug.Log("First button hit");
                RotateSpeed = 2.5f;
                updownspeed = 2.5f;
                orbTime = 3.5f;
                round = 3;
            }

        }

        // Phase 3
        else if (round == 3 && recoil1Once && recoil2Once)
        {
            if ((!leftButton && rightButton) || (leftButton && !rightButton))
            {
                normalMove();
            }

            else if (leftButton && rightButton)
            {
                if (angryOnce)
                {
                    angryOnce = false;
                    StartCoroutine(AngryAnim());
                    Debug.Log("Second button hit");
                    orbTime = 3f;
                }
            }
        }


        // Phase 4
        else if (round == 4 && recoil1Once && recoil2Once)
        {
            if (!jawcrush)
            {
                _angle += RotateSpeed * Time.deltaTime;
            }
            
            var offset = new Vector3(Mathf.Sin(_angle), Mathf.Cos(_angle), 0) * Radius;
            

            if (_centre.y != 0 && !round4init)
            {
                _centre = Vector3.MoveTowards(_centre, startPlace, updownspeed * Time.deltaTime);
                transform.position = _centre + offset;
            }

            else if (_centre.y == 0 && !round4init)
            {
                round4init = true;
            }

            else if(round4init)
            {
                if (killerspeed < 5.5f)
                {
                    killerspeed = killerspeed + 0.01f;
                }
                
                float xCoord = Mathf.Round(_player.transform.position.x);
                if (xCoord == Mathf.Round(_centre.x) && !jawcrush && !rotating)
                {
                    killerspeed = 0f;
                    jawcrush = true;
                }
                else if (!jawcrush && !rotating)
                {
                    _centre = Vector3.MoveTowards(_centre, new Vector3(xCoord, 0, 0), killerspeed * Time.deltaTime);
                }
                else if (jawcrush)
                {
                    if (!foundFixedAngle)
                    {
                        _fixedAngle = _angle;
                        foundFixedAngle = true;
                    }
                    offset = new Vector3(Mathf.Sin(_fixedAngle), Mathf.Cos(_fixedAngle), 0) * Radius;
                    if (crushOnce)
                    {
                        crushOnce = false;
                        StartCoroutine(Crush());
                    }
                }
                transform.position = _centre + offset;
            } 


            if (middleButton)
            {
                round = 5;
            }
        }


        else if (round == 5 && recoil1Once && recoil2Once)
        {
            Debug.Log("Boss1 Defeated!");
            if (endOnce)
            {
                StartCoroutine(EndFightJaw());
            }
        }

        if(round > 1 && round < 5)
        {
            if (!orbInstantiated)
            {
                orbInstantiated = true;
                StartCoroutine(OrbCoolDown(orbTime));
                StartCoroutine(InstantiateOrb());
            }
            
            Vector3 pos = _player.transform.position;
            float diffx = pos.x - gameObject.transform.position.x;
            float diffy = pos.y - gameObject.transform.position.y;

            ResetEyes();
            
            if (diffx < -3)
            {
                if (diffy < -3)
                {
                    left_eye.GetComponent<Animator>().SetBool("SW", true);
                    right_eye.GetComponent<Animator>().SetBool("SW", true);
                }
                else if (diffy > 3)
                {
                    left_eye.GetComponent<Animator>().SetBool("NW", true);
                    right_eye.GetComponent<Animator>().SetBool("NW", true);
                }
                else
                {
                    left_eye.GetComponent<Animator>().SetBool("Left", true);
                    right_eye.GetComponent<Animator>().SetBool("Left", true);
                }
            }
            else if (diffx > 3)
            {
                if (diffy < -3)
                {
                    left_eye.GetComponent<Animator>().SetBool("SE", true);
                    right_eye.GetComponent<Animator>().SetBool("SE", true);
                }
                else if (diffy > 3)
                {
                    left_eye.GetComponent<Animator>().SetBool("NE", true);
                    right_eye.GetComponent<Animator>().SetBool("NE", true);
                }
                else
                {
                    left_eye.GetComponent<Animator>().SetBool("Right", true);
                    right_eye.GetComponent<Animator>().SetBool("Right", true);
                }
            }
            else
            {
                if (diffy < -3)
                {
                    left_eye.GetComponent<Animator>().SetBool("Down", true);
                    right_eye.GetComponent<Animator>().SetBool("Down", true);
                }
                else if (diffy > 3)
                {
                    left_eye.GetComponent<Animator>().SetBool("Up", true);
                    right_eye.GetComponent<Animator>().SetBool("Up", true);
                }
                else
                {
                    left_eye.GetComponent<Animator>().SetBool("Forward", true);
                    right_eye.GetComponent<Animator>().SetBool("Forward", true);
                }
            }
        }

    }

    private void ResetEyes()
    {
        left_eye.GetComponent<Animator>().SetBool("Up", false);
        left_eye.GetComponent<Animator>().SetBool("Down", false);
        left_eye.GetComponent<Animator>().SetBool("Left", false);
        left_eye.GetComponent<Animator>().SetBool("Right", false);
        left_eye.GetComponent<Animator>().SetBool("Forward", false);
        left_eye.GetComponent<Animator>().SetBool("NE", false);
        left_eye.GetComponent<Animator>().SetBool("NW", false);
        left_eye.GetComponent<Animator>().SetBool("SE", false);
        left_eye.GetComponent<Animator>().SetBool("SW", false);

        right_eye.GetComponent<Animator>().SetBool("Up", false);
        right_eye.GetComponent<Animator>().SetBool("Down", false);
        right_eye.GetComponent<Animator>().SetBool("Left", false);
        right_eye.GetComponent<Animator>().SetBool("Right", false);
        right_eye.GetComponent<Animator>().SetBool("Forward", false);
        right_eye.GetComponent<Animator>().SetBool("NE", false);
        right_eye.GetComponent<Animator>().SetBool("NW", false);
        right_eye.GetComponent<Animator>().SetBool("SE", false);
        right_eye.GetComponent<Animator>().SetBool("SW", false);
    }

    IEnumerator OrbCoolDown(float time)
    {
        yield return new WaitForSeconds(time);
        orbInstantiated = false;
        yield return null;
    }

    IEnumerator InstantiateOrb()
    {
        Vector3 pos = gameObject.transform.position;
        pos = new Vector3(pos.x, pos.y + 0.6f, pos.z);
        GameObject orb = (GameObject)Resources.Load("Prefabs/Orb", typeof(GameObject));
        GameObject actualOrb = Instantiate(orb, pos, Quaternion.identity);
        actualOrb.transform.parent = gameObject.transform;
        yield return null;
    }

    IEnumerator GoToStartingPosition()
    {
        while(transform.localPosition != startPlace)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, startPlace, 3f * Time.deltaTime);
            yield return null;
        }
        Debug.Log("Head in start position");
        round = 2;

        _centre = new Vector3(transform.position.x, transform.position.y - Radius, 0);
        yield return null;
    }

    IEnumerator Crush()
    {
        yield return new WaitForSeconds(0.5f);
        GameObject jaw = GameObject.Find("Boss1_Jaw");
        jaw.GetComponent<Jaw>().goingDown = true;
        jaw.transform.SetParent(null);
        Vector3 jawStart = jaw.transform.position;

        while (jaw.transform.position.y > -9.5)
        {
            if(middleButton)
            {
                break;
            }
            jaw.transform.localPosition = Vector3.MoveTowards(jaw.transform.localPosition, new Vector3(jawStart.x, -9.5f, 0), 10f * Time.deltaTime);
            yield return null;
        }
        jaw.GetComponent<Jaw>().goingDown = false;
        audio.PlayOneShot(thump, 0.1f);
        yield return new WaitForSeconds(0.5f);
        while (jaw.transform.position.y < jawStart.y)
        {
            if (middleButton)
            {
                break;
            }
            jaw.transform.localPosition = Vector3.MoveTowards(jaw.transform.localPosition, jawStart, 5f * Time.deltaTime);
            yield return null;
        }

        jaw.transform.SetParent(gameObject.transform);
        foundFixedAngle = false;
        jawcrush = false;
        Debug.Log("Jawcrush complete.");
        crushOnce = true;
        yield return null;
    }

    IEnumerator AngryAnim()
    {
        angry = true;
        Color eye_color = new Color(0.75f , 0, 0, 1);
        left_eye.GetComponent<SpriteRenderer>().color = eye_color;
        right_eye.GetComponent<SpriteRenderer>().color = eye_color;
        yield return new WaitForSeconds(1f);
        round = 4;
        yield return null;
    }

    IEnumerator EndFightJaw()
    {
        endOnce = false;
        left_eye.GetComponent<Animator>().SetBool("Forward", true);
        right_eye.GetComponent<Animator>().SetBool("Forward", true);
        yield return new WaitForSeconds(2f);
        GameObject jaw = GameObject.Find("Boss1_Jaw");
        jaw.GetComponent<Jaw>().goingDown = true;
        jaw.transform.SetParent(null);
        Vector3 jawStart = jaw.transform.position;

        StartCoroutine(DeathRotateHead());

        while (jaw.transform.position.y > -9.5)
        {
            jaw.transform.localPosition = Vector3.MoveTowards(jaw.transform.localPosition, new Vector3(jawStart.x, -9.5f, 0), 5f * Time.deltaTime);
            yield return null;
        }
        audio.PlayOneShot(thump, 0.1f);
        jaw.GetComponent<Jaw>().goingDown = false;
    }

    IEnumerator DeathRotateHead()
    {
        float speedOfRotation = 10;
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(DestroyHead());
        while(endspinning)
        {
            gameObject.transform.Rotate(new Vector3(0,0,1)*speedOfRotation*Time.deltaTime);
            speedOfRotation = speedOfRotation + 1;
            yield return null;
        }
    }

    IEnumerator DestroyHead()
    {
        yield return new WaitForSeconds(1.5f);

        //gameObject.GetComponent<Renderer>().sortingLayerName = "Door";

        Vector3 currentPos = transform.localPosition;
        Vector3 newPos = new Vector3(currentPos.x, currentPos.y + 25, currentPos.z);

        while(transform.localPosition != newPos)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, newPos, 6f * Time.deltaTime);
            yield return null;
        }

        GameObject.Find("Gate").GetComponent<Gate>().open();

        endspinning = false;
        gameObject.SetActive(false);

    }



    IEnumerator HitBySpear(float direction)
    {

            Color eye_color = new Color(0.75f , 0, 0, 1);
            left_eye.GetComponent<SpriteRenderer>().color = eye_color;
            right_eye.GetComponent<SpriteRenderer>().color = eye_color;

            Vector3 currentPos = transform.localPosition;
            Vector3 newPos = new Vector3(currentPos.x - direction, currentPos.y, currentPos.z);

            while(transform.localPosition != newPos)
            {
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, newPos, 6f * Time.deltaTime);
                yield return null;
            }

            while(transform.localPosition != currentPos)
            {
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, currentPos, 3f * Time.deltaTime);
                yield return null;
            }

            recoil1Once = true;
            recoil2Once = true;
            yield return new WaitForSeconds(0.1f);
            if(!rotating && !angry)
            {
                left_eye.GetComponent<SpriteRenderer>().color = original;
                right_eye.GetComponent<SpriteRenderer>().color = original;
            }
            yield return null;
    }

    private void normalMove()
    {
        _angle += RotateSpeed * Time.deltaTime;

        var offset = new Vector3(Mathf.Sin(_angle), Mathf.Cos(_angle), 0) * Radius;


        if (upwards && _centre.y < upperlimit.y)
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
        }

        transform.position = _centre + offset;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            spin(collision);
        }
    }

    public void spin(Collider2D collision)
    {
            float xPos = collision.transform.position.x;
            float myPos = gameObject.transform.position.x;
            float diff = myPos - xPos;
            if (diff > 0 && !rotating && !jawcrush)
            {
                Debug.Log("turn left");
                rotating = true;
                Rigidbody2D m_Rigidbody2D = collision.gameObject.GetComponent<Rigidbody2D>();
                StartCoroutine(objectRotation(gameObject.transform, 50, m_Rigidbody2D));
            }
            else if (!rotating && !jawcrush)
            {
                Debug.Log("turn right");
                rotating = true;
                Rigidbody2D m_Rigidbody2D = collision.gameObject.GetComponent<Rigidbody2D>();
                StartCoroutine(objectRotation2(gameObject.transform, 50, m_Rigidbody2D));
            }
    }

    IEnumerator objectRotation(Transform subject,float speedOfRotation, Rigidbody2D m_Rigidbody2D)
    {
        Color eye_color = new Color(0.75f , 0, 0, 1);
        left_eye.GetComponent<SpriteRenderer>().color = eye_color;
        right_eye.GetComponent<SpriteRenderer>().color = eye_color;

        yield return new WaitForSeconds(0.2f);
        while(subject.transform.eulerAngles.z < 90)
        {
            m_Rigidbody2D.gravityScale = 3f;
            subject.Rotate(new Vector3(0,0,1)*speedOfRotation*Time.deltaTime);
            yield return null;
        }
        Debug.Log("Here");
        subject.transform.eulerAngles = new Vector3(subject.transform.eulerAngles.x,subject.transform.eulerAngles.y,90);

        while(subject.transform.eulerAngles.z > 3)
        {
            subject.Rotate(new Vector3(0,0,-1)*speedOfRotation*Time.deltaTime);
            yield return null;
            //Debug.Log("hello " + subject.transform.eulerAngles.z);
        }
        Debug.Log("Now Here");

        subject.transform.eulerAngles = new Vector3(subject.transform.eulerAngles.x,subject.transform.eulerAngles.y,0);

        Debug.Log("Done all");
        rotating = false;
        yield return new WaitForSeconds(0.1f);
        if(!rotating)
        {
            left_eye.GetComponent<SpriteRenderer>().color = original;
            right_eye.GetComponent<SpriteRenderer>().color = original;
        }
        yield return null;
    }

    IEnumerator objectRotation2(Transform subject,float speedOfRotation, Rigidbody2D m_Rigidbody2D)
    {
        Color eye_color = new Color(0.75f , 0, 0, 1);
        left_eye.GetComponent<SpriteRenderer>().color = eye_color;
        right_eye.GetComponent<SpriteRenderer>().color = eye_color;
        
        Debug.Log("hello " + subject.transform.eulerAngles.z);
        yield return new WaitForSeconds(0.2f);
        while(subject.transform.eulerAngles.z > 270 || subject.transform.eulerAngles.z == 0)
        {
            m_Rigidbody2D.gravityScale = 3f;
            subject.Rotate(new Vector3(0,0,-1)*speedOfRotation*Time.deltaTime);
            yield return null;
        }
        Debug.Log("Here");
        subject.transform.eulerAngles = new Vector3(subject.transform.eulerAngles.x,subject.transform.eulerAngles.y,270);

        while(subject.transform.eulerAngles.z < 357)
        {
            subject.Rotate(new Vector3(0,0,1)*speedOfRotation*Time.deltaTime);
            yield return null;
        }
        Debug.Log("Now Here");

        subject.transform.eulerAngles = new Vector3(subject.transform.eulerAngles.x,subject.transform.eulerAngles.y,0);

        Debug.Log("Done all");
        rotating = false;
        yield return new WaitForSeconds(0.1f);
        if(!rotating)
        {
            left_eye.GetComponent<SpriteRenderer>().color = original;
            right_eye.GetComponent<SpriteRenderer>().color = original;
        }
        yield return null;
    }
}
