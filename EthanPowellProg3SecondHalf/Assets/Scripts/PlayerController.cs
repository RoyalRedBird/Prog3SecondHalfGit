using UnityEngine;

public class PlayerController : MonoBehaviour
{

    bool moveKeyDown;
    public float MaxSpeed;
    public float accellTime;
    public float decelSpeed;
    public float decelTime;
    [SerializeField] Vector3 velocity = Vector3.zero;

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
            moveKeyDown = true;
            lastKeyPressed = KeyCode.LeftArrow;

        }

        if (Input.GetKey(KeyCode.RightArrow))
        {

            playerInput.x = 1;
            moveKeyDown = true;
            lastKeyPressed = KeyCode.RightArrow;

        }


        MovementUpdate(playerInput);
        IsWalking();
        IsGrounded();
        GetFacingDirection();

    }

    private void MovementUpdate(Vector2 playerInput)
    {

        moveKeyDown = false;

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
        return true;
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
