using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Control of camera in main game scene. Borrowed from
/// Skeleton Hunter 3(TM) prototype project.
/// </summary>
public class CameraLogic : MonoBehaviour
{
    public GameObject m_target;
    public float pathSmoothing = 0.01f;

    // Update is called once per frame
    void Update()
    {
        var destination = m_target.transform.position;
        float z = transform.position.z;
        var newPosition = Vector3.Lerp(transform.position, destination, pathSmoothing);
        newPosition.z = z;
        transform.position = newPosition;
    }
}
