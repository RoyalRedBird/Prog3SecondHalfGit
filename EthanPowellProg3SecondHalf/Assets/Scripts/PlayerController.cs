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

    void Update()
    {
        // The input from the player needs to be determined and
        // then passed in the to the MovementUpdate which should
        // manage the actual movement of the character.
        //Vector2 playerInput = new Vector2();

        MovementUpdate();
        IsWalking();
        IsGrounded();
        GetFacingDirection();

    }

    private void MovementUpdate()
    {

        moveKeyDown = false;

        if (Input.GetKey(KeyCode.LeftArrow))
        {

            velocity.x -= (MaxSpeed / accellTime) * Time.deltaTime;
            moveKeyDown = true;
            lastKeyPressed = KeyCode.LeftArrow;

        }

        if (Input.GetKey(KeyCode.RightArrow))
        {

            velocity.x += (MaxSpeed / accellTime) * Time.deltaTime;
            moveKeyDown = true;
            lastKeyPressed = KeyCode.RightArrow;

        }

        if (!moveKeyDown)
        {

            if (velocity.x > 0)
            {

                velocity.x -= (decelSpeed / decelTime) * Time.deltaTime;

            }
            else if (velocity.x < 0)
            {

                velocity.x += (decelSpeed / decelTime) * Time.deltaTime;

            }

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
