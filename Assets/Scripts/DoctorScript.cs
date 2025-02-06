using UnityEngine;

public class DoctorScript : MonoBehaviour
{

    enum States
    {
        idle,
        run,
        jump,
    }

    CharacterController cc;
    public Transform cam;

    public float moveSpeed = 6;
    float gravity = -18;
    float jumpVelocity = 10;
    Vector3 velocity;
    bool isGrounded;
    float targetAngle;
    float angle;
    float turnSmoothVelocity;
    public float turnSmoothTime = 0.1f;
    bool canWalk;

    Vector3 moveDir;

    Animator anim;
    States state;

    bool jumpStarted;


    private void Start()
    {
        cc = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        velocity = Vector3.zero;

        canWalk = false;
        state = States.idle;


        jumpStarted = false;


    }
    // Update is called once per frame
    void Update()
    {
        SetDebugSpeed();
        PlayerLogic();
    }

    void PlayerLogic()
    {
        if (state == States.idle)
        {

            if (InputDirection() > 0.1f)
            {
                state = States.run;
            }

            if (CanJump() == true)
            {
                DoJump();
            }
            DoMovement();


        }

        if (state == States.run)
        {
            if (InputDirection() < 0.1f)
            {
                state = States.idle;
            }


            if (CanJump() == true)
            {
                DoJump();
            }
            else
            {
                DoMovement();
            }


        }

        if (state == States.jump)
        {
            if (velocity.y < -2)
            {
                anim.SetBool("startjump", false);
                anim.SetBool("fall", true);

            }

            if (CheckForLanding() == true)
            {
                state = States.idle;
                //state = States.land;
                velocity.y = -3;
                anim.SetBool("jump", false);
                anim.SetBool("startjump", false);
                anim.SetBool("run", false);
                anim.SetBool("fall", false);

                print("Landed");
                //anim.SetBool("land", true);

            }



        }





        //apply gravity
        velocity.y += gravity * Time.deltaTime;
        cc.Move((moveDir * moveSpeed + velocity) * Time.deltaTime);


    }





    void DoMovement()
    {

        //move character controller

        //get horizontal and vertical inputs
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;

        //calculate current angle and target angle
        targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
        angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

        //calculate move direction based on target angle
        moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

        //calculate rotation
        if (direction.magnitude > 0.1f)
        {
            //only rotate if a direction is pressed
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            anim.SetBool("run", true);
        }
        else
        {
            // zero velocity if magnitude is too small
            moveDir = Vector3.zero;
            anim.SetBool("run", false);
            //state = States.idle;
        }




    }

    float InputDirection()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;
        return direction.magnitude;
    }

    void ApplyGravity()
    {
        velocity.y += gravity * Time.deltaTime;
        if (cc.isGrounded)
        {
            //velocity.y = -3;
        }
        cc.Move(velocity * Time.deltaTime);
    }

    bool CanJump()
    {
        // check for jump
        if (cc.isGrounded)
        {
            // can only jump if grounded

            velocity.y = -3; // groundcheck fix - force controller onto ground
            if (Input.GetButtonDown("Jump"))
            {
                return true;
            }
        }
        return false;
    }

    void DoJump()
    {
        velocity.y = jumpVelocity;
        anim.SetBool("run", false);
        anim.SetBool("startjump", true);
        state = States.jump;


    }


    bool CheckForLanding()
    {
        if (cc.isGrounded)
        {
            if (velocity.y < (jumpVelocity - 1)) // give player time to move away from ground
            {
                return true;
            }
        }
        return false;
    }

    public void PlayerJumpForce()
    {
        //this is called from the animation event
        velocity.y = jumpVelocity;
        jumpStarted = true;
    }
    public void PlayerLanded()
    {
        //this is called from the animation event
        state = States.idle;
        anim.SetBool("land", false);
        jumpStarted = false;

    }


    private void OnGUI()
    {
        string text = "Grounded=" + cc.isGrounded;
        text += "\nState=" + state;
        text += "\nYvel=" + velocity.y;
        text += "\nJump started=" + jumpStarted;
        GUI.Label(new Rect(10, 10, 300, 300), text);

    }


    void SetDebugSpeed()
    {
        // set speed
        if (Input.GetKeyDown("1"))
            Time.timeScale = 1f;

        if (Input.GetKeyDown("2"))
            Time.timeScale = 0.8f;

        if (Input.GetKeyDown("3"))
            Time.timeScale = 0.6f;

        if (Input.GetKeyDown("4"))
            Time.timeScale = 0.4f;

        if (Input.GetKeyDown("5"))
            Time.timeScale = 0.2f;

        if (Input.GetKeyDown("6"))
            Time.timeScale = 0.1f;

        if (Input.GetKeyDown("7"))
            Time.timeScale = 0.05f;

        if (Input.GetKeyDown("8"))
            Time.timeScale = 0.03f;

        if (Input.GetKeyDown("9"))
            Time.timeScale = 0.01f;

        if (Input.GetKeyDown("0"))
            Time.timeScale = 0.001f;
    }

}