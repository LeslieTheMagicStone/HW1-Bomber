using UnityEngine;

public class Detector : MonoBehaviour
{
    public bool detected => detectedCount > 0;
    private int detectedCount = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Voxel"))
            detectedCount++;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new(0, 1f, 0, 0.5f);
        Gizmos.DrawCube(transform.position, transform.lossyScale);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Voxel"))
            detectedCount--;
    }
}
