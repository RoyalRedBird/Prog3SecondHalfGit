using UnityEngine;
using System.Collections.Generic;

public class TestballScript : MonoBehaviour
{

    ContactPoint2D[] contPoints = new ContactPoint2D[50];
    List<Collider2D> hitColliders = new List<Collider2D>();
    int overlapIndex = 0;

    int contIndex = 1;

    [SerializeField] Transform markerPos;

    // Update is called once per frame
    void Update()
    {

        contPoints = new ContactPoint2D[50];

        Debug.Log(gameObject.GetComponent<Rigidbody2D>().GetContacts(contPoints));

        foreach (ContactPoint2D contPoint in contPoints)
        {

            Debug.DrawLine(gameObject.transform.position, contPoint.point, Color.green);

            if (gameObject.GetComponent<Rigidbody2D>().OverlapPoint(markerPos.position))
            {

                Debug.Log("Hitting the marker.");

            }

            
        }

        Debug.Log(Physics2D.OverlapCollider(gameObject.GetComponent<Collider2D>(), hitColliders) + " colliders overlapped.");

        foreach(Collider2D hitCollider in hitColliders)
        {

            Debug.DrawLine(gameObject.transform.position, hitCollider.transform.position, Color.blue);
            
        }

        contIndex = 1;
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        contPoints[contIndex] = collision.GetContact(0);

        contIndex++;

    }

}
