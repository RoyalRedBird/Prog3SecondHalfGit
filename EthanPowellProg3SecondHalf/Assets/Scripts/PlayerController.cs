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

    public enum FacingDirection
    {
        left, right
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

        if (Input.GetKey(KeyCode.LeftArrow))
        {

            playerInput.x = -1;

        }

        if (Input.GetKey(KeyCode.RightArrow))
        {

            playerInput.x = 1;


        }

        if (Input.GetKey(KeyCode.UpArrow) && CanJump() && jumpDelayTimeCurrent <= 0)
        {

            playerInput.y = 1;
            print("Boing.");
            jumpDelayTimeCurrent = jumpDelayTime;
            coyoteTimeCurrent = 0;

        }

        MovementUpdate(playerInput);
        IsWalking();
        GetFacingDirection();
        IsGrounded();
        CanJump();
        HitHeadCheck();

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

        if (playerInput.x < 0)
        {

            velocity.x -= (MaxSpeed / accellTime) * Time.fixedDeltaTime;
            moveKeyDown = true;
            lastKeyPressed = KeyCode.LeftArrow;

        }

        if (playerInput.x > 0)
        {

            velocity.x += (MaxSpeed / accellTime) * Time.fixedDeltaTime;
            moveKeyDown = true;
            lastKeyPressed = KeyCode.RightArrow;

        }      

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
     

        if(velocity.y <= -terminalVelocity)
        {

            velocity.y = -terminalVelocity;

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

        if (Physics2D.BoxCast(transform.position, Vector2.one * 0.8f, 0f, Vector3.down, 0.27f, groundLayer)) 
        {

            Debug.DrawLine(transform.position, (Vector3.down * 0.8f) + transform.position, Color.green);
            canJump = true;
            coyoteTimeCurrent = coyoteTime;
            return true;

        }
        else
        {

            Debug.DrawLine(transform.position, (Vector3.down * 0.8f) + transform.position, Color.red);
            coyoteTimeCurrent -= Time.deltaTime;
            return false;

        }        

    }

    private void HitHeadCheck()
    {

        if (Physics2D.BoxCast(transform.position, Vector2.one * 0.8f, 0f, Vector3.up, 0.27f, groundLayer))
        {

            velocity.y = 0;

        }

    }

    public bool CanJump()
    {

        if(!IsGrounded() && coyoteTimeCurrent > 0)
        {

            return true;

        }else if (IsGrounded())
        {

            return true;

        }

        return false;

    }

    public FacingDirection GetFacingDirection()
    {

        if(lastKeyPressed == KeyCode.LeftArrow)
        {

            return FacingDirection.left;

        }
        else if(lastKeyPressed == KeyCode.RightArrow)
        {

            return FacingDirection.right;

        }

        return FacingDirection.left;

    }
}
