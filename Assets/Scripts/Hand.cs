using Unity.Mathematics;
using UnityEngine;

public class Hand : MonoBehaviour
{
    [SerializeField]
    private float animSpeed = 2.5f;
    [SerializeField]
    private float animAmplitude = 0.17f;

    private void Update()
    {
        Vector3 pos = transform.localPosition;
        pos.y = math.sin(Time.time * animSpeed) * animAmplitude;
        transform.localPosition = pos;
    }
}
