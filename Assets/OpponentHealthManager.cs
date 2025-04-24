using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpponentHealthManager : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision != null)
        {
            if (collision.gameObject.CompareTag("Arrow"))
            {
                float value = GameManager.Instance.opponentHealthValue;
                value -= 20f;
                GameManager.Instance.UpdateHealthUI(value);

                Destroy(collision.gameObject);
                Debug.LogError("Head Shot");

            }
        }
    }
}
