using System.Collections.Generic;
using UnityEngine;

public class Detector : MonoBehaviour
{
    public bool detected => detectedCount > 0;
    private int detectedCount;

    private void OnDrawGizmos()
    {
        Gizmos.color = new(0, 1f, 0, 0.5f);
        Gizmos.DrawCube(transform.position, transform.lossyScale);
    }

    private void FixedUpdate()
    {
        detectedCount = 0;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Voxel") || other.CompareTag("Ground"))
            detectedCount++;
    }
}
