using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBallScript : MonoBehaviour
{
    IEnumerator Start()
    {
        yield return new WaitForSeconds(3f);

        Destroy(gameObject);
    }
}
