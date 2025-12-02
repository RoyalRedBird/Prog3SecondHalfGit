using UnityEngine;

public class ParallaxLayerController : MonoBehaviour
{

    [SerializeField] private Camera viewCamera;
    [SerializeField] private float cameraDeltaScalar = 1f;

    private Vector3 camStartPos;
    private Vector3 layerStartPos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        camStartPos = viewCamera.transform.position;
        layerStartPos = transform.position;
        
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 cameraDelta = viewCamera.transform.position - camStartPos;

        float deltaX = cameraDelta.x * cameraDeltaScalar;
        float deltaY = cameraDelta.y * cameraDeltaScalar;

        transform.position = layerStartPos + new Vector3(deltaX, deltaY);
        
    }
}
