using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float moveSpeed = 7f;
    public float zoomSize = 5f;
    public Camera cam;
    public float maxFollowDistance = 20f;  // Maximum distance to follow the arrow
    public Vector3 cameraOffset = new Vector3(0, 2, -10);  // Offset from the target position
    private Coroutine currentCoroutine;
    private Vector3 initialPosition;  // Store the position where arrow was fired from

    void Start()
    {
        //cam = Camera.main;
    }

    public void FocusOnPlayer(Transform player)
    {
        if (currentCoroutine != null)
            StopCoroutine(currentCoroutine);
        currentCoroutine = StartCoroutine(PanToPosition(player.position + cameraOffset));
    }

    public void FollowArrow(Transform arrow, float followDuration)
    {
        if (currentCoroutine != null)
            StopCoroutine(currentCoroutine);
        
        initialPosition = arrow.position;  // Store the initial position
        currentCoroutine = StartCoroutine(FollowArrowCoroutine(arrow, followDuration));
    }

    private IEnumerator FollowArrowCoroutine(Transform arrow, float followDuration)
    {
        float elapsedTime = 0f;
        
        while (elapsedTime < followDuration && arrow != null)
        {
            // Calculate distance from initial position
            float distanceFromStart = Vector3.Distance(initialPosition, arrow.position);
            
            if (distanceFromStart <= maxFollowDistance)
            {
                // Normal following within max distance
                Vector3 targetPosition = arrow.position + cameraOffset;
                cam.transform.position = Vector3.Lerp(cam.transform.position, targetPosition, Time.deltaTime * moveSpeed);
            }
            else
            {
                // Stop following and maintain the max distance
                Vector3 directionFromStart = (arrow.position - initialPosition).normalized;
                Vector3 maxPosition = initialPosition + directionFromStart * maxFollowDistance;
                Vector3 targetPosition = maxPosition + cameraOffset;
                cam.transform.position = Vector3.Lerp(cam.transform.position, targetPosition, Time.deltaTime * moveSpeed);
                break;  // Exit the coroutine since we've reached max distance
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator PanToPosition(Vector3 targetPosition)
    {
        Vector3 startPosition = cam.transform.position;
        float elapsedTime = 0f;
        while (elapsedTime < 1f)
        {
            cam.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime);
            elapsedTime += Time.deltaTime * moveSpeed;
            yield return null;
        }

        cam.transform.position = targetPosition;
    }
}
