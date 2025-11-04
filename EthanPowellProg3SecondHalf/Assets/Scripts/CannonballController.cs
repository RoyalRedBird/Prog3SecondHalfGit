using UnityEngine;

public class CannonballController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 3);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if(collision.tag == "Ship")
        {

            ScoreboardController.Instance.Score += 1;

        }

        Destroy(gameObject);

    }

}
