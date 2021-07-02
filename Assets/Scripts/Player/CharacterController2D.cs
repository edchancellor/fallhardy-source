using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterController2D : MonoBehaviour
{
	[SerializeField] private float m_JumpForce = 400f;							// Amount of force added when the player jumps.
	[Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;	// How much to smooth out the movement
	[SerializeField] private bool m_AirControl = false;							// Whether or not a player can steer while jumping;
	[SerializeField] private LayerMask m_WhatIsGround;							// A mask determining what is ground to the character
	[SerializeField] private Transform m_GroundCheck;							// A position marking where to check if the player is grounded.
	[SerializeField] private Transform m_CeilingCheck;							// A position marking where to check for ceilings
    [SerializeField] private float m_AirControlReduce = 0.08f;
	
	const float k_GroundedRadius = 0.1f; // Radius of the overlap circle to determine if grounded
	public bool m_Grounded;            // Whether or not the player is grounded.
	const float k_CeilingRadius = .1f; // Radius of the overlap circle to determine if the player can stand up
	private Rigidbody2D m_Rigidbody2D;
	private bool m_FacingRight = true;  // For determining which way the player is currently facing.
	private Vector3 m_Velocity = Vector3.zero;
    private Vector3 actualVelocity;
    public bool isAlive = true;
    private double decelerationTolerance = 17.0;
    private int jumpAllowance;
    private bool airJumpAllowed = true;
    private bool jumpsEnabled = true;
    private bool throwsEnabled = true;
    private bool dieOnce = true;
    private bool squished = false;
    private Animator anim;

    public AudioClip jumpSound;
    public AudioClip deathSound;
    public AudioClip throwSound;
    public AudioClip throwRejectSound;
    public bool jumpSoundAllowed;

    private AudioSource audio;

    [Header("Events")]
	[Space]

	public UnityEvent OnLandEvent;

	[System.Serializable]
	public class BoolEvent : UnityEvent<bool> { }

    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        audio = gameObject.GetComponent<AudioSource>();

        StartCoroutine(jumpSounds());
    }

    IEnumerator jumpSounds()
    {
        yield return new WaitForSeconds(0.2f);
        jumpSoundAllowed = true;
    }

	private void Awake()
	{
		m_Rigidbody2D = GetComponent<Rigidbody2D>();

        if (OnLandEvent == null)
			OnLandEvent = new UnityEvent();
	}

    private void Update()
    {
        if (!m_Grounded)
        {
            m_Rigidbody2D.gravityScale = 3f;
        }
    }

	private void FixedUpdate()
	{


        if (!m_Grounded)
        {
            m_Rigidbody2D.gravityScale = 3f;
        }

        if(gameObject.transform.eulerAngles.z != 0)
        {
            gameObject.transform.eulerAngles = new Vector3(gameObject.transform.eulerAngles.x,gameObject.transform.eulerAngles.y,0);
        }
        
        
        // For checking if the player is alive from fall damage.
        if (isAlive)
        {
            //isAlive = Vector3.Distance(m_Rigidbody2D.velocity, actualVelocity) < decelerationTolerance;
            isAlive = m_Rigidbody2D.velocity.y - actualVelocity.y < decelerationTolerance;
            actualVelocity = m_Rigidbody2D.velocity;
        }
        else if (!isAlive || anim.GetBool("Alive") == false)
        {
            gameObject.GetComponent<PlayerMovement>().movementAllowed = false;
            if (m_Grounded)
            {
                m_Rigidbody2D.gravityScale = 100f;
            }
            //m_Rigidbody2D.gravityScale = 3f;

            StartCoroutine(Death());
        }


        bool wasGrounded = m_Grounded;
		m_Grounded = false;

        // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
        // This can be done using layers instead but Sample Assets will not overwrite your project settings.

        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
		for (int i = 0; i < colliders.Length; i++)
		{
            Debug.Log(colliders[i] + "hello");
            if (colliders[i].gameObject != gameObject)
			{
				m_Grounded = true;
                airJumpAllowed = true;
                gameObject.GetComponent<PlayerMovement>().resetAirJumps();

                
				if (!wasGrounded)
					OnLandEvent.Invoke();
			}
		}
	}


	public void Move(float move, bool jump, int allowance, bool throwSpear)
	{
        if (anim.IsInTransition(0) && anim.GetCurrentAnimatorStateInfo(0).IsName("Player_Throw"))
        {
            anim.SetBool("Throwing", false);
        }
            
        if (throwSpear)
        {
            if (m_FacingRight)
            {
                Vector3 pos = gameObject.transform.position;
                pos.y = pos.y + 0.2f;
                pos.x = pos.x + 0.13f;
                RaycastHit2D spaceForThrow = Physics2D.Raycast(pos, new Vector2(1, 0), 1.3f); //1.3f

                bool overallTest = false;
                bool test = spaceForThrow.collider == null;
                if (!test)
                {
                    if (spaceForThrow.collider.name == "Player" || spaceForThrow.collider.name == "Door")
                    {
                        overallTest = true;
                    }
                }

                
                if (test && throwsEnabled || overallTest && throwsEnabled)
                {
                    audio.PlayOneShot(throwSound, 0.05f);
                    anim.SetBool("Throwing", true);
                    throwsEnabled = false;
                    StartCoroutine(throwCooldown());
                    pos.x = pos.x + 0.5f; // 0.9
                    GameObject spear = (GameObject)Resources.Load("Prefabs/SpearRight", typeof(GameObject));
                    GameObject actualSpear = Instantiate(spear, pos, Quaternion.identity);
                    int layer = gameObject.GetComponent<PlayerMovement>().noOfSpears;
                    if (layer == 3)
                    {
                        actualSpear.GetComponent<Renderer>().sortingLayerName = "Spear3";
                        // No of spears down to 2
                        GameObject txt = GameObject.Find("SpearsUIText");
                        Text ptxt = txt.GetComponent<Text>();
                        ptxt.text = "x2";

                    }
                    else if (layer == 2)
                    {
                        actualSpear.GetComponent<Renderer>().sortingLayerName = "Spear2";
                        // No of spears down to 1
                        GameObject txt = GameObject.Find("SpearsUIText");
                        Text ptxt = txt.GetComponent<Text>();
                        ptxt.text = "x1";
                    }
                    else
                    {
                        actualSpear.GetComponent<Renderer>().sortingLayerName = "Spear1";
                        // No of spears down to 0
                        GameObject txt = GameObject.Find("SpearsUIText");
                        Text ptxt = txt.GetComponent<Text>();
                        ptxt.text = "x0";
                    }
                    gameObject.GetComponent<PlayerMovement>().noOfSpears--;
                }
                else
                {
                    audio.PlayOneShot(throwRejectSound, 0.04f);
                }
            }
            else
            {
                Vector3 pos = gameObject.transform.position;
                pos.y = pos.y + 0.2f;
                pos.x = pos.x - 0.13f;
                RaycastHit2D spaceForThrow = Physics2D.Raycast(pos, new Vector2(-1, 0), 1.3f); //1.3f

                bool overallTest = false;
                bool test = spaceForThrow.collider == null;
                if (!test)
                {
                    if (spaceForThrow.collider.name == "Player" || spaceForThrow.collider.name == "Door")
                    {
                        overallTest = true;
                    }
                }


                if (test && throwsEnabled || overallTest && throwsEnabled)
                {
                    audio.PlayOneShot(throwSound, 0.05f);
                    anim.SetBool("Throwing", true);
                    throwsEnabled = false;
                    StartCoroutine(throwCooldown());
                    pos.x = pos.x - 0.5f;
                    GameObject spear = (GameObject)Resources.Load("Prefabs/SpearLeft", typeof(GameObject));
                    GameObject actualSpear = Instantiate(spear, pos, Quaternion.identity);
                    int layer = gameObject.GetComponent<PlayerMovement>().noOfSpears;
                    if (layer == 3)
                    {
                        actualSpear.GetComponent<Renderer>().sortingLayerName = "Spear3";
                        GameObject txt = GameObject.Find("SpearsUIText");
                        Text ptxt = txt.GetComponent<Text>();
                        ptxt.text = "x2";
                    }
                    else if (layer == 2)
                    {
                        actualSpear.GetComponent<Renderer>().sortingLayerName = "Spear2";
                        GameObject txt = GameObject.Find("SpearsUIText");
                        Text ptxt = txt.GetComponent<Text>();
                        ptxt.text = "x1";
                    }
                    else
                    {
                        actualSpear.GetComponent<Renderer>().sortingLayerName = "Spear1";
                        GameObject txt = GameObject.Find("SpearsUIText");
                        Text ptxt = txt.GetComponent<Text>();
                        ptxt.text = "x0";
                    }
                    gameObject.GetComponent<PlayerMovement>().noOfSpears--;
                }
                else
                {
                    audio.PlayOneShot(throwRejectSound, 0.04f);
                }
            }
        }
        else
        {
            if(m_Grounded)
            {
                if (move != 0f)
                {
                    anim.SetInteger("AnimationState", 4);
                }
                else
                {
                    anim.SetInteger("AnimationState", 0);
                }
            }
            else
            {
                m_Rigidbody2D.gravityScale = 3f;
                if (m_Rigidbody2D.velocity.y < 0)
                {
                    anim.SetInteger("AnimationState", 2);
                }
                else
                {
                    anim.SetInteger("AnimationState", 1);
                }
            }

            jumpAllowance = allowance;
        
        
            //only control the player if grounded or airControl is turned on
		    if (m_Grounded || m_AirControl)
		    {
                float multiplier = m_MovementSmoothing;

                if(!m_Grounded)
                {
                    multiplier = m_AirControlReduce;
                }
            
                // Move the character by finding the target velocity
			    Vector3 targetVelocity = new Vector2(move * 8f, m_Rigidbody2D.velocity.y);
			    // And then smoothing it out and applying it to the character
			    m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, multiplier);

			    // If the input is moving the player right and the player is facing left...
			    if (move > 0 && !m_FacingRight)
			    {
				    // ... flip the player.
				    Flip();
			    }
			    // Otherwise if the input is moving the player left and the player is facing right...
			    else if (move < 0 && m_FacingRight)
			    {
				    // ... flip the player.
				    Flip();
			    }
		    }

		    // If the player should jump...
		    if (m_Grounded && jump && jumpsEnabled || jumpAllowance > 0 && jump && jumpsEnabled)
		    {
                jumpsEnabled = false;
                m_Grounded = false;
                airJumpAllowed = false;
                StartCoroutine(timeBetweenJumps());
                StartCoroutine(jumpForce());
		    }

        }
	}


	private void Flip()
	{
		// Switch the way the player is labelled as facing.
		m_FacingRight = !m_FacingRight;

		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

    public bool airJump()
    {
        return airJumpAllowed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if squished
        if (collision.tag == "FallingBlock")
        {
            decelerationTolerance = 30f;
            Debug.Log("yeah");
        }

        bool fallingObject = m_Grounded && collision.gameObject.GetComponent<Rigidbody2D>().velocity.y < 0;

        if (collision.tag == "Orb")
        {
                Debug.Log("hello orb");
                gameObject.GetComponent<PlayerMovement>().movementAllowed = false;
                m_Rigidbody2D.gravityScale = 3f;
                StartCoroutine(Death());
        }
        
        else if (fallingObject && collision.gameObject.transform.position.y > gameObject.transform.position.y)
        {
            Debug.Log("Squished1");
            squished = true;
            gameObject.GetComponent<PlayerMovement>().movementAllowed = false;
            m_Rigidbody2D.gravityScale = 100f;
            m_Rigidbody2D.bodyType = RigidbodyType2D.Static;
            StartCoroutine(Death());
        }

        else if((collision.gameObject.name == "Boss1_Left" || collision.gameObject.name == "Boss1_Right") &&  collision.gameObject.transform.position.y > gameObject.transform.position.y && m_Grounded)
        {
            Debug.Log("Squished by hand");
            squished = true;
            gameObject.GetComponent<PlayerMovement>().movementAllowed = false;
            m_Rigidbody2D.gravityScale = 100f;
            m_Rigidbody2D.bodyType = RigidbodyType2D.Static;
            StartCoroutine(Death());
        }

        else if(collision.gameObject.tag == "DeathBlock" && !squished)
        {
            m_Rigidbody2D.gravityScale = 100f;
            m_Rigidbody2D.bodyType = RigidbodyType2D.Static;
            StartCoroutine(Death());
        }

        else if(collision.gameObject.tag == "Spear" && (collision.transform.parent.gameObject.name == "Boss1_Left" || collision.transform.parent.gameObject.name == "Boss1_Right") && collision.transform.parent.gameObject.transform.position.y > gameObject.transform.position.y && m_Grounded)
        {
            Debug.Log("Squished by spear hand");
            squished = true;
            gameObject.GetComponent<PlayerMovement>().movementAllowed = false;
            m_Rigidbody2D.gravityScale = 100f;
            m_Rigidbody2D.bodyType = RigidbodyType2D.Static;
            StartCoroutine(Death());
        }

        //if(collision.gameObject.tag == "Spear" && collision.transform.parent.gameObject.GetComponent<Rigidbody2D>().velocity.y < 0 && m_Grounded)
        else if(collision.gameObject.tag == "Spear" && collision.transform.parent.gameObject.transform.position.y > gameObject.transform.position.y && collision.transform.parent.gameObject.GetComponent<Rigidbody2D>().velocity.y < 0 && m_Grounded)
        {
            Debug.Log("Squished2");
            squished = true;
            gameObject.GetComponent<PlayerMovement>().movementAllowed = false;
            m_Rigidbody2D.gravityScale = 100f;
            m_Rigidbody2D.bodyType = RigidbodyType2D.Static;
            StartCoroutine(Death());
        }

        else if (collision.gameObject.tag == "Spear" && collision.transform.parent.gameObject.tag == "Boss" && m_Grounded)
        {
            if (collision.transform.parent.gameObject.GetComponent<Jaw>() != null)
            {
                if (collision.gameObject.transform.parent.GetComponent<Jaw>().goingDown == true)
                {
                    Debug.Log("Squished3");
                    squished = true;
                    gameObject.GetComponent<PlayerMovement>().movementAllowed = false;
                    m_Rigidbody2D.gravityScale = 100f;
                    //m_Rigidbody2D.bodyType = RigidbodyType2D.Static;
                    collision.GetComponent<BoxCollider2D>().enabled = false;
                    collision.GetComponent<PolygonCollider2D>().enabled = false;
                    StartCoroutine(Death());
                }
            }
        }

        if (collision.gameObject.tag == "Spear" && (collision.transform.parent.gameObject.name == "Boss1_Head" || collision.transform.parent.gameObject.name == "Boss1_Right_Ear" || collision.transform.parent.gameObject.name == "Boss1_Left_Ear"))
        {
            Debug.Log("I should spin");
            Boss1Head b1h = GameObject.Find("Boss1_Head").GetComponent<Boss1Head>();
            b1h.spin(collision);
        }

        if (collision.gameObject.tag == "Boss" && collision.transform.position.y > GameObject.Find("GroundCheck").transform.position.y && m_Grounded)
        {
            if (collision.gameObject.GetComponent<Jaw>() != null)
            {
                if (collision.gameObject.GetComponent<Jaw>().goingDown == true)
                {
                    collision.GetComponent<PolygonCollider2D>().enabled = false;
                    Debug.Log("Squished");
                    squished = true;
                    transform.parent = null;
                    gameObject.GetComponent<PlayerMovement>().movementAllowed = false;
                    m_Rigidbody2D.gravityScale = 100f;
                    m_Rigidbody2D.bodyType = RigidbodyType2D.Static;
                    //collision.GetComponent<PolygonCollider2D>().enabled = false;
                    collision.GetComponent<BoxCollider2D>().enabled = false;

                    //collision.GetComponent<BoxCollider2D>().enabled = false;
                    StartCoroutine(Death());
                }
            }

            else if(collision.name == "Boss1_Head")
            {
                if (collision.GetComponent<Boss1Head>() != null)
                {
                    if (collision.GetComponent<Boss1Head>().rotating == false)
                    {
                        Debug.Log("Squished");
                        collision.GetComponent<BoxCollider2D>().enabled = false;
                        gameObject.GetComponent<PlayerMovement>().movementAllowed = false;
                        m_Rigidbody2D.gravityScale = 3f; // I changed from 0, is this right?
                        m_Rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
                
                        //gameObject.GetComponent<BoxCollider2D>().enabled = false;
                        //collision.GetComponent<BoxCollider2D>().enabled = false;
                        StartCoroutine(Death());
                    }
                }
            }
        }


        if (collision.gameObject.tag == "MovingBlock" || collision.gameObject.tag == "Spear" || collision.gameObject.tag == "FallingBlock" || collision.gameObject.tag == "Boss")
        {
                if (collision.GetComponent<Boss1Head>() != null)
                {
                    m_Rigidbody2D.gravityScale = 3f;
                    transform.parent = collision.gameObject.transform;
                }
                else if (collision.gameObject.tag == "Spear" && collision.transform.parent.gameObject.name == "Boss1_Head")
                {
                    m_Rigidbody2D.gravityScale = 3f;
                    transform.parent = collision.gameObject.transform;
                }
                else if (collision.gameObject.name == "Boss1_Jaw" || (collision.gameObject.tag == "Spear" && collision.transform.parent.gameObject.name == "Boss1_Jaw"))
                {
                    GameObject head = GameObject.Find("Boss1_Head");
                    if (head != null)
                    {
                        if (head.GetComponent<Boss1Head>().rotating)
                        {
                            m_Rigidbody2D.gravityScale = 3f;
                            transform.parent = collision.gameObject.transform;
                        }
                        else
                        {
                            transform.parent = collision.gameObject.transform;
                            m_Rigidbody2D.gravityScale = 100f;
                        }
                    }
                    else
                    {
                        transform.parent = collision.gameObject.transform;
                        //m_Rigidbody2D.gravityScale = 100f;
                    }
                }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "FallingBlock")
        {
            decelerationTolerance = 30f;
            Debug.Log("yeah");
        }
        
        if (collision.gameObject.tag == "ShiftingBlock" || collision.gameObject.tag == "MovingBlock" || collision.gameObject.tag == "Spear" || collision.gameObject.tag == "FallingBlock" || collision.gameObject.tag == "Boss")
        {
            if (!squished)
            {
                // REMOVE GRAVITY IF ROTATING!!
                if (collision.GetComponent<Boss1Head>() != null)
                {
                    m_Rigidbody2D.gravityScale = 3f;
                    transform.parent = collision.gameObject.transform;
                }

                else if (collision.gameObject.tag == "Spear" && collision.transform.parent.gameObject.name == "Boss1_Head")
                {
                    m_Rigidbody2D.gravityScale = 3f;
                    transform.parent = collision.gameObject.transform;
                }

                else if (collision.gameObject.name == "Boss1_Jaw" || (collision.gameObject.tag == "Spear" && collision.transform.parent.gameObject.name == "Boss1_Jaw"))
                {
                    GameObject head = GameObject.Find("Boss1_Head");
                    if (head != null)
                    {
                        if (head.GetComponent<Boss1Head>().rotating)
                        {
                            m_Rigidbody2D.gravityScale = 3f;
                            transform.parent = collision.gameObject.transform;
                        }
                    }
                    else
                    {
                        transform.parent = collision.gameObject.transform;
                        m_Rigidbody2D.gravityScale = 100f;
                    }
                }

                else
                {
                    transform.parent = collision.gameObject.transform;
                    m_Rigidbody2D.gravityScale = 100f;
                }
            }
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "FallingBlock")
        {
            StartCoroutine(IHopeThisWorks());
        }

        m_Rigidbody2D.gravityScale = 3f;
        if (collision.gameObject.tag == "ShiftingBlock" || collision.gameObject.tag == "MovingBlock" || collision.gameObject.tag == "Spear" || collision.gameObject.tag == "FallingBlock" || collision.gameObject.tag == "Boss")
        {
            transform.parent = null;
        }
    }

    IEnumerator IHopeThisWorks()
    {
        yield return new WaitForSeconds(1f);
        decelerationTolerance = 17f;
    }

    IEnumerator Death()
    {
        if (dieOnce)
        {
            audio.PlayOneShot(deathSound, 0.1f);
            anim.SetBool("Alive", false);
            dieOnce = false;
            yield return new WaitForSeconds(0.5f);

            TransitionOpen scr = GameObject.Find("Transition").GetComponent<TransitionOpen>();
            scr.CloseScene();

            while(scr.closed == false)
            {
                yield return null;
            }

            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
            gameObject.GetComponent<PlayerMovement>().movementAllowed = true;
        }
        yield return null;
    }

    IEnumerator timeBetweenJumps()
    {
        yield return new WaitForSeconds(0.2f);
        jumpsEnabled = true;
        yield return null;
    }

    IEnumerator throwCooldown()
    {
        yield return new WaitForSeconds(0.35f);
        throwsEnabled = true;
        yield return null;
    }

    IEnumerator jumpForce()
    {
        if(isAlive && jumpSoundAllowed)
        {
            audio.PlayOneShot(jumpSound, 0.1f);
        }
        Vector3 ycheck = m_Rigidbody2D.velocity;
        float yaddition = 0f;
        if (transform.parent != null)
        {
            if(transform.parent.name == "Boss1_Left" || transform.parent.name == "Boss1_Right")
            {
                if (transform.parent.gameObject.GetComponent<Boss1Hand>().upwards == true)
                {
                   Debug.Log("jump force added for boss");
                    yaddition = 100f;
                }
            }

            else if (transform.parent.gameObject.tag == "Spear")
            {
                if (transform.parent.gameObject.transform.parent.GetComponent<MovePlatform>() != null)
                {
                   if (transform.parent.gameObject.transform.parent.GetComponent<MovePlatform>().upwards == true)
                   {
                       Debug.Log("jump force added for platform");
                        yaddition = 170f;
                   }
                }
            }
        }

        if (ycheck.y < 0)
        {
            Debug.Log("Resetting velocity");
            m_Rigidbody2D.velocity = new Vector3(ycheck.x, 0f, 0f);
        }
        
        m_Rigidbody2D.gravityScale = 3f;
        m_Rigidbody2D.AddForce(new Vector3(0f, m_JumpForce + yaddition, 0));
        yield return null;

        //can be any value, maybe this is a start ascending force, up to you
        float currentForce = m_JumpForce;
        int i = 50;
        float decrement = 5f;

        while (Input.GetKey(KeyCode.Space) && i > 0)
        {
            m_Rigidbody2D.AddForce(new Vector2(0f, decrement));
            decrement = decrement - 0.01f;
            i--;
            yield return null;
        }
    }

}
