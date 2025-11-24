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
    public float jumpForce;
    public float groundCheckOffTime;
    public float groundCheckOffTimeCurrent;
    public float coyoteTime;
    public float coyoteTimeCurrent;
    public float terminalVelocity;
    [SerializeField] Vector3 velocity = Vector3.zero;

    public LayerMask groundLayer;

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

        if (groundCheckOffTimeCurrent > 0)
        {
            groundCheckOffTimeCurrent -= Time.deltaTime;
        }
        else
        {
           
            groundCheckOffTimeCurrent = 0;
        }

        if (Input.GetKeyDown(KeyCode.UpArrow) && IsGrounded())
        {

            playerInput.y = 1;
            groundCheckOffTimeCurrent = groundCheckOffTime;

        }

        MovementUpdate(playerInput);
        IsWalking();
        GetFacingDirection();
        IsGrounded();

        if (coyoteTimeCurrent > 0)
        {

            coyoteTime -= Time.deltaTime;

        }

    }

    private void MovementUpdate(Vector2 playerInput)
    {

        moveKeyDown = false;

        float gravity = -2 * apexHeight / (apexTime * apexTime);
        float jumpVel = 2 * apexHeight / apexTime;

        if (playerInput.x < 0)
        {

            velocity.x -= (MaxSpeed / accellTime) * Time.deltaTime;
            moveKeyDown = true;
            lastKeyPressed = KeyCode.LeftArrow;

        }

        if (playerInput.x > 0)
        {

            velocity.x += (MaxSpeed / accellTime) * Time.deltaTime;
            moveKeyDown = true;
            lastKeyPressed = KeyCode.RightArrow;

        }

        if (playerInput.y > 0)
        {

            velocity.y = jumpVel;

        }

        if (!moveKeyDown)
        {

            if (velocity.x > 0)
            {

                if(velocity.x - ((decelSpeed / decelTime) * Time.deltaTime) <= 0)
                {

                    velocity.x = 0;

                }
                else
                {

                    velocity.x -= (decelSpeed / decelTime) * Time.deltaTime;

                }

            }
            else if (velocity.x < 0)
            {

                if (velocity.x + ((decelSpeed / decelTime) * Time.deltaTime) >= 0)
                {

                    velocity.x = 0;

                }
                else
                {

                    velocity.x += (decelSpeed / decelTime) * Time.deltaTime;

                }

            }

        }

        velocity.x = Mathf.Clamp(velocity.x, -(MaxSpeed), MaxSpeed);
        velocity.y += gravity * Time.deltaTime;

        if (IsGrounded())
        {

            velocity.y = 0;

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

        //Physics2D.BoxCast(transform.position, Vector2.one * 0.9f, 0f, (Vector3.down * 0.4f) + transform.position, groundLayer);

        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.position + Vector3.down * 0.8f, groundLayer);

        if (hit) 
        {

            Debug.DrawLine(transform.position, (Vector3.down * 0.8f) + transform.position, Color.green);
            print("I am grounded.");
            return true;

        }
        else
        {

            Debug.DrawLine(transform.position, (Vector3.down * 0.8f) + transform.position);
            print("I am not grounded.");
            return false;

        }        

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
