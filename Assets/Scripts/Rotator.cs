using UnityEngine;

public class Rotator : MonoBehaviour
{
    public float rotateSpeedX, rotateSpeedY, rotateSpeedZ;
    private void Update()
    {
        transform.Rotate(new Vector3(rotateSpeedX, rotateSpeedY, rotateSpeedZ) * Time.deltaTime);
    }
}
