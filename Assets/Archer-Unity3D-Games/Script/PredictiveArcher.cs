using UnityEngine;

[System.Serializable]
public class ProjectileProperties_New
{
    public Vector3 initialPosition;
    public Vector3 direction;
    public float initialSpeed;
    public float mass = 1f;
    public float drag = 0.1f;
}

public class PredictiveArcher : MonoBehaviour
{
    [Header("Bow Settings")]
    public float Tension;
    public Transform RopeTransform;
    public Vector3 RopeNearLocalPosition;
    public Vector3 RopeFarLocalPosition;
    public AnimationCurve RopeReturnAnimation;
    public float ReturnTime;
    public Arrow CurrentArrow = null;
    public float ArrowSpeed;
    public AudioSource ArrowAudio;
    public AudioSource BowAudio;
    public GameObject arrowPrefab;

    [Header("Trajectory Settings")]
    public TrajectoryPredictor trajectoryPredictor;
    public float arrowMass = 1f;
    public float arrowDrag = 0.05f;
    [Tooltip("Scale factor for trajectory visualization")]
    public float trajectoryScale = 1f;
    public Material trajectoryMaterial;
    public Color trajectoryColor = Color.red;
    [Range(0.01f, 0.5f)]
    public float trajectoryWidth = 0.05f;

    private bool _pressed;
    private ProjectileProperties projectileProperties = new ProjectileProperties();
    private LineRenderer trajectoryLine;

    void Start()
    {
        RopeNearLocalPosition = RopeTransform.localPosition;

        if (trajectoryPredictor == null)
        {
            trajectoryPredictor = GetComponentInChildren<TrajectoryPredictor>();
            if (trajectoryPredictor == null)
            {
                GameObject predictorObj = new GameObject("TrajectoryPredictor");
                predictorObj.transform.SetParent(transform);
                predictorObj.transform.localPosition = Vector3.zero;
                trajectoryPredictor = predictorObj.AddComponent<TrajectoryPredictor>();
                

                if (predictorObj.GetComponent<LineRenderer>() == null)
                {
                    trajectoryLine = predictorObj.AddComponent<LineRenderer>();
                }
            }
        }

        // Setup LineRenderer
        trajectoryLine = trajectoryPredictor.GetComponent<LineRenderer>();
        if (trajectoryLine != null)
        {
            trajectoryLine.useWorldSpace = true;
            trajectoryLine.startWidth = trajectoryWidth;
            trajectoryLine.endWidth = trajectoryWidth;
            trajectoryLine.material = trajectoryMaterial;
            trajectoryLine.startColor = trajectoryColor;
            trajectoryLine.endColor = trajectoryColor;
            trajectoryLine.enabled = false;
            trajectoryLine.positionCount = 50;  // Set initial position count
        }
        else
        {
            Debug.LogError("LineRenderer component not found on TrajectoryPredictor!");
        }
    }

    void Update()
    {
        float screenPosition_x = Input.mousePosition.x;
        float screenPosition_y = Input.mousePosition.y;

        if (screenPosition_x > 90 * Screen.width / 100 && screenPosition_y < Screen.width / 10)
            return;

        if (!GameManager.Instance.clickArrow)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            _pressed = true;
            if (CurrentArrow == null)
                CurrentArrow = Instantiate(arrowPrefab).GetComponent<Arrow>();

            CurrentArrow.SetToRope(RopeTransform, transform);

            BowAudio.pitch = Random.Range(0.8f, 1.2f);
            BowAudio.Play();
            
            // Enable trajectory visualization
            if (trajectoryLine != null)
            {
                trajectoryLine.enabled = true;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            _pressed = false;
            StartCoroutine(RopeReturn());
            
            float currentSpeed = ArrowSpeed * Tension;
            CurrentArrow.Shot(currentSpeed);
            
            // Disable trajectory visualization
            if (trajectoryLine != null)
            {
                trajectoryLine.enabled = false;
            }
            trajectoryPredictor.SetTrajectoryVisible(false);
            Tension = 0;

            BowAudio.Stop();
            ArrowAudio.pitch = Random.Range(0.8f, 1.2f);
            ArrowAudio.Play();
            CurrentArrow = null;
        }

        if (_pressed && CurrentArrow != null)
        {
            if (Tension < 1f)
            {
                Tension += Time.deltaTime;
            }
            RopeTransform.localPosition = Vector3.Lerp(RopeNearLocalPosition, RopeFarLocalPosition, Tension);
            
            // Update trajectory prediction
            UpdateTrajectoryPrediction();
        }
    }

    private void UpdateTrajectoryPrediction()
    {
        if (trajectoryPredictor != null && CurrentArrow != null)
        {
            projectileProperties.initialPosition = CurrentArrow.transform.position;
            projectileProperties.direction = CurrentArrow.transform.forward;
            projectileProperties.initialSpeed = ArrowSpeed * Tension * 2f;
            projectileProperties.mass = arrowMass;
            projectileProperties.drag = arrowDrag;
            
            trajectoryPredictor.SetTrajectoryVisible(true);
            trajectoryPredictor.PredictTrajectory(projectileProperties);

            // Debug visualization
            Debug.DrawRay(projectileProperties.initialPosition, projectileProperties.direction * 5f, Color.blue);
        }
    }

    System.Collections.IEnumerator RopeReturn()
    {
        Vector3 startLocalPosition = RopeTransform.localPosition;
        for (float f = 0; f < 1f; f += Time.deltaTime / ReturnTime)
        {
            RopeTransform.localPosition = Vector3.LerpUnclamped(startLocalPosition, RopeNearLocalPosition, RopeReturnAnimation.Evaluate(f));
            yield return null;
        }
        RopeTransform.localPosition = RopeNearLocalPosition;
    }
} 