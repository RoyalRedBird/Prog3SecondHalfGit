using System.Collections;
using UnityEngine;
using UnityEngine.U2D;

public class BallSpawner : MonoBehaviour
{

    public GameObject ballFab;
    public int ballSpawnCount = 300;
    public float BallSpawnInterval = 0.3f;
    public bool randomColours = true;

    public float impulseForce = 5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    IEnumerator Start()
    {

        for (int i = 0; i < ballSpawnCount; i++) { 
        
            GameObject ball = Instantiate(ballFab, transform.position, Quaternion.identity);

            Rigidbody2D body2D = ball.GetComponent<Rigidbody2D>();

            body2D.AddForce(Random.insideUnitCircle.normalized * impulseForce, ForceMode2D.Impulse);

            if (randomColours)
            {

                ball.GetComponent<SpriteRenderer>().color = new Color(Random.value, Random.value, Random.value);
                
            }

            yield return new WaitForSeconds(BallSpawnInterval);

        }  
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
