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

        velocity.y -= ballGravity;
        transform.position += velocity;

        RaycastHit2D hitRight = Physics2D.Raycast(transform.position, Vector3.right, 0.25f, groundLayer);
        RaycastHit2D hitLeft = Physics2D.Raycast(transform.position, Vector3.left, 0.25f, groundLayer);
        RaycastHit2D hitTop = Physics2D.Raycast(transform.position, Vector3.up, 0.25f, groundLayer);
        RaycastHit2D hitBottom = Physics2D.Raycast(transform.position, Vector3.down, 0.25f, groundLayer);

        if (hitTop || hitBottom)
        {

            velocity.y = -velocity.y;

        }

        if(hitLeft || hitRight)
        {

            velocity.x = -velocity.x;

        }

        lifeTimer -= Time.fixedDeltaTime;

        if(lifeTimer <= 0)
        {

            Destroy(gameObject);

        }

    }

    public void InitializeBall(Vector2 vel, float timer)
    {

        velocity = vel;
        lifeTimer = timer;

    }

}
