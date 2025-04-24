using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadCollisionDetection : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision != null)
        {
            if (collision.gameObject.CompareTag("Arrow"))
            {
                GameManager.Instance.opponentHealthValue -= 10f;
                Destroy(collision.gameObject);
                Debug.LogError("Body Shot");
            }
        }
    }
}
