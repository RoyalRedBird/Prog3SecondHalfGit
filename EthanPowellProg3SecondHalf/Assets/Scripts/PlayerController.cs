using UnityEngine;

public class PlayerController : MonoBehaviour
{

    bool moveKeyDown;
    public float MaxSpeed;
    public float accellTime;
    public float decelSpeed;
    public float decelTime;
    public float apexHeight;
    public float apexTime;
    public float jumpDelayTime;
    public float jumpDelayTimeCurrent;
    public float coyoteTime;
    public float coyoteTimeCurrent;
    public float terminalVelocity;
    [SerializeField] Vector3 velocity = Vector3.zero;

    public LayerMask groundLayer;
    public bool canJump;

    KeyCode lastKeyPressed = KeyCode.LeftArrow;

    public bool isDashing;
    public float dashSpeed;
    public float dashTime = 0.2f;
    public float dashTimeCurrent;
    public float dashTimeHorizontalModifier;
    public bool canDash;
    public Vector3 dashDirection;

    public float wallJumpTimer;
    public float wallJumpTimerCurrent;

    public GameObject teleBall;
    public GameObject activeTeleBall;
    public float throwForce;

    public enum FacingDirection
    {
        left, right
    }

    public enum CharacterState
    {

        Idle, Walking, Jumping, Dead

    }

    void Start()
    {
        
    }

    void FixedUpdate()
    {
        // The input from the player needs to be determined and
        // then passed in the to the MovementUpdate which should
        // manage the actual movement of the character.
        Vector2 playerInput = new Vector2();       

        if (Input.GetKey(KeyCode.A))
        {

            playerInput.x = -1;

        }

        if (Input.GetKey(KeyCode.D))
        {

            playerInput.x = 1;


        }

        if (Input.GetKey(KeyCode.W) && CanJump() && jumpDelayTimeCurrent <= 0)
        {

            playerInput.y = 1;
            print("Boing.");
            jumpDelayTimeCurrent = jumpDelayTime;
            coyoteTimeCurrent = 0;

        }

        //If the player presses space, they are able to dash and aren't already dashing...
        if (Input.GetKey(KeyCode.Space) && canDash && !isDashing)
        {

            //Start the dash.
            isDashing = true;
            canDash = false;
            Vector3 newDashVector = new Vector3(); //Set a direction for the dash.

            RaycastHit2D hitRight = Physics2D.Raycast(transform.position, Vector3.right, 1f, groundLayer);
            RaycastHit2D hitLeft = Physics2D.Raycast(transform.position, Vector3.left, 1f, groundLayer);

            //Set horizontal dash direction.
            newDashVector.x = playerInput.x;

            //Do not allow the player to dash left or right if they are next to a wall.
            if (newDashVector.x > 0 && hitRight)
            {

                newDashVector.x = 0;

            }

            if (newDashVector.x < 0 && hitLeft)
            {

                newDashVector.x = 0;

            }

            //Reset dash timer.
            dashTimeCurrent = dashTime;

            //Set vertical dash direction of W is pressed.
            if (Input.GetKey(KeyCode.W))
            {

                newDashVector.y = 1;

            }

            //Adjust horizontal dash speed to be more in line with vertical dash.
            if(newDashVector.y == 0)
            {

                dashTimeCurrent = dashTime * dashTimeHorizontalModifier;

            }

            dashDirection = newDashVector;

        }

        MovementUpdate(playerInput);
        IsWalking();
        GetFacingDirection();
        IsGrounded();
        CanJump();
        HitHeadCheck();
        BallThrow();

        if (coyoteTimeCurrent > 0)
        {

            coyoteTimeCurrent -= Time.deltaTime;

        }

        jumpDelayTimeCurrent -= Time.deltaTime;

    }

    private void MovementUpdate(Vector2 playerInput)
    {

        moveKeyDown = false;

        float gravity = -2 * apexHeight / (apexTime * apexTime);
        float jumpVel = 2 * apexHeight / apexTime;

        if (playerInput.y > 0)
        {

            velocity.y = jumpVel;

        }

        //Wall checkers for wall jumping.
        RaycastHit2D hitRight = Physics2D.Raycast(transform.position, Vector3.right, 1f, groundLayer);
        RaycastHit2D hitLeft = Physics2D.Raycast(transform.position, Vector3.left, 1f, groundLayer);

        //If a raycast hits a wall, jump and set the horizontal speed of the player to max left or right velocity, depending on
        //where the wall is located relative to the player.
        if (hitRight)
        {

            Debug.DrawLine(transform.position, transform.position + Vector3.right, Color.blue);

            if (Input.GetKey(KeyCode.W))
            {

                velocity.y = jumpVel;
                velocity.x = -MaxSpeed;

            }

        }

        if (hitLeft)
        {

            Debug.DrawLine(transform.position, transform.position + Vector3.left, Color.green);

            if (Input.GetKey(KeyCode.W))
            {

                velocity.y = jumpVel;
                velocity.x = MaxSpeed;

            }

        }

        //Horizontal Movement
        if (playerInput.x < 0)
        {

            velocity.x -= (MaxSpeed / accellTime) * Time.fixedDeltaTime;
            moveKeyDown = true;
            lastKeyPressed = KeyCode.A;

        }

        if (playerInput.x > 0)
        {

            velocity.x += (MaxSpeed / accellTime) * Time.fixedDeltaTime;
            moveKeyDown = true;
            lastKeyPressed = KeyCode.D;

        }      

        //Slows the player to a halt if no horizontal movement buttons are being pressed.
        if (!moveKeyDown)
        {

            if (velocity.x > 0)
            {

                if(velocity.x - ((decelSpeed / decelTime) * Time.fixedDeltaTime) <= 0)
                {

                    velocity.x = 0;

                }
                else
                {

                    velocity.x -= (decelSpeed / decelTime) * Time.fixedDeltaTime;

                }

            }
            else if (velocity.x < 0)
            {

                if (velocity.x + ((decelSpeed / decelTime) * Time.fixedDeltaTime) >= 0)
                {

                    velocity.x = 0;

                }
                else
                {

                    velocity.x += (decelSpeed / decelTime) * Time.fixedDeltaTime;

                }

            }

        }

        velocity.x = Mathf.Clamp(velocity.x, -(MaxSpeed), MaxSpeed);
        velocity.y += gravity * Time.fixedDeltaTime;

        if (playerInput.y == 0)
        {

            if (IsGrounded() && velocity.y < 0)
            {

                velocity.y = 0;

            }

        }    

        //Terminal velocity application for up and down.
        if(velocity.y <= -terminalVelocity)
        {

            velocity.y = -terminalVelocity;

        }

        if (velocity.y > terminalVelocity) { 
        
            velocity.y = terminalVelocity;
        
        }

        //Runs the Dash method and counts down the dash time.
        if (isDashing)
        {

            DashMove();
            dashTimeCurrent -= Time.fixedDeltaTime;


        }       

        //Stops dashing if the player is no longer dashing.
        if(isDashing && dashTimeCurrent <= 0)
        {

            isDashing = false;

        }

        transform.position += velocity;

    }

