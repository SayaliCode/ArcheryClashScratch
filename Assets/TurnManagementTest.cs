    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManagementTest : MonoBehaviour
{
    public List<Transform> players; // Assign player objects in the inspector
    public CameraController cameraController;
    public int currentPlayerIndex = 0;
    public static TurnManagementTest instance;

    void Start()
    {
        instance = this;
        if (players.Count > 0)
        {
            StartTurn();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q)) 
        {
            StartTurn();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            EndTurn();
        }
    }

    void StartTurn()
    {
        Transform activePlayer = players[currentPlayerIndex];
        cameraController.FocusOnPlayer(activePlayer);
    }

    public void EndTurn()
    {
        Debug.LogError("Heree");
        currentPlayerIndex = (currentPlayerIndex + 1) % players.Count;
        StartTurn();
    }

    public void OnArrowFired(Transform arrow)
    {
        cameraController.FollowArrow(arrow, 2f); // Follow arrow for 2 seconds
        //StartCoroutine(ResetCameraAfterDelay(2f));
        StartCoroutine(HandleEndTurnAfterDelay(4f));
    }

    private IEnumerator ResetCameraAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        cameraController.FocusOnPlayer(players[currentPlayerIndex]);
    }

    private IEnumerator HandleEndTurnAfterDelay(float delay)
    {
        yield return new WaitForSeconds(1f);
        //cameraController.FocusOnPlayer(players[currentPlayerIndex]);
        //
        //yield return new WaitForSeconds(delay);
        //
        //// Proceed to the next turn
        //EndTurn();
        //
        //// Enable trajectory of the next player
        //CustomBow bow = players[currentPlayerIndex].GetComponentInChildren<CustomBow>();
        //if (bow != null)
        //{
        //    bow.enabled = true;
        //    bow.EnableTrajectory();
        //}
    }
}
