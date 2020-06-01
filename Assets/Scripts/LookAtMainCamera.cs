using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtMainCamera : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        RotateSpriteTowardsCamera();
    }

    void RotateSpriteTowardsCamera()
    {
        Vector3 targetVector = Camera.main.transform.position - transform.position;

        float newYAngle = Mathf.Atan2(targetVector.z, targetVector.x) * Mathf.Rad2Deg - 90;

        transform.rotation = Quaternion.Euler(0, -1 * newYAngle, 0);
    }
}
