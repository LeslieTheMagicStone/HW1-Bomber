using UnityEngine;
using UnityEngine.Events;

public class TimeLogic : MonoBehaviour
{
    [SerializeField] private Light directionalLight;
    [SerializeField] private Transform pinLight;
    [SerializeField] private Transform pinCenter;
    const float MAX_TIME = 60f;
    const float MAX_TEMPERATURE = 5000f;
    const float MIN_TEMPERATURE = 1500f;
    public float time = MAX_TIME;
    public UnityEvent onComplete;

    private void Start()
    {
        time = MAX_TIME;
    }

    private void Update()
    {
        if (time <= 0) return;

        time -= Time.deltaTime;
        if (time <= 0)
        {
            onComplete.Invoke();
            time = 0;
        }

        var temperature = MIN_TEMPERATURE + (MAX_TEMPERATURE - MIN_TEMPERATURE) * (time / MAX_TIME);
        directionalLight.colorTemperature = temperature;

        var degree = 45 + 360f * time / MAX_TIME;
        var distance = Vector3.Distance(pinLight.position, pinCenter.position);
        var pos = pinLight.position;
        pos.x = distance * Mathf.Cos(degree * Mathf.Deg2Rad);
        pos.z = distance * Mathf.Sin(degree * Mathf.Deg2Rad);
        pinLight.SetPositionAndRotation(pos, Quaternion.Euler(90, -degree, 0));
    }
}
