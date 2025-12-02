using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Camera))]
public class FollowCamera2D : MonoBehaviour
{

    [SerializeField] private Transform target;
    [SerializeField] private float speed;
    [SerializeField] private Tilemap tilemap;

    private Camera followCamera;
    private Vector3 offset;

    private float leftCamBoundary;
    private float rightCamBoundary;
    private float bottomCamBoundary;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        followCamera = GetComponent<Camera>();
        offset = transform.position - target.position;

        CalculateBounds();
        
    }

    private void CalculateBounds()
    {

        tilemap.CompressBounds();

        Camera cam = GetComponent<Camera>();

        float orthoSize = cam.orthographicSize;
        Vector3 viewportHalfSize = new Vector3(orthoSize * cam.aspect, orthoSize);

        Vector3Int tilemapMin = tilemap.cellBounds.min;
        Vector3Int tilemapMax = tilemap.cellBounds.max;

        leftCamBoundary = tilemapMin.x + viewportHalfSize.x;
        rightCamBoundary = tilemapMax.x - viewportHalfSize.x;
        bottomCamBoundary = tilemapMin.y + viewportHalfSize.y;

    }

    // Update is called once per frame
    void LateUpdate()
    {

        Vector3 targetCamPos = target.position + offset;
        Vector3 steppedPos = Vector3.Lerp(transform.position, targetCamPos, speed * Time.deltaTime);

        steppedPos.x = Mathf.Clamp(steppedPos.x, leftCamBoundary, rightCamBoundary);
        steppedPos.y = Mathf.Clamp(steppedPos.y, bottomCamBoundary, steppedPos.y);

        transform.position = steppedPos;
        
    }
}
