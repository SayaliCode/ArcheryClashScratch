using UnityEngine;

public class RotateOnDrag : MonoBehaviour
{
    public float rotationSpeed = 5f; // Adjusts rotation sensitivity
    private bool isDragging = false;
    private Vector3 lastMousePosition;

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Left Mouse Button Pressed
        {
            isDragging = true;
            lastMousePosition = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0)) // Left Mouse Button Released
        {
            isDragging = false;
        }

        if (isDragging)
        {
            Vector3 currentMousePosition = Input.mousePosition;
            float deltaY = currentMousePosition.y - lastMousePosition.y; // Get vertical movement

            // Rotate object based on drag direction
            transform.Rotate(Vector3.forward, -deltaY * rotationSpeed * Time.deltaTime);

            lastMousePosition = currentMousePosition; // Update last mouse position
        }
    }
}