    public bool IsWalking()
    {

        if(velocity.x > 0 || velocity.x < 0)
        {
            return true;
        }
        else{
            return false;
        }

        
    }

    public bool IsGrounded()
    {

        //If the boxcast at the players feet is touching the ground layer...
        if (Physics2D.BoxCast(transform.position, Vector2.one * 0.8f, 0f, Vector3.down, 0.27f, groundLayer)) 
        {

            //Reset jump, dash, coyote time and return true.
            Debug.DrawLine(transform.position, (Vector3.down * 0.8f) + transform.position, Color.green);
            canJump = true;
            canDash = true;
            coyoteTimeCurrent = coyoteTime;
            return true;

        }
        else
        {

            //Otherwise count down the players coyote time and return false.
            Debug.DrawLine(transform.position, (Vector3.down * 0.8f) + transform.position, Color.red);
            coyoteTimeCurrent -= Time.deltaTime;
            return false;

        }        

    }

    //Sets the Y velocity of the character to zero if they hit the roof of something.
    private void HitHeadCheck()
    {

        if (Physics2D.BoxCast(transform.position, Vector2.one * 0.8f, 0f, Vector3.up, 0.27f, groundLayer))
        {

            velocity.y = 0;

        }

    }

    public bool CanJump()
    {

        //If the player isn't grounded but they still have coyote time...
        if(!IsGrounded() && coyoteTimeCurrent > 0)
        {

            return true;

        }else if (IsGrounded()) //...or the player is grounded.
        {

            return true;

        }

        return false;

    }

    public FacingDirection GetFacingDirection()
    {

        if(lastKeyPressed == KeyCode.A)
        {

            return FacingDirection.left;

        }
        else if(lastKeyPressed == KeyCode.D)
        {

            return FacingDirection.right;

        }

        return FacingDirection.left;

    }

    public void DashMove()
    {

        //Apply a constant force to the player so long as the player is logged as dashing.
        if(dashTimeCurrent >= dashTime)
        {

            velocity.x += dashDirection.x * dashSpeed;
            velocity.y += dashDirection.y * dashSpeed;

        }

    }

    public void BallThrow()
    {

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 position2D = transform.position;

        Vector2 transformToMousePos = (position2D - mousePos).normalized;

        Debug.DrawLine(transform.position, position2D - transformToMousePos, Color.yellow);

        //When left clicking if there is no ball...
        if (Input.GetMouseButton(0) && activeTeleBall == null)
        {

            //Spawn a new ball and set it as the active ball.
            activeTeleBall = GameObject.Instantiate(teleBall, transform.position, Quaternion.identity);

            //Get its script.
            TeleBallScript tbScript = activeTeleBall.GetComponent<TeleBallScript>();

            //Initialize.
            tbScript.InitializeBall(-(transformToMousePos * throwForce), 5);

        }

        //When right clicking if a ball is active...
        if (Input.GetMouseButton(1) && activeTeleBall != null)
        {

            //Get the ball's position.
            Vector2 teleportPos = activeTeleBall.transform.position;

            //Check for potential clipping.
            RaycastHit2D hitRight = Physics2D.Raycast(transform.position, Vector3.right, 1f, groundLayer);
            RaycastHit2D hitLeft = Physics2D.Raycast(transform.position, Vector3.left, 1f, groundLayer);
            RaycastHit2D hitTop = Physics2D.Raycast(transform.position, Vector3.up, 1f, groundLayer);
            RaycastHit2D hitBottom = Physics2D.Raycast(transform.position, Vector3.down, 1f, groundLayer);

            //Resolve as needed.
            if (hitRight)
            {

                teleportPos.x -= 1;

            }

            if (hitLeft)
            {

                teleportPos.x += 1;

            }

            if (hitTop)
            {

                teleportPos.y -= 1;

            }

            if (hitBottom)
            {

                teleportPos.y += 1;

            }

            //Apply transform and kill the ball.
            transform.position = teleportPos;

            Destroy(activeTeleBall);

        }        

    }

}
