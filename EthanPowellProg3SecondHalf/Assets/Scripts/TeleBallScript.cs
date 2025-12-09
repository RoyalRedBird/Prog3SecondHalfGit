using UnityEngine;

public class TeleBallScript : MonoBehaviour
{

    public LayerMask groundLayer;
    Vector3 velocity;

    public float lifeTimer;
    public float ballGravity;

    // Update is called once per frame
    void FixedUpdate()
    {

        //Constant application of gravity and velocity.
        velocity.y -= ballGravity;
        transform.position += velocity;

        //Raycasts to check for collisions.
        RaycastHit2D hitRight = Physics2D.Raycast(transform.position, Vector3.right, 0.25f, groundLayer);
        RaycastHit2D hitLeft = Physics2D.Raycast(transform.position, Vector3.left, 0.25f, groundLayer);
        RaycastHit2D hitTop = Physics2D.Raycast(transform.position, Vector3.up, 0.25f, groundLayer);
        RaycastHit2D hitBottom = Physics2D.Raycast(transform.position, Vector3.down, 0.25f, groundLayer);

        //Vertical bouncing.
        if (hitTop || hitBottom)
        {

            velocity.y = -velocity.y;

        }

        //Horizontal bouncing.
        if(hitLeft || hitRight)
        {

            velocity.x = -velocity.x;

        }

        lifeTimer -= Time.fixedDeltaTime;

        //Kill the ball if it runs out of time.
        if(lifeTimer <= 0)
        {

            Destroy(gameObject);

        }

    }

    //Initialize the ball.
    public void InitializeBall(Vector2 vel, float timer)
    {

        velocity = vel;
        lifeTimer = timer;

    }

}
