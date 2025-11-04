using UnityEngine;

public class KillPlaneScript : MonoBehaviour
{

    private void OnCollisionEnter2D(Collision2D collision)
    {

        Destroy(collision.gameObject);
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        Destroy (collision.gameObject);

    }
}
