using UnityEngine;

[System.Serializable]
//public class ProjectileProperties
//{
//    public Vector3 initialPosition;
//    public Vector3 direction;
//    public float initialSpeed;
//    public float mass = 1f;
//    public float drag = 0.1f;
//}

public class FixedTrajectoryArcher : MonoBehaviour
{
    [Header("Arrow Settings")]
    public float fixedArrowSpeed = 40f;
    public GameObject arrowPrefab;
    public Transform ropeTransform;  // Reference to the rope transform
    public AudioSource shootSound;

    [Header("Trajectory Settings")]
    public TrajectoryPredictor trajectoryPredictor;
    public float arrowMass = 1f;
    public float arrowDrag = 0.05f;
    public Material trajectoryMaterial;
    public Color trajectoryColor = Color.red;
    [Range(0.01f, 0.5f)]
    public float trajectoryWidth = 0.05f;

    private Arrow currentArrow;
    private ProjectileProperties projectileProperties = new ProjectileProperties();
    private LineRenderer trajectoryLine;
    private bool isReady = false;

    void Start()
    {
        SetupTrajectoryPredictor();
        SpawnNewArrow();
    }

    void Update()
    {
        if (isReady && currentArrow != null)
        {
            UpdateTrajectoryPrediction();
        }
    }

    private void SetupTrajectoryPredictor()
    {
        // Create and setup trajectory predictor if not assigned
        if (trajectoryPredictor == null)
        {
            GameObject predictorObj = new GameObject("TrajectoryPredictor");
            predictorObj.transform.SetParent(transform);
            predictorObj.transform.localPosition = Vector3.zero;
            trajectoryPredictor = predictorObj.AddComponent<TrajectoryPredictor>();
            trajectoryLine = predictorObj.AddComponent<LineRenderer>();
        }

        // Get or create line renderer
        trajectoryLine = trajectoryPredictor.GetComponent<LineRenderer>();
        if (trajectoryLine != null)
        {
            SetupLineRenderer();
        }
        else
        {
            Debug.LogError("LineRenderer component not found on TrajectoryPredictor!");
        }
    }

    private void SetupLineRenderer()
    {
        trajectoryLine.useWorldSpace = true;
        trajectoryLine.startWidth = trajectoryWidth;
        trajectoryLine.endWidth = trajectoryWidth;
        trajectoryLine.material = trajectoryMaterial;
        trajectoryLine.startColor = trajectoryColor;
        trajectoryLine.endColor = trajectoryColor;
        trajectoryLine.positionCount = 50;
        trajectoryLine.enabled = true;
    }

    public void SpawnNewArrow()
    {
        if (currentArrow != null)
        {
            Destroy(currentArrow.gameObject);
        }

        if (ropeTransform != null && arrowPrefab != null)
        {
            // Instantiate new arrow
            GameObject newArrow = Instantiate(arrowPrefab);
            currentArrow = newArrow.GetComponent<Arrow>();
            
            // Set arrow to rope using the original SetToRope method
            currentArrow.SetToRope(ropeTransform, transform);
            isReady = true;
        }
        else
        {
            Debug.LogError("Rope transform or arrow prefab not assigned!");
            isReady = false;
        }
    }

    public void ShootArrow()
    {
        
        if (!isReady || currentArrow == null) return;

        // Shoot the arrow with fixed speed
        currentArrow.Shot(fixedArrowSpeed);

        // Play sound effect
        if (shootSound != null)
        {
            shootSound.pitch = Random.Range(0.8f, 1.2f);
            shootSound.Play();
        }

        // Hide trajectory temporarily
        trajectoryPredictor.SetTrajectoryVisible(false);
        
        // Spawn new arrow
        SpawnNewArrow();
    }

    private void UpdateTrajectoryPrediction()
    {
        if (trajectoryPredictor == null || currentArrow == null) return;

        // Update projectile properties
        projectileProperties.initialPosition = currentArrow.transform.position;
        projectileProperties.direction = currentArrow.transform.forward;
        projectileProperties.initialSpeed = fixedArrowSpeed;
        projectileProperties.mass = arrowMass;
        projectileProperties.drag = arrowDrag;
        
        // Update trajectory visualization
        trajectoryPredictor.SetTrajectoryVisible(true);
        trajectoryPredictor.PredictTrajectory(projectileProperties);
    }

    // Call this to rotate the arrow/trajectory
    public void SetRotation(float yRotation)
    {
        if (ropeTransform != null)
        {
            Vector3 currentRotation = ropeTransform.rotation.eulerAngles;
            ropeTransform.rotation = Quaternion.Euler(currentRotation.x, yRotation, currentRotation.z);
        }
    }

    // Call this to adjust the arrow's vertical angle
    public void SetPitch(float pitch)
    {
        if (ropeTransform != null)
        {
            Vector3 currentRotation = ropeTransform.rotation.eulerAngles;
            ropeTransform.rotation = Quaternion.Euler(pitch, currentRotation.y, currentRotation.z);
        }
    }
} 