using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomBow : MonoBehaviour
{
    public Transform RopeTransform;
    public Arrow CurrentArrow = null;
    public float ArrowSpeed;

    public GameObject ar;

    public TrajectoryPredictor trajectoryPredictor;

    public int assignedIndex;

    private void Start()
    {
        SpawnArrow();
    }
    void Update()
    {
        float screenPosition_x = Input.mousePosition.x;
        float screenPosition_y = Input.mousePosition.y;


        if (screenPosition_x > 90 * Screen.width / 100 && screenPosition_y < Screen.width / 10)
            return;

        //if (!GameManager.Instance.clickArrow)
        //    return;

        if (assignedIndex != TurnManagementTest.instance.currentPlayerIndex)
            return;

        if (Input.GetMouseButtonUp(0))
        {
            CurrentArrow.Shot(ArrowSpeed);
            //CurrentArrow = null;

            trajectoryPredictor.SetTrajectoryVisible(false);

            // Reset arrow reference
            CurrentArrow = null;
            SpawnArrow();
        }

        UpdateTrajectoryPredictor();
    }

    void UpdateTrajectoryPredictor()
    {
        ProjectileProperties projectileProperties = new ProjectileProperties();

        projectileProperties.direction = CurrentArrow.transform.forward;
        projectileProperties.initialPosition = CurrentArrow.transform.position;
        projectileProperties.initialSpeed = ArrowSpeed;
        projectileProperties.drag = CurrentArrow.rb.drag;
        projectileProperties.mass = CurrentArrow.rb.mass;

        trajectoryPredictor.SetTrajectoryVisible(true);

        trajectoryPredictor.PredictTrajectory(projectileProperties);
    }

    void SpawnArrow()
    {
        if (CurrentArrow == null)
            CurrentArrow = Instantiate(ar).GetComponent<Arrow>();

        CurrentArrow.SetToRope(RopeTransform, transform);
    }

    public void EnableTrajectory()
    {
        trajectoryPredictor.SetTrajectoryVisible(true);
    }
}